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

    private Vector2 topLeftPositionOfCurrentElement;
    private Field playingField;
    private int currentElementSize;
    private FieldState[,] currentElementArray;
    private Color32 currentElementColor;
    private int fullRowsCount;

    #region PROPERTIES

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
            else if (value.x > PlayingField.Width - CurrentElementSize)
            {
                topLeftPositionOfCurrentElement.x = PlayingField.Width - CurrentElementSize;
                topLeftPositionOfCurrentElement.y = value.y;
            }
            else
            {
                topLeftPositionOfCurrentElement = value;
            }
        }
    }

    public Field PlayingField { get => playingField; set => playingField = value; }
    public int CurrentElementSize { get => currentElementSize; set => currentElementSize = value; }
    public FieldState[,] CurrentElementArray { get => currentElementArray; set => currentElementArray = value; }
    public Color32 CurrentElementColor { get => currentElementColor; set => currentElementColor = value; }
    public int FullRowsCount { get => fullRowsCount; set => fullRowsCount = value; }

    #endregion    

    public readonly Vector2 TOP_LEFT_POSITION_DEFAULT = new Vector2(SpawnController.SPAWN_POINT, 0);

    public const int PLAYING_FIELD_HEIGHT = 20;
    public const int PLAYING_FIELD_WIDTH = 10;
    private const float PLAYING_FIELD_X_SHIFT = -4.5f;
    private const float PLAYING_FIELD_Y_SHIFT = -10.5f;

    public event Action RowDeleted;
    public event Action ElementFell;

    private void Start()
    {
        PlayingFieldInit();
        PlayingFieldBorderInit();

        TopLeftPositionOfCurrentElement = TOP_LEFT_POSITION_DEFAULT;

        elementMovement.ElementMoved += FullUpdate;
        elementMovement.LastRowOrElementsCollided += FallingToFallen;
    }

    private void PlayingFieldBorderInit()
    {
        playingFieldBorder = gameObject.AddComponent(typeof(Border)) as Border;
        playingFieldBorder.SpriteShift = Tools.GetSpriteShift(playingFieldBorderBlockPrefab);
        playingFieldBorder.TopLeftPoint = new Vector2(-PLAYING_FIELD_WIDTH / 2 - 1, GameController.ScreenBounds.y - 1);
        playingFieldBorder.CreateBorder(PLAYING_FIELD_WIDTH + 1, PLAYING_FIELD_HEIGHT + 1, playingFieldBorderBlockPrefab, playingFiedBorderBlocksParent);
    }

    private void PlayingFieldInit()
    {
        PlayingField = gameObject.AddComponent(typeof(Field)) as Field;
        PlayingField.Height = PLAYING_FIELD_HEIGHT;
        PlayingField.Width = PLAYING_FIELD_WIDTH;
        PlayingField.Matrix = new FieldState[PLAYING_FIELD_HEIGHT, PLAYING_FIELD_WIDTH];
        PlayingField.Objects = new GameObject[PLAYING_FIELD_HEIGHT, PLAYING_FIELD_WIDTH];
        PlayingField.Sprites = new SpriteRenderer[PLAYING_FIELD_HEIGHT, PLAYING_FIELD_WIDTH];
        FillTheField(PlayingField, PLAYING_FIELD_X_SHIFT, PLAYING_FIELD_Y_SHIFT);
        UpdatePlayingFieldState(PlayingField, CurrentElementColor);
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
                field.Sprites[y, x] = field.Objects[y, x].GetComponent<SpriteRenderer>();
            }
        }
    }

    /// <summary>
    /// Обновление состояния игрового поля
    /// </summary>
    public void UpdatePlayingFieldState(Field field, Color32 elementColor)
    {
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Objects[y, x].SetActive(field.Matrix[y, x] != FieldState.Empty);

                if (field.Matrix[y, x] == FieldState.Falling)
                {
                    field.Sprites[y, x].color = elementColor;
                }
            }
        }
    }

    public void FallingToFallen()
    {
        for (int y = 0; y < PlayingField.Height; y++)
        {
            for (int x = 0; x < PlayingField.Width; x++)
            {
                if (PlayingField.Matrix[y, x] != FieldState.Empty)
                    PlayingField.Matrix[y, x] = FieldState.Fallen;
            }
        }
        FullRowCheck();
        ElementFell?.Invoke();
    }

    public void FullRowCheck()
    {
        for (int y = PlayingField.Height - 1; y >= 0; y--)
        {
            bool isFullRow = true;

            for (int x = PlayingField.Width - 1; x >= 0; x--)
            {
                isFullRow &= PlayingField.Matrix[y, x] == FieldState.Fallen;
            }
            if (isFullRow)
            {
                FullRowsCount++;
                DeleteFullRow(y);
            }
        }
    }

    private void DeleteFullRow(int numberOfRowToDelete)
    {
        for (int x = PlayingField.Width - 1; x >= 0; x--)
        {
            PlayingField.Matrix[numberOfRowToDelete, x] = FieldState.Empty;
        }

        FullRowCheck(); // повторная проверка на случай, если заполненных рядов несколько

        if (FullRowsCount != 0)
        {
            RowDeleted?.Invoke();
        }

        FullRowsCount = 0;
        MoveRowsAboveDeletedRow(numberOfRowToDelete);
    }

    private void MoveRowsAboveDeletedRow(int numberOfRowToDelete)
    {
        for (int y = numberOfRowToDelete - 1; y >= 0; y--)
        {
            for (int x = PlayingField.Width - 1; x >= 0; x--)
            {
                // Проверка, чтобы НЕ опускать падающий элемент
                if (PlayingField.Matrix[y, x] == FieldState.Falling)
                    return;
                PlayingField.Matrix[y + 1, x] = PlayingField.Matrix[y, x];
            }
        }        
    }

    /// <summary>
    /// Запись упавших элементов из оригинальной матрицы во временную
    /// </summary>
    public void FallenToTemp(FieldState[,] tempMatrix)
    {
        for (int y = PlayingField.Height - 1; y > 0; y--)
        {
            for (int x = PlayingField.Width - 1; x >= 0; x--)
            {
                if (PlayingField.Matrix[y, x] == FieldState.Fallen)
                {
                    tempMatrix[y, x] = FieldState.Fallen;
                }
            }
        }
    }

    public void ClearField(Field field)
    {
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Matrix[y, x] = FieldState.Empty;
            }
        }
    }

    public void FullUpdate(FieldState[,] tempMatrix, Vector2 topLeftPointOfElementShift)
    {
        FallenToTemp(tempMatrix);
        PlayingField.Matrix = tempMatrix;
        FullRowCheck();
        UpdatePlayingFieldState(PlayingField, CurrentElementColor);
        TopLeftPositionOfCurrentElement += topLeftPointOfElementShift;
    }
}
