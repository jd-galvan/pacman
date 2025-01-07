using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla las transiciones entre escenas.
/// </summary>
public class LevelManager : MonoBehaviour
{
  /// <summary>
  /// Inicia el juego.
  /// </summary>
  public void StartButton()
  {
    SceneManager.LoadScene(1);
  }

  /// <summary>
  /// Transiciona al menu de inicio
  /// </summary>
  public void MenuButton()
  {
    SceneManager.LoadScene(0);
  }

}

