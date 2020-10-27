using UnityEngine;

public class ElementRotation : MonoBehaviour
{
    [SerializeField]
    private PlayingFieldManager PlayingFieldManager;

    private int[,] currentElementMatrix; 
    private int xShift;
    private int yShift;

    private void PrepareRotate()
    {        
        currentElementMatrix = new int[PlayingFieldManager.currentElementSize, PlayingFieldManager.currentElementSize]; // все матрицы элементов квадратные по дефолту

        xShift = (int)PlayingFieldManager.topLeftPositionOfCurrentElement.x;
        yShift = (int)PlayingFieldManager.topLeftPositionOfCurrentElement.y;

        // Записываем часть поля с элементом в отдельный массив
        for (int y = 0; y < PlayingFieldManager.currentElementSize; y++)
        {
            for (int x = 0; x < PlayingFieldManager.currentElementSize; x++)
            {
                if (PlayingFieldManager.playingFieldMatrix[y + yShift, x + xShift] == (int)PlayingFieldManager.FieldState.Fallen)
                    return;

                if (PlayingFieldManager.playingFieldMatrix[y + yShift, x + xShift] == (int)PlayingFieldManager.FieldState.Falling)
                    currentElementMatrix[y, x] = PlayingFieldManager.playingFieldMatrix[y + yShift, x + xShift];
            }
        }       
    }

    public void Rotate()
    {
        PrepareRotate();
        int[,] temp = new int[PlayingFieldManager.currentElementSize, PlayingFieldManager.currentElementSize];

        // Меняем столбцы и строки местами, заполняя temp матрицу уже перевернутым элементом
        for (int y = 0; y < PlayingFieldManager.currentElementSize; y++)
            for (int x = 0; x < PlayingFieldManager.currentElementSize; x++)
                temp[y, x] = currentElementMatrix[PlayingFieldManager.currentElementSize - 1 - x, y];

        // Записываем в базовую матрицу-поле перевернутый элемент
        for (int y = 0; y < PlayingFieldManager.currentElementSize; y++)
            for (int x = 0; x < PlayingFieldManager.currentElementSize; x++)
                PlayingFieldManager.playingFieldMatrix[y + yShift, x + xShift] = temp[y, x];

        PlayingFieldManager.UpdateThePlayingField();
    }
}
