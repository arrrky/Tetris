using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementMovementFunMode : ElementMovement, IElementMovement
{
    public override void HorizontalMovement()
    {
        FieldState[,] tempMatrix = new FieldState[playingField.Height, playingField.Width];
        int direction = 0; // +1 - вправо, -1 - влево
        int nearBorderShift = 0; // смещение элемента в матрице, в случае перехода через границу

        if (Input.GetButtonDown("MoveToTheRight"))
        {
            direction = 1;
            BorderCheck = IsRightBorderNear;
            nearBorderShift = -playingField.Width;
        }

        if (Input.GetButtonDown("MoveToTheLeft"))
        {
            direction = -1;
            BorderCheck = IsLeftBorderNear;
            nearBorderShift = +playingField.Width;
        }

        for (int y = playingField.Height - 1; y >= 0; y--)
        {
            for (int x = playingField.Width - 1; x >= 0; x--)
            {
                if (playingField.Matrix[y, x] == FieldState.Falling)
                {
                    Debug.Log($"X coordinate: {x}");
                    if (BorderCheck(x))
                    {                        
                        tempMatrix[y, x + direction + nearBorderShift] = FieldState.Falling;
                        playingFieldController.IsElementDivided = true;
                        Debug.Log("through borders!");
                    }
                    else
                    {
                        if (IsOtherBlockNear(x, y, direction))
                            return;

                        tempMatrix[y, x + direction] = FieldState.Falling;
                        Debug.Log("inside borders!");
                        playingFieldController.IsElementDivided = false;
                    }
                }
            }
        }
        OnElementMoved(tempMatrix, new Vector2(direction, 0));
        Debug.Log($"Is element divided: {playingFieldController.IsElementDivided}");

    }
}
