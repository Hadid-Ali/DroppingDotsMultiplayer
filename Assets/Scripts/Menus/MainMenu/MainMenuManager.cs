using UnityEngine;

public class MainMenuManager : MenusController
{
    [SerializeField] private CanvasGroup canvas;

    private void Awake()
    {
        GameEvents.MenuEvents.OnMenuEnable.Register(OnMenuEnable);
    }

    private void OnMenuEnable(bool val)
    {
        if (val)
        {
            canvas.alpha = 1;
            canvas.interactable = true;
        }
        else
        {
            canvas.alpha = 0;
            canvas.interactable = false;
        }
    }

    private void OnDestroy()
    {
        GameEvents.MenuEvents.OnMenuEnable.UnRegister(OnMenuEnable);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //m_RoomJoinFailedEvent.Register(ShowRoomCreationScreen);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //m_RoomJoinFailedEvent.Unregister(ShowRoomCreationScreen);
    }
    
    private void ShowRoomCreationScreen()
    {
        SetMenuState(MenuName.CreateRoom);
    }
}
