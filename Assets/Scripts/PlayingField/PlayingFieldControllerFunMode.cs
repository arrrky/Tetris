using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayingFieldControllerFunMode : PlayingFieldController, IPlayingFieldController
{
    public override int PlayingFieldHeight { get; } = 20;
    public override int PlayingFieldWidth { get; } = 12;

    public override float PlayingFieldXShift { get; } = -5.5f;

    public new event Action RowDeleted;

    private List<int> rowsToDelete = new List<int>();    

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
                Debug.Log(y);
                rowsToDelete.Add(y);
                FullRowsCount++;

                StartCoroutine(DeleteFullRow());
            }
        }
    }

    protected IEnumerator DeleteFullRow()
    {
        // Удаление ряда с задержкой
        for (int x = 0; x < Field.Width; x++)
        {
            Field.Matrix[rowsToDelete[0], x] = FieldState.Empty;
            Field.Objects[rowsToDelete[0], x].SetActive(false);
            yield return new WaitForSeconds(RowDeletingDelay);
        }

        if (FullRowsCount != 0)
        {
            RowDeleted?.Invoke();
        }

        MoveRowsAboveDeletedRow(rowsToDelete[0]);

        FullRowsCount = 0;
        rowsToDelete.Clear();       
    }

    protected override void MoveRowsAboveDeletedRow(int numberOfRowToDelete)
    {
        Debug.Log($"Move rows above row: {numberOfRowToDelete}");
        for (int y = numberOfRowToDelete; y >= 0; y--)
        {
            for (int x = 0; x < PlayingFieldWidth; x++)
            {
                // Проверка, чтобы НЕ опускать падающий элемент
                if (Field.Matrix[y - 1, x] == FieldState.Falling)
                    return;
                Field.Matrix[y, x] = Field.Matrix[y - 1, x];
            }
        }        
    }
}

