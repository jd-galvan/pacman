using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Administra la lógica general del juego, incluyendo puntuación, vidas y el estado de los personajes.
/// </summary>
[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }

  [SerializeField] private Ghost[] ghosts; ///< Lista de fantasmas en el juego.
  [SerializeField] private Pacman pacman; ///< Referencia a Pacman.
  [SerializeField] private Transform pellets; ///< Contenedor de las bolitas del juego.
  [SerializeField] private Text gameOverText; ///< Texto que se muestra cuando el juego termina.
  [SerializeField] private Text scoreText; ///< Texto de la puntuación.
  [SerializeField] private Text livesText; ///< Texto de las vidas restantes.

  public int score { get; private set; } = 0; ///< Puntuación actual.
  public int lives { get; private set; } = 3; ///< Cantidad de vidas restantes.

  private int ghostMultiplier = 1; ///< Multiplicador de puntos por fantasma.

  /// <summary>
  /// Se ejecuta cuando el script es cargado.
  /// </summary>
  private void Awake()
  {
    if (Instance != null)
    {
      DestroyImmediate(gameObject);
    }
    else
    {
      Instance = this;
    }
  }

  /// <summary>
  /// Se ejecuta cuando el objeto es destruido.
  /// </summary>
  private void OnDestroy()
  {
    if (Instance == this)
    {
      Instance = null;
    }
  }

  /// <summary>
  /// Inicia un nuevo juego.
  /// </summary>
  private void Start()
  {
    NewGame();
  }

  /// <summary>
  /// Configura un nuevo juego con valores iniciales.
  /// </summary>
  private void NewGame()
  {
    SetScore(0);
    SetLives(3);
    NewRound();
  }

  /// <summary>
  /// Inicia una nueva ronda.
  /// </summary>
  private void NewRound()
  {
    gameOverText.enabled = false;

    foreach (Transform pellet in pellets)
    {
      pellet.gameObject.SetActive(true);
    }

    ResetState();
  }

  /// <summary>
  /// Reinicia el estado de Pacman y los fantasmas.
  /// </summary>
  private void ResetState()
  {
    for (int i = 0; i < ghosts.Length; i++)
    {
      ghosts[i].ResetState();
    }

    pacman.ResetState();
  }

  /// <summary>
  /// Finaliza el juego y desactiva los personajes.
  /// </summary>
  private void GameOver()
  {
    gameOverText.enabled = true;

    for (int i = 0; i < ghosts.Length; i++)
    {
      ghosts[i].gameObject.SetActive(false);
    }

    pacman.gameObject.SetActive(false);
  }

  /// <summary>
  /// Establece el número de vidas restantes.
  /// </summary>
  private void SetLives(int lives)
  {
    this.lives = lives;
    livesText.text = "x" + lives.ToString();
  }

  /// <summary>
  /// Establece la puntuación actual.
  /// </summary>
  private void SetScore(int score)
  {
    this.score = score;
    scoreText.text = score.ToString().PadLeft(2, '0');
  }

  /// <summary>
  /// Método llamado cuando Pacman es comido por un fantasma.
  /// </summary>
  public void PacmanEaten()
  {
    pacman.DeathSequence();

    SetLives(lives - 1);

    if (lives > 0)
    {
      Invoke(nameof(ResetState), 3f);
    }
    else
    {
      GameOver();
    }
  }

  /// <summary>
  /// Método llamado cuando un fantasma es comido.
  /// </summary>
  public void GhostEaten(Ghost ghost)
  {
    int points = ghost.points * ghostMultiplier;
    SetScore(score + points);

    ghostMultiplier++;
  }

  /// <summary>
  /// Método llamado cuando Pacman come una bolita.
  /// </summary>
  public void PelletEaten(Pellet pellet)
  {
    pellet.gameObject.SetActive(false);

    SetScore(score + pellet.points);

    if (!HasRemainingPellets())
    {
      pacman.gameObject.SetActive(false);
      Invoke(nameof(NewRound), 3f);
    }
  }

  /// <summary>
  /// Método llamado cuando Pacman come una bolita de poder.
  /// </summary>
  public void PowerPelletEaten(PowerPellet pellet)
  {
    for (int i = 0; i < ghosts.Length; i++)
    {
      ghosts[i].frightened.Enable(pellet.duration);
    }

    PelletEaten(pellet);
    CancelInvoke(nameof(ResetGhostMultiplier));
    Invoke(nameof(ResetGhostMultiplier), pellet.duration);
  }

  /// <summary>
  /// Verifica si quedan bolitas en el escenario.
  /// </summary>
  private bool HasRemainingPellets()
  {
    foreach (Transform pellet in pellets)
    {
      if (pellet.gameObject.activeSelf)
      {
        return true;
      }
    }

    return false;
  }

  /// <summary>
  /// Reinicia el multiplicador de puntos por fantasma.
  /// </summary>
  private void ResetGhostMultiplier()
  {
    ghostMultiplier = 1;
  }
}