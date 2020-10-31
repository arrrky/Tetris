using System.Collections;
using System.Collections.Generic;
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
    
    private int goal = 10;   

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
   
    private int numberOfLevel = 1;

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
    }
}
