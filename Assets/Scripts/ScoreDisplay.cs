using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private void Start()
    {
        scoreText.text = "Final Score: " + GameManager.finalScore.ToString();
    }
}
