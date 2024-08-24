using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetImage(int val)
    {
        spriteRenderer.color = val == 1 ? Color.blue : Color.red;
    }
}
