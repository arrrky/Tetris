using UnityEngine;
using UnityEngine.UI;

public class GameModeToggleController : MonoBehaviour
{
    [SerializeField] private Toggle levelMode;
    [SerializeField] private Toggle scoreMode;

    private void Start()
    {
        levelMode.isOn = true;
        levelMode.interactable = false;
        scoreMode.isOn = false;
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

    public void FunModeChoice()
    {
        GameModeManager.Instance.IsFunMode = !GameModeManager.Instance.IsFunMode;
    }
}
