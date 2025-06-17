using UnityEngine;
using UnityEngine.SceneManagement;
public class pauseSystemScript : MonoBehaviour
{
    bool isPaused = false;
    bool isStartMenu = false;
    public Canvas[] canvases;
    GameManagerScript gameManager;
    
    void Start()
    {
        gameManager=FindFirstObjectByType<GameManagerScript>();
        // Initialize the pause state
        isPaused = false;
        Time.timeScale = 1f;
        if (FindAnyObjectByType<playerController>() == null)
        {
            isStartMenu = true; // If no player controller exists, assume it's the start menu
        }

    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&isStartMenu==false)
        {
            if (isPaused == true)
            {
                ClosePauseMenu();
                TogglePause();
                print("Pause menu closed");
            }
            else if (isPaused == false)
            {
                OpenPauseMenu();
                TogglePause();
                print("Pause menu opened");
            }
        }

    }
    void OnApplicationFocus(bool focus)
    {
        if (focus == false && isStartMenu == false)
        {
            isPaused = true; // Pause the game when the application loses focus
        }
    }
    void OpenPauseMenu()
    {
        canvases[0].enabled = true; 
    }
    void ClosePauseMenu()
    {
        canvases[0].enabled = false;
        canvases[1].gameObject.SetActive(false);
        canvases[2].gameObject.SetActive(false);
        canvases[3].gameObject.SetActive(false);
        canvases[4].gameObject.SetActive(false);
        
    }
    public void ContinueBtn()
    {
        ClosePauseMenu();
    }
    public void ReloadBtn1()
    {
        canvases[3].gameObject.SetActive(true);
    }
    public void ReloadBtn2()
    {
        gameManager.loadGame();
    }
    public void RestartLvlBtn1()
    {
        canvases[4].gameObject.SetActive(true);
    }
    public void RestartLvlBtn2()
    {
        gameManager.restartLevel();
    }
    public void SettingsBtn()
    {

    }
    public void ExitToMainMenuBtn1()
    {
        canvases[2].gameObject.SetActive(true);
    }
    public void ExitToMainMenuBtn2()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitToDesktopBtn1()
    {
        canvases[1].gameObject.SetActive(true);
    }
    public void ExitToDesktopBtn2()
    {
        Application.Quit(); // Quit the application
    }
    public void BackBtn()
    {
        canvases[0].enabled = true;
        canvases[1].gameObject.SetActive(false);
        canvases[2].gameObject.SetActive(false);
        canvases[3].gameObject.SetActive(false);
        canvases[4].gameObject.SetActive(false);    
    }
}


