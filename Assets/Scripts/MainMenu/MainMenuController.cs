using UnityEngine;
using MiscTools;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuBorderBlockPrefab;
    [SerializeField] private GameObject mainMenuBorderBlocksParent;

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
    }

    public void PlayTheGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
