using UnityEngine;
using MiscTools;

public class BorderController : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private GameObject borderBlockPrefab;

    [SerializeField] private GameObject mainBorderBlocksParent;   
    [SerializeField] private GameObject playingFiedBorderBlocksParent;   
    [SerializeField] private GameObject nextFieldBorderBlocksParent;

    private Border mainBorder;
    private Border playingFieldBorder;
    private Border nextElementBorder;

    private int nextElementBorderXShift; // смещение относительно центра экрана
    private int nextElementBorderSize;

    public static Vector2 TopLeftPointOfNextElementBorder { get; set; }

    public static Vector3 ScreenBounds { get; set; }

   
    void Start()
    {
        ScreenBounds = Tools.GetScreenBounds();
        MainBorderInit();
        PlayingFieldBorderInit();
        NextElementBorderInit();
        TopLeftPointOfNextElementBorder = nextElementBorder.TopLeftPoint;
    }

    private void MainBorderInit()
    {
        mainBorder = gameObject.AddComponent(typeof(Border)) as Border;
        mainBorder.SpriteShift = Tools.GetSpriteShift(borderBlockPrefab);
        mainBorder.TopLeftPoint = new Vector2(-ScreenBounds.x, ScreenBounds.y - 1);

        mainBorder.CreateBorder(
            ScreenBounds.x * 2 - 1,
            ScreenBounds.y * 2 - 1,
            borderBlockPrefab,
            mainBorderBlocksParent);
    }

    private void PlayingFieldBorderInit()
    {
        playingFieldBorder = gameObject.AddComponent(typeof(Border)) as Border;
        playingFieldBorder.SpriteShift = Tools.GetSpriteShift(borderBlockPrefab);
        playingFieldBorder.TopLeftPoint = new Vector2(-gameController.PlayingFieldController.Field.Width / 2 - 1, ScreenBounds.y - 1);

        playingFieldBorder.CreateBorder(
            gameController.PlayingFieldController.Field.Width + 1,
            gameController.PlayingFieldController.Field.Height + 1,
            borderBlockPrefab,
            playingFiedBorderBlocksParent);
    }

    private void NextElementBorderInit()
    {
        nextElementBorderXShift = gameController.PlayingFieldController.Field.Width / 2 + 3;
        nextElementBorderSize = 9;

        nextElementBorder = gameObject.AddComponent(typeof(Border)) as Border;
        nextElementBorder.SpriteShift = Tools.GetSpriteShift(borderBlockPrefab);
        nextElementBorder.TopLeftPoint = new Vector2(nextElementBorderXShift, 0);

        nextElementBorder.CreateBorder(
            nextElementBorderSize,
            nextElementBorderSize,
            borderBlockPrefab,
            nextFieldBorderBlocksParent);
    }
}
