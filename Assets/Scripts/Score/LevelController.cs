using UnityEngine;

public class LevelController : MonoBehaviour
{   
    public static LevelController Instance { get; set; }

    private const int GoalAtStart = 10;
    private const int LevelAtStart = 1;
    private const float FallingDownAutoSpeedAtStart = 1.5f;
    private const float FallingDownAutoSpeedIncrease = 0.15f;

    private readonly int[] goalMultipliers = new int[2] { 5, 2 };

    public int Goal { get; set; } = GoalAtStart;
    public int Level { get; set; } = LevelAtStart;
    public float FallingDownAutoSpeed { get; set; } = FallingDownAutoSpeedAtStart;

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

    public void ChangeLevel()
    {
        Level++;
        Goal *= (Level % 2 == 0) ? goalMultipliers[0] : goalMultipliers[1];
        FallingDownAutoSpeed -= FallingDownAutoSpeedIncrease;
    }

    public void Reset()
    {
        Goal = GoalAtStart;
        Level = LevelAtStart;
        FallingDownAutoSpeed = FallingDownAutoSpeedAtStart;       
    }
}
