using UnityEngine;
using MiscTools;
using System;

public class PlayingFieldController : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject parentOfBlocks;
    [SerializeField] private GameObject playingFieldBorderBlockPrefab;
    [SerializeField] private GameObject playingFiedBorderBlocksParent; 
    [SerializeField] private ElementMovement elementMovement;

    private Border playingFieldBorder;    

    public Field playingField;
    public int currentElementSize;
    public FieldState[,] currentElementArray;
    public Color32 currentElementColor;
    public Vector2 topLeftPositionDefault;

    private Vector2 topLeftPositionOfCurrentElement;
    public Vector2 TopLeftPositionOfCurrentElement
    {
        get => topLeftPositionOfCurrentElement;
        set
        {
            // Проверки для того, чтобы верхний левый угол текущего элемента находился в пределах массива
            // с учетом места, необходимого для разворота элемента
            if (value.x < 0)
            {
                topLeftPositionOfCurrentElement.x = 0;
                topLeftPositionOfCurrentElement.y = value.y;
            }
            else if (value.x > playingField.Width - currentElementSize)
            {
                topLeftPositionOfCurrentElement.x = playingField.Width - currentElementSize;
                topLeftPositionOfCurrentElement.y = value.y;
            }
            else
            {
                topLeftPositionOfCurrentElement = value;
            }
        }
    }

    public event Action RowDeleted;
    public event Action ElementFell;
    
    public const int PLAYING_FIELD_HEIGHT = 20;
    public const int PLAYING_FIELD_WIDTH = 10;
    private const float PLAYING_FIELD_X_SHIFT = -4.5f;
    private const float PLAYING_FIELD_Y_SHIFT = -10.5f;

    private void Start()
    {
        PlayingFieldInit();
        PlayingFieldBorderInit();

        topLeftPositionDefault = new Vector2(SpawnController.SPAWN_POINT, 0);
        TopLeftPositionOfCurrentElement = topLeftPositionDefault;       

        elementMovement.LastRowOrElementsCollide += FallingToFallen;
    }

    private void PlayingFieldBorderInit()
    {
        playingFieldBorder = gameObject.AddComponent(typeof(Border)) as Border;
        playingFieldBorder.SpriteShift = Tools.GetSpriteShift(playingFieldBorderBlockPrefab);
        playingFieldBorder.TopLeftPoint = new Vector2(-PLAYING_FIELD_WIDTH / 2 - 1, GameController.screenBounds.y - 1);
        playingFieldBorder.CreateBorder(PLAYING_FIELD_WIDTH + 1, PLAYING_FIELD_HEIGHT + 1, playingFieldBorderBlockPrefab, playingFiedBorderBlocksParent);
    }

    private void PlayingFieldInit()
    {
        playingField = gameObject.AddComponent(typeof(Field)) as Field;
        playingField.Height = PLAYING_FIELD_HEIGHT;
        playingField.Width = PLAYING_FIELD_WIDTH;
        playingField.Matrix = new FieldState[PLAYING_FIELD_HEIGHT, PLAYING_FIELD_WIDTH];
        playingField.Objects = new GameObject[PLAYING_FIELD_HEIGHT, PLAYING_FIELD_WIDTH];
        FillTheField(playingField, PLAYING_FIELD_X_SHIFT, PLAYING_FIELD_Y_SHIFT);
        UpdateThePlayingField(playingField, currentElementColor);
    }

    public void FillTheField(Field field, float xShift, float yShift)
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
    public void UpdateThePlayingField(Field field, Color32 elementColor)
    {
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Objects[y, x].SetActive(field.Matrix[y, x] != FieldState.Empty);
                
                if (field.Matrix[y, x] == FieldState.Falling)
                {
                    field.Objects[y, x].GetComponent<SpriteRenderer>().color = elementColor;
                }
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
        ElementFell?.Invoke();
    }

    public int fullRowsCount;

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
            RowDeleted?.Invoke();
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

    public void ClearField(Field field)
    {
        for(int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Matrix[y, x] = FieldState.Empty;                
            }
        }
    }       

    public void WriteAndUpdate(FieldState[,] tempMatrix)
    {
        playingField.Matrix = tempMatrix;
        FullRowCheck();
        UpdateThePlayingField(playingField, currentElementColor);
    }
}
