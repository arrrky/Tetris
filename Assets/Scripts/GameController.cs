using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MiscTools;

public class GameController : MonoBehaviour
{
    [SerializeField] private ElementMovement elementMovement;

    [SerializeField] private GameObject playerInput;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject youWinText;
    [SerializeField] private GameObject pauseText;
    [SerializeField] private GameObject pressToStartText;
    [SerializeField] private GameObject controlsText;
    [SerializeField] private GameObject goToMainMenuButton;

    [SerializeField] private Text lblLevel;
    [SerializeField] private Text lblGoal;

    [SerializeField] private GameObject mainBorderBlockPrefab;
    [SerializeField] private GameObject mainBorderBlocksParent;

    public const float GameStartTime = 2f;
    public static Vector3 ScreenBounds { get; set; }

    private Border mainBorder;

    public event Action GameStarted;
    public event Action GameOver;
    public event Action NextLevel;

    private void Start()
    {        
        ScreenBounds = Tools.GetScreenBounds();
        MainBorderInit();

        GameModeSetup();

        StartCoroutine(StartTheGameRoutine());
    }

    private void MainBorderInit()
    {
        mainBorder = gameObject.AddComponent(typeof(Border)) as Border;
        mainBorder.SpriteShift = Tools.GetSpriteShift(mainBorderBlockPrefab);
        mainBorder.TopLeftPoint = new Vector2(-ScreenBounds.x, ScreenBounds.y - 1);
        mainBorder.CreateBorder(ScreenBounds.x * 2 - 1, ScreenBounds.y * 2 - 1, mainBorderBlockPrefab, mainBorderBlocksParent);
    }

    private IEnumerator StartTheGameRoutine()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Tab));
        pressToStartText.SetActive(false);
        controlsText.SetActive(false);
        GameStarted?.Invoke();
    }

    public IEnumerator GameOverRoutine()
    {
        GameOver?.Invoke();
        gameOverText.SetActive(true);
        playerInput.SetActive(false);

        LevelController.Instance.Reset();

        yield return new WaitForSeconds(3f);
        Tools.LoadScene(Scenes.MainMenu);
    }

    public IEnumerator NextLevelRoutine()
    {
        NextLevel?.Invoke();
        youWinText.SetActive(true);
        playerInput.SetActive(false);

        LevelController.Instance.ChangeLevel();

        yield return new WaitForSeconds(3f);
        Tools.CurrentSceneReload();
    }

    public void GamePause()
    {
        bool gameOnPause = Time.timeScale == 0;
        Time.timeScale = gameOnPause ? 1 : 0;

        pauseText.SetActive(!gameOnPause);
        controlsText.SetActive(!gameOnPause);
        goToMainMenuButton.SetActive(!gameOnPause);
    }

    public void GoToMainMenu()
    {
        Tools.LoadScene(Scenes.MainMenu);
    }   

    private void GameModeSetup()
    {
        switch(GameModeManager.Instance.ChosenGameMode)
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
}
