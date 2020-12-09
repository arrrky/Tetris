using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject youWinText;
    [SerializeField] private GameObject pauseText;
    [SerializeField] private GameObject pressToStartText;
    [SerializeField] private GameObject controlsText;
    [SerializeField] private GameObject goToMainMenuButton;

    [SerializeField] private Text lblLevel;
    [SerializeField] private Text lblGoal;

    [SerializeField] GameController gameController;
 
    void Start()
    {
        GameModeUISetup();
        gameController.GameStarted += StartTheGameUISetup;
        gameController.GameOver += GameOverUISetup;
        gameController.GamePaused += GameOnPauseUISetup;
        gameController.NextLevel += NextLevelUISetup;
    }

    private void GameModeUISetup()
    {
        switch (GameModeManager.Instance.ChosenGameMode)
        {
            case GameMode.Level:
                lblGoal.text = $"Goal: {LevelController.Instance.Goal}";
                lblLevel.text = $"Level: {LevelController.Instance.Level}";
                break;
            case GameMode.Score:
                lblGoal.text = "";
                lblLevel.text = "";
                break;
        }
    }

    private void StartTheGameUISetup()
    {
        pressToStartText.SetActive(false);
        controlsText.SetActive(false);
    }

    private void GameOverUISetup()
    {
        gameOverText.SetActive(true);        
    }

    private void NextLevelUISetup()
    {
        youWinText.SetActive(true);        
    }

    private void GameOnPauseUISetup(bool isGameOnPause)
    {
        pauseText.SetActive(!isGameOnPause);
        controlsText.SetActive(!isGameOnPause);
        goToMainMenuButton.SetActive(!isGameOnPause);
    }
}
