using System.Collections;
using UnityEngine;

/// <summary>
/// Comportamiento del fantasma dentro de su casa.
/// Controla la transición de entrada y salida del fantasma.
/// </summary>
public class GhostHome : GhostBehavior
{
  public Transform inside; ///< Posición dentro de la casa del fantasma.
  public Transform outside; ///< Posición fuera de la casa del fantasma.

  /// <summary>
  /// Se ejecuta cuando el comportamiento se activa, deteniendo todas las corrutinas en curso.
  /// </summary>
  private void OnEnable()
  {
    StopAllCoroutines();
  }

  /// <summary>
  /// Se ejecuta cuando el comportamiento se desactiva, iniciando la salida del fantasma de la casa.
  /// </summary>
  private void OnDisable()
  {
    // Verifica si el objeto sigue activo antes de iniciar la corrutina
    if (gameObject.activeInHierarchy)
    {
      StartCoroutine(ExitTransition());
    }
  }

  /// <summary>
  /// Maneja la colisión con obstáculos para cambiar la dirección del fantasma dentro de la casa.
  /// </summary>
  /// <param name="collision">Información de la colisión detectada.</param>
  private void OnCollisionEnter2D(Collision2D collision)
  {
    // Invierte la dirección cuando choca con una pared, simulando rebote
    if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
    {
      ghost.movement.SetDirection(-ghost.movement.direction);
    }
  }

  /// <summary>
  /// Controla la transición de salida del fantasma desde la casa.
  /// </summary>
  private IEnumerator ExitTransition()
  {
    // Desactiva el movimiento automático para animar manualmente la posición
    ghost.movement.SetDirection(Vector2.up, true);
    ghost.movement.rb.isKinematic = true;
    ghost.movement.enabled = false;

    Vector3 position = transform.position;
    float duration = 0.5f;
    float elapsed = 0f;

    // Anima la entrada a la posición inicial dentro de la casa
    while (elapsed < duration)
    {
      ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
      elapsed += Time.deltaTime;
      yield return null;
    }

    elapsed = 0f;

    // Anima la salida del fantasma desde la casa
    while (elapsed < duration)
    {
      ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
      elapsed += Time.deltaTime;
      yield return null;
    }

    // Selecciona una dirección aleatoria (izquierda o derecha) y reactiva el movimiento
    ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
    ghost.movement.rb.isKinematic = false;
    ghost.movement.enabled = true;
  }
}
