using UnityEngine;
using System;
using Zenject;

public class NextElementFieldController : FieldController, INextElementFieldController
{  
    private Elements elements;
    private Field nextElementField;
    private Element nextElement;    

    public event Action<FieldState[,], Field> FirstElementSpawned;

    #region PROPERTIES

    public Field NextElementField { get => nextElementField; set => nextElementField = value; }
    public Element NextElement { get => nextElement; set => nextElement = value; }

    #endregion    

    private void Start()
    { 
        FirstElementSpawned?.Invoke(NextElementField.Matrix, NextElementField);
        
        //UpdatePlayingFieldState(NextElementField, NextElement.Color);
    }   

    [Inject]
    private void ElementsInit(Elements elements)
    {
        this.elements = elements;
    }

    public void NextElementFieldControllerInit(GameObject blockPrefab, GameObject blocksParent)
    {
        Height = 4;
        Width = 4;

        NextElementFieldInit();

        NextElement = elements.GetRandomElement();
    }

    public void NextElementFieldInit()
    {
        int nextElementFieldXShift = (int)BorderController.TopLeftPointOfNextElementBorder.x + 3;
        int nextElementFieldYShift = (int)BorderController.TopLeftPointOfNextElementBorder.y - 7;

        NextElementField = new Field(Width, Height);       
        FillTheField(NextElementField, nextElementFieldXShift, nextElementFieldYShift);
        UpdatePlayingFieldState(NextElementField, NextElement.Color);
    }
}
