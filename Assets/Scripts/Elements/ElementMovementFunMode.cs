using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementMovementFunMode : ElementMovement, IElementMovement
{   

    public override void HorizontalMovement()
    {
        FieldState[,] tempMatrix = new FieldState[playingField.Height, playingField.Width];
        int direction = 0;

        if (Input.GetButtonDown("MoveToTheRight"))
        {
            direction = 1;
            BorderCheck = IsRightBorderNear;
        }
        if (Input.GetButtonDown("MoveToTheLeft"))
        {
            direction = -1;
            BorderCheck = IsLeftBorderNear;
        }

        for (int y = playingField.Height - 1; y >= 0; y--)
        {
            for (int x = playingField.Width - 1; x >= 0; x--)
            {
                if (playingField.Matrix[y, x] == FieldState.Falling)
                {
                    if (BorderCheck(x))
                        return;

                    if (IsOtherBlockNear(x, y, direction))
                        return;

                    tempMatrix[y, x + direction] = FieldState.Falling;
                }
            }
        }
        OnElementMoved(tempMatrix, new Vector2(direction, 0));
    }
}
