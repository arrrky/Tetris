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
        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }
        set
        {
            height = value;
        }
    }

    public GameObject[,] field;
    private int fullRowsCount = 0;
    
    public FieldState[,] matrix;
    public FieldState[,] currentElementArray;
    public int currentElementSize;
    public Vector2 topLeftPositionDefault;
    public Vector2 topLeftPositionOfCurrentElement;

    void Start()
    {
        LevelController.Instance.InitializeLevel();
        matrix = new FieldState[Height, Width];
        field = new GameObject[Height, Width];
        FillThePlayingField();

        topLeftPositionDefault = new Vector2(SpawnManager.spawnPoint, 0);
        topLeftPositionOfCurrentElement = topLeftPositionDefault;

        //spawnManager.SpawnRandomElement(matrix);           
    }

    public void FillThePlayingField()
    {
        // Из-за разницы в нумерации элементов матрицы-поля и отсчета координат в Unity удобнее инициализировать объекты именно таким образом.
        // Поэтому 'y' кооордината инстанирования имеет вид height - y - 1, чтобы блоки заполнялись сверху вниз (как в матрице-поле).

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                field[y, x] = Instantiate(blockPrefab, new Vector3(x, height - y - 1, 0), Quaternion.identity, parentOfBlocks.transform);
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
                field[y, x].SetActive(matrix[y, x] != FieldState.Empty); 
            }
        }
    }

    public void FallingToFallen()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {                
                if (matrix[y, x] != FieldState.Empty)
                    matrix[y, x] = FieldState.Fallen;
            }
        }
        //spawnManager.SpawnRandomElement(matrix);
    }

    public void FullRowCheck()
    {
        for (int y = Height - 1; y >= 0; y--)
        {
            bool isFullRow = true;
            for (int x = Width - 1; x >= 0; x--)
            {
                isFullRow &= matrix[y, x] == FieldState.Fallen;
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
            matrix[rowNumber, x] = FieldState.Empty;
        }

        // Повторная проверка на случай, если заполненых рядов несколько
        FullRowCheck(); 
        if (fullRowsCount != 0)
        {
            scoreController.IncreaseScore(fullRowsCount);
        }
        
        fullRowsCount = 0;

        // Смещаем вниз все элементы над уничтоженным рядом
        for (int y = rowNumber - 1; y >= 0; y--)
        {
            for (int x = Width - 1; x >= 0; x--)
            {
                // Проверка, чтобы НЕ опускать падающий элемент
                if (matrix[y, x] == FieldState.Falling)
                    return;
                matrix[y + 1, x] = matrix[y, x];
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
                if (matrix[y, x] == FieldState.Fallen)
                {
                    tempMatrix[y, x] = FieldState.Fallen;
                }
            }
        }
    }
}
