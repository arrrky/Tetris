using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private Text lblScore;    
    [SerializeField]
    private int scoreForOneRow = 10;

    public int Score { get; set; }

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
            StartCoroutine(gameController.NextLevel());            
        }
    }      
}
