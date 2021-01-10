using System;
using UnityEngine;

public class ElementRotation : MonoBehaviour, IElementRotation
{
    protected IPlayingFieldController playingFieldController;
    protected GameController gameController;

    protected FieldState[,] currentElementMatrixOnTheField;

    protected int xShift;
    protected int yShift;

    public event Action ElementWasRotated;   

    public void ElementRotationInit(GameController gameController, IPlayingFieldController playingFieldController)
    {
        this.gameController = gameController;
        this.playingFieldController = playingFieldController;
    }

    protected virtual bool IsRotateValid()
    {
        // Все матрицы элементов квадратные по дефолту
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

    public void Rotate()
    {
        if (IsRotateValid())
        {
            FieldState[,] temp = new FieldState[playingFieldController.CurrentElementSize, playingFieldController.CurrentElementSize];

            // Меняем столбцы и строки местами, заполняя temp матрицу уже перевернутым элементом
            for (int y = 0; y < playingFieldController.CurrentElementSize; y++)
            {
                for (int x = 0; x < playingFieldController.CurrentElementSize; x++)
                {
                    temp[y, x] = currentElementMatrixOnTheField[playingFieldController.CurrentElementSize - 1 - x, y];
                }
            }

            // Записываем в базовую матрицу-поле перевернутый элемент
            for (int y = 0; y < playingFieldController.CurrentElementSize; y++)
            {
                for (int x = 0; x < playingFieldController.CurrentElementSize; x++)
                {
                    playingFieldController.Field.Matrix[y + yShift, x + xShift] = temp[y, x];
                }
            }
            OnElementWasRotated();
        }
    }

    protected void OnElementWasRotated()
    {
        ElementWasRotated?.Invoke();
    }
}