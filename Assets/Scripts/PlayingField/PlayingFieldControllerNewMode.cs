using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayingFieldControllerNewMode : PlayingFieldController, IPlayingFieldController, IFieldController
{
    public new event Action RowDeleted;    

    private List<int> rowsToDelete = new List<int>(); // TODO - удаления рядов

    public override Vector2 TopLeftPositionOfCurrentElement
    {
        get => base.TopLeftPositionOfCurrentElement;
        
        set
        {            
            if (value.x < 0)
            {
                topLeftPositionOfCurrentElement.x = Field.Width - 1;
                topLeftPositionOfCurrentElement.y = value.y;
            }
            else if (value.x > Field.Width - 1)
            {
                topLeftPositionOfCurrentElement.x = 0;
                topLeftPositionOfCurrentElement.y = value.y;
            }
            else
            {
                topLeftPositionOfCurrentElement = value;
            }           
        }
    }

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

