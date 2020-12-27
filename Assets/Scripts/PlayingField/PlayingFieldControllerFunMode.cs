using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayingFieldControllerFunMode : PlayingFieldController, IPlayingFieldController, IFieldController
{
    public new event Action RowDeleted;

    private List<int> rowsToDelete = new List<int>();

    public override void PlayingFieldControllerInit(IElementMovement elementMovement, IElementRotation elementRotation)
    {
        this.elementMovement = elementMovement;
        this.elementRotation = elementRotation;

        Height = 20;
        Width = 12;

        FieldXShift = -5.5f;
        FieldYShift = -10.5f;

        FieldInit();
    }    
}

