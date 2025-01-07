using UnityEngine;

/// <summary>
/// Representa una bolita (pellet) en el juego, que Pacman puede comer para obtener puntos.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
  /// <summary>
  /// Cantidad de puntos que otorga la bolita al ser comida.
  /// </summary>
  public int points = 10;

  /// <summary>
  /// Maneja la lógica cuando la bolita es comida por Pacman.
  /// </summary>
  protected virtual void Eat()
  {
    GameManager.Instance.PelletEaten(this);
  }

  /// <summary>
  /// Detecta la colisión con Pacman y activa la mecánica de consumo de la bolita.
  /// </summary>
  /// <param name="other">Colisionador del objeto que entra en contacto.</param>
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
    {
      Eat();
    }
  }
}
