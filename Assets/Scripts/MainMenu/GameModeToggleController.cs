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
        Debug.Log($"Fun mode: {GameModeManager.Instance.IsFunMode}");

    }
}
