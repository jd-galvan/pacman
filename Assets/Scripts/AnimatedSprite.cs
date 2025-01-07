using RTT_Time = System.Int64;
using HRT_Time = System.Int64;

using UnityEngine;

/// <summary>
/// Clase encargada de manejar la animación de un Sprite en Unity utilizando RTDESK.
/// </summary>
[RequireComponent(typeof(RTDESKEntity))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
  // RTDESK 
  private HRT_Time animationTime; ///< Tiempo de animación en ticks.
  private RTDESKEngine engine; ///< Referencia al motor RTDESK.
  private SpriteAnimateMsg spriteMsg; ///< Mensaje de animación de sprites.
  // FIN RTDESK 

  public Sprite[] sprites = new Sprite[0]; ///< Arreglo de sprites para la animación.
  public bool loop = true; ///< Indica si la animación debe repetirse en bucle.

  private SpriteRenderer spriteRenderer; ///< Referencia al componente SpriteRenderer.
  private int animationFrame; ///< Índice del frame de animación actual.

  /// <summary>
  /// Inicialización de referencias.
  /// </summary>
  private void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    GetComponent<RTDESKEntity>().MailBox = ReceiveMessage;
  }

  /// <summary>
  /// Activa el SpriteRenderer cuando el objeto es activado.
  /// </summary>
  private void OnEnable()
  {
    spriteRenderer.enabled = true;
  }

  /// <summary>
  /// Desactiva el SpriteRenderer cuando el objeto es desactivado.
  /// </summary>
  private void OnDisable()
  {
    spriteRenderer.enabled = false;
  }

  /// <summary>
  /// Configuración inicial de la animación en RTDESK.
  /// </summary>
  private void Start()
  {
    engine = GetComponent<RTDESKEntity>().RTDESKEngineScript;
    spriteMsg = (SpriteAnimateMsg)engine.PopMsg((int)UserMsgTypes.Animation);
    spriteMsg.spriteRenderer = spriteRenderer;

    animationTime = engine.ms2Ticks(80); // Convierte 80ms a ticks

    engine.SendMsg(spriteMsg, gameObject, ReceiveMessage, animationTime);
  }

  /// <summary>
  /// Reinicia la animación estableciendo el frame inicial.
  /// </summary>
  public void Restart()
  {
    animationFrame = -1;
  }

  /// <summary>
  /// Método que recibe los mensajes de RTDESK y actualiza el sprite animado.
  /// </summary>
  /// <param name="Msg">Mensaje recibido con el contenido de animación.</param>
  private void ReceiveMessage(MsgContent Msg)
  {
    SpriteAnimateMsg m = (SpriteAnimateMsg)Msg;
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
