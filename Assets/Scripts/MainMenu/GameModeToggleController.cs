using UnityEngine;
using UnityEngine.UI;

public class GameModeToggleController : MonoBehaviour
{
    [SerializeField] private Toggle levelMode;
    [SerializeField] private Toggle scoreMode;
    [SerializeField] private Toggle newMode;

    private void Start()
    {
        levelMode.isOn = true;
        levelMode.interactable = false;
        GameModeManager.Instance.IsNewMode = newMode.isOn;
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

    public void NewModeChange() => GameModeManager.Instance.IsNewMode = newMode.isOn;
}