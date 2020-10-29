using UnityEngine;
using System.Collections.Generic;
using MiscTools;

public class ElementsArrays : MonoBehaviour
{
    // Массивы элементов
    // Выделить в класс с именем, массивом, шансом

    public Dictionary<string, int[,]> dicOfElements = new Dictionary<string, int[,]>
    {
        {"O", new int[,]
            {
                 {1,1},
                 {1,1}            }
        },
        {"T", new int [,]
            {
                {1,1,1},
                {0,1,0},
                {0,0,0}
            }
        },
        {"I", new int [,]
            {
                {1,1,1,1},
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0}
            }
        },
        {"L", new int [,]
            {
                {1,1,1},
                {1,0,0},
                {0,0,0}
            }
        },
        {"J", new int [,]
            {
                {1,1,1},
                {0,0,1},
                {0,0,0}
            }
        },
        {"Z", new int [,]
            {
                {1,1,0},
                {0,1,1},
                {0,0,0}
            }
        },
        {"S", new int [,]
            {
                {0,1,1},
                {1,1,0},
                {0,0,0}
            }
        }
    };
        
    public static FieldState[,] ConvertToFieldState(int[,] element)
    {
        FieldState[,] temp = new FieldState[element.GetLength(0), element.GetLength(1)];

        for (int y = 0; y < element.GetLength(0); y++)
            for (int x = 0; x < element.GetLength(1); x++)
                temp[y, x] = (FieldState)element[y, x];
        return temp;
    }
}


