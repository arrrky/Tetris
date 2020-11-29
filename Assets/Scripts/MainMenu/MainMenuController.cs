using UnityEngine;
using MiscTools;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuBorderBlockPrefab;
    [SerializeField] private GameObject mainMenuBorderBlocksParent;

    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private GameObject LogOutButton;

    [SerializeField] private GameObject RegisterInputFields;
    [SerializeField] private GameObject LoginInputFields;

    [SerializeField] private Text CurrentPlayerProfile;

    private LoginManager loginManager;

    private Border mainMenuBorder;
    private Vector2 screenBounds;

    private void Start()
    {     
        UpdatePlayerDisplay();

        loginManager = LoginInputFields.GetComponentInChildren<LoginManager>();
        loginManager.PlayerLoggedIn += UpdatePlayerDisplay;

        Time.timeScale = 1;
        MainBorderInit();

        LogOutButton.SetActive(PlayerProfileController.Instance.playerProfile.Name != null);
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
        RegisterInputFields.SetActive(true);
    }

    public void GoToLoginInputFuelds()
    {
        LoginInputFields.SetActive(true);
    }

    private void UpdatePlayerDisplay()
    {
        if (PlayerProfileController.Instance.playerProfile.Name == null)
        {
            CurrentPlayerProfile.text = "Player: \nunknown";
        }
        else
        {
            CurrentPlayerProfile.text =
                "Player: \n" + PlayerProfileController.Instance.playerProfile.Name + "\n" +
                "Max Level: \n" + PlayerProfileController.Instance.playerProfile.MaxLevel + "\n" +
                "Max Score: \n" + PlayerProfileController.Instance.playerProfile.MaxScore;
        }
    }

    public void LogOut()
    {       
        if (PlayerProfileController.Instance.playerProfile.Name == null)
            return;
        PlayerProfileController.Instance.playerProfile.Name = null;
        Tools.CurrentSceneReload();
    }
}
