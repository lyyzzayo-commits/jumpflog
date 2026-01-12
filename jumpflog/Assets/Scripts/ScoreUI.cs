using TMPro;
using UnityEngine;

public sealed class ScoreUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text scoreText;

    private void OnEnable()
    {
        if (gameManager != null)
            gameManager.OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        if (gameManager != null)
            gameManager.OnScoreChanged -= UpdateScore;
    }

    private void UpdateScore(float score)
    {
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }
}
