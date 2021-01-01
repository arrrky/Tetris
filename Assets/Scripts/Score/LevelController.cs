using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{   
    public static LevelController Instance { get; set; }

    private const int GoalAtStart = 10;
    private const int LevelAtStart = 1;
    private const float FallingDownAutoSpeedAtStart = 1.5f;    
    private const float FallingDownAutoSpeedLerpRatio = 0.02f;

    [SerializeField] private float fallingDownAutoSpeed;
    private readonly int[] goalMultipliers = new int[2] { 5, 2 };

    public int Goal { get; set; } = GoalAtStart;
    public int Level { get; set; } = LevelAtStart;
    public float FallingDownAutoSpeed { get => fallingDownAutoSpeed; set => fallingDownAutoSpeed = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {            
            Destroy(gameObject);
        }        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ChangeAutoFallingSpeedToDefault;
    }

    private void ChangeAutoFallingSpeedToDefault(Scene scene, LoadSceneMode mode)
    {
        FallingDownAutoSpeed = FallingDownAutoSpeedAtStart;

        if (scene.name == Scenes.MainMenu.ToString())
        {
            Reset();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ChangeAutoFallingSpeedToDefault;
    }

    public void ChangeLevel()
    {
        // Записываем в профиль игрока новый лучший результат
        if (PlayerProfileController.Instance.PlayerProfile.MaxLevel < Level)
        {
            PlayerProfileController.Instance.PlayerProfile.MaxLevel = Level;
        }        

        Level++;
        Goal *= (Level % 2 == 0) ? goalMultipliers[0] : goalMultipliers[1];        
    }

    public void Reset()
    {
        Goal = GoalAtStart;
        Level = LevelAtStart;            
    }

    public void IncreaseAutoFallingSpeed()
    {
        FallingDownAutoSpeed = Mathf.Lerp(FallingDownAutoSpeed, 0,FallingDownAutoSpeedLerpRatio);
    }
}
