﻿using UnityEngine;
using System.Collections.Generic;

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
}
