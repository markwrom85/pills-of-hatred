using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI player1ScoreText;
    [SerializeField] private TextMeshProUGUI player2ScoreText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private SceneController sceneController;
    private int p1Score;
    private int p2Score;

    void Start()
    {
        player1ScoreText.text = "Player 1 Score: " + 0;
        player2ScoreText.text = "Player 2 Score: " + 0;
        winPanel.SetActive(false);
    }

    public void AddPlayer1Score(int point)
    {
        p1Score += point;
        player1ScoreText.text = "Player 1 Score: " + p1Score.ToString();
        if (p1Score >= 5)
        {
            Win();
        }
    }
    public void AddPlayer2Score(int point)
    {
        p2Score += point;
        player2ScoreText.text = "Player 2 Score: " + p2Score.ToString();
        if (p2Score >= 5)
        {
            Win();
        }
    }
    private void Win()
    {
        winPanel.SetActive(true);
        winPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Player 2 Wins!";
        sceneController.PauseGame();
    }
}
