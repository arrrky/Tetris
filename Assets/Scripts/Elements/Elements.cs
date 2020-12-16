using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class Elements : MonoBehaviour
{    
    private List<Element> listOfElements;
    private const int sumOfChances = 100;

    // Нужно инициализировать список элементов ДО спауна первого элемента - поэтому делаем это в Awake
    private void Awake()
    {
        listOfElements = new List<Element>();
        ElementsInit();
    }

    private void Start()
    {      
        //ElementsSortByChance();
        CorrectChancesCheck();
    }

    #region Инициализация элементов
    private void ElementsInit()
    {
        listOfElements.Add(new Element
        {
            Name = "O",
            Matrix = Tools.ConvertToFieldState(new int[,]
            {
                 {1,1},
                 {1,1}
            }),
            SpawnChance = 10,
            Color = Tools.rainbowColors["red"],
        });

        listOfElements.Add(new Element
        {
            Name = "T",
            Matrix = Tools.ConvertToFieldState(new int[,]
            {
                {1,1,1},
                {0,1,0},
                {0,0,0}
            }),
            SpawnChance = 10,
            Color = Tools.rainbowColors["orange"],
        });

        listOfElements.Add(new Element
        {
            Name = "I",
            Matrix = Tools.ConvertToFieldState(new int[,]
            {
                {1,1,1,1},
                {0,0,0,0},
                {0,0,0,0},
                {0,0,0,0}
            }),
            SpawnChance = 10,
            Color = Tools.rainbowColors["yellow"],
        });

        listOfElements.Add(new Element
        {
            Name = "L",
            Matrix = Tools.ConvertToFieldState(new int[,]
            {
                {1,1,1},
                {1,0,0},
                {0,0,0}
            }),
            SpawnChance = 15,
            Color = Tools.rainbowColors["green"],
        });

        listOfElements.Add(new Element
        {
            Name = "J",
            Matrix = Tools.ConvertToFieldState(new int[,]
            {
                {1,1,1},
                {0,0,1},
                {0,0,0}
            }),
            SpawnChance = 15,
            Color = Tools.rainbowColors["lightblue"],
        });

        listOfElements.Add(new Element
        {
            Name = "Z",
            Matrix = Tools.ConvertToFieldState(new int[,]
            {
                {1,1,0},
                {0,1,1},
                {0,0,0}
            }),
            SpawnChance = 20,
            Color = Tools.rainbowColors["blue"],
        });

        listOfElements.Add(new Element
        {
            Name = "S",
            Matrix = Tools.ConvertToFieldState(new int[,]
            {
                {0,1,1},
                {1,1,0},
                {0,0,0}
            }),
            SpawnChance = 20,
            Color = Tools.rainbowColors["violet"],
        });

        if (GameModeManager.Instance.IsFunMode)
        {
            listOfElements.Add(new Element
            {
                Name = "Cross",
                Matrix = Tools.ConvertToFieldState(new int[,]
            {
                {0,1,0},
                {1,1,1},
                {0,1,0}
            }),
                SpawnChance = 20,
                Color = Tools.additionalColors["grey"],
            });

            listOfElements.Add(new Element
            {
                Name = "Bench",
                Matrix = Tools.ConvertToFieldState(new int[,]
            {
                {1,1,1},
                {1,0,1},
                {0,0,0}
            }),
                SpawnChance = 20,
                Color = Tools.additionalColors["brown"],
            });

            listOfElements.Add(new Element
            {
                Name = "Stair",
                Matrix = Tools.ConvertToFieldState(new int[,]
            {
                {1,0,0},
                {1,1,0},
                {0,1,1}
            }),
                SpawnChance = 20,
                Color = Tools.additionalColors["pink"],
            });
        }
    }
    #endregion

    private void CorrectChancesCheck()
    {
        int sum = 0;
        foreach(var element in listOfElements)
        {
            sum += element.SpawnChance;
        }
        if (sum != 100)
        {
            Debug.LogError("Chances are incorrect!");
        } 
    }

    private int GetSumOfChances ()
    {
        int sumOfChances = 0;

        for (int i = 0; i < listOfElements.Count; i++)
        {
            sumOfChances += listOfElements[i].SpawnChance;
        }

        return sumOfChances;
    }

    // Используется метод из этой статьи: https://jonlabelle.com/snippets/view/csharp/pick-random-elements-based-on-probability
    // Важно задать шансы так, чтобы суммарно они были равны 100 (для корректной работы метода)
    // Метод будет работать в любом случае, просто шанс выпадения элемента не будет статистически верен
    public Element GetRandomElement()
    {
        int roll = Random.Range(1, GetSumOfChances());
        int cumulativeChance = 0;

        for (int i = 0; i < listOfElements.Count; i++)
        {
            cumulativeChance += listOfElements[i].SpawnChance;

            if (roll < cumulativeChance)
                return listOfElements[i];
        }
        return null;
    }
    
    private void ElementsSortByChance()
    {        
        var sortedElements =
            from element in listOfElements
            orderby element.SpawnChance
            select element;      

        listOfElements = sortedElements.ToList();
    }
}
