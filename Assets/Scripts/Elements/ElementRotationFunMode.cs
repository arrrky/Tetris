using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementRotationFunMode : ElementRotation, IElementRotation
{
    protected override bool IsRotateValid()
    {
        // Не даем поворачиывать, если элемент разделен, т.е. "торчит" и с одной, и с другой стороны
        if (playingFieldController.IsElementDivided)
            return false;

        currentElementMatrixOnTheField = new FieldState[playingFieldController.CurrentElementSize, playingFieldController.CurrentElementSize];

        xShift = (int)playingFieldController.TopLeftPositionOfCurrentElement.x;
        yShift = (int)playingFieldController.TopLeftPositionOfCurrentElement.y;

        // Записываем часть поля с элементом в отдельный массив
        for (int y = 0; y < playingFieldController.CurrentElementSize; y++)
        {
            for (int x = 0; x < playingFieldController.CurrentElementSize; x++)
            {
                // Если в пределах поворота элемента (квадрат n*n, где n - самая длинная сторона элемента) есть упавшие блоки - не даем поворачивать
                if (playingFieldController.Field.Matrix[y + yShift, x + xShift] == FieldState.Fallen)
                    return false;

                if (playingFieldController.Field.Matrix[y + yShift, x + xShift] == FieldState.Falling)
                {
                    currentElementMatrixOnTheField[y, x] = playingFieldController.Field.Matrix[y + yShift, x + xShift];
                }
            }
        }
        return true;
    }    
}
