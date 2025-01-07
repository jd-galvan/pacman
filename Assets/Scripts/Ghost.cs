using UnityEngine;

/// <summary>
/// Clase que representa el comportamiento de un fantasma en el juego.
/// Controla sus estados y su interacción con Pacman.
/// </summary>
[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
  public Movement movement { get; private set; } ///< Referencia al componente de movimiento.
  public GhostHome home { get; private set; } ///< Referencia al comportamiento de inicio del fantasma.
  public GhostScatter scatter { get; private set; } ///< Referencia al comportamiento de dispersión.
  public GhostChase chase { get; private set; } ///< Referencia al comportamiento de persecución.
  public GhostFrightened frightened { get; private set; } ///< Referencia al comportamiento de asustado.
  public GhostBehavior initialBehavior; ///< Comportamiento inicial del fantasma.
  public Transform target; ///< Objetivo al que el fantasma persigue.
  public int points = 200; ///< Puntos que otorga el fantasma cuando es comido.

  /// <summary>
  /// Inicializa las referencias de los componentes del fantasma.
  /// </summary>
  private void Awake()
  {
    movement = GetComponent<Movement>();
    home = GetComponent<GhostHome>();
    scatter = GetComponent<GhostScatter>();
    chase = GetComponent<GhostChase>();
    frightened = GetComponent<GhostFrightened>();
  }

  /// <summary>
  /// Llama a la función ResetState al inicio del juego.
  /// </summary>
  private void Start()
  {
    ResetState();
  }

  /// <summary>
  /// Reinicia el estado del fantasma, activando sus comportamientos iniciales.
  /// </summary>
  public void ResetState()
  {
    gameObject.SetActive(true);
    movement.ResetState();

    frightened.Disable();
    chase.Disable();
    scatter.Enable();

    if (home != initialBehavior)
    {
      home.Disable();
    }

    if (initialBehavior != null)
    {
      initialBehavior.Enable();
    }
  }

  /// <summary>
  /// Establece la posición del fantasma manteniendo la profundidad (Z) constante.
  /// </summary>
  /// <param name="position">Nueva posición del fantasma.</param>
  public void SetPosition(Vector3 position)
  {
    position.z = transform.position.z;
    transform.position = position;
  }

  /// <summary>
  /// Maneja la colisión con Pacman, determinando si debe ser comido o si Pacman pierde una vida.
  /// </summary>
  /// <param name="collision">Información de la colisión detectada.</param>
  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
    {
        if (frightened.enabled) {
            GameManager.Instance.GhostEaten(this);
        } else {
            Pacman pacmanHit = collision.gameObject.GetComponent<Pacman>();
            if (pacmanHit != null)
            {
                GameManager.Instance.PacmanEaten(pacmanHit);
            }
        }
        
    }
  }
}
