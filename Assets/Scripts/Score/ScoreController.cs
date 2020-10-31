using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private Text lblScore;
    [SerializeField]
    private LevelController levelController;
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
        Score += (int)Mathf.Pow(scoreForOneRow, fullRowsCount); // формула для теста!!! подумать над более интересным вариантом   
        UpdateScore();

        if (Score >= LevelController.Instance.Goal)
        {
            StartCoroutine(GameOver());            
        }
    }   
    
    private IEnumerator GameOver()
    {
        youWin.SetActive(true);
        elementMovement.StopFallingDown();
        playerInput.SetActive(false);
        LevelController.Instance.ChangeLevel();

        yield return new WaitForSeconds(3f);        
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
