using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour
{
    //this script would go on the UI object that is a child of the player prefab 
    [SerializeField] private RealPlayerMovement id;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject deathScreen;
    public float score;

    void Start()
    {
        scoreText.text = "Player " + id.playerID + " Score: " + 0;
        deathScreen.SetActive(false);
    }

    void Update()
    {
        if (id.canMove == true)
        {
            deathScreen.SetActive(false);
        }
        else
        {
            deathScreen.SetActive(true);
        }
    }

    public void AddPlayerScore(float point)
    {
        score += point;
        //converts to int
        int intScore = Mathf.RoundToInt(score);
        scoreText.text = "Player " + id.playerID + " Score: " + intScore.ToString();

        //test stuff
        /*if (score >= 5)
        {
            FindFirstObjectByType<SceneController>().Win("Player " + id.playerID);
        }*/
    }
}
