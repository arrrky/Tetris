using UnityEngine;

public class CreateBorder : MonoBehaviour
{
    [SerializeField]
    private GameObject borderBlockPrefab;
    [SerializeField]
    private GameObject parentOfBorderBlocks;
    [SerializeField]
    PlayingFieldController playingFieldController;

    private Vector3 screenBounds;

    private SpriteRenderer spriteRenderer;
    private Vector2 spriteShift;
    
    void Start()
    {
        spriteRenderer = borderBlockPrefab.GetComponent<SpriteRenderer>();

        spriteShift = new Vector2(spriteRenderer.bounds.extents.x, spriteRenderer.bounds.extents.y);        

        screenBounds = GetScreenBounds();
        ScreenBordersInstantiate();
        PlayingFieldBordersInstantiate();
    }   

    private void ScreenBordersInstantiate()
    {
        for (int x = -(int)screenBounds.x; x <= (int)screenBounds.x; x++)
        {
            Instantiate(borderBlockPrefab, new Vector3(x - spriteShift.x, (int)screenBounds.y - spriteShift.y, 0), Quaternion.identity, parentOfBorderBlocks.transform);
            Instantiate(borderBlockPrefab, new Vector3(x - spriteShift.x, -(int)screenBounds.y + spriteShift.y, 0), Quaternion.identity, parentOfBorderBlocks.transform);
        }

        for (int y = (int)screenBounds.y; y > -(int)screenBounds.y; y--)
        {
            Instantiate(borderBlockPrefab, new Vector3(screenBounds.x - spriteShift.x, y - spriteShift.y, 0), Quaternion.identity, parentOfBorderBlocks.transform);
            Instantiate(borderBlockPrefab, new Vector3(-screenBounds.x + spriteShift.x, y - spriteShift.y, 0), Quaternion.identity, parentOfBorderBlocks.transform);
        }
    }

    private void PlayingFieldBordersInstantiate()
    {
        for (int y = (int)screenBounds.y; y >= -(int)screenBounds.y; y--)
        {
            Instantiate(borderBlockPrefab, new Vector3(PlayingFieldController.playingFieldWidth / 2 + spriteShift.x, y - spriteShift.y, 0), Quaternion.identity, parentOfBorderBlocks.transform);
            Instantiate(borderBlockPrefab, new Vector3(-PlayingFieldController.playingFieldWidth / 2 - spriteShift.x, y - spriteShift.y, 0), Quaternion.identity, parentOfBorderBlocks.transform);
        }
    }

    private Vector3 GetScreenBounds()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenVector = new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(screenVector);
    }
}
