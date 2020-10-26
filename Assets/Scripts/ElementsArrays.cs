using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsArrays : MonoBehaviour
{
    // Массивы элементов

    public static Dictionary<string, int[,]> elementsArrays = new Dictionary<string, int[,]>
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

    public static readonly List<int[,]> elementsList = new List<int[,]>(elementsArrays.Values);    
}
