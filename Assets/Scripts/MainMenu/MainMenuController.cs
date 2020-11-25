using UnityEngine;
using MiscTools;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuBorderBlockPrefab;
    [SerializeField] private GameObject mainMenuBorderBlocksParent;

    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private GameObject EnterNicknameField;

    private Border mainMenuBorder;
    private Vector2 screenBounds;

    private void Start()
    {
        Time.timeScale = 1;
        MainBorderInit();
    }

    private void MainBorderInit()
    {
        screenBounds = Tools.GetScreenBounds();
        mainMenuBorder = gameObject.AddComponent(typeof(Border)) as Border;
        mainMenuBorder.SpriteShift = Tools.GetSpriteShift(mainMenuBorderBlockPrefab);
        mainMenuBorder.TopLeftPoint = new Vector2(-screenBounds.x, screenBounds.y - 1);
        mainMenuBorder.CreateBorder(screenBounds.x * 2 - 1, screenBounds.y * 2 - 1, mainMenuBorderBlockPrefab, mainMenuBorderBlocksParent);

        Debug.Log(PlayerProfileController.Instance.playerProfile.Name);
        Debug.Log(PlayerProfileController.Instance.playerProfile.MaxLevel);

    }

    public void PlayTheGame()
    {
        //SceneManager.LoadScene(1);
        PlayButton.SetActive(false);
        QuitButton.SetActive(false);
        EnterNicknameField.SetActive(true);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    public void SubmitNickname()
    {
        PlayerProfileController.Instance.playerProfile.Name = EnterNicknameField.GetComponent<InputField>().text;
        Tools.LoadScene(Scenes.Game);
    }
}
