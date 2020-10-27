using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int score = 0;
    private int scoreForOneRow = 10;

    public int Score
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

    public void IncreaseScore(int fullRowsCount)
    {
        Score += (int)Mathf.Pow(scoreForOneRow, fullRowsCount); // формула для теста!!! подумать над более интересным вариантом        
    }    
}
