using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private PlayingFieldController playingFieldController;
    [SerializeField] private Text lblScore;
    [SerializeField] private int scoreForOneRow = 10;

    public int Score { get; set; }

    private void Start()
    {
        lblScore.text = $"Score: {Score}";
        playingFieldController.RowDeleted += IncreaseScore;
    }

    public void IncreaseScore()
    {
        Score += (int)Mathf.Pow(scoreForOneRow, playingFieldController.FullRowsCount);
        lblScore.text = $"Score: {Score}";

        if (Score >= LevelController.Instance.Goal && GameModeManager.Instance.ChosenGameMode == GameMode.Level)
        {
            StartCoroutine(gameController.NextLevelRoutine());
        }

        if (Score > PlayerProfileController.Instance.playerProfile.MaxScore && GameModeManager.Instance.ChosenGameMode == GameMode.Score)
        {
            PlayerProfileController.Instance.playerProfile.MaxScore = Score;
        }
    }
}
