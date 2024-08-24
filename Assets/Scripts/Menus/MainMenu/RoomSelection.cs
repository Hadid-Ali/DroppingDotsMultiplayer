using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSelection : UIMenuBase
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Transform roomTParent;

    [SerializeField] private Color[] _colors;

    private List<RoomBox> instantiatedObj = new();
    private void Awake()
    {
        GameEvents.MenuEvents.RoomsListUpdated.Register(OnRoomsUpdated);
    }
    private void OnDestroy()
    {
        GameEvents.MenuEvents.RoomsListUpdated.UnRegister(OnRoomsUpdated);
    }

    private void OnRoomsUpdated(List<string> obj)
    {
        foreach(var v in instantiatedObj)
            Destroy(v.gameObject);
        
        instantiatedObj.Clear();

        for (int i = 0; i < obj.Count; i++)
        {
            RoomBox room = Instantiate(roomPrefab.gameObject,roomTParent).GetComponent<RoomBox>();

            Color roomColor =  i % 2 == 0 ? _colors[0] : _colors[1];
            
            room.Initialize($"GameRoom-{i + 1}", obj[i], OnRoomButtonClicked,roomColor);
            instantiatedObj.Add(room);
        }
    }

    private void OnRoomButtonClicked(string name)
    {
        GameEvents.MenuEvents.RoomJoinRequested.Raise(name);
        
        foreach(var v in instantiatedObj)
            Destroy(v.gameObject);
        
        instantiatedObj.Clear();
        
        GameEvents.MenuEvents.MenuTransitionEvent.Raise(MenuName.ConnectionScreen);
    }

    
}