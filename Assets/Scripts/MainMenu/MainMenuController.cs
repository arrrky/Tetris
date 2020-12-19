using UnityEngine;
using MiscTools;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuBorderBlockPrefab;
    [SerializeField] private GameObject mainMenuBorderBlocksParent;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject logOutButton;

    [SerializeField] private GameObject registerInputFields;
    [SerializeField] private GameObject loginInputFields;

    [SerializeField] private Text currentPlayerProfile;

    private LoginManager loginManager;

    private Border mainMenuBorder;
    private Vector2 screenBounds;

    private void Start()
    {     
        UpdatePlayerDisplay();

        loginManager = loginInputFields.GetComponentInChildren<LoginManager>();
        loginManager.PlayerLoggedIn += UpdatePlayerDisplay;

        Time.timeScale = 1;
        MainBorderInit();

        logOutButton.SetActive(PlayerProfileController.Instance.PlayerProfile.Name != null);
    }

    private void MainBorderInit()
    {
        screenBounds = Tools.GetScreenBounds();
        mainMenuBorder = gameObject.AddComponent(typeof(Border)) as Border;
        mainMenuBorder.SpriteShift = Tools.GetSpriteShift(mainMenuBorderBlockPrefab);
        mainMenuBorder.TopLeftPoint = new Vector2(-screenBounds.x, screenBounds.y - 1);
        mainMenuBorder.CreateBorder(screenBounds.x * 2 - 1, screenBounds.y * 2 - 1, mainMenuBorderBlockPrefab, mainMenuBorderBlocksParent);
    }

    public void PlayTheGame()
    {
        //SceneManager.LoadScene(1);       
        Tools.LoadScene(Scenes.Game);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    public void GoToRegisterInputFields()
    {
        registerInputFields.SetActive(true);
    }

    public void GoToLoginInputFuelds()
    {
        loginInputFields.SetActive(true);
    }

    private void UpdatePlayerDisplay()
    {
        if (PlayerProfileController.Instance.PlayerProfile.Name == null)
        {
            currentPlayerProfile.text = "Player: \n" +
                                        "unknown";
        }
        else
        {
            currentPlayerProfile.text =
                "Player: \n"    + PlayerProfileController.Instance.PlayerProfile.Name + "\n" +
                "Max Level: \n" + PlayerProfileController.Instance.PlayerProfile.MaxLevel + "\n" +
                "Max Score: \n" + PlayerProfileController.Instance.PlayerProfile.MaxScore;
        }
    }

    public void LogOut()
    {       
        if (PlayerProfileController.Instance.PlayerProfile.Name == null)
            return;
        PlayerProfileController.Instance.PlayerProfile.Name = null;
        Tools.CurrentSceneReload();
    }

    public void OpenScoreboardPage()
    {
        Application.OpenURL(PlayerProfileController.linkToDB + "scoreboard.php");
    }       
}
