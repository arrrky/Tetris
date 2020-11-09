using UnityEngine;

public class CreateBorder : MonoBehaviour
{
    [SerializeField]
    private GameObject borderBlockPrefab;
    [SerializeField]
    private GameObject parentOfBorderBlocks;

    [SerializeField]
    private Vector3 topLeftPoint;
    [SerializeField]
    private Vector3 topRightPoint;
    [SerializeField]
    private Vector3 bottomLeftPoint;
    [SerializeField]
    private Vector3 bottomRightPoint;


    // Start is called before the first frame update
    void Start()
    {
        BordersInstantiate();
    }

    private void PointsInit()
    {
        // Выставил в инспекторе
    }

    private void BordersInstantiate()
    {
        for (int x = (int)topLeftPoint.x; x <= (int)topRightPoint.x; x++)
            Instantiate(borderBlockPrefab, new Vector3(x, (int)topLeftPoint.y, 0), Quaternion.identity, parentOfBorderBlocks.transform);
        for (int x = (int)topLeftPoint.x; x <= (int)topRightPoint.x; x++)
            Instantiate(borderBlockPrefab, new Vector3(x, (int)bottomLeftPoint.y, 0), Quaternion.identity, parentOfBorderBlocks.transform);

        for (int y = (int)topLeftPoint.y; y >= (int)bottomLeftPoint.y; y--)
            Instantiate(borderBlockPrefab, new Vector3(topLeftPoint.x, y, 0), Quaternion.identity, parentOfBorderBlocks.transform);
        for (int y = (int)topLeftPoint.y; y >= (int)bottomLeftPoint.y; y--)
            Instantiate(borderBlockPrefab, new Vector3(topRightPoint.x, y, 0), Quaternion.identity, parentOfBorderBlocks.transform);
    }    
}
