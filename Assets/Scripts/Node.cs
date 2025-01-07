using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un nodo en el mapa del juego.
/// Determina las direcciones disponibles en las que un personaje puede moverse.
/// </summary>
public class Node : MonoBehaviour
{
  /// <summary>
  /// Capa que define los obstáculos en el mapa.
  /// </summary>
  public LayerMask obstacleLayer;

  /// <summary>
  /// Lista de direcciones disponibles desde este nodo.
  /// </summary>
  public readonly List<Vector2> availableDirections = new();

  /// <summary>
  /// Inicializa las direcciones disponibles en función de los obstáculos cercanos.
  /// </summary>
  private void Start()
  {
    availableDirections.Clear();

    // Determina si las direcciones son accesibles realizando un BoxCast
    // Si no hay colisión con un obstáculo, la dirección se agrega a la lista
    CheckAvailableDirection(Vector2.up);
    CheckAvailableDirection(Vector2.down);
    CheckAvailableDirection(Vector2.left);
    CheckAvailableDirection(Vector2.right);
  }

  /// <summary>
  /// Verifica si una dirección está libre de obstáculos.
  /// </summary>
  /// <param name="direction">Dirección a verificar.</param>
  private void CheckAvailableDirection(Vector2 direction)
  {
    RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

    // Si no se detecta colisión, se considera una dirección válida
    if (hit.collider == null)
    {
      availableDirections.Add(direction);
    }
  }
}
