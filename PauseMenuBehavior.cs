using System.Diagnostics;

public class PauseMenuBehavior : MonoBehavior
{
    public GameObject pauseMenuPanel;
    bool isGamePaused = false;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {

            }
            else
            {
                
            }
        }
    }
    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }
    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
    }
    public void LoadMainMenu()
    {
        Debug.Log("Loading main menu scene..");
        //SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Debug.Log("Exiting the game...");
        Application.Quit();
    }
}