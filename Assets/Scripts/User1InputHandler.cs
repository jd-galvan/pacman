using UnityEngine;

public class User1InputHandler : MonoBehaviour, IInputHandler
{
  public Vector2 GetInputDirection()
  {
    if (Input.GetKey(KeyCode.W)) return Vector2.up;
    if (Input.GetKey(KeyCode.S)) return Vector2.down;
    if (Input.GetKey(KeyCode.A)) return Vector2.left;
    if (Input.GetKey(KeyCode.D)) return Vector2.right;
    return Vector2.zero;
  }
}
