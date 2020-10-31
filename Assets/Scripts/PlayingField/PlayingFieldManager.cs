using UnityEngine;
using MiscTools;

public class PlayingFieldManager : MonoBehaviour
{
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject parentOfBlocks;
    [SerializeField]
    private ScoreController scoreController;
    [SerializeField]
    private SpawnManager spawnManager;
    
    private int width = 10;
    private int height = 20;

    public int Width
    {
        get
        {
            return width;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }
    }

    private GameObject[,] playingField;
    private int fullRowsCount = 0;
    
    public FieldState[,] fieldMatrix;
    public FieldState[,] currentElementArray;
    public int currentElementSize;
    public Vector2 topLeftPositionDefault;
    public Vector2 topLeftPositionOfCurrentElement;

    void Start()
    {
        LevelController.Instance.InitializeLevel();
        fieldMatrix = new FieldState[Height, Width];
        playingField = new GameObject[Height, Width];
        FillThePlayingField();

        topLeftPositionDefault = new Vector2(SpawnManager.spawnPoint, 0);
        topLeftPositionOfCurrentElement = topLeftPositionDefault;

        spawnManager.SpawnRandomElement(fieldMatrix);
    }

    private void FillThePlayingField()
    {
        // Из-за разницы в нумерации элементов матрицы-поля и отсчета координат в Unity удобнее инициализировать объекты именно таким образом.
        // Поэтому 'y' кооордината инстанирования имеет вид height - y - 1, чтобы блоки заполнялись сверху вниз (как в матрице-поле).

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                playingField[y, x] = Instantiate(blockPrefab, new Vector3(x, height - y - 1, 0), Quaternion.identity, parentOfBlocks.transform);
            }
        }
    }

    // Тестовый метод, потом можно выпилить
    //private void CreateRandomFieldState()
    //{
    //    for (int y = 0; y < height; y++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            matrix[y, x] = Random.Range(0, 2);
    //        }
    //    }
    //}

    /// <summary>
    /// Обновление состояния игрового поля
    /// </summary>
    public void UpdateThePlayingField()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {                
                playingField[y, x].SetActive(fieldMatrix[y, x] != FieldState.Empty); 
            }
        }
    }

    public void FallingToFallen()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {                
                if (fieldMatrix[y, x] != FieldState.Empty)
                    fieldMatrix[y, x] = FieldState.Fallen;
            }
        }
        spawnManager.SpawnRandomElement(fieldMatrix);
    }

    public void FullRowCheck()
    {
        for (int y = Height - 1; y >= 0; y--)
        {
            bool isFullRow = true;
            for (int x = Width - 1; x >= 0; x--)
            {
                isFullRow &= fieldMatrix[y, x] == FieldState.Fallen;
            }
            if (isFullRow)
            {
                fullRowsCount++;
                DeleteFullRow(y);
            }            
        }
    }

    public void DeleteFullRow(int rowNumber)
    {
        for (int x = Width - 1; x >= 0; x--)
        {
            fieldMatrix[rowNumber, x] = FieldState.Empty;
        }
        // Повторная проверка на случай, если заполненых рядов несколько
        FullRowCheck(); 
        if (fullRowsCount != 0)
        {
            scoreController.IncreaseScore(fullRowsCount);
        }
        Debug.Log($"Your score: {scoreController.Score}");
        fullRowsCount = 0;

        // Смещаем вниз все элементы над уничтоженным рядом
        for (int y = rowNumber - 1; y >= 0; y--)
        {
            for (int x = Width - 1; x >= 0; x--)
            {
                // Проверка, чтобы НЕ опускать падающий элемент
                if (fieldMatrix[y, x] == FieldState.Falling)
                    return;
                fieldMatrix[y + 1, x] = fieldMatrix[y, x];
            }
        }
    }

    /// <summary>
    /// Запись упавших элементов из оригинальной матрицы во временную
    /// </summary>
    public void FallenToTemp(FieldState[,] tempMatrix)
    {
        for (int y = Height - 1; y > 0; y--)
        {
            for (int x = Width - 1; x >= 0; x--)
            {
                if (fieldMatrix[y, x] == FieldState.Fallen)
                {
                    tempMatrix[y, x] = FieldState.Fallen;
                }
            }
        }
    }
}
