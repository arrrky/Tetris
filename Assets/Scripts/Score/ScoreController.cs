using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private ShowScore showScore;
    [SerializeField]
    private ShowGoal showGoal;
    [SerializeField]
    private GameObject youWin;
    [SerializeField]
    private ElementMovement elementMovement;
    [SerializeField]
    private GameObject playerInput;

    private int score = 0;
    public int Score
    {
        get
        {
            return score;
        }

        private set
        {
            score = value;
            showScore.UpdateScore();
        }
    }

    private int scoreForOneRow = 10;

    private int goal = 100;

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

    public void IncreaseScore(int fullRowsCount)
    {
        Score += (int)Mathf.Pow(scoreForOneRow, fullRowsCount); // формула для теста!!! подумать над более интересным вариантом   
        if (Score >= Goal)
        {
            StartCoroutine(GameOver());            
        }
    }   
    
    private IEnumerator GameOver()
    {
        youWin.SetActive(true);
        elementMovement.StopFallingDown();
        playerInput.SetActive(false);
        yield return new WaitForSeconds(3f);        
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
