using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MiscTools;

public class GameController : MonoBehaviour
{ 
    [SerializeField]
    private ElementMovement elementMovement;
    [SerializeField]
    private GameObject playerInput;      
    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    private GameObject youWinText;
    [SerializeField]
    private Text lblLevel;
    [SerializeField]
    private Text lblGoal;

    public const float gameStartTime = 2f;


    private void Start()
    {       
        lblGoal.text = $"Goal: {LevelController.Instance.Goal}";
        lblLevel.text = $"Level: {LevelController.Instance.Level}";
    }

    public IEnumerator GameOver()
    {
        gameOverText.SetActive(true);
        elementMovement.StopFallingDown();
        playerInput.SetActive(false);

        LevelController.Instance.Reset();

        yield return new WaitForSeconds(3f);
        Tools.CurrentSceneReload();
    }

    public IEnumerator NextLevel()
    {
        youWinText.SetActive(true);
        elementMovement.StopFallingDown();
        playerInput.SetActive(false);

        LevelController.Instance.ChangeLevel();

        yield return new WaitForSeconds(3f);
        Tools.CurrentSceneReload();
    }
}
