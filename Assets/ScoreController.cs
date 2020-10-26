using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private static int score = 0;
    private static int scoreForOneRow = 10;

    public static int Score
    {
        get
        {
            return score;
        }

        private set
        {
            score = value;
        }
    }

    public static void IncreaseScore(int fullRowsCount)
    {
        Score += (int)Mathf.Pow(scoreForOneRow, fullRowsCount); // формула для теста!!! подумать над более интересным вариантом
        
    }

    
}
