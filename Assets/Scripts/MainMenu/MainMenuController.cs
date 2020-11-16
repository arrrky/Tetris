using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuBorderBlockPrefab;
    [SerializeField] private GameObject mainMenuBorderBlocksParent;

    private Border mainMenuBorder;
    private Vector2 screenBounds;

    private void Start()
    {
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


}
