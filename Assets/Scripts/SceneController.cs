using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private GameObject player1;
    private GameObject player2;


    public bool player1HateCalculated = false;
    public bool player2HateCalculated = false;

    public Transform player1Spawn;
    public Transform player2Spawn;

    [SerializeField] private Transform[] itemSpawns;
    [SerializeField] private GameObject itemPrefab;
    private int item1Spot;
    private int item2Spot;
    private int item3Spot;
    private int item4Spot;
    private GameObject item1;
    private GameObject item2;
    private GameObject item3;
    private GameObject item4;

    //UI stuff
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI timeText;
    private float timeLeft = 120;


    void Start()
    {

        player1 = Instantiate(playerPrefab, player1Spawn.position, Quaternion.identity);
        player1.GetComponentInChildren<RealPlayerMovement>().playerID = 1;
        player1.GetComponentInChildren<Renderer>().material.color = Color.cyan;

        player2 = Instantiate(playerPrefab, player2Spawn.position, Quaternion.identity);
        player2.GetComponentInChildren<RealPlayerMovement>().playerID = 2;
        player2.GetComponentInChildren<Renderer>().material.color = Color.magenta;


        //UI stuff
        winPanel.SetActive(false);
        timeText.text = timeLeft.ToString();
        StartCoroutine(Timer());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ResumeGame();
    }

    void Update()
    {
        if (timeLeft <= 0)
        {
            Debug.Log("Time done");
            Win(player1.GetComponentInChildren<PlayerUI>().score, player2.GetComponentInChildren<PlayerUI>().score);
        }

        //spawn new items when old ones are grabbed
        ItemSpawn();

        //checks if player has been killed, increases opposite player hate count by amount specified in PlayerCharacter
        if (!player1.GetComponentInChildren<RealPlayerMovement>().canMove && !player2HateCalculated)
        {
            player2.GetComponentInChildren<PlayerCharacter>().HateScoreCalc(player2.GetComponentInChildren<PlayerCharacter>().killHateIncrease);
            player2HateCalculated = true;
        }

        if (!player2.GetComponentInChildren<RealPlayerMovement>().canMove && !player1HateCalculated)
        {
            player1.GetComponentInChildren<PlayerCharacter>().HateScoreCalc(player1.GetComponentInChildren<PlayerCharacter>().killHateIncrease);
            player1HateCalculated = true;
        }
    }

    private void ItemSpawn()
    {
        //pretty bad but it works
        if (item1 == null)
        {
            item1Spot = Random.Range(0, 10);
            if ((item1Spot == item2Spot) || (item1Spot == item3Spot) || (item1Spot == item4Spot))
            {
                item1Spot = Random.Range(0, 10);
            }

            item1 = Instantiate(itemPrefab, itemSpawns[item1Spot]);
        }

        if (item2 == null)
        {
            item2Spot = Random.Range(0, 10);
            if ((item2Spot == item3Spot) || (item2Spot == item1Spot) || (item2Spot == item4Spot))
            {
                item2Spot = Random.Range(0, 10);
            }

            item2 = Instantiate(itemPrefab, itemSpawns[item2Spot]);
        }

        if (item3 == null)
        {
            item3Spot = Random.Range(0, 10);
            if ((item3Spot == item1Spot) || (item3Spot == item2Spot) || (item3Spot == item4Spot))
            {
                item3Spot = Random.Range(0, 10);
            }

            item3 = Instantiate(itemPrefab, itemSpawns[item3Spot]);
        }
        if (item4 == null)
        {
            item4Spot = Random.Range(0, 10);
            if (item4Spot == item1Spot || item4Spot == item2Spot || item4Spot == item3Spot)
            {
                item4Spot = Random.Range(0, 10);
            }

            item4 = Instantiate(itemPrefab, itemSpawns[item4Spot]);
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private IEnumerator Timer()
    {   
        while(timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            timeText.text = timeLeft.ToString();
        }
    }

    //UI Stuff
    public void Win(float p1Score, float p2Score)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (p1Score > p2Score)
        {
            winText.text = "Player 1 Wins!";
        }
        else if (p1Score < p2Score)
        {
            winText.text = "Player 2 Wins!";
        }
        else
        {
            winText.text = "Draw!";
        }

        winPanel.SetActive(true);
        //winPanel.GetComponentInChildren<TextMeshProUGUI>().text = player + " Wins!";
        PauseGame();
    }

}