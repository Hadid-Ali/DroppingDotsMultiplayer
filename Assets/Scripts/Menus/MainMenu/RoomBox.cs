
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomBox : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private Button select;

    public void Initialize(string name,string serverN, Action<string> OnClicked, Color color)
    {
        image.color = color;
        roomName.SetText(name);
        select.onClick.AddListener(()=> OnClicked(serverN));
    }
    
}
