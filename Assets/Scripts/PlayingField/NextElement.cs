using UnityEngine;
using MiscTools;

public class NextElement : MonoBehaviour
{
    private Field nextElementField;

    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject parentOfBlocks;

    private const int nextElementFieldHeight = 4;
    private const int nextElementFieldWidth = 4;
    private const int nextElementFieldxShift = 15;
    private const int nextElementFieldyShift = 3;

    private void Start()
    {        
        
    }

    private void NextElementFieldInit()
    {
        nextElementField = gameObject.AddComponent(typeof(Field)) as Field;
    }
}
