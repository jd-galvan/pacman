using UnityEngine;

/// <summary>
/// Comportamiento de dispersión de un fantasma.
/// En este estado, el fantasma se mueve de manera aleatoria sin perseguir a Pacman.
/// </summary>
public class GhostScatter : GhostBehavior
{
  /// <summary>
  /// Cuando el estado de dispersión finaliza, activa el modo de persecución.
  /// </summary>
  private void OnDisable()
  {
    ghost.chase.Enable();
  }

  /// <summary>
  /// Cuando el fantasma entra en contacto con un nodo, elige una dirección aleatoria para moverse.
  /// </summary>
  /// <param name="other">Colisionador que activó el evento.</param>
  private void OnTriggerEnter2D(Collider2D other)
  {
    Node node = other.GetComponent<Node>();

    // No hacer nada si el fantasma está asustado
    if (node != null && enabled && !ghost.frightened.enabled)
    {
      // Selecciona una dirección aleatoria entre las disponibles
      int index = Random.Range(0, node.availableDirections.Count);

      // Evita que el fantasma regrese en la misma dirección
      if (node.availableDirections.Count > 1 && node.availableDirections[index] == -ghost.movement.direction)
      {
        index++;

        // Ajusta el índice si excede el límite
        if (index >= node.availableDirections.Count)
        {
          index = 0;
        }
      }

      // Establece la nueva dirección de movimiento
      ghost.movement.SetDirection(node.availableDirections[index]);
    }
  }
}
