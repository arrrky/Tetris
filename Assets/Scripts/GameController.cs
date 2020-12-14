using System;
using UnityEngine;
using System.Collections;
using MiscTools;

public class GameController : MonoBehaviour
{ 
    [SerializeField] private GameObject playerInput;     

    public event Action GameStarted;
    public event Action GameOver;
    public event Action NextLevel;
    public event Action<bool> GamePaused;

    public bool isGameOver = false;

    private void Start()
    {     
        StartCoroutine(StartTheGameRoutine());
    }    

    private IEnumerator StartTheGameRoutine()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Tab));       
        GameStarted?.Invoke();
    }

    public IEnumerator GameOverRoutine()
    {
        isGameOver = true;
        GameOver?.Invoke();       
        playerInput.SetActive(false);        

        LevelController.Instance.Reset();

        yield return new WaitForSeconds(3f);
        Tools.LoadScene(Scenes.MainMenu);
    }

    public IEnumerator NextLevelRoutine()
    {
        NextLevel?.Invoke();       
        playerInput.SetActive(false);

        LevelController.Instance.ChangeLevel();

        yield return new WaitForSeconds(3f);
        Tools.CurrentSceneReload();
    }

    public void GamePause()
    {
        bool isGameOnPause = Time.timeScale == 0;
        Time.timeScale = isGameOnPause ? 1 : 0;

        GamePaused?.Invoke(isGameOnPause);
    }

    public void GoToMainMenu()
    {
        Tools.LoadScene(Scenes.MainMenu);
    }       
}
