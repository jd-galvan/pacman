using UnityEngine;

/// <summary>
/// Clase abstracta que define el comportamiento base de un fantasma.
/// Permite habilitar y deshabilitar un comportamiento con una duración específica.
/// </summary>
[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour
{
  /// <summary>
  /// Referencia al fantasma asociado a este comportamiento.
  /// </summary>
  public Ghost ghost { get; private set; }

  /// <summary>
  /// Duración del comportamiento antes de deshabilitarse automáticamente.
  /// </summary>
  public float duration;

  /// <summary>
  /// Inicializa la referencia al fantasma cuando el script se despierta.
  /// </summary>
  private void Awake()
  {
    ghost = GetComponent<Ghost>();
  }

  /// <summary>
  /// Habilita el comportamiento usando la duración predefinida.
  /// </summary>
  public void Enable()
  {
    Enable(duration);
  }

  /// <summary>
  /// Habilita el comportamiento por un tiempo determinado y programa su desactivación.
  /// </summary>
  /// <param name="duration">Tiempo en segundos antes de deshabilitar el comportamiento.</param>
  public virtual void Enable(float duration)
  {
    enabled = true;
    CancelInvoke();
    Invoke(nameof(Disable), duration);
  }

  /// <summary>
  /// Deshabilita el comportamiento y cancela cualquier invocación pendiente.
  /// </summary>
  public virtual void Disable()
  {
    enabled = false;
    CancelInvoke();
  }
}
