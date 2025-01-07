using RTT_Time = System.Int64;
using HRT_Time = System.Int64;

using UnityEngine;

/// <summary>
/// Controla el movimiento de los personajes en el juego, incluyendo Pacman y los fantasmas.
/// Utiliza RTDESK para gestionar la comunicación y la actualización de posiciones.
/// </summary>
[RequireComponent(typeof(RTDESKEntity))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
  // RTDESK 
  private HRT_Time userTime, tenMillis;
  private float samplingPeriodSeconds = 1.0f / 50.0f;
  private RTDESKEngine engine;
  private TransformMsg movementMsg;
  // FIN RTDESK 

  public float speed = 1.6f; ///< Velocidad de desplazamiento.
  public float speedMultiplier = 0.02f; ///< Factor de ajuste para velocidad (afectado por power pellets, etc.).
  public Vector2 initialDirection; ///< Dirección inicial del personaje.
  public LayerMask obstacleLayer; ///< Capa de obstáculos para detección de colisiones.

  public Rigidbody2D rb { get; private set; } ///< Referencia al Rigidbody2D del personaje.
  public Vector2 direction { get; private set; } ///< Dirección actual del movimiento.
  public Vector2 nextDirection { get; private set; } ///< Próxima dirección a tomar si es posible.
  public Vector3 startingPosition { get; private set; } ///< Posición inicial del personaje.

  /// <summary>
  /// Inicializa las variables y configura la comunicación con RTDESK.
  /// </summary>
  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    startingPosition = transform.position;
    GetComponent<RTDESKEntity>().MailBox = ReceiveMessage;
  }

  /// <summary>
  /// Reinicia el estado del movimiento al inicio del juego.
  /// </summary>
  private void Start()
  {
    ResetState();
  }

  /// <summary>
  /// Restablece la configuración inicial del movimiento.
  /// </summary>
  public void ResetState()
  {
    speedMultiplier = 1f;
    direction = initialDirection;
    nextDirection = Vector2.zero;
    transform.position = startingPosition;
    rb.isKinematic = false;
    enabled = true;

    // Configuración de RTDESK
    engine = GetComponent<RTDESKEntity>().RTDESKEngineScript;
    movementMsg = (TransformMsg)engine.PopMsg((int)UserMsgTypes.Speed);
    movementMsg.V2 = rb.position;
    tenMillis = engine.ms2Ticks(10);
    engine.SendMsg(movementMsg, gameObject, ReceiveMessage, tenMillis);
  }

  /// <summary>
  /// Establece la dirección de movimiento del personaje.
  /// </summary>
  /// <param name="direction">Nueva dirección a tomar.</param>
  /// <param name="forced">Si es verdadero, la dirección se cambia sin verificar colisiones.</param>
  public void SetDirection(Vector2 direction, bool forced = false)
  {
    if (forced || !Occupied(direction))
    {
      this.direction = direction;
      nextDirection = Vector2.zero;
    }
    else
    {
      nextDirection = direction;
    }
  }

  /// <summary>
  /// Verifica si hay un obstáculo en la dirección dada.
  /// </summary>
  /// <param name="direction">Dirección a evaluar.</param>
  /// <returns>Devuelve true si hay un obstáculo, de lo contrario false.</returns>
  public bool Occupied(Vector2 direction)
  {
    RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
    return hit.collider != null;
  }

  /// <summary>
  /// Recibe mensajes de RTDESK y procesa las actualizaciones de posición.
  /// </summary>
  /// <param name="Msg">Mensaje recibido con información de posición o velocidad.</param>
  public void ReceiveMessage(MsgContent Msg)
  {
    if (!this.enabled)
    {
      engine.PushMsg(Msg);
      return;
    }

    TransformMsg m = (TransformMsg)Msg;

    switch (Msg.Type)
    {
      case (int)UserMsgTypes.Position:
        rb.MovePosition(m.V2);
        rb.position = m.V2;
        engine.PushMsg(Msg);
        break;
      case (int)UserMsgTypes.Speed:
        if (nextDirection != Vector2.zero)
        {
          SetDirection(nextDirection);
        }
        Vector2 translation = speed * speedMultiplier * samplingPeriodSeconds * direction;
        rb.MovePosition(m.V2 + translation);
        m.V2 = rb.position;
        engine.SendMsg(Msg, tenMillis);
        break;
    }
  }

  /// <summary>
  /// Se ejecuta cuando el objeto es activado y reinicia la comunicación con RTDESK.
  /// </summary>
  private void OnEnable()
  {
    if (movementMsg != null)
    {
      movementMsg = (TransformMsg)engine.PopMsg((int)UserMsgTypes.Speed);
      movementMsg.V2 = rb.position;
      engine.SendMsg(movementMsg, gameObject, ReceiveMessage, tenMillis);
    }
  }
}
