using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MiscTools;

public class PlayingField : Field
{
    private int fullRowsCount;
    public void FallingToFallen()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (matrix[y, x] != FieldState.Empty)
                    matrix[y, x] = FieldState.Fallen;
            }
        }
        //spawnManager.SpawnRandomElement(matrix);
    }

    public void FullRowCheck(Action<int> increaseScore)
    {
        for (int y = Height - 1; y >= 0; y--)
        {
            bool isFullRow = true;
            for (int x = Width - 1; x >= 0; x--)
            {
                isFullRow &= matrix[y, x] == FieldState.Fallen;
            }
            if (isFullRow)
            {
                fullRowsCount++;
                DeleteFullRow(y, increaseScore);
            }
        }
    }


    public void DeleteFullRow(int rowNumber, Action<int> increaseScore)
    {
        for (int x = Width - 1; x >= 0; x--)
        {
            matrix[rowNumber, x] = FieldState.Empty;
        }

        // Повторная проверка на случай, если заполненых рядов несколько
        FullRowCheck(increaseScore);
        if (fullRowsCount != 0)
        {
            increaseScore(fullRowsCount);
        }

        fullRowsCount = 0;

        // Смещаем вниз все элементы над уничтоженным рядом
        for (int y = rowNumber - 1; y >= 0; y--)
        {
            for (int x = Width - 1; x >= 0; x--)
            {
                // Проверка, чтобы НЕ опускать падающий элемент
                if (matrix[y, x] == FieldState.Falling)
                    return;
                matrix[y + 1, x] = matrix[y, x];
            }
        }
    }

    /// <summary>
    /// Запись упавших элементов из оригинальной матрицы во временную
    /// </summary>
    public void FallenToTemp(FieldState[,] tempMatrix)
    {
        for (int y = Height - 1; y > 0; y--)
        {
            for (int x = Width - 1; x >= 0; x--)
            {
                if (matrix[y, x] == FieldState.Fallen)
                {
                    tempMatrix[y, x] = FieldState.Fallen;
                }
            }
        }
    }

}
