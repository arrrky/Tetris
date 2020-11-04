using UnityEngine;
using MiscTools;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject parentOfBlocks;
    [SerializeField]
    private ScoreController scoreController;
    [SerializeField]
    private SpawnManager spawnManager;

    public Field playingField;
    public int currentElementSize;
    public FieldState[,] currentElementArray;
    public Vector2 topLeftPositionDefault;
    public Vector2 topLeftPositionOfCurrentElement;

    // Нулевые координаты в Unity - левый нижний угол игрового поля
    private const int playingFieldHeight = 20;
    private const int playingFieldWidth = 10;
    private const int playingFieldXShift = 0;
    private const int playingFieldYShift = -1;

    private int fullRowsCount;

    private void Start()
    {
        PlayingFieldInit();
        topLeftPositionDefault = new Vector2(SpawnManager.spawnPoint, 0);
        topLeftPositionOfCurrentElement = topLeftPositionDefault;
        LevelController.Instance.InitializeLevel();
        spawnManager.SpawnRandomElement(playingField);
    }

    private void PlayingFieldInit()
    {
        playingField = gameObject.AddComponent(typeof(Field)) as Field;
        playingField.Height = playingFieldHeight;
        playingField.Width = playingFieldWidth;
        playingField.Matrix = new FieldState[playingFieldHeight, playingFieldWidth];
        playingField.Objects = new GameObject[playingFieldHeight, playingFieldWidth];        
        FillTheField(playingField, playingFieldXShift, playingFieldYShift);
        UpdateThePlayingField(playingField);
    }

    public void FillTheField(Field field, int xShift, int yShift)
    {
        // Из-за разницы в нумерации элементов матрицы-поля и отсчета координат в Unity удобнее инициализировать объекты именно таким образом.
        // Поэтому 'y' кооордината инстанирования имеет вид height - y - yShift (где Shift - смещение по осям),
        // чтобы блоки заполнялись сверху вниз (как в матрице-поле).

        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Objects[y, x] = Instantiate(blockPrefab, new Vector3(x + xShift, field.Height - y + yShift, 0), Quaternion.identity, parentOfBlocks.transform);
            }
        }
    }

    /// <summary>
    /// Обновление состояния игрового поля
    /// </summary>
    public void UpdateThePlayingField(Field field)
    {       
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Objects[y, x].SetActive(field.Matrix[y, x] != FieldState.Empty);
            }
        }
    }

    public void FallingToFallen()
    {
        for (int y = 0; y < playingField.Height; y++)
        {
            for (int x = 0; x < playingField.Width; x++)
            {
                if (playingField.Matrix[y, x] != FieldState.Empty)
                    playingField.Matrix[y, x] = FieldState.Fallen;
            }
        }
    }

    public void FullRowCheck()
    {
        for (int y = playingField.Height - 1; y >= 0; y--)
        {
            bool isFullRow = true;

            for (int x = playingField.Width - 1; x >= 0; x--)
            {
                isFullRow &= playingField.Matrix[y, x] == FieldState.Fallen;
            }
            if (isFullRow)
            {
                fullRowsCount++;
                DeleteFullRow(y);
            }
        }        
    }

    private void DeleteFullRow(int numberOfRowToDelete)
    {
        for (int x = playingField.Width - 1; x >= 0; x--)
        {
            playingField.Matrix[numberOfRowToDelete, x] = FieldState.Empty;
        }
                
        FullRowCheck(); // Повторная проверка на случай, если заполненых рядов несколько

        if (fullRowsCount != 0)
        {
            scoreController.IncreaseScore(fullRowsCount);
        }

        fullRowsCount = 0;
        MoveRowsAboveDeletedRow(numberOfRowToDelete);
    }

    private void MoveRowsAboveDeletedRow(int numberOfRowToDelete)
    {
        for (int y = numberOfRowToDelete - 1; y >= 0; y--)
        {
            for (int x = playingField.Width - 1; x >= 0; x--)
            {
                // Проверка, чтобы НЕ опускать падающий элемент
                if (playingField.Matrix[y, x] == FieldState.Falling)
                    return;
                playingField.Matrix[y + 1, x] = playingField.Matrix[y, x];
            }
        }
    }

    /// <summary>
    /// Запись упавших элементов из оригинальной матрицы во временную
    /// </summary>
    public void FallenToTemp(FieldState[,] tempMatrix)
    {
        for (int y = playingField.Height - 1; y > 0; y--)
        {
            for (int x = playingField.Width - 1; x >= 0; x--)
            {
                if (playingField.Matrix[y, x] == FieldState.Fallen)
                {
                    tempMatrix[y, x] = FieldState.Fallen;
                }
            }
        }
    }

    public void WriteAndUpdate(FieldState[,] tempMatrix)
    {
        playingField.Matrix = tempMatrix;
        FullRowCheck();
        UpdateThePlayingField(playingField);
    }
}
