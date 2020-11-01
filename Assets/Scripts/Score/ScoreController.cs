using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MiscTools;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private Text lblScore;
    [SerializeField]
    private LevelController levelController;
    [SerializeField]
    private GameObject youWinText;   
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
        }
    }

    private int scoreForOneRow = 10;    

    private void Start()
    {
        lblScore.text = $"Score: {Score.ToString()}";        
    }

    private void UpdateScore()
    {
        lblScore.text = $"Score: {Score.ToString()}";
    }

    public void IncreaseScore(int fullRowsCount)
    {
        Score += (int)Mathf.Pow(scoreForOneRow, fullRowsCount);
        UpdateScore();

        if (Score >= LevelController.Instance.Goal)
        {
            StartCoroutine(NextLevel());            
        }
    }   
    
    private IEnumerator NextLevel()
    {
        youWinText.SetActive(true);
        elementMovement.StopFallingDown();
        playerInput.SetActive(false);
        LevelController.Instance.ChangeLevel();

        yield return new WaitForSeconds(3f);
        Tools.CurrentSceneReload();
    }
}
