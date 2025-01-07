using UnityEngine;

/// <summary>
/// Comportamiento de persecución de un fantasma. 
/// Cuando está activo, el fantasma se mueve en dirección a Pacman.
/// </summary>
public class GhostChase : GhostBehavior
{
  /// <summary>
  /// Cuando el comportamiento de persecución se desactiva, activa el comportamiento de dispersión.
  /// </summary>
  private void OnDisable()
  {
    ghost.scatter.Enable();
  }

  /// <summary>
  /// Detecta la colisión con un nodo y determina la mejor dirección para perseguir a Pacman.
  /// </summary>
  /// <param name="other">Colisionador que activó el evento.</param>
  private void OnTriggerEnter2D(Collider2D other)
  {
    Node node = other.GetComponent<Node>();

    // No hacer nada si el fantasma está asustado
    if (node != null && enabled && !ghost.frightened.enabled)
    {
      Vector2 direction = Vector2.zero;
      float minDistance = float.MaxValue;

      // Encuentra la dirección disponible más cercana a Pacman
      foreach (Vector2 availableDirection in node.availableDirections)
      {
        // Calcula la distancia en esta dirección y compara con la mínima actual
        Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
        float distance = (ghost.target.position - newPosition).sqrMagnitude;

        if (distance < minDistance)
        {
          direction = availableDirection;
          minDistance = distance;
        }
      }

      // Establece la nueva dirección de movimiento del fantasma
      ghost.movement.SetDirection(direction);
    }
  }
}
