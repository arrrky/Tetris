using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayingFieldController : FieldController, IPlayingFieldController, IFieldController
{
    protected GameController gameController;
    protected IElementRotation elementRotation;
    protected IElementMovement elementMovement;

    protected List<int> rowsToDelete = new List<int>();
    protected int rowsCountToInitDeleting;

    protected Vector2 topLeftPositionOfCurrentElement;
    private int fullRowsCount;   

    #region PROPERTIES   

    public virtual Vector2 TopLeftPositionOfCurrentElement
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

    public virtual void PlayingFieldControllerInit(GameController gameController, IElementMovement elementMovement, IElementRotation elementRotation)
    {
        this.gameController = gameController;
        this.elementMovement = elementMovement;
        this.elementRotation = elementRotation;

        Height = 20;
        Width = 10;

        FieldXShift = -4.5f;
        FieldYShift = -10.5f;

        rowsCountToInitDeleting = 1;
    }

    private void EventsSetup()
    {
        elementMovement.ElementMoved += UpdateAfterMovement;
        elementMovement.LastRowOrElementsCollided += FallingToFallen;
        elementRotation.ElementWasRotated += UpdateAfterRotation;
    }

    protected void OnRowDeleted() => RowDeleted?.Invoke();
    protected void OnElementFell() => ElementFell?.Invoke();

    public virtual void FallingToFallen()
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
        OnElementFell();
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

            if (isFullRow && !rowsToDelete.Contains(y))
            {
                rowsToDelete.Add(y);
            }
        }     
        
        if(GameModeManager.Instance.IsNewMode)
        {
            StackedRowsCheck();
        }

        FullRowsCount = rowsToDelete.Count;

        if (rowsToDelete.Count >= rowsCountToInitDeleting)
        {
            DeleteFullRowsNew();
        }

        FullRowsCount = 0;
        rowsToDelete.Clear();
    }

    protected virtual void DeleteFullRowsNew()
    {
        for (int i = 0; i < rowsToDelete.Count; i++)
        {
            for (int x = 0; x < Field.Width; x++)
            {
                Field.Matrix[rowsToDelete[i], x] = FieldState.Empty;
            }
        }

        for (int i = rowsToDelete.Count - 1; i >= 0; i--)
        {
            MoveRowAboveDeletedRow(i);
        }

        if (FullRowsCount != 0)
        {
            OnRowDeleted();
        }
    }

    protected virtual void MoveRowAboveDeletedRow(int numberOfRowInList)
    {
        for (int y = rowsToDelete[numberOfRowInList]; y > 0; y--)
        {
            for (int x = 0; x < Width; x++)
            {
                // Проверка, чтобы НЕ опускать падающий элемент
                if (Field.Matrix[y - 1, x] == FieldState.Moving)
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

        // Переместил сюда, чтобы отработало удаление рядов, элементы сместились, и только после этого запустилась смена раунда
        if (gameController.ScoreController.Score >= LevelController.Instance.Goal && GameModeManager.Instance.ChosenGameMode == GameMode.Level)
        {
            StartCoroutine(gameController.NextLevelRoutine());
        }
    }

    private void StackedRowsCheck()
    {
        bool isRowsStacked = false;
        for (int i = 0; i < rowsToDelete.Count - 1; i++)
        {
            // Если разность идущих подряд рядов в списке равна 1 - значит они идут подряд
            if ((rowsToDelete[i] - rowsToDelete[i + 1]) == 1)
            {
                isRowsStacked = true;
            }
            // Если нет, но уже есть застаканные ряды - удаляем из списка крайний ряд
            // Например, случай 19-18-16
            else if (isRowsStacked)
            {
                rowsToDelete.RemoveAt(i + 1);
            }
            // Если застаканных рядов нет, можно удалять из списка первый ряд
            // Например, случай 19-17-16
            else
            {
                rowsToDelete.RemoveAt(i);
            }
        }
    }
}