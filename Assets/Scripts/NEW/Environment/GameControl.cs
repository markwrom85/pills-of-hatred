using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private GameObject playerPrefab;
    private GameObject player1;
    private GameObject player2;
    public Transform player1Spawn;
    public Transform player2Spawn;

    [Header("Item Components")]
    [SerializeField] private Transform[] itemSpawns;
    [SerializeField] private GameObject itemPrefab;
    private GameObject[] itemObjects = new GameObject[4];

    [Header("UI Components")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI timeText;
    private float timeLeft = 120;

    private List<int> availableSpots;

    void Start()
    {
        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SpawnPlayers();
        StartCoroutine(Timer());

        // Initialize available spots list
        availableSpots = new List<int>();
        for (int i = 0; i < itemSpawns.Length; i++)
        {
            availableSpots.Add(i);
        }

        // Randomize item spawn locations
        SpawnItems();

        // Make sure the game runs normally when started
        Time.timeScale = 1;
    }

    void Update()
    {
        if (timeLeft <= 0)
        {
            Debug.Log("Time done");
            Win(player1.GetComponentInChildren<PlayerUI>().score, player2.GetComponentInChildren<PlayerUI>().score);
        }
    }

    private void SpawnPlayers()
    {
        // Player 1 and 2 are separate with their own IDs
        player1 = Instantiate(playerPrefab, player1Spawn.position, Quaternion.identity);
        player1.GetComponentInChildren<RealPlayerMovement>().playerID = 1;
        player1.GetComponentInChildren<Renderer>().material.color = Color.cyan;

        player2 = Instantiate(playerPrefab, player2Spawn.position, Quaternion.identity);
        player2.GetComponentInChildren<RealPlayerMovement>().playerID = 2;
        player2.GetComponentInChildren<Renderer>().material.color = Color.magenta;

        // Initialize UI components
        winPanel.SetActive(false);
        timeText.text = timeLeft.ToString();
    }

    private IEnumerator Timer()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            timeText.text = timeLeft.ToString();
        }
    }

    private void SpawnItems()
    {
        for (int i = 0; i < itemObjects.Length; i++)
        {
            if (availableSpots.Count == 0)
            {
                Debug.LogWarning("Not enough spawn spots for all items.");
                break;
            }

            int randomIndex = Random.Range(0, availableSpots.Count);
            int spawnIndex = availableSpots[randomIndex];
            availableSpots.RemoveAt(randomIndex);

            itemObjects[i] = Instantiate(itemPrefab, itemSpawns[spawnIndex].position, Quaternion.identity);
            itemObjects[i].GetComponent<Item>().OnItemDestroyed += HandleItemDestroyed;
        }
    }

    private void HandleItemDestroyed(GameObject item)
    {
        // Find the index of the destroyed item
        for (int i = 0; i < itemObjects.Length; i++)
        {
            if (itemObjects[i] == item)
            {
                // Add the old spawn spot back to the available spots list
                int oldSpawnIndex = System.Array.IndexOf(itemSpawns, item.transform.parent);
                availableSpots.Add(oldSpawnIndex);

                // Destroy the old item
                Destroy(itemObjects[i]);

                // Spawn a new item in a new random spot
                if (availableSpots.Count > 0)
                {
                    int randomIndex = Random.Range(0, availableSpots.Count);
                    int newSpawnIndex = availableSpots[randomIndex];
                    availableSpots.RemoveAt(randomIndex);

                    itemObjects[i] = Instantiate(itemPrefab, itemSpawns[newSpawnIndex].position, Quaternion.identity);

                    //subscribe OnItemDestroyed in Item class to this method. Means that item doesn't need to reference this entire script
                    //instead item can just run this specific class without needing to reference the entire GameControl class
                    itemObjects[i].GetComponent<Item>().OnItemDestroyed += HandleItemDestroyed;
                }
                break;
            }
        }
    }

    private void Win(float p1Score, float p2Score)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Determine who won
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

        // Pause gameplay and show the win screen
        winPanel.SetActive(true);
        Time.timeScale = 0;
    }
}