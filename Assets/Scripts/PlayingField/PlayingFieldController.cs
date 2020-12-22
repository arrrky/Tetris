using UnityEngine;
using MiscTools;
using System;
using System.Collections;

public class PlayingFieldController : MonoBehaviour, IPlayingFieldController
{
    private GameController gameController;

    private GameObject blockPrefab;
    private GameObject blocksParent;
    private ElementMovement elementMovement;
    private ElementRotation elementRotation;

    private Field playingField;
    private Vector2 topLeftPositionOfCurrentElement;
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

    public readonly Vector2 TopLeftPositionDefault = new Vector2(SpawnController.SpawnPoint, 0);

    public virtual int PlayingFieldHeight { get; } = 20;
    public virtual int PlayingFieldWidth { get; } = 10;

    public virtual float PlayingFieldXShift { get; } = -4.5f;
    public virtual float PlayingFieldYShift { get; } = -10.5f;
        
    protected const float RowDeletingDelay = 0.01f;

    public event Action RowDeleted;
    public event Action ElementFell;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        DependenciesSetup();
    }

    protected void Start()
    {
        PlayingFieldInit();
        EventsSetup();
        TopLeftPositionOfCurrentElement = TopLeftPositionDefault;
    } 
    
    private void DependenciesSetup()
    {
        blockPrefab = gameController.blockPrefab;
        blocksParent = gameController.blocksParent;
        elementMovement = gameController.elementMovement;
        elementRotation = gameController.elementRotation;
    }    

    private void EventsSetup()
    {
        elementMovement.ElementMoved += UpdateAfterMovement;
        elementMovement.LastRowOrElementsCollided += FallingToFallen;
        elementRotation.ElementWasRotated += UpdateAfterRotation;
    }

    protected void PlayingFieldInit()
    {
        PlayingField = new Field
        {
            Height = PlayingFieldHeight,
            Width = PlayingFieldWidth,
            Matrix = new FieldState[PlayingFieldHeight, PlayingFieldWidth],
            Objects = new GameObject[PlayingFieldHeight, PlayingFieldWidth],
            Sprites = new SpriteRenderer[PlayingFieldHeight, PlayingFieldWidth]
        };
        FillTheField(PlayingField, PlayingFieldXShift, PlayingFieldYShift);
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
                field.Objects[y, x] = Instantiate(blockPrefab, new Vector3(x + xShift, field.Height - y + yShift, 0), Quaternion.identity, blocksParent.transform);
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

    public virtual void FullRowCheck()
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
                StartCoroutine(DeleteFullRow(y));              
            }
        }
    }

    protected virtual IEnumerator DeleteFullRow(int numberOfRowToDelete)
    {
        // Удаление ряда с задержкой
        for (int x = 0; x < PlayingField.Width; x++)
        {
            PlayingField.Matrix[numberOfRowToDelete, x] = FieldState.Empty;
            PlayingField.Objects[numberOfRowToDelete, x].SetActive(false);
            yield return new WaitForSeconds(RowDeletingDelay);
        }

        FullRowCheck(); // повторная проверка на случай, если заполненных рядов несколько

        if (FullRowsCount != 0)
        {
            RowDeleted?.Invoke();
        }

        for (int i = 0; i < FullRowsCount; i++)
        {
            MoveRowsAboveDeletedRow(numberOfRowToDelete);
        }

        FullRowsCount = 0;
    }

    protected virtual void MoveRowsAboveDeletedRow(int numberOfRowToDelete)
    {
        for (int y = numberOfRowToDelete; y >= 0; y--)
        {
            for (int x = 0; x < PlayingFieldWidth; x++)
            {
                // Проверка, чтобы НЕ опускать падающий элемент
                if (PlayingField.Matrix[y - 1, x] == FieldState.Falling)
                    return;
                PlayingField.Matrix[y, x] = PlayingField.Matrix[y - 1, x];
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

    public void UpdateAfterRotation()
    {
        UpdatePlayingFieldState(PlayingField, CurrentElementColor);
    }

    public void UpdateAfterMovement(FieldState[,] tempMatrix, Vector2 topLeftPointOfElementShift)
    {
        FallenToTemp(tempMatrix);
        PlayingField.Matrix = tempMatrix;
        FullRowCheck();
        UpdatePlayingFieldState(PlayingField, CurrentElementColor);
        TopLeftPositionOfCurrentElement += topLeftPointOfElementShift;
    }
}
