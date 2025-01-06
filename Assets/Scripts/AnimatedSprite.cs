using RTT_Time = System.Int64;
using HRT_Time = System.Int64;

using UnityEngine;

[RequireComponent(typeof(RTDESKEntity))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{

  // RTDESK 
  HRT_Time animationTime;
  RTDESKEngine engine;
  SpriteAnimateMsg spriteMsg;
  // FIN RTDESK 

  public Sprite[] sprites = new Sprite[0];
  public bool loop = true;

  private SpriteRenderer spriteRenderer;
  private int animationFrame;

  private void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();

    GetComponent<RTDESKEntity>().MailBox = ReceiveMessage;
  }

  private void OnEnable()
  {
    spriteRenderer.enabled = true;
  }

  private void OnDisable()
  {
    spriteRenderer.enabled = false;
  }

  private void Start()
  {
    // Configuracion para RTDESK
    engine = GetComponent<RTDESKEntity>().RTDESKEngineScript;
    spriteMsg = (SpriteAnimateMsg)engine.PopMsg((int)UserMsgTypes.Animation);
    spriteMsg.spriteRenderer = spriteRenderer;

    animationTime = engine.ms2Ticks(80);

    engine.SendMsg(spriteMsg, gameObject, ReceiveMessage, animationTime);
  }

  public void Restart()
  {
    animationFrame = -1;
  }

  void ReceiveMessage(MsgContent Msg)
  {
    SpriteAnimateMsg m = (SpriteAnimateMsg)Msg;

    if (!m.spriteRenderer.enabled)
    {
      engine.PushMsg(Msg);
      return;
    }

    animationFrame++;

    if (animationFrame >= sprites.Length && loop)
    {
      animationFrame = 0;
    }

    if (animationFrame >= 0 && animationFrame < sprites.Length)
    {
      m.spriteRenderer.sprite = sprites[animationFrame];
    }

    engine.SendMsg(m, gameObject, ReceiveMessage, animationTime);
  }

}
