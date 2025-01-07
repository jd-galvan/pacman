using UnityEngine;

/// <summary>
/// Representa una bolita de poder (Power Pellet) en el juego, que otorga habilidades especiales a Pacman al ser comida.
/// </summary>
public class PowerPellet : Pellet
{
  /// <summary>
  /// Duración del efecto especial otorgado por la bolita de poder.
  /// </summary>
  public float duration = 8f;

  /// <summary>
  /// Maneja la lógica cuando la bolita de poder es comida por Pacman.
  /// </summary>
  protected override void Eat()
  {
    GameManager.Instance.PowerPelletEaten(this);
  }
}
