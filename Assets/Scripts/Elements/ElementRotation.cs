using UnityEngine;
using MiscTools;

public class ElementRotation : MonoBehaviour
{
    //[SerializeField]
    //private PlayingFieldManager playingFieldManager;
    [SerializeField]
    private GameController gameController;

    private PlayingField playingFieldManager;

    private FieldState[,] currentElementMatrix; 
    private int xShift;
    private int yShift;

    private void Start()
    {
        playingFieldManager = gameController.playingField;
    }

    private bool IsRotateValid()
    {  
        // Все матрицы элементов квадратные по дефолту
        currentElementMatrix = new FieldState[gameController.currentElementSize, gameController.currentElementSize]; 

        xShift = (int)gameController.topLeftPositionOfCurrentElement.x;
        yShift = (int)gameController.topLeftPositionOfCurrentElement.y;

        // Записываем часть поля с элементом в отдельный массив
        for (int y = 0; y < gameController.currentElementSize; y++)
        {
            for (int x = 0; x < gameController.currentElementSize; x++)
            {
                if (playingFieldManager.matrix[y + yShift, x + xShift] == FieldState.Fallen)
                    return false;

                if (playingFieldManager.matrix[y + yShift, x + xShift] == FieldState.Falling)
                    currentElementMatrix[y, x] = playingFieldManager.matrix[y + yShift, x + xShift];
            }
        }
        return true;
    }

    public void Rotate()
    {
        if (IsRotateValid())
        {
            FieldState[,] temp = new FieldState[gameController.currentElementSize, gameController.currentElementSize];

            // Меняем столбцы и строки местами, заполняя temp матрицу уже перевернутым элементом
            for (int y = 0; y < gameController.currentElementSize; y++)
                for (int x = 0; x < gameController.currentElementSize; x++)
                    temp[y, x] = currentElementMatrix[gameController.currentElementSize - 1 - x, y];

            // Записываем в базовую матрицу-поле перевернутый элемент
            for (int y = 0; y < gameController.currentElementSize; y++)
                for (int x = 0; x < gameController.currentElementSize; x++)
                    playingFieldManager.matrix[y + yShift, x + xShift] = temp[y, x];

            playingFieldManager.UpdateThePlayingField();
        }
    }
}
