using UnityEngine;

public class Border : MonoBehaviour
{
    public Vector2 SpriteShift { get; set; }
    public Vector2 TopLeftPoint { get; set; }

    /// <summary>
    /// Создает прямоугольную границу, беря за основу указанный верхний левый угол.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="borderBlockPrefab"> Префаб (в нашем случае - спрайт), который будет служить основой границы. </param>
    /// <param name="parentOfBlocks"> Объект на сцене, куда будут "складироваться" все префабы-спрайты, чтобы не мешались. </param>

    public void CreateBorder(float width, float height, GameObject borderBlockPrefab, GameObject parentOfBlocks)
    {
        for (int x = 0; x < width; x++)
        {
            Instantiate(
                borderBlockPrefab,
                new Vector3(TopLeftPoint.x + x + SpriteShift.x, TopLeftPoint.y + SpriteShift.y, 0),
                Quaternion.identity, parentOfBlocks.transform);

            Instantiate(
                borderBlockPrefab,
                new Vector3(TopLeftPoint.x + x + SpriteShift.x, TopLeftPoint.y - height + SpriteShift.y, 0),
                Quaternion.identity, parentOfBlocks.transform);
        }

        for (int y = 0; y < height; y++)
        {
            Instantiate(
                borderBlockPrefab,
                new Vector3(TopLeftPoint.x + SpriteShift.x, TopLeftPoint.y - y + SpriteShift.y, 0),
                Quaternion.identity, parentOfBlocks.transform);
            Instantiate(
                borderBlockPrefab,
                new Vector3(TopLeftPoint.x + width + SpriteShift.x, TopLeftPoint.y - y + SpriteShift.y, 0),
                Quaternion.identity, parentOfBlocks.transform);
        }
    }    
}