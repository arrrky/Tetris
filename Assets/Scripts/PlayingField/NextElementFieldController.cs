using UnityEngine;
using System;
using Zenject;

public class NextElementFieldController : FieldController, INextElementFieldController, IFieldController
{    
    public event Action<FieldState[,], Field> FirstElementSpawned;

    #region PROPERTIES
    public Element NextElement { get; set; }
    #endregion    

    private void Start()
    {         
        FirstElementSpawned?.Invoke(NextElement.Matrix, Field);        
        //UpdatePlayingFieldState(Field, NextElement.Color);
    }       

    public void NextElementFieldControllerInit(ElementsList elements)
    {
        Height = 4;
        Width = 4;

        FieldXShift = 12;
        FieldYShift = -7;
        
        NextElement = elements.GetRandomElement();
    }      
}
