using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private Text lblLevel;
    [SerializeField]
    private Text lblGoal;

    private static LevelController instance;    

    public static LevelController Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private const int goalAtStart = 10;
    private const int levlelAtStart = 1;
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
   
    private int numberOfLevel = levlelAtStart;

    public int NumberOfLevel
    {
        get
        {
            return numberOfLevel;
        }
        set
        {
            numberOfLevel = value;
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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {            
            Destroy(gameObject);
        }
    }    

    public void InitializeLevel()
    {        
        lblGoal.text = $"Goal: {Goal}";
        lblLevel.text = $"Level {numberOfLevel}";
    }

    private bool isEvenIteration = true;
    private readonly int[] goalMultipliers = new int[2] { 5, 2 };

    public void ChangeLevel()
    {
        NumberOfLevel++;

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
        NumberOfLevel = levlelAtStart;
        FallingDownAutoSpeed = fallingDownAutoSpeedAtStart;
        isEvenIteration = true;
    }
}
