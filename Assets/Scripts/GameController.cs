using System;
using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Turn
{
    Blue = 1,
    Red = 2
}
public class GameController : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Camera cam;
    [SerializeField] private Block blockFab;
    
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _WinText;
    [SerializeField] private CanvasGroup _CanvasGroup;
    
    [SerializeField] private int row;
    [SerializeField] private int col;
    private int _length = 4;

    private BlockMatrix _matrix;

    public static Action OnTurnCompleted;
    public static bool CanMakeMove() => 
        NetworkGameplayManager.GetLocalPlayerVal() == NetworkGameplayManager.CurrentTurn 
        && !_isInTransition;

    private static bool _isInTransition; 
    private Turn _currentTurn = Turn.Blue;


    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnGameStarted.Register(StartGame);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnGameStarted.UnRegister(StartGame);
    }
    private void ResetGame()=> _photonView.RPC(nameof(ResetGameRPC), RpcTarget.All);

    private void ResetGameRPC()
    {
        _matrix.Reset();
        GameEvents.MenuEvents.MenuTransitionEvent.Raise(MenuName.None);
        SpawnMatrix();
    }

    private void SpawnMatrix()
    {
        _matrix = new BlockMatrix(row, col, 0);
        
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                var spawnedBlock = Instantiate(blockFab, new Vector3(j,-i), Quaternion.identity);
                spawnedBlock.Initialize(i,j, OnMouseButtonClicked);
                spawnedBlock.name = i + "," + j;
                _matrix.InitializeBlocks(i, j, spawnedBlock);
            }
        }
        cam.transform.position = new Vector3((float) col/2 - 1f , (float) -row/2  + 0.5f, -10); //manually set
    }

    private void StartGame()
    {
        _button.onClick.AddListener(ResetGame);
        SpawnMatrix();
        _CanvasGroup.alpha = 1;
        
        GameEvents.MenuEvents.MenuTransitionEvent.Raise(MenuName.None);
    }

    private int EvaluateMatrix(int x, int y)
    {
        if (_matrix.IsRow(x, y, _length) ||_matrix.IsColumn(x, y, _length) ||
            _matrix.IsDiagonal(x, y, _length) || _matrix.IsReverseDiagonal(x, y, _length))
        {
            return _matrix.GetElement(x, y);
        }
        
        return -1;

    }
    private int _cRow;
    private int _cCol;

    private void OnMouseButtonClicked(int x, int y)
    {
        _photonView.RPC(nameof(OnCellClicked),RpcTarget.All,x,y);
    }

    [PunRPC]
    private void OnCellClicked(int x, int y)
    {
        _currentTurn = NetworkGameplayManager.CurrentTurn;
        
        _cRow = 0;
        _cCol = y;
        
        StartCoroutine(StartFall());
    }



    IEnumerator  StartFall()
    {
        _isInTransition = true;
        while (true)
        {
            _matrix.SetElementAt(_cRow,_cCol, (int) _currentTurn);
            
            int bottomCell = _matrix.GetElement(_cRow + 1, _cCol);

            if (bottomCell is -1 or >= 1)
            {
                break;
            }
        
            yield return new WaitForSeconds(0.2f);
            
            _matrix.SetElementAt(_cRow, _cCol, 0);
            _cRow++;
        }
        
        

        TurnCompleted();

        yield return new WaitForSeconds(1f);
        _isInTransition = false;
    }

    private void TurnCompleted()
    {
        int winnerV = EvaluateMatrix(_cRow, _cCol);
        _cRow  = -1; 
        _cCol = -1;
        
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (winnerV != -1)
        {
            Turn winner = (Turn) winnerV;
            _photonView.RPC(nameof(OnWin), RpcTarget.All, (int) winner);
            
        }
        else
        {
            OnTurnCompleted?.Invoke();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        _WinText.SetText("");
    }

    [PunRPC]
    private void OnWin(int winner)
    {
        GameEvents.NetworkGameplayEvents.OnGameWin.Raise((Turn) winner);
        _WinText.SetText($"{winner} has won the game");
        StartCoroutine(Wait());
        
        _isInTransition = true;
        
    }






}
