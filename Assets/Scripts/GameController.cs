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
    private IMove elementMovement;
    private IRotate elementRotation;
    private INextElementFieldController nextElementFieldController;

    private SpawnController spawnController;

    #region PROPERTIES
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
    public IPlayingFieldController PlayingFieldController { get => playingFieldController; set => playingFieldController = value; }
    public IMove ElementMovement { get => elementMovement; set => elementMovement = value; }
    public IRotate ElementRotation { get => elementRotation; set => elementRotation = value; }
    public INextElementFieldController NextElementFieldController { get => nextElementFieldController; set => nextElementFieldController = value; }
    public SpawnController SpawnController { get => spawnController; set => spawnController = value; }
    #endregion

    private void Awake()
    {
        MainInit();

    }
    private void Start()
    {
        StartCoroutine(StartTheGameRoutine());
    }

    private void MainInit()
    {      
        ElementRotation = gameObject.AddComponent<ElementRotation>();

        ElementMovement = GameModeManager.Instance.IsFunMode
            ? gameObject.AddComponent<ElementMovementFunMode>()
            : gameObject.AddComponent<ElementMovement>();

        PlayingFieldController = GameModeManager.Instance.IsFunMode
            ? gameObject.AddComponent<PlayingFieldControllerFunMode>()
            : gameObject.AddComponent<PlayingFieldController>();

        PlayingFieldController.FieldControllerInit(blockPrefab, blocksParentForPlayingField);
        PlayingFieldController.PlayingFieldControllerInit(ElementMovement, ElementRotation);
        PlayingFieldController.FieldInit();

        NextElementFieldController = gameObject.AddComponent<NextElementFieldController>();       
        NextElementFieldController.FieldControllerInit(blockPrefab, blocksParentForNextElementField);
        NextElementFieldController.NextElementFieldControllerInit();
        NextElementFieldController.FieldInit();

        SpawnController = gameObject.AddComponent<SpawnController>();
        SpawnController.SpawnControllerInit(this, NextElementFieldController, PlayingFieldController);
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
