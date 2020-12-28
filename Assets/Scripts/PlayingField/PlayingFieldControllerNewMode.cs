using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayingFieldControllerNewMode : PlayingFieldController, IPlayingFieldController, IFieldController
{
    public new event Action RowDeleted;    

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

            if (isFullRow)
            {
                FullRowsCount++;
                StartCoroutine(DeleteFullRow(y));
            }
        }
    }

    protected override IEnumerator DeleteFullRow(int numberOfRowToDelete)
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

    protected override void MoveRowsAboveDeletedRow(int numberOfRowToDelete)
    {
        for (int y = numberOfRowToDelete; y >= 0; y--)
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

