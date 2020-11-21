using UnityEngine;

public class LevelController : MonoBehaviour
{   
    public static LevelController Instance { get; set; }

    private const int GOAL_AT_START = 10;
    private const int LEVEL_AT_START = 1;
    private const float FALLING_DOWN_AUTO_SPEED_AT_START = 1.5f;
    private const float FALLING_DOWN_AUTO_SPEED_INCREASE = 0.15f;

    private readonly int[] goalMultipliers = new int[2] { 5, 2 };

    public int Goal { get; set; } = GOAL_AT_START;
    public int Level { get; set; } = LEVEL_AT_START;
    public float FallingDownAutoSpeed { get; set; } = FALLING_DOWN_AUTO_SPEED_AT_START;

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
        FallingDownAutoSpeed -= FALLING_DOWN_AUTO_SPEED_INCREASE;
    }

    public void Reset()
    {
        Goal = GOAL_AT_START;
        Level = LEVEL_AT_START;
        FallingDownAutoSpeed = FALLING_DOWN_AUTO_SPEED_AT_START;       
    }
}
