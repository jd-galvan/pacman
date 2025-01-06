using RTT_Time = System.Int64;
using HRT_Time = System.Int64;

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Passage : MonoBehaviour
{
  HRT_Time userTime;
  HRT_Time oneSecond, halfSecond, tenMillis;
  RTDESKEngine engine;
  MessageManager pacmanManagerMailBox,
                ghostBlinky1ManagerMailBox,
                ghostInky1ManagerMailBox,
                ghostPinky1ManagerMailBox,
                ghostClyde1ManagerMailBox;

  public Transform connection;

  private void Awake()
  {
    pacmanManagerMailBox = RTDESKEntity.getMailBox("Pacman");
    ghostBlinky1ManagerMailBox = RTDESKEntity.getMailBox("Ghost_Blinky");
    ghostInky1ManagerMailBox = RTDESKEntity.getMailBox("Ghost_Inky");
    ghostPinky1ManagerMailBox = RTDESKEntity.getMailBox("Ghost_Pinky");
    ghostClyde1ManagerMailBox = RTDESKEntity.getMailBox("Ghost_Clyde");
  }

  private void Start()
  {
    engine = GetComponent<RTDESKEntity>().RTDESKEngineScript;
  }

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
