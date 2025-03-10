using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private GameObject enemy1;
    private GameObject enemy2;
    [SerializeField] private Transform player1Spawn;
    [SerializeField] private Transform player2Spawn;

    void Start()
    {
        GameObject player1 = Instantiate(playerPrefab, player1Spawn.position, Quaternion.identity);
        player1.GetComponentInChildren<RealPlayerMovement>().playerID = 1;
        player1.GetComponentInChildren<Renderer>().material.color = Color.cyan;

        GameObject player2 = Instantiate(playerPrefab, player2Spawn.position, Quaternion.identity);
        player2.GetComponentInChildren<RealPlayerMovement>().playerID = 2;
        player2.GetComponentInChildren<Renderer>().material.color = Color.magenta;


        ResumeGame();
    }

    void Update()
    {
        /*if (enemy1 == null)
        {
            enemy1 = Instantiate(enemyPrefab, new Vector3(5, 1.25f, 0), Quaternion.identity) as GameObject;
            OnEnemySpawn(enemy1);
        }
        if (enemy2 == null)
        {
            enemy2 = Instantiate(enemyPrefab, new Vector3(5, 1.25f, 0), Quaternion.identity) as GameObject;
            OnEnemySpawn(enemy2);
        }*/
    }

    private void OnEnemySpawn(GameObject enemy)
    {
        enemy.transform.position = new Vector3(0, 1, 0);
        float angle = Random.Range(0, 360);
        enemy.transform.Rotate(0, angle, 0);
        /*float height = Random.Range(1.0f, 5.0f);
        enemy.transform.localScale = new Vector3(1, height, 1);
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        Renderer renderer = enemy.GetComponentInChildren<Renderer>();
        renderer.material.color = randomColor;*/
    }

    public void RestartGame(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void PauseGame(){
        Time.timeScale = 0;
    }
    public void ResumeGame(){
        Time.timeScale = 1;
    }
}