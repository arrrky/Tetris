using UnityEngine;
using MiscTools;
using System;
using System.Collections;

public class PlayingFieldController : FieldController, IPlayingFieldController, IFieldController
{
    protected IElementRotation elementRotation;
    protected IElementMovement elementMovement;

    private Vector2 topLeftPositionOfCurrentElement;
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
            else if (value.x > Field.Width - CurrentElementSize)
            {
                topLeftPositionOfCurrentElement.x = Field.Width - CurrentElementSize;
                topLeftPositionOfCurrentElement.y = value.y;
            }
            else
            {
                topLeftPositionOfCurrentElement = value;
            }
        }
    }

    public int FullRowsCount { get => fullRowsCount; set => fullRowsCount = value; }

    #endregion    

    public readonly Vector2 TopLeftPositionDefault = new Vector2(SpawnController.SpawnPoint, 0);

    protected const float RowDeletingDelay = 0.01f;

    public event Action RowDeleted;
    public event Action ElementFell;

    protected void Start()
    {
        EventsSetup();
        TopLeftPositionOfCurrentElement = TopLeftPositionDefault;
    }

    public virtual void PlayingFieldControllerInit(IElementMovement elementMovement, IElementRotation elementRotation)
    {
        this.elementMovement = elementMovement;
        this.elementRotation = elementRotation;

        Height = 20;
        Width = 10;

        FieldXShift = -4.5f;
        FieldYShift = -10.5f;       
    }

    private void EventsSetup()
    {
        elementMovement.ElementMoved += UpdateAfterMovement;
        elementMovement.LastRowOrElementsCollided += FallingToFallen;
        elementRotation.ElementWasRotated += UpdateAfterRotation;
    }

    public void FallingToFallen()
    {
        for (int y = 0; y < Field.Height; y++)
        {
            for (int x = 0; x < Field.Width; x++)
            {
                if (Field.Matrix[y, x] != FieldState.Empty)
                    Field.Matrix[y, x] = FieldState.Fallen;
            }
        }
        FullRowCheck();
        ElementFell?.Invoke();
    }

    public virtual void FullRowCheck()
    {
        for (int y = Field.Height - 1; y >= 0; y--)
        {
            bool isFullRow = true;

            for (int x = Field.Width - 1; x >= 0; x--)
            {
                isFullRow &= Field.Matrix[y, x] == FieldState.Fallen;
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
        for (int x = 0; x < Field.Width; x++)
        {
            Field.Matrix[numberOfRowToDelete, x] = FieldState.Empty;
            Field.Objects[numberOfRowToDelete, x].SetActive(false);
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
            for (int x = 0; x < Width; x++)
            {
                // Проверка, чтобы НЕ опускать падающий элемент
                if (Field.Matrix[y - 1, x] == FieldState.Falling)
                    return;
                Field.Matrix[y, x] = Field.Matrix[y - 1, x];
            }
        }
    }

    /// <summary>
    /// Запись упавших элементов из оригинальной матрицы во временную
    /// </summary>
    public void FallenToTemp(FieldState[,] tempMatrix)
    {
        for (int y = Field.Height - 1; y > 0; y--)
        {
            for (int x = Field.Width - 1; x >= 0; x--)
            {
                if (Field.Matrix[y, x] == FieldState.Fallen)
                {
                    tempMatrix[y, x] = FieldState.Fallen;
                }
            }
        }
    }

    /// <summary>
    /// Записываем временную матрицу в актуальную
    /// </summary>
    /// <param name="tempMatrix"></param>    

    public void UpdateAfterRotation()
    {
        UpdatePlayingFieldState(Field, CurrentElementColor);
    }

    public void UpdateAfterMovement(FieldState[,] tempMatrix, Vector2 topLeftPointOfElementShift)
    {        
        FallenToTemp(tempMatrix);
        Field.Matrix = tempMatrix;
        FullRowCheck();
        UpdatePlayingFieldState(Field, CurrentElementColor);
        TopLeftPositionOfCurrentElement += topLeftPointOfElementShift;
    }
}
