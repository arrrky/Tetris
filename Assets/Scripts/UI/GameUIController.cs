﻿using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject youWinText;
    [SerializeField] private GameObject pauseText;
    [SerializeField] private GameObject pressToStartText;
    [SerializeField] private GameObject controlsText;
    [SerializeField] private GameObject goToMainMenuButton;

    [SerializeField] private Text lblLevel;
    [SerializeField] private Text lblGoal;
    [SerializeField] private Text lblControls;
 
    private void Start()
    {
        GameModeUISetup();
        EventsSetup();       
    }

    private void GameModeUISetup()
    {
        switch (GameModeManager.Instance.ChosenGameMode)
        {
            case GameMode.Level:
                lblGoal.text = $"{ChosenLanguage.Instance.currentDictionary["lblGoal"]}: {LevelController.Instance.Goal}";
                lblLevel.text = $"{ChosenLanguage.Instance.currentDictionary["lblLevel"]}: {LevelController.Instance.Level}";
                break;
            case GameMode.Score:
                lblGoal.text = "";
                lblLevel.text = "";
                break;
        }

        switch(ChosenLanguage.Instance.Language)
        {
            case Language.English:
                lblControls.text = "Controls:\nA - left\nD - right\nS - down\nSpace - fall\nR - rotate\nEsc - pause";
                break;
            case Language.Russian:
                lblControls.text = "Управление:\nA - влево\nD - вправо\nS - вниз\nSpace - падение\nR - поворот\nEsc - пауза";
                break;
        }
    }

    private void StartTheGameUISetup()
    {
        pressToStartText.SetActive(false);
        controlsText.SetActive(false);
    }

    private void GameOverUISetup()
    {
        gameOverText.SetActive(true);        
    }

    private void NextLevelUISetup()
    {
        youWinText.SetActive(true);        
    }

    private void GameOnPauseUISetup(bool isGameOnPause)
    {
        pauseText.SetActive(!isGameOnPause);
        controlsText.SetActive(!isGameOnPause);
        goToMainMenuButton.SetActive(!isGameOnPause);
    }

    private void EventsSetup()
    {
        gameController.GameStarted += StartTheGameUISetup;
        gameController.GameOver += GameOverUISetup;
        gameController.GamePaused += GameOnPauseUISetup;
        gameController.NextLevel += NextLevelUISetup;
    }
}