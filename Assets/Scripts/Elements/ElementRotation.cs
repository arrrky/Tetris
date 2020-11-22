using UnityEngine;

public class ElementRotation : MonoBehaviour
{    
    [SerializeField] private PlayingFieldController playingFieldController;

    private Field playingField;
    private FieldState[,] currentElementMatrixOnTheField; 
    private int xShift;
    private int yShift;

    private void Start()
    {
        playingField = playingFieldController.PlayingField;
    }

    private bool IsRotateValid()
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
                if (playingField.Matrix[y + yShift, x + xShift] == FieldState.Fallen)
                    return false;

                if (playingField.Matrix[y + yShift, x + xShift] == FieldState.Falling)
                    currentElementMatrixOnTheField[y, x] = playingField.Matrix[y + yShift, x + xShift];
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
                for (int x = 0; x < playingFieldController.CurrentElementSize; x++)
                    temp[y, x] = currentElementMatrixOnTheField[playingFieldController.CurrentElementSize - 1 - x, y];

            // Записываем в базовую матрицу-поле перевернутый элемент
            for (int y = 0; y < playingFieldController.CurrentElementSize; y++)
                for (int x = 0; x < playingFieldController.CurrentElementSize; x++)
                    playingField.Matrix[y + yShift, x + xShift] = temp[y, x];

            playingFieldController.UpdatePlayingFieldState(playingField, playingFieldController.CurrentElementColor);
        }
    }
}
