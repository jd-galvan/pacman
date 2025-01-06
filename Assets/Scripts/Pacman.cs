using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
  // RTDESK 
  RTDESKEngine engine;
  // FIN RTDESK 

  [SerializeField]
  private AnimatedSprite deathSequence;
  private SpriteRenderer spriteRenderer;
  private CircleCollider2D circleCollider;
  private Movement movement;

  private void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    circleCollider = GetComponent<CircleCollider2D>();
    movement = GetComponent<Movement>();
  }

  private void Start()
  {
    engine = GetComponent<RTDESKEntity>().RTDESKEngineScript;
    RTDESKInputManager IM = engine.GetInputManager();

    //Register keys that we want to be signaled in case the user press them
    IM.RegisterKeyCode(ReceiveMessage, KeyCode.UpArrow);
    IM.RegisterKeyCode(ReceiveMessage, KeyCode.DownArrow);
    IM.RegisterKeyCode(ReceiveMessage, KeyCode.LeftArrow);
    IM.RegisterKeyCode(ReceiveMessage, KeyCode.RightArrow);
  }
  public void ResetState()
  {
    enabled = true;
    spriteRenderer.enabled = true;
    circleCollider.enabled = true;
    deathSequence.enabled = false;
    movement.ResetState();
    gameObject.SetActive(true);
  }

  public void DeathSequence()
  {
    enabled = false;
    spriteRenderer.enabled = false;
    circleCollider.enabled = false;
    movement.enabled = false;
    deathSequence.enabled = true;
    deathSequence.Restart();
  }

  private void ReceiveMessage(MsgContent Msg)
  {
    switch (Msg.Type)
    {
      case (int)RTDESKMsgTypes.Input:
        RTDESKInputMsg IMsg = (RTDESKInputMsg)Msg;
        switch (IMsg.c)
        {
          case KeyCode.UpArrow:
            movement.SetDirection(Vector2.up);
            break;
          case KeyCode.DownArrow:
            movement.SetDirection(Vector2.down);
            break;
          case KeyCode.LeftArrow:
            movement.SetDirection(Vector2.left);
            break;
          case KeyCode.RightArrow:
            movement.SetDirection(Vector2.right);
            break;
          default:
            break;
        }
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        break;
      default:
        break;
    }
  }

}
