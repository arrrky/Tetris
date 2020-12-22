using UnityEngine;
using UnityEngine.UI;

public class GameModeToggleController : MonoBehaviour
{
    [SerializeField] private Toggle levelMode;
    [SerializeField] private Toggle scoreMode;
    [SerializeField] private Toggle funMode;

    private void Start()
    {
        levelMode.isOn = true;
        levelMode.interactable = false;
        GameModeManager.Instance.IsFunMode = funMode.isOn;
    }

    public void LevelModeOn()
    {
        GameModeManager.Instance.ChosenGameMode = GameMode.Level;
        scoreMode.interactable = true;
        levelMode.interactable = false;
    }

    public void ScoreModeOn()
    {
        GameModeManager.Instance.ChosenGameMode = GameMode.Score;
        levelMode.interactable = true;
        scoreMode.interactable = false;
    }

    public void FunModeChange() => GameModeManager.Instance.IsFunMode = funMode.isOn;
}
