using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, optionsMenu, confirmQuit;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        confirmQuit.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.activeInHierarchy || confirmQuit.activeInHierarchy)
            {
                mainMenu.SetActive(true);
                optionsMenu.SetActive(false);
                confirmQuit.SetActive(false);
            }
            else
            {
                QuitGame();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        confirmQuit.SetActive(false);
    }

    public void CloseOptions()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        confirmQuit.SetActive(false);
    }

    public void QuitGame()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        confirmQuit.SetActive(true);
    }

    public void ConfirmQuit() => Application.Quit();

    public void DenyQuit()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        confirmQuit.SetActive(false);
    }
}
