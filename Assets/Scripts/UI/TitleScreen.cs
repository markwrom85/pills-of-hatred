using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject howToScreen;

    void Start()
    {
        howToScreen.SetActive(false);
    }

    public void Playgame(){
        SceneManager.LoadScene(0);
    }
    public void ShowHowTo(){
        howToScreen.SetActive(true);
    }

    public void Back(){
        howToScreen.SetActive(false);
    }
}
