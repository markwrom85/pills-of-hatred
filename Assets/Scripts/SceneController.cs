using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private GameObject player1;
    private GameObject player2;

    public Transform player1Spawn;
    public Transform player2Spawn;

    [SerializeField] private Transform[] itemSpawns;
    [SerializeField] private GameObject itemPrefab;
    private int item1Spot;
    private int item2Spot;
    private int item3Spot;
    private GameObject item1;
    private GameObject item2;
    private GameObject item3;

    void Start()
    {
        player1 = Instantiate(playerPrefab, player1Spawn.position, Quaternion.identity);
        player1.GetComponentInChildren<RealPlayerMovement>().playerID = 1;
        player1.GetComponentInChildren<Renderer>().material.color = Color.cyan;

        player2 = Instantiate(playerPrefab, player2Spawn.position, Quaternion.identity);
        player2.GetComponentInChildren<RealPlayerMovement>().playerID = 2;
        player2.GetComponentInChildren<Renderer>().material.color = Color.magenta;


        ResumeGame();
    }

    void Update()
    {
        ItemSpawn();

        //checks if player has been killed, increases opposite player hate count by amount specified in PlayerCharacter
        if (!player1.GetComponentInChildren<RealPlayerMovement>().canMove)
        {
            player2.GetComponentInChildren<PlayerCharacter>().HateScoreCalc(player2.GetComponentInChildren<PlayerCharacter>().killHateIncrease);
        }
        if (!player2.GetComponentInChildren<RealPlayerMovement>().canMove)
        {
            player1.GetComponentInChildren<PlayerCharacter>().HateScoreCalc(player1.GetComponentInChildren<PlayerCharacter>().killHateIncrease);
        }
    }

    private void ItemSpawn()
    {
        //pretty bad but it works
        if (item1 == null)
        {
            item1Spot = Random.Range(0, 10);
            if ((item1Spot == item2Spot) || (item1Spot == item3Spot))
            {
                item1Spot = Random.Range(0, 10);
            }

            item1 = Instantiate(itemPrefab, itemSpawns[item1Spot]);
        }

        if (item2 == null)
        {
            item2Spot = Random.Range(0, 10);
            if ((item2Spot == item3Spot) || (item2Spot == item1Spot))
            {
                item2Spot = Random.Range(0, 10);
            }

            item2 = Instantiate(itemPrefab, itemSpawns[item2Spot]);
        }

        if (item3 == null)
        {
            item3Spot = Random.Range(0, 10);
            if ((item3Spot == item1Spot) || (item3Spot == item2Spot))
            {
                item3Spot = Random.Range(0, 10);
            }

            item3 = Instantiate(itemPrefab, itemSpawns[item3Spot]);
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
}