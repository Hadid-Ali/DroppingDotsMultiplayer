using UnityEngine;
using Photon.Pun;

public class NetworkMatchStartHandler : MonoBehaviour
{
    [SerializeField] private PhotonView _view;
    
    private bool m_IsAutoStartRequestSent = false;

    private string _matchStartTitle = "Starting The Match";

    private int CurrentPlayersCount => PhotonNetwork.PlayerList.Length;

    private void Awake()
    {
        GameEvents.NetworkEvents.PlayerJoinedRoom.Register(OnPlayerEnteredInRoom);
        GameEvents.NetworkEvents.OnMasterGameplayLoaded.Register(OnMasterGameplayLoaded);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkEvents.PlayerJoinedRoom.Register(OnPlayerEnteredInRoom);
        GameEvents.NetworkEvents.OnMasterGameplayLoaded.UnRegister(OnMasterGameplayLoaded);
    }

    public void OnPlayerEnteredInRoom()
    {
        print($" IS master : {PhotonNetwork.IsMasterClient}");
        if (PhotonNetwork.IsMasterClient)
        {
            CheckForMinimumPlayersCount();
            CheckForMaximumPlayersCount();
        }
    }

    private void CheckForMinimumPlayersCount()
    {
        if (m_IsAutoStartRequestSent)
            return;
        
        if (CurrentPlayersCount >= GameData.MetaData.MinimumRequiredPlayers)
        { 
            GameEvents.TimerEvents.ExecuteActionRequest.Raise(new TimerDataObject()
            {
                Title = _matchStartTitle,
                TimeDuration = GameData.MetaData.WaitBeforeAutomaticMatchStart,
                ActionToExecute =  StartMatchInternal,
                TickTimeEvent = TimerTick,
                IsNetworkGlobal = true
            });
            m_IsAutoStartRequestSent = true;
        }
    }

    private void CheckForMaximumPlayersCount()
    {
        // if (CurrentPlayersCount >= GameData.MetaData.MaximumRequiredPlayers)
        // {
        //     TerminateAutoMatchStartRequest();
        //     StartMatchInternal();
        // }
    }

    public void OnPlayerLeftRoom()
    {
        if (CurrentPlayersCount < GameData.MetaData.MinimumRequiredPlayers)
            TerminateAutoMatchStartRequest();
    }

    private void TerminateAutoMatchStartRequest()
    {
        GameEvents.TimerEvents.CancelActionRequest.Raise();
        m_IsAutoStartRequestSent = false;
    }

    public void TimerTick(string time)
    {
        _view.RPC(nameof(GlobalTimerTick), RpcTarget.All, time);
    }

    [PunRPC]
    public void GlobalTimerTick(string time)
    {
        GameEvents.NetworkEvents.MatchStartTimer.Raise(time);
    }
    
    public void StartMatchInternal()
    {
        m_IsAutoStartRequestSent = false;
        
        if (!PhotonNetwork.IsMasterClient)
            return;
        
        PhotonNetwork.CurrentRoom.IsOpen = false;
        
        _view.RPC(nameof(LoadScene), RpcTarget.All);
        
    }

    private void OnMasterGameplayLoaded()
    {
        _view.RPC(nameof(LoadScene), RpcTarget.Others);
    }

    [PunRPC]
    private void LoadScene()
    { 
        GameEvents.NetworkGameplayEvents.OnGameStarted.Raise();
    }
}
