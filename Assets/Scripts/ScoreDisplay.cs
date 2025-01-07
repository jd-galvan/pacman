using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Representa la pantalla de score final.
/// </summary>
public class ScoreDisplay : MonoBehaviour
{
  [SerializeField] private Text scoreText;

  /// <summary>
  /// Muestra score final.
  /// </summary>
  private void Start()
  {
    scoreText.text = "Final Score: " + GameManager.finalScore.ToString();
  }
}
