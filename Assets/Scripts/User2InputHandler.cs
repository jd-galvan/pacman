using UnityEngine;

public class User2InputHandler : MonoBehaviour, IInputHandler
{
  public Vector2 GetInputDirection()
  {
    if (Input.GetKey(KeyCode.UpArrow)) return Vector2.up;
    if (Input.GetKey(KeyCode.DownArrow)) return Vector2.down;
    if (Input.GetKey(KeyCode.LeftArrow)) return Vector2.left;
    if (Input.GetKey(KeyCode.RightArrow)) return Vector2.right;
    return Vector2.zero;
  }
}
