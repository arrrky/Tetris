using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayingFieldControllerFunMode : PlayingFieldController, IPlayingFieldController
{
    public new event Action RowDeleted;

    private List<int> rowsToDelete = new List<int>();

    public void PlayingFieldControllerFunModeInit(IMove elementMovement, IRotate elementRotation)
    {
        this.elementMovement = elementMovement;
        this.elementRotation = elementRotation;

        Height = 22;
        Width = 10;

        FieldXShift = -5.5f;
        FieldYShift = -10.5f;

        FieldInit();
    }    
}

