using UnityEngine;

public class CreateBorder : MonoBehaviour
{
    [SerializeField]
    private GameObject borderBlockPrefab;
    [SerializeField]
    private GameObject parentOfMainFrame;    
    [SerializeField]
    private GameObject parentOfPlayingFieldFrame;
    [SerializeField]
    private GameObject parentOfNextElementFrame;
    [SerializeField]
    private  PlayingFieldController playingFieldController;

    private Vector3 screenBounds;

    private SpriteRenderer spriteRenderer;
    private Vector2 spriteShift;

    private const int nextElementBorderSize = 8;
    private const float nextElementFrameXShift = 4;
    public Vector2 topLeftPointOfFrame;

    void Start()
    {
        spriteRenderer = borderBlockPrefab.GetComponent<SpriteRenderer>();
        spriteShift = new Vector2(spriteRenderer.bounds.extents.x, spriteRenderer.bounds.extents.y);        

        screenBounds = GetScreenBounds();
     
        topLeftPointOfFrame = new Vector2(PlayingFieldController.playingFieldWidth / 2 + nextElementFrameXShift, 0);

        // Рамка экрана
        CreateFrame(new Vector2(-screenBounds.x -spriteShift.x + 1, screenBounds.y - spriteShift.y), screenBounds.x * 2 - 1, screenBounds.y * 2 - 1, parentOfMainFrame);
        
        // Рамка следующего элемента
        CreateFrame(new Vector2(nextElementFrameXShift + PlayingFieldController.playingFieldWidth / 2, 0), nextElementBorderSize, nextElementBorderSize, parentOfNextElementFrame);
        
        //Границы игрового поля
        CreateFrame(new Vector2(-PlayingFieldController.playingFieldWidth / 2 - spriteShift.x, screenBounds.y - spriteShift.y),
                    PlayingFieldController.playingFieldWidth + 1, PlayingFieldController.playingFieldHeight + 1, parentOfPlayingFieldFrame);
        
        
        //ScreenBordersInstantiate();
        //PlayingFieldBordersInstantiate();
        //NextElementBordersInstantiate();
    }   


    private Vector3 GetScreenBounds()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenVector = new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(screenVector);
    }

    public void CreateFrame(Vector2 topLeftPoint, float width, float height, GameObject parentOfBlocks)
    {
        for (int x = 0; x < width; x++)
        {
            Instantiate(
                borderBlockPrefab,
                new Vector3(topLeftPoint.x + x, topLeftPoint.y, 0),
                Quaternion.identity, parentOfBlocks.transform);

            Instantiate(
                borderBlockPrefab,
                new Vector3(topLeftPoint.x + x, topLeftPoint.y - height, 0),
                Quaternion.identity, parentOfBlocks.transform);
        }

        for (int y = 0; y < height; y++)
        {
            Instantiate(
                borderBlockPrefab,
                new Vector3(topLeftPoint.x, topLeftPoint.y - y, 0),
                Quaternion.identity, parentOfBlocks.transform);
            Instantiate(
                borderBlockPrefab,
                new Vector3(topLeftPoint.x + width, topLeftPoint.y - y, 0),
                Quaternion.identity, parentOfBlocks.transform);
        }
    }


    //private void ScreenBordersInstantiate()
    //{
    //    for (int x = -(int)screenBounds.x; x <= (int)screenBounds.x; x++)
    //    {
    //        Instantiate(
    //            borderBlockPrefab,
    //            new Vector3(x - spriteShift.x, (int)screenBounds.y - spriteShift.y, 0),
    //            Quaternion.identity, parentOfBorderBlocks.transform);

    //        Instantiate(
    //            borderBlockPrefab,
    //            new Vector3(x - spriteShift.x, -(int)screenBounds.y + spriteShift.y, 0),
    //            Quaternion.identity, parentOfBorderBlocks.transform);
    //    }

    //    for (int y = (int)screenBounds.y; y > -(int)screenBounds.y; y--)
    //    {
    //        Instantiate(
    //            borderBlockPrefab,
    //            new Vector3(screenBounds.x - spriteShift.x, y - spriteShift.y, 0),
    //            Quaternion.identity, parentOfBorderBlocks.transform);

    //        Instantiate(
    //            borderBlockPrefab,
    //            new Vector3(-screenBounds.x + spriteShift.x, y - spriteShift.y, 0),
    //            Quaternion.identity, parentOfBorderBlocks.transform);
    //    }
    //}

    //private void PlayingFieldBordersInstantiate()
    //{
    //    for (int y = (int)screenBounds.y; y >= -(int)screenBounds.y; y--)
    //    {
    //        Instantiate(
    //            borderBlockPrefab,
    //            new Vector3(PlayingFieldController.playingFieldWidth / 2 + spriteShift.x, y - spriteShift.y, 0),
    //            Quaternion.identity, parentOfBorderBlocks.transform);

    //        Instantiate(borderBlockPrefab,
    //            new Vector3(-PlayingFieldController.playingFieldWidth / 2 - spriteShift.x, y - spriteShift.y, 0),
    //            Quaternion.identity, parentOfBorderBlocks.transform);
    //    }
    //}

    //private void NextElementBordersInstantiate()
    //{
    //    for (int x = (int)topLeftPointOfFrame.x; x < topLeftPointOfFrame.x + nextElementBorderSize; x++)
    //    {
    //        Instantiate(
    //            borderBlockPrefab,
    //            new Vector3(x + spriteShift.x, -spriteShift.y, 0),
    //            Quaternion.identity, parentOfBorderBlocks.transform);

    //        Instantiate(
    //            borderBlockPrefab,
    //            new Vector3(x + spriteShift.x, -nextElementBorderSize - spriteShift.y, 0),
    //            Quaternion.identity, parentOfBorderBlocks.transform);
    //    }

    //    for (int y = (int)topLeftPointOfFrame.y; y >= -(int)topLeftPointOfFrame.y - nextElementBorderSize; y--)
    //    {
    //        Instantiate(
    //            borderBlockPrefab,
    //            new Vector3(topLeftPointOfFrame.x - spriteShift.x, y - spriteShift.y, 0),
    //            Quaternion.identity, parentOfBorderBlocks.transform);
    //        Instantiate(
    //            borderBlockPrefab,
    //            new Vector3(topLeftPointOfFrame.x + nextElementBorderSize - spriteShift.x, y - spriteShift.y, 0),
    //            Quaternion.identity, parentOfBorderBlocks.transform);
    //    }
    //}
}
