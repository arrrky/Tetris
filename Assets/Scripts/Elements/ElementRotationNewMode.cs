﻿public class ElementRotationNewMode : ElementRotation, IElementRotation
{
    protected override bool IsRotateValid()
    {
        if (IsElementDivided())
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

                if (playingFieldController.Field.Matrix[y + yShift, x + xShift] == FieldState.Moving)
                {
                    currentElementMatrixOnTheField[y, x] = playingFieldController.Field.Matrix[y + yShift, x + xShift];
                }
            }
        }
        return true;
    }

    private bool IsElementDivided()
    {
        return playingFieldController.TopLeftPositionOfCurrentElement.x < 0
            || playingFieldController.TopLeftPositionOfCurrentElement.x > playingFieldController.Width - playingFieldController.CurrentElementSize;
    }
}