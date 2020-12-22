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
    public int NumberOfRowsToInitiateDeleting { get; } = 2;

    private List<int> rowsToDelete = new List<int>();

    private int previousRowNumber = 0;
    private bool isTwoInRow = false;

    public new event Action RowDeleted;

    public override void FullRowCheck()
    {
        

        for (int y = PlayingField.Height - 1; y >= 0; y--)
        {
            bool isFullRow = true;

            for (int x = PlayingField.Width - 1; x >= 0; x--)
            {
                isFullRow &= PlayingField.Matrix[y, x] == FieldState.Fallen;
            }

            if (isFullRow && !rowsToDelete.Contains(y))
            {
                isTwoInRow = (previousRowNumber - y) == 1;
                previousRowNumber = y;

                rowsToDelete.Add(y);
                Debug.Log($"Row to delete added: {y}");
                Debug.Log($"IsTwoInRow: {isTwoInRow}");

            }
        }

        if (rowsToDelete.Count >= NumberOfRowsToInitiateDeleting)
        {
            StartCoroutine(DeleteFullRows());
        }

        
    }

    private IEnumerator DeleteFullRows()
    {
        // Удаление рядов с задержкой
        for (int i = 0; i < rowsToDelete.Count; i++)
        {
            for (int x = 0; x < PlayingField.Width; x++)
            {
                PlayingField.Matrix[rowsToDelete[i], x] = FieldState.Empty;
                PlayingField.Objects[rowsToDelete[i], x].SetActive(false);
                yield return new WaitForSeconds(RowDeletingDelay);
            }            
        }

        MoveRowAboveDeletedRows();
      
        if (rowsToDelete.Count != 0)
        {
            RowDeleted?.Invoke();
        }

        rowsToDelete.Clear();
    }

    protected void MoveRowAboveDeletedRows()
    {
        for (int y = rowsToDelete[0]; y >= 0; y--)
        {
            for (int x = 0; x < PlayingFieldWidth; x++)
            {
                // Проверка, чтобы НЕ опускать падающий элемент                
                if (PlayingField.Matrix[y - rowsToDelete.Count, x] == FieldState.Falling)
                    return;

                PlayingField.Matrix[y, x] = PlayingField.Matrix[y - rowsToDelete.Count, x];
            }
        }
    }        
}
