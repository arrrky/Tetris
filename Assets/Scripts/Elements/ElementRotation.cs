using UnityEngine;
using MiscTools;

public class ElementRotation : MonoBehaviour
{
    [SerializeField]
    private PlayingFieldManager playingFieldManager;

    private FieldState[,] currentElementMatrix; 
    private int xShift;
    private int yShift;

    private bool IsRotateValid()
    {  
        // Все матрицы элементов квадратные по дефолту
        currentElementMatrix = new FieldState[playingFieldManager.currentElementSize, playingFieldManager.currentElementSize]; 

        xShift = (int)playingFieldManager.topLeftPositionOfCurrentElement.x;
        yShift = (int)playingFieldManager.topLeftPositionOfCurrentElement.y;

        // Записываем часть поля с элементом в отдельный массив
        for (int y = 0; y < playingFieldManager.currentElementSize; y++)
        {
            for (int x = 0; x < playingFieldManager.currentElementSize; x++)
            {
                if (playingFieldManager.fieldMatrix[y + yShift, x + xShift] == FieldState.Fallen)
                    return false;

                if (playingFieldManager.fieldMatrix[y + yShift, x + xShift] == FieldState.Falling)
                    currentElementMatrix[y, x] = playingFieldManager.fieldMatrix[y + yShift, x + xShift];
            }
        }
        return true;
    }

    public void Rotate()
    {
        if (IsRotateValid())
        {
            FieldState[,] temp = new FieldState[playingFieldManager.currentElementSize, playingFieldManager.currentElementSize];

            // Меняем столбцы и строки местами, заполняя temp матрицу уже перевернутым элементом
            for (int y = 0; y < playingFieldManager.currentElementSize; y++)
                for (int x = 0; x < playingFieldManager.currentElementSize; x++)
                    temp[y, x] = currentElementMatrix[playingFieldManager.currentElementSize - 1 - x, y];

            // Записываем в базовую матрицу-поле перевернутый элемент
            for (int y = 0; y < playingFieldManager.currentElementSize; y++)
                for (int x = 0; x < playingFieldManager.currentElementSize; x++)
                    playingFieldManager.fieldMatrix[y + yShift, x + xShift] = temp[y, x];

            playingFieldManager.UpdateThePlayingField();
        }
    }
}
