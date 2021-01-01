using System;
using UnityEngine;
using System.Collections;
using MiscTools;

public class GameController : MonoBehaviour
{ 
    [SerializeField] private GameObject playerInput;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject blocksParentForPlayingField;
    [SerializeField] private GameObject blocksParentForNextElementField;

    public event Action GameStarted;
    public event Action GameOver;
    public event Action NextLevel;
    public event Action<bool> GamePaused;

    private bool isGameOver = false;    

    private IPlayingFieldController playingFieldController;
    private INextElementFieldController nextElementFieldController;
    private IElementMovement elementMovement;
    private IElementRotation elementRotation;

    private ScoreController scoreController;
    private SpawnController spawnController;
    private Elements elements;   

    #region PROPERTIES
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }    
    public IPlayingFieldController PlayingFieldController { get => playingFieldController; set => playingFieldController = value; }
    public IElementMovement ElementMovement { get => elementMovement; set => elementMovement = value; }
    public IElementRotation ElementRotation { get => elementRotation; set => elementRotation = value; }
    public INextElementFieldController NextElementFieldController { get => nextElementFieldController; set => nextElementFieldController = value; }
    public SpawnController SpawnController { get => spawnController; set => spawnController = value; }
    public Elements Elements { get => elements; set => elements = value; }
    public ScoreController ScoreController { get => scoreController; set => scoreController = value; }
    #endregion

    private void Awake()
    {
        MainInit();
    }
    
    private void MainInit()
    {
        Elements = new Elements();

        ElementMovement = GameModeManager.Instance.IsNewMode
            ? gameObject.AddComponent<ElementMovementNewMode>()
            : gameObject.AddComponent<ElementMovement>();

        ElementMovement.ElementsMovementInit(this);

        PlayingFieldController = GameModeManager.Instance.IsNewMode
            ? gameObject.AddComponent<PlayingFieldControllerNewMode>()
            : gameObject.AddComponent<PlayingFieldController>();

        ElementRotation = GameModeManager.Instance.IsNewMode
            ? gameObject.AddComponent<ElementRotationNewMode>()
            : gameObject.AddComponent<ElementRotation>();

        ElementRotation.ElementRotationInit(this, PlayingFieldController);

        PlayingFieldController.FieldControllerInit(blockPrefab, blocksParentForPlayingField);
        PlayingFieldController.PlayingFieldControllerInit(this, ElementMovement, ElementRotation);
        PlayingFieldController.FieldInit();

        NextElementFieldController = gameObject.AddComponent<NextElementFieldController>();
        NextElementFieldController.FieldControllerInit(blockPrefab, blocksParentForNextElementField);
        NextElementFieldController.NextElementFieldControllerInit(Elements);
        NextElementFieldController.FieldInit();

        SpawnController = gameObject.AddComponent<SpawnController>();
        SpawnController.SpawnControllerInit(this, NextElementFieldController, PlayingFieldController, Elements);

        ScoreController = FindObjectOfType<ScoreController>();
    }

    private void Start()
    {
        StartCoroutine(StartTheGameRoutine());
    }    

    private IEnumerator StartTheGameRoutine()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Tab));       
        GameStarted?.Invoke();
    }

    public IEnumerator GameOverRoutine()
    {
        IsGameOver = true;
        GameOver?.Invoke();       
        playerInput.SetActive(false);        

        LevelController.Instance.Reset();

        yield return new WaitForSeconds(3f);
        Tools.LoadScene(Scenes.MainMenu);
    }

    public IEnumerator NextLevelRoutine()
    {        
        NextLevel?.Invoke();       
        playerInput.SetActive(false);

        LevelController.Instance.ChangeLevel();

        yield return new WaitForSeconds(3f);
        Tools.CurrentSceneReload();
    }

    public void GamePause()
    {
        bool isGameOnPause = Time.timeScale == 0;
        Time.timeScale = isGameOnPause ? 1 : 0;

        GamePaused?.Invoke(isGameOnPause);
    }

    public void GoToMainMenu()
    {
        Tools.LoadScene(Scenes.MainMenu);
    }       
}
