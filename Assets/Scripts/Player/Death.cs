using System.Collections;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private MeshRenderer playerMesh;
    [SerializeField] private GameObject gunMesh;
    [SerializeField] private GameObject player;
    private PlayerCharacter playerCharacter;
    private SceneController sceneController;
    private RealPlayerMovement realPlayerMovement;

    void Start()
    {
        playerCharacter = GetComponentInChildren<PlayerCharacter>();
        realPlayerMovement = GetComponentInChildren<RealPlayerMovement>();
        sceneController = FindFirstObjectByType<SceneController>();
    }

    void Update()
    {
        if (playerCharacter.health <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {           
        //hide player object
        playerMesh.enabled = false;
        gunMesh.SetActive(false);

        //player can't move
        realPlayerMovement.canMove = false;
        
        yield return new WaitForSeconds(1f);
        if(realPlayerMovement.playerID == 1){
            player.transform.position = sceneController.player1Spawn.position;
        }else
        {
            player.transform.position = sceneController.player2Spawn.position;
        }
        playerMesh.enabled = true;
        gunMesh.SetActive(true);
        realPlayerMovement.canMove = true;
        
        //reset health and hate to the start
        playerCharacter.hateCount = 1;
        playerCharacter.health = 5;

    }
}
