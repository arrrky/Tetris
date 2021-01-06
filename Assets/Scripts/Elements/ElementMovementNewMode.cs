using UnityEngine;

public class ElementMovementNewMode : ElementMovement, IElementMovement
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
                if (playingField.Matrix[y, x] == FieldState.Moving)
                {                   
                    if (BorderCheck(x))
                    {
                        // Проверка, чтобы элемент не проходил сквозь упавшие элементы с другой стороны стены
                        if (IsOtherBlockBehindTheWall(x, y, direction, nearBorderShift))
                            return;

                        tempMatrix[y, x + direction + nearBorderShift] = FieldState.Moving;                        
                    }
                    else
                    {
                        if (IsOtherBlockNear(x, y, direction))
                            return;

                        tempMatrix[y, x + direction] = FieldState.Moving;                       
                    }                        
                }
            }
        }
        OnElementMoved(tempMatrix, new Vector2(direction, 0));    
    }

    private bool IsOtherBlockBehindTheWall(int x, int y, int direction, int nearBorderShift)
    {
        return playingField.Matrix[y, x + direction + nearBorderShift] == FieldState.Fallen;
    }
}
