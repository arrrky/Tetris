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

    private const int NextElementFieldHeight = 4;
    private const int NextElementFieldWidth = 4;

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
        nextElementBorderXShift = PlayingFieldWIdth / 2 + 3;
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
        NextElementField.Height = NextElementFieldHeight;
        NextElementField.Width = NextElementFieldWidth;
        NextElementField.Matrix = new FieldState[NextElementFieldHeight, NextElementFieldWidth];
        NextElementField.Objects = new GameObject[NextElementFieldHeight, NextElementFieldWidth];
        NextElementField.Sprites = new SpriteRenderer[NextElementFieldHeight, NextElementFieldWidth];
        FillTheField(NextElementField, nextElementFieldXShift, nextElementFieldYShift);
    }
}
