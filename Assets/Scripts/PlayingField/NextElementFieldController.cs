using UnityEngine;
using System;
using Zenject;

public class NextElementFieldController : PlayingFieldController
{
    [SerializeField] private BorderController borderController;

    private Elements elements;
    private Field nextElementField;
    private Element nextElement;

    public static readonly int NextElementFieldHeight = 4;
    public static readonly int NextElementFieldWidth = 4;

    public event Action<FieldState[,], Field> FirstElementSpawned;

    #region PROPERTIES

    public Field NextElementField { get => nextElementField; set => nextElementField = value; }
    public Element NextElement { get => nextElement; set => nextElement = value; }

    #endregion

    private new void Start()
    {      
        NextElementFieldInit();

        NextElement = elements.GetRandomElement();

        FirstElementSpawned?.Invoke(NextElementField.Matrix, NextElementField);
        
        UpdatePlayingFieldState(NextElementField, NextElement.Color);
    }   

    [Inject]
    private void ElementsInit(Elements elements)
    {
        this.elements = elements;
    }

    private void NextElementFieldInit()
    {
        int nextElementFieldXShift = (int)borderController.TopLeftPointOfNextElementBorder.x + 3;
        int nextElementFieldYShift = (int)borderController.TopLeftPointOfNextElementBorder.y - 7;

        NextElementField = new Field
        {
            Height = NextElementFieldHeight,
            Width = NextElementFieldWidth,
            Matrix = new FieldState[NextElementFieldHeight, NextElementFieldWidth],
            Objects = new GameObject[NextElementFieldHeight, NextElementFieldWidth],
            Sprites = new SpriteRenderer[NextElementFieldHeight, NextElementFieldWidth]
        };
        FillTheField(NextElementField, nextElementFieldXShift, nextElementFieldYShift);
    }
}
