using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Text lblScore;
    [SerializeField] private int scoreForOneRow = 10;

    private IPlayingFieldController playingFieldController;

    public int Score { get; set; }

    private void Awake()
    {
        playingFieldController = gameController.PlayingFieldController;
    }

    private void Start()
    {
        lblScore.text = $"Score: {Score}";
        playingFieldController.RowDeleted += IncreaseScore;
    }

    public void IncreaseScore()
    {
        Score += (int)Mathf.Pow(scoreForOneRow, playingFieldController.FullRowsCount);
        lblScore.text = $"Score: {Score}";

        //if (Score >= LevelController.Instance.Goal && GameModeManager.Instance.ChosenGameMode == GameMode.Level)
        //{
        //    StartCoroutine(gameController.NextLevelRoutine());
        //}

        if (Score > PlayerProfileController.Instance.PlayerProfile.MaxScore && GameModeManager.Instance.ChosenGameMode == GameMode.Score)
        {
            PlayerProfileController.Instance.PlayerProfile.MaxScore = Score;
        }
    }
}
