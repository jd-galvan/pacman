using UnityEngine;

/// <summary>
/// Comportamiento del fantasma cuando está asustado. 
/// Modifica su apariencia y su dirección para alejarse de Pacman.
/// </summary>
public class GhostFrightened : GhostBehavior
{
  public SpriteRenderer body; ///< Representación del cuerpo del fantasma.
  public SpriteRenderer eyes; ///< Representación de los ojos del fantasma.
  public SpriteRenderer blue; ///< Representación azul cuando está asustado.
  public SpriteRenderer white; ///< Representación parpadeante antes de volver a la normalidad.

  private bool eaten; ///< Indica si el fantasma ha sido comido.

  /// <summary>
  /// Habilita el estado de asustado con una duración específica.
  /// </summary>
  /// <param name="duration">Duración en segundos.</param>
  public override void Enable(float duration)
  {
    base.Enable(duration);

    body.enabled = false;
    eyes.enabled = false;
    blue.enabled = true;
    white.enabled = false;

    Invoke(nameof(Flash), duration / 2f);
  }

  /// <summary>
  /// Deshabilita el estado de asustado y restaura la apariencia original.
  /// </summary>
  public override void Disable()
  {
    base.Disable();

    body.enabled = true;
    eyes.enabled = true;
    blue.enabled = false;
    white.enabled = false;
  }

  /// <summary>
  /// Maneja la lógica cuando el fantasma es comido por Pacman.
  /// </summary>
  private void Eaten()
  {
    eaten = true;
    ghost.SetPosition(ghost.home.inside.position);
    ghost.home.Enable(duration);

    body.enabled = false;
    eyes.enabled = true;
    blue.enabled = false;
    white.enabled = false;
  }

  /// <summary>
  /// Activa el parpadeo blanco cuando el estado de asustado está por terminar.
  /// </summary>
  private void Flash()
  {
    if (!eaten)
    {
      blue.enabled = false;
      white.enabled = true;
      white.GetComponent<AnimatedSprite>().Restart();
    }
  }

  /// <summary>
  /// Se ejecuta cuando el estado de asustado es activado.
  /// </summary>
  private void OnEnable()
  {
    blue.GetComponent<AnimatedSprite>().Restart();
    ghost.movement.speedMultiplier = 0.5f;
    eaten = false;
  }

  /// <summary>
  /// Se ejecuta cuando el estado de asustado es desactivado.
  /// </summary>
  private void OnDisable()
  {
    ghost.movement.speedMultiplier = 1f;
    eaten = false;
  }

  /// <summary>
  /// Cuando el fantasma asustado colisiona con un nodo, elige la dirección más alejada de Pacman.
  /// </summary>
  /// <param name="other">Colisionador que activó el evento.</param>
  private void OnTriggerEnter2D(Collider2D other)
  {
    Node node = other.GetComponent<Node>();

    if (node != null && enabled)
    {
      Vector2 direction = Vector2.zero;
      float maxDistance = float.MinValue;

      // Encuentra la dirección disponible más lejana de Pacman
      foreach (Vector2 availableDirection in node.availableDirections)
      {
        // Calcula la distancia en esta dirección y compara con la máxima actual
        Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
        float distance = (ghost.target.position - newPosition).sqrMagnitude;

        if (distance > maxDistance)
        {
          direction = availableDirection;
          maxDistance = distance;
        }
      }

      // Establece la nueva dirección de movimiento del fantasma
      ghost.movement.SetDirection(direction);
    }
  }

  /// <summary>
  /// Maneja la colisión con Pacman para determinar si debe ser comido.
  /// </summary>
  /// <param name="collision">Información de la colisión detectada.</param>
  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
    {
      if (enabled)
      {
        Eaten();
      }
    }
  }
}
