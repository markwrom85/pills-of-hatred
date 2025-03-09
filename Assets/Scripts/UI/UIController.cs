using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    //update this script in case separated UI is possible for each player

    [SerializeField] private GameObject winPanel;
    [SerializeField] private SceneController sceneController;

    void Start()
    {
        winPanel.SetActive(false);

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

    }
    public void Win(string player)
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        winPanel.SetActive(true);
        winPanel.GetComponentInChildren<TextMeshProUGUI>().text = player + " Wins!";
        sceneController.PauseGame();
    }
}
