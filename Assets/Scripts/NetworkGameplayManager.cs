using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkGameplayManager : MonoBehaviour
{
    [SerializeField] private PhotonView view;
    [SerializeField] private TurnUI UI;
    
    public static Turn GetLocalPlayerVal()
    {
        int num = PhotonNetwork.LocalPlayer.ActorNumber;
        return (Turn)num;
    } 
    private int _turnCounter = 1;
    public static Turn CurrentTurn = Turn.Red;

    private void Awake()
    {
        GameController.OnTurnCompleted += OnTurnMade;
        GameEvents.NetworkGameplayEvents.OnGameStarted.Register(OnGameReset);
        GameEvents.NetworkGameplayEvents.OnGameReset.Register(OnGameReset);
    }

    private void OnGameReset()
    {
        _turnCounter = 1;
        CurrentTurn = Turn.Red;
        UpdatePlayersListView();
        UI.UpdateTurnImage(CurrentTurn);
    }

    private void OnDestroy()
    {
        GameController.OnTurnCompleted -= OnTurnMade;
        GameEvents.NetworkGameplayEvents.OnGameStarted.UnRegister(OnGameReset);
        GameEvents.NetworkGameplayEvents.OnGameReset.UnRegister(OnGameReset);
    }

    private void FixedUpdate()
    {
        print($"{GetLocalPlayerVal()} {CurrentTurn} {_turnCounter}");
    }

    private void UpdatePlayersListView()
    {
        Player[] players = PhotonNetwork.PlayerList;
        string[] playersName = new string[players.Length];
        
        foreach (var t in players)
        {
            switch (t.ActorNumber)
            {
                case (int)Turn.Blue:
                    playersName[0] = t.NickName;
                    break;
                case (int)Turn.Red:
                    playersName[1] = t.NickName;
                    break;
            }
        }
        
        UI.UpdateNames(playersName[0],playersName[1]);
    }


    private void OnTurnMade() 
    {
        _turnCounter++;
        CurrentTurn = _turnCounter % 2 == 0 ? (Turn)1 : (Turn)2;
        view.RPC(nameof(SyncTurn),RpcTarget.All, (int) CurrentTurn);
    }

    [PunRPC]
    private void SyncTurn(int turn)
    {
        CurrentTurn = (Turn)turn;
        UI.UpdateTurnImage(CurrentTurn);
    }

}
