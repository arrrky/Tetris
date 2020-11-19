using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MiscTools;

public class GameController : MonoBehaviour
{ 
    [SerializeField] private ElementMovement elementMovement;
    [SerializeField] private SpawnManager spawnManager;
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

    public const float GAME_START_TIME = 2f;
    public static Vector3 screenBounds;

    private Border mainBorder;   

    private void Start()
    {
        screenBounds = Tools.GetScreenBounds();
        MainBorderInit();
        lblGoal.text = $"Goal: {LevelController.Instance.Goal}";
        lblLevel.text = $"Level: {LevelController.Instance.Level}";
        StartCoroutine(StartTheGame());
    }    

    private void MainBorderInit()
    {
        mainBorder = gameObject.AddComponent(typeof(Border)) as Border;
        mainBorder.SpriteShift = Tools.GetSpriteShift(mainBorderBlockPrefab);
        mainBorder.TopLeftPoint = new Vector2(-screenBounds.x, screenBounds.y - 1);
        mainBorder.CreateBorder(screenBounds.x * 2 - 1, screenBounds.y * 2 - 1, mainBorderBlockPrefab, mainBorderBlocksParent);
    }

    public IEnumerator GameOver()
    {
        gameOverText.SetActive(true);
        elementMovement.StopFallingDown();
        playerInput.SetActive(false);

        LevelController.Instance.Reset();

        yield return new WaitForSeconds(3f);
        Tools.LoadMainMenu();
    }

    public IEnumerator NextLevel()
    {
        youWinText.SetActive(true);
        elementMovement.StopFallingDown();
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

    private IEnumerator StartTheGame()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Tab));
        pressToStartText.SetActive(false);
        controlsText.SetActive(false);
        spawnManager.SpawnRandomElement();
    } 
    
    public void GoToMainMenu()
    {
        Tools.LoadMainMenu();
    }        
}
