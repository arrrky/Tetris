using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Level = 0,
    Score = 1,
}

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; set; }
    public GameMode ChosenGameMode { get; set; } = GameMode.Level;
    public bool IsNewMode { get; set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Reset;
    }

    private void Reset(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == Scenes.MainMenu.ToString())
        {
            IsNewMode = false;
            ChosenGameMode = GameMode.Level;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Reset;
    }
}