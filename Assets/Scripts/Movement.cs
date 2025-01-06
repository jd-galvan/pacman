using RTT_Time = System.Int64;
using HRT_Time = System.Int64;

using UnityEngine;

[RequireComponent(typeof(RTDESKEntity))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
  // RTDESK 
  HRT_Time userTime, tenMillis;
  float samplingPeriodSeconds = 1.0f / 50.0f;
  RTDESKEngine engine;
  TransformMsg movementMsg;
  // FIN RTDESK 


  public float speed = 1.6f; // Velocidad de desplazamiento
  public float speedMultiplier = 0.02f; //Factor de ajuste para velocidad (e.g. fantasmas afectados por power pellet modificaran este valor)
  public Vector2 initialDirection; //Direccion inicial de pacman
  public LayerMask obstacleLayer; //Capa de obstaculo para deteccion de colision 

  public Rigidbody2D rb { get; private set; }
  public Vector2 direction { get; private set; }
  public Vector2 nextDirection { get; private set; }
  public Vector3 startingPosition { get; private set; }

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    startingPosition = transform.position;
    GetComponent<RTDESKEntity>().MailBox = ReceiveMessage;
  }

  private void Start()
  {
    ResetState();
  }

  public void ResetState()
  {
    // Configuracion inicial
    speedMultiplier = 1f;
    direction = initialDirection;
    nextDirection = Vector2.zero;
    transform.position = startingPosition;
    rb.isKinematic = false;
    enabled = true;

    // Configuracion para RTDESK
    engine = GetComponent<RTDESKEntity>().RTDESKEngineScript;
    movementMsg = (TransformMsg)engine.PopMsg((int)UserMsgTypes.Speed);
    movementMsg.V2 = rb.position;
    tenMillis = engine.ms2Ticks(10);
    engine.SendMsg(movementMsg, gameObject, ReceiveMessage, tenMillis);
  }

  public void SetDirection(Vector2 direction, bool forced = false)
  {
    if (forced || !Occupied(direction))  // Cambio de direccion 
    {
      this.direction = direction;
      nextDirection = Vector2.zero;
    }
    else // Cuando pacman no puede cambiar de direccion por presencia de un bloque
    {
      nextDirection = direction;
    }
  }

  public bool Occupied(Vector2 direction)
  {
    // If no collider is hit then there is no obstacle in that direction
    RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
    return hit.collider != null;
  }

  public void ReceiveMessage(MsgContent Msg)
  {
    if (!this.enabled)
    {
      engine.PushMsg(Msg);
      return;
    }

    if (nextDirection != Vector2.zero)
    {
      SetDirection(nextDirection);
    }

    TransformMsg m = (TransformMsg)Msg;

    Vector2 translation = speed * speedMultiplier * samplingPeriodSeconds * direction;

    rb.MovePosition(m.V2 + translation);

    m.V2 = rb.position;

    engine.SendMsg(Msg, tenMillis);
  }

  public void OnEnable()
  {
    Debug.Log("REACTIVA EL JUEGO");
    if (movementMsg != null)
    {
      engine.SendMsg(movementMsg, gameObject, ReceiveMessage, tenMillis);
    }
  }
}
