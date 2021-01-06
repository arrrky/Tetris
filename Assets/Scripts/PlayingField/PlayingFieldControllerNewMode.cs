using System.Collections.Generic;
using UnityEngine;


public class PlayingFieldControllerNewMode : PlayingFieldController, IPlayingFieldController, IFieldController
{
    private List<int> rowsToDelete = new List<int>();
    private readonly int rowsCountToInitDeleting = 2;

    public override Vector2 TopLeftPositionOfCurrentElement
    {
        get => base.TopLeftPositionOfCurrentElement;

        set
        {
            if (value.x < 0)
            {
                topLeftPositionOfCurrentElement.x = Field.Width - 1;
                topLeftPositionOfCurrentElement.y = value.y;
            }
            else if (value.x > Field.Width - 1)
            {
                topLeftPositionOfCurrentElement.x = 0;
                topLeftPositionOfCurrentElement.y = value.y;
            }
            else
            {
                topLeftPositionOfCurrentElement = value;
            }
        }
    }

    public override void PlayingFieldControllerInit(GameController gameController, IElementMovement elementMovement, IElementRotation elementRotation)
    {
        this.gameController = gameController;        
        this.elementMovement = elementMovement;
        this.elementRotation = elementRotation;

        Height = 20;
        Width = 12;

        FieldXShift = -5.5f;
        FieldYShift = -10.5f;

        FieldInit();
    }

    public override void FullRowCheck()
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

        StackedRowsCheck();

        FullRowsCount = rowsToDelete.Count;

        if (rowsToDelete.Count >= rowsCountToInitDeleting)
        {
            DeleteFullRowsNew();
        }

        FullRowsCount = 0;
        rowsToDelete.Clear();
    }

    protected void DeleteFullRowsNew()
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

    protected override void MoveRowAboveDeletedRow(int numberOfRowInList)
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

    // Если в игре будет элемент "палка" длиной в 5 блоков, и возникнет ситуация 19-18-16-14-13 (или аналогичная) - метод нормально работать не будет    
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
            else if(isRowsStacked)
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

