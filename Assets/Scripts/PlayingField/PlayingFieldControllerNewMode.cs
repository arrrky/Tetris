using UnityEngine;

public class PlayingFieldControllerNewMode : PlayingFieldController, IPlayingFieldController, IFieldController
{    
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

    public override void PlayingFieldControllerInit(GameController gameController, IElementMovement elementMovement, IElementRotation elementRotation)
    {
        this.gameController = gameController;        
        this.elementMovement = elementMovement;
        this.elementRotation = elementRotation;

        Height = 20;
        Width = 12;

        FieldXShift = -5.5f;
        FieldYShift = -10.5f;

        FieldInit();

        rowsCountToInitDeleting = 2;
    }
}