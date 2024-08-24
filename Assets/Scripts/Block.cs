using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private SpriteRenderer filler;
    
    [SerializeField] public int row;
    [SerializeField] public int col;

    
    private Action<int,int> OnMouseClicked;
    public void Initialize(int _row, int _col, Action<int,int> OnMouseClick)
    {
        row = _row;
        col = _col;
        SetStatus(0);
        
        OnMouseClicked += OnMouseClick;
    }

    public void SetStatus(int status)
    {
        switch (status)
        {
            case 0:
                filler.color = Color.clear;
                break;
            case 1:
                filler.color = Color.blue;
                break;
            case 2:
                filler.color = Color.red;
                break;
        }
    }

    private void OnMouseDown()
    {
        if(!GameController.CanMakeMove())
            return;
        
        OnMouseClicked?.Invoke(row, col);  
    }
}
