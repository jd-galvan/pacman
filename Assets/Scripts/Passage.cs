using RTT_Time = System.Int64;
using HRT_Time = System.Int64;

using UnityEngine;

/// <summary>
/// Gestiona el comportamiento de los pasajes en el juego, permitiendo a Pacman y los fantasmas teletransportarse entre posiciones.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Passage : MonoBehaviour
{
  private HRT_Time userTime;
  private HRT_Time oneSecond, halfSecond, tenMillis;
  private RTDESKEngine engine;
  private MessageManager pacmanManagerMailBox,
                        ghostBlinky1ManagerMailBox,
                        ghostInky1ManagerMailBox,
                        ghostPinky1ManagerMailBox,
                        ghostClyde1ManagerMailBox;

  public Transform connection; ///< Punto de conexión al otro lado del pasaje.

  /// <summary>
  /// Inicializa los buzones de mensajes de cada entidad relevante del juego.
  /// </summary>
  private void Awake()
  {
    pacmanManagerMailBox = RTDESKEntity.getMailBox("Pacman");
    ghostBlinky1ManagerMailBox = RTDESKEntity.getMailBox("Ghost_Blinky");
    ghostInky1ManagerMailBox = RTDESKEntity.getMailBox("Ghost_Inky");
    ghostPinky1ManagerMailBox = RTDESKEntity.getMailBox("Ghost_Pinky");
    ghostClyde1ManagerMailBox = RTDESKEntity.getMailBox("Ghost_Clyde");
  }

  /// <summary>
  /// Obtiene la referencia al motor RTDESK al inicio del juego.
  /// </summary>
  private void Start()
  {
    engine = GetComponent<RTDESKEntity>().RTDESKEngineScript;
  }

  /// <summary>
  /// Maneja la teletransportación de Pacman y los fantasmas al cruzar el pasaje.
  /// </summary>
  /// <param name="other">Objeto que entra en contacto con el pasaje.</param>
  private void OnTriggerEnter2D(Collider2D other)
  {
    TransformMsg msg = (TransformMsg)engine.PopMsg((int)UserMsgTypes.Position);
    msg.V2 = connection.position;

    switch (other.gameObject.name)
    {
      case "Pacman":
        engine.SendMsg(msg, gameObject, pacmanManagerMailBox, HRTimer.HRT_INMEDIATELY);
        break;
      case "Ghost_Blinky":
        engine.SendMsg(msg, gameObject, ghostBlinky1ManagerMailBox, HRTimer.HRT_INMEDIATELY);
        break;
      case "Ghost_Inky":
        engine.SendMsg(msg, gameObject, ghostInky1ManagerMailBox, HRTimer.HRT_INMEDIATELY);
        break;
      case "Ghost_Pinky":
        engine.SendMsg(msg, gameObject, ghostPinky1ManagerMailBox, HRTimer.HRT_INMEDIATELY);
        break;
      case "Ghost_Clyde":
        engine.SendMsg(msg, gameObject, ghostClyde1ManagerMailBox, HRTimer.HRT_INMEDIATELY);
        break;
      default:
        break;
    }
  }
}
