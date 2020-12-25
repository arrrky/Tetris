using UnityEngine;
using System;
using Zenject;

public class NextElementFieldController : FieldController, INextElementFieldController, IFieldController
{  
    private Elements elements;

    public event Action<FieldState[,], Field> FirstElementSpawned;

    #region PROPERTIES
    public Element NextElement { get; set; }
    #endregion    

    private void Start()
    { 
        //FirstElementSpawned?.Invoke(NextElementField.Matrix, NextElementField);
        
        UpdatePlayingFieldState(Field, NextElement.Color);
    }   

    [Inject]
    private void ElementsInit(Elements elements)
    {
        this.elements = elements;
    }

    public void NextElementFieldControllerInit()
    {
        Height = 4;
        Width = 4;

        FieldXShift = (int)BorderController.TopLeftPointOfNextElementBorder.x + 3;
        FieldYShift = (int)BorderController.TopLeftPointOfNextElementBorder.y - 7;
        
        NextElement = elements.GetRandomElement();
    }   
}
