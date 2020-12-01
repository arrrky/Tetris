using UnityEngine;
using UnityEngine.UI;

public class GameModeToggleController : MonoBehaviour
{
    [SerializeField] private Toggle levelMode;
    [SerializeField] private Toggle scoreMode;

    private void Awake()
    {
        levelMode.isOn = true;
        scoreMode.isOn = false;
    }

    public void LevelModeOn()
    {
        GameModeManager.Instance.ChosenGameMode = GameMode.Level;
        scoreMode.isOn = false;
        levelMode.interactable = false;
        scoreMode.interactable = true;
    }

    public void ScoreModeOn()
    {
        GameModeManager.Instance.ChosenGameMode = GameMode.Score;
        levelMode.isOn = false;
        scoreMode.interactable = false;
        levelMode.interactable = true;
    }
}
