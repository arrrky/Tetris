using UnityEngine;
using MiscTools;

public class NextElementFieldController : PlayingFieldController
{
    [SerializeField] private Elements elements;
    [SerializeField] private SpawnController spawnController;
    [SerializeField] private GameObject nextFieldBorderBlockPrefab;
    [SerializeField] private GameObject nextFieldBorderBlocksParent;

    private Border nextElementBorder;

    private Field nextElementField;
    private Element nextElement;

    private const int NEXT_ELEMENT_FIELD_HEIGHT = 4;
    private const int NEXT_ELEMENT_FIELD_WIDTH = 4;

    private int nextElementBorderXShift; // смещение относительно центра экрана
    private int nextElementBorderSize;

    #region PROPERTIES
    public Field NextElementField { get => nextElementField; set => nextElementField = value; }
    public Element NextElement { get => nextElement; set => nextElement = value; }
    #endregion

    private void Start()
    {
        NextElementBorderInit();
        NextElementFieldInit();

        NextElement = elements.GetRandomElement();

        spawnController.SpawnElement(NextElement.Matrix, NextElementField);
        UpdatePlayingFieldState(NextElementField, NextElement.Color);
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

        NextElementField = gameObject.AddComponent(typeof(Field)) as Field;
        NextElementField.Height = NEXT_ELEMENT_FIELD_HEIGHT;
        NextElementField.Width = NEXT_ELEMENT_FIELD_WIDTH;
        NextElementField.Matrix = new FieldState[NEXT_ELEMENT_FIELD_HEIGHT, NEXT_ELEMENT_FIELD_WIDTH];
        NextElementField.Objects = new GameObject[NEXT_ELEMENT_FIELD_HEIGHT, NEXT_ELEMENT_FIELD_WIDTH];
        NextElementField.Sprites = new SpriteRenderer[NEXT_ELEMENT_FIELD_HEIGHT, NEXT_ELEMENT_FIELD_WIDTH];
        FillTheField(NextElementField, nextElementFieldXShift, nextElementFieldYShift);
    }
}
