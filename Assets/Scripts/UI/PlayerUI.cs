using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour
{
    //this script would go on the UI object that is a child of the player prefab 
    [SerializeField] private RealPlayerMovement id;

    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

    void Start()
    {
        scoreText.text = "Player " + id.playerID + " Score: " + 0;
    }

    public void AddPlayerScore(int point)
    {
        score += point;
        scoreText.text = "Player " + id.playerID + " Score: " + score.ToString();
        if (score >= 5)
        {
            FindFirstObjectByType<UIController>().Win("Player " + id.playerID);
        }
    }
}
