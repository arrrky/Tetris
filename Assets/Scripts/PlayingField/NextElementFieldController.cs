using UnityEngine;
using MiscTools;

public class NextElementFieldController : PlayingFieldController
{
    public Field nextElementField;    

    private const int nextElementFieldHeight = 4;
    private const int nextElementFieldWidth = 4;
    private const int nextElementFieldXShift = 15;
    private const int nextElementFieldYShift = 2;

    private void Start()
    {
        NextElementFieldInit();   
    }

    private void NextElementFieldInit()
    {        
        nextElementField = gameObject.AddComponent(typeof(Field)) as Field;
        nextElementField.Height = nextElementFieldHeight;
        nextElementField.Width = nextElementFieldWidth;
        nextElementField.Matrix = new FieldState[nextElementFieldHeight, nextElementFieldWidth];
        nextElementField.Objects = new GameObject[nextElementFieldHeight, nextElementFieldWidth];
        FillTheField(nextElementField, nextElementFieldXShift, nextElementFieldYShift);
        UpdateThePlayingField(nextElementField);
    }
}
