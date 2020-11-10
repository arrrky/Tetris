using UnityEngine;

public class CreateBorder : MonoBehaviour
{
    [SerializeField]
    private GameObject borderBlockPrefab;
    [SerializeField]
    private GameObject parentOfBorderBlocks;   

    private Vector3 screenBounds;

    private SpriteRenderer spriteRenderer;
    private float halfWidthOfSprite;
    private float halfHeightOfSprite;
    
    void Start()
    {
        spriteRenderer = borderBlockPrefab.GetComponent<SpriteRenderer>();

        halfHeightOfSprite = spriteRenderer.bounds.extents.y;
        halfWidthOfSprite = spriteRenderer.bounds.extents.x;

        screenBounds = GetScreenBounds();
        ScreenBordersInstantiate();     
    }   

    private void ScreenBordersInstantiate()
    {
        for (int x = -(int)screenBounds.x; x <= (int)screenBounds.x; x++)
        {
            Instantiate(borderBlockPrefab, new Vector3(x - halfWidthOfSprite, (int)screenBounds.y - halfHeightOfSprite, 0), Quaternion.identity, parentOfBorderBlocks.transform);
            Instantiate(borderBlockPrefab, new Vector3(x - halfWidthOfSprite, -(int)screenBounds.y + halfHeightOfSprite, 0), Quaternion.identity, parentOfBorderBlocks.transform);
        }

        for (int y = (int)screenBounds.y; y >= -(int)screenBounds.y; y--)
        {
            Instantiate(borderBlockPrefab, new Vector3(screenBounds.x - halfWidthOfSprite, y - halfHeightOfSprite, 0), Quaternion.identity, parentOfBorderBlocks.transform);
            Instantiate(borderBlockPrefab, new Vector3(-screenBounds.x + halfWidthOfSprite, y - halfHeightOfSprite, 0), Quaternion.identity, parentOfBorderBlocks.transform);
        }
    }

    private Vector3 GetScreenBounds()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenVector = new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(screenVector);
    }
}
