using UnityEngine;

/// <summary>
/// Controla el comportamiento de Pacman, incluyendo su movimiento, colisiones y la secuencia de muerte.
/// </summary>
[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
  // RTDESK 
  private RTDESKEngine engine; ///< Motor RTDESK para la gestión de eventos y entradas.
  // FIN RTDESK 

  [SerializeField] private KeyCode upKey = KeyCode.UpArrow; ///< Referencia tecla arriba.
  [SerializeField] private KeyCode downKey = KeyCode.DownArrow; ///< Referencia tecla abajo.
  [SerializeField] private KeyCode leftKey = KeyCode.LeftArrow; ///< Referencia tecla izquierda.
  [SerializeField] private KeyCode rightKey = KeyCode.RightArrow; ///< Referencia tecla derecha.

  [SerializeField]
  private AnimatedSprite deathSequence; ///< Animación de muerte de Pacman.
  private SpriteRenderer spriteRenderer; ///< Componente de renderizado de sprite.
  private CircleCollider2D circleCollider; ///< Colisionador circular de Pacman.
  private Movement movement; ///< Componente de movimiento de Pacman.

  /// <summary>
  /// Inicializa los componentes internos.
  /// </summary>
  private void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    circleCollider = GetComponent<CircleCollider2D>();
    movement = GetComponent<Movement>();
  }

  /// <summary>
  /// Configura la entrada de usuario mediante RTDESK.
  /// </summary>
  private void Start()
  {
    engine = GetComponent<RTDESKEntity>().RTDESKEngineScript;
    RTDESKInputManager IM = engine.GetInputManager();

    // Registrar las teclas de dirección para capturar la entrada del usuario
    IM.RegisterKeyCode(ReceiveMessage, upKey);
    IM.RegisterKeyCode(ReceiveMessage, downKey);
    IM.RegisterKeyCode(ReceiveMessage, leftKey);
    IM.RegisterKeyCode(ReceiveMessage, rightKey);
  }

  /// <summary>
  /// Reinicia el estado de Pacman para un nuevo juego o ronda.
  /// </summary>
  public void ResetState()
  {
    enabled = true;
    spriteRenderer.enabled = true;
    circleCollider.enabled = true;
    deathSequence.enabled = false;
    movement.ResetState();
    gameObject.SetActive(true);
  }

  /// <summary>
  /// Inicia la secuencia de muerte de Pacman, deshabilitando su control y activando la animación.
  /// </summary>
  public void DeathSequence()
  {
    enabled = false;
    spriteRenderer.enabled = false;
    circleCollider.enabled = false;
    movement.enabled = false;
    deathSequence.enabled = true;
    deathSequence.Restart();
  }

  /// <summary>
  /// Maneja los mensajes recibidos por RTDESK, principalmente las entradas del usuario.
  /// </summary>
  /// <param name="Msg">Mensaje recibido.</param>
  private void ReceiveMessage(MsgContent Msg)
  {
    switch (Msg.Type)
    {
      case (int)RTDESKMsgTypes.Input:
        RTDESKInputMsg IMsg = (RTDESKInputMsg)Msg;
        Vector2 newDirection = Vector2.zero;

        switch (IMsg.c)
        {
          case var key when key == upKey:
            newDirection = Vector2.up;
            break;
          case var key when key == downKey:
            newDirection = Vector2.down;
            break;
          case var key when key == leftKey:
            newDirection = Vector2.left;
            break;
          case var key when key == rightKey:
            newDirection = Vector2.right;
            break;
          default:
            return; // Salimos de la función si no es una tecla válida
        }

        movement.SetDirection(newDirection);

        // Calcula el ángulo y rota inmediatamente Pacman en la dirección correcta
        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        engine.PushMsg(Msg);
        break;
      default:
        engine.PushMsg(Msg);
        break;
    }
  }
}
