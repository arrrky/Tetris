using UnityEngine;

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
}
