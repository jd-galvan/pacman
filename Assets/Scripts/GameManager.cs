using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }

  [SerializeField] private Ghost[] ghosts1;
  [SerializeField] private Ghost[] ghosts2;
  [SerializeField] private Pacman pacman1;
  [SerializeField] private Pacman pacman2;
  [SerializeField] private Transform pellets;
  [SerializeField] private Text scoreText1;
  [SerializeField] private Text scoreText2;
  [SerializeField] private Text livesText1;
  [SerializeField] private Text livesText2;

  public int score1 { get; private set; } = 0;
  public int score2 { get; private set; } = 0;
  public int lives1 { get; private set; } = 3;
  public int lives2 { get; private set; } = 3;

  private int ghostMultiplier1 = 1;
  private int ghostMultiplier2 = 1;

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

  private void OnDestroy()
  {
    if (Instance == this)
    {
      Instance = null;
    }
  }

  private void Start()
  {
    NewGame();
  }

  private void Update()
  {
    if (lives1 <= 0 && Input.anyKeyDown)
    {
      NewGame();
    }
  }

  private void NewGame()
  {
    SetScore(0, 0);
    SetLives(3, 0);
    NewRound();
  }

  private void NewRound()
  {

    foreach (Transform pellet in pellets)
    {
      pellet.gameObject.SetActive(true);
    }

    ResetStatePlayer1();
    ResetStatePlayer2();
  }

  private void ResetStatePlayer1()
  {
    for (int i = 0; i < ghosts1.Length; i++)
    {
      ghosts1[i].ResetState();
    }

    pacman1.ResetState();
  }

  private void ResetStatePlayer2()
  {
    for (int i = 0; i < ghosts2.Length; i++)
    {
      ghosts2[i].ResetState();
    }

    pacman2.ResetState();
  }

  private void GameOver()
  {
    for (int i = 0; i < ghosts1.Length; i++)
    {
      ghosts1[i].gameObject.SetActive(false);
    }

    pacman1.gameObject.SetActive(false);
    pacman2.gameObject.SetActive(false);
  }

  private void SetLives(int lives, int player)
  {
    switch (player)
    {
      case 1:
        this.lives1 = lives;
        livesText1.text = "x" + lives.ToString();
        break;
      case 2:
        this.lives2 = lives;
        livesText2.text = "x" + lives.ToString();
        break;
      default:
        this.lives1 = lives;
        livesText1.text = "x" + lives.ToString();
        this.lives2 = lives;
        livesText2.text = "x" + lives.ToString();
        break;
    }
  }

  private void SetScore(int score, int p)
  {
    switch (p)
    {
      case 1:
        this.score1 = score;
        scoreText1.text = score.ToString().PadLeft(2, '0');
        break;
      case 2:
        this.score2 = score;
        scoreText2.text = score.ToString().PadLeft(2, '0');
        break;
      default:
        this.score1 = score;
        scoreText1.text = score.ToString().PadLeft(2, '0');
        this.score2 = score;
        scoreText2.text = score.ToString().PadLeft(2, '0');
        break;
    }
  }

  public void PacmanEaten(GameObject pacman)
  {
    if (pacman.name == "Pacman_1")
    {
      pacman1.DeathSequence();
      SetLives(lives1 - 1, 1);
    }
    else if (pacman.name == "Pacman_2")
    {
      pacman2.DeathSequence();
      SetLives(lives2 - 1, 2);
    }

    if (lives1 > 0)
    {
      Invoke(nameof(ResetStatePlayer1), 3f);
    }
    else if (lives2 > 0)
    {
      Invoke(nameof(ResetStatePlayer2), 3f);
    }
    else
    {
      GameOver();
    }
  }

  public void GhostEaten(Ghost ghost, GameObject pacman)
  {

    if (pacman.name == "Pacman_1")
    {
      int points = ghost.points * ghostMultiplier1;
      SetScore(score1 + points, 1);
      ghostMultiplier1++;
    }
    else if (pacman.name == "Pacman_2")
    {
      int points = ghost.points * ghostMultiplier2;
      SetScore(score2 + points, 2);
      ghostMultiplier2++;
    }

  }

  public void PelletEaten(Pellet pellet, GameObject pacman)
  {
    pellet.gameObject.SetActive(false);

    if (pacman.name == "Pacman_1")
    {
      SetScore(score1 + pellet.points, 1);
      if (!HasRemainingPellets())
      {
        pacman1.gameObject.SetActive(false);
        Invoke(nameof(NewRound), 3f);
      }
    }
    else if (pacman.name == "Pacman_2")
    {
      SetScore(score2 + pellet.points, 2);
      if (!HasRemainingPellets())
      {
        pacman2.gameObject.SetActive(false);
        Invoke(nameof(NewRound), 3f);
      }
    }

  }

  public void PowerPelletEaten(PowerPellet pellet, GameObject pacman)
  {
    if (pacman.name == "Pacman_1")
    {
      for (int i = 0; i < ghosts1.Length; i++)
      {
        ghosts1[i].frightened.Enable(pellet.duration);
      }

      PelletEaten(pellet, pacman);
      CancelInvoke(nameof(ResetGhostMultiplier1));
      Invoke(nameof(ResetGhostMultiplier1), pellet.duration);
    }
    else if (pacman.name == "Pacman_2")
    {
      for (int i = 0; i < ghosts2.Length; i++)
      {
        ghosts2[i].frightened.Enable(pellet.duration);
      }

      PelletEaten(pellet, pacman);
      CancelInvoke(nameof(ResetGhostMultiplier2));
      Invoke(nameof(ResetGhostMultiplier2), pellet.duration);
    }
  }

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

  private void ResetGhostMultiplier1()
  {
    ghostMultiplier1 = 1;
  }

  private void ResetGhostMultiplier2()
  {
    ghostMultiplier2 = 2;
  }

}
