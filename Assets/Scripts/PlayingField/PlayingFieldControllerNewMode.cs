using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayingFieldControllerNewMode : PlayingFieldController, IPlayingFieldController, IFieldController
{

    private List<int> rowsToDelete = new List<int>(); // TODO - удаления рядов    

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

    public override void PlayingFieldControllerInit(IElementMovement elementMovement, IElementRotation elementRotation)
    {
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
                FullRowsCount++;
            }
        }

        if (rowsToDelete.Count >= 2)
        {
            DeleteFullRowsNew();
        }
    }

    protected void DeleteFullRowsNew()
    {
        for (int i = 0; i < rowsToDelete.Count; i++)
        {
            Debug.Log($"row to delete: {rowsToDelete[i]}");
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

        FullRowsCount = 0;
        rowsToDelete.Clear();
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
}

