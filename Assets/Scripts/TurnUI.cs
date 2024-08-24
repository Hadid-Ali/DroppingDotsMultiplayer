
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{
   [SerializeField] private Image turnImage;
   [SerializeField] private TextMeshProUGUI Player1;
   [SerializeField] private TextMeshProUGUI Player2;

   [SerializeField] private Color blueColor;
   [SerializeField] private Color RedColor;
   
   public void UpdateNames(string player1, string player2) 
   {
      Player1.SetText(player1);
      Player2.SetText(player2);
   }

   public void UpdateTurnImage(Turn turn)
   {
      turnImage.color = turn == Turn.Blue ? blueColor : RedColor;
   }
   
}
