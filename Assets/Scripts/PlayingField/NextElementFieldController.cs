using UnityEngine;
using MiscTools;

public class NextElementFieldController : PlayingFieldController
{
    [SerializeField] private Elements elements;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private GameObject nextFieldBorderBlockPrefab;
    [SerializeField] private GameObject nextFieldBorderBlocksParent;

    private Border nextElementBorder;    

    public Field nextElementField;
    public Element nextElement;

    private const int nextElementFieldHeight = 4;
    private const int nextElementFieldWidth = 4;

    private int nextElementBorderXShift; // смещение относительно центра экрана
    private int nextElementBorderSize;

    private void Start()
    {        
        NextElementBorderInit();
        NextElementFieldInit();

        nextElement = elements.GetRandomElement();
        spawnManager.SpawnElement(nextElement.Matrix, nextElementField);
        UpdateThePlayingField(nextElementField);
    }

    private void NextElementBorderInit()
    {
        nextElementBorderXShift = PLAYING_FIELD_WIDTH / 2 + 3;
        nextElementBorderSize = 8;
        nextElementBorder = gameObject.AddComponent(typeof(Border)) as Border;
        nextElementBorder.SpriteShift = Tools.GetSpriteShift(nextFieldBorderBlockPrefab);
        nextElementBorder.TopLeftPoint = new Vector2(nextElementBorderXShift, 0);
        nextElementBorder.CreateBorder(nextElementBorderSize, nextElementBorderSize, nextFieldBorderBlockPrefab, nextFieldBorderBlocksParent);
    }

    private void NextElementFieldInit()
    {
        int nextElementFieldXShift = (int)nextElementBorder.TopLeftPoint.x + 3;
        int nextElementFieldYShift = (int)nextElementBorder.TopLeftPoint.y - 7;

        nextElementField = gameObject.AddComponent(typeof(Field)) as Field;
        nextElementField.Height = nextElementFieldHeight;
        nextElementField.Width = nextElementFieldWidth;
        nextElementField.Matrix = new FieldState[nextElementFieldHeight, nextElementFieldWidth];
        nextElementField.Objects = new GameObject[nextElementFieldHeight, nextElementFieldWidth];
        FillTheField(nextElementField, nextElementFieldXShift, nextElementFieldYShift);
        UpdateThePlayingField(nextElementField);
    }    
}
