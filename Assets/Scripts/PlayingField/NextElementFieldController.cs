using UnityEngine;
using MiscTools;

public class NextElementFieldController : PlayingFieldController
{
    [SerializeField] private Elements elements;
    [SerializeField] private SpawnController spawnController;
    [SerializeField] private GameObject nextFieldBorderBlockPrefab;
    [SerializeField] private GameObject nextFieldBorderBlocksParent;

    private Border nextElementBorder;

    public Field nextElementField;
    public Element nextElement;

    private const int NEXT_ELEMENT_FIELD_HEIGHT = 4;
    private const int NEXT_ELEMENT_FIELD_WIDTH = 4;

    private int nextElementBorderXShift; // смещение относительно центра экрана
    private int nextElementBorderSize;

    private void Start()
    {
        NextElementBorderInit();
        NextElementFieldInit();

        nextElement = elements.GetRandomElement();

        spawnController.SpawnElement(nextElement.Matrix, nextElementField);
        UpdateThePlayingField(nextElementField, nextElement.Color);
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
        nextElementField.Height = NEXT_ELEMENT_FIELD_HEIGHT;
        nextElementField.Width = NEXT_ELEMENT_FIELD_WIDTH;
        nextElementField.Matrix = new FieldState[NEXT_ELEMENT_FIELD_HEIGHT, NEXT_ELEMENT_FIELD_WIDTH];
        nextElementField.Objects = new GameObject[NEXT_ELEMENT_FIELD_HEIGHT, NEXT_ELEMENT_FIELD_WIDTH];
        FillTheField(nextElementField, nextElementFieldXShift, nextElementFieldYShift);
    }
}
