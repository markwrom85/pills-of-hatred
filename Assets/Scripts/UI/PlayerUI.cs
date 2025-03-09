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
    private int score;

    [SerializeField] private GameObject deathPanel;
    [SerializeField] private TextMeshProUGUI deathText;
    public int respawnTime = 3;

    void Start()
    {
        deathPanel.SetActive(false);
        scoreText.text = "Player " + id.playerID + " Score: " + 0;
    }

    void Update()
    {
        if (id == null)
        {
            deathPanel.SetActive(true);
            StartCoroutine(RespawnCountdown());
        }
    }

    private IEnumerator RespawnCountdown()
    {
        while (respawnTime > 0)
        {
            deathText.text = "You died! Respawning in " + respawnTime + " seconds";
            yield return new WaitForSeconds(1);
            respawnTime--;
        }
        deathPanel.SetActive(false);
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
