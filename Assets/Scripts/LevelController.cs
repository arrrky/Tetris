using UnityEngine;

public class LevelController : MonoBehaviour
{   
    public static LevelController Instance { get; set; }

    private const int goalAtStart = 10;
    private const int levelAtStart = 1;
    private const float fallingDownAutoSpeedAtStart = 1.5f;

    private int goal = goalAtStart;   

    public int Goal
    {
        get
        {
            return goal;
        }
        set
        {
            goal = value;
        }
    }
   
    private int level = levelAtStart;

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

    private float fallingDownAutoSpeed = fallingDownAutoSpeedAtStart;

    public float FallingDownAutoSpeed
    {
        get
        {
            return fallingDownAutoSpeed;
        }
        set
        {
            fallingDownAutoSpeed = value;
        }
    }

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

    private bool isEvenIteration = true;
    private readonly int[] goalMultipliers = new int[2] { 5, 2 };

    public void ChangeLevel()
    {
        Level++;

        if (isEvenIteration)
        {
            Goal *= goalMultipliers[0];            
        }
        else
        {
            Goal *= goalMultipliers[1];          
        }
        isEvenIteration = !isEvenIteration;

        FallingDownAutoSpeed -= 0.1f;
    }

    public void Reset()
    {
        Goal = goalAtStart;
        Level = levelAtStart;
        FallingDownAutoSpeed = fallingDownAutoSpeedAtStart;
        isEvenIteration = true;
    }
}
