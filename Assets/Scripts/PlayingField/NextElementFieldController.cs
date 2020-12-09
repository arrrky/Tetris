using UnityEngine;
using MiscTools;

public class NextElementFieldController : PlayingFieldController
{
    [SerializeField] private Elements elements;
    [SerializeField] private SpawnController spawnController;
    [SerializeField] private BorderController borderController;

    private Field nextElementField;
    private Element nextElement;

    public static readonly int NextElementFieldHeight = 4;
    public static readonly int NextElementFieldWidth = 4;   

    #region PROPERTIES

    public Field NextElementField { get => nextElementField; set => nextElementField = value; }
    public Element NextElement { get => nextElement; set => nextElement = value; }

    #endregion

    private void Start()
    {      
        NextElementFieldInit();

        NextElement = elements.GetRandomElement();

        spawnController.SpawnElement(NextElement.Matrix, NextElementField);
        UpdatePlayingFieldState(NextElementField, NextElement.Color);
    }   

    private void NextElementFieldInit()
    {
        int nextElementFieldXShift = (int)borderController.TopLeftPointOfNextElementBorder.x + 3;
        int nextElementFieldYShift = (int)borderController.TopLeftPointOfNextElementBorder.y - 7;

        NextElementField = gameObject.AddComponent(typeof(Field)) as Field;
        NextElementField.Height = NextElementFieldHeight;
        NextElementField.Width = NextElementFieldWidth;
        NextElementField.Matrix = new FieldState[NextElementFieldHeight, NextElementFieldWidth];
        NextElementField.Objects = new GameObject[NextElementFieldHeight, NextElementFieldWidth];
        NextElementField.Sprites = new SpriteRenderer[NextElementFieldHeight, NextElementFieldWidth];
        FillTheField(NextElementField, nextElementFieldXShift, nextElementFieldYShift);
    }
}
