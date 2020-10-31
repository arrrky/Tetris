using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class Elements : MonoBehaviour
{
    public List<Element> listOfElements;
    private const int sumOfChances = 100;
    
    void Start()
    {
        listOfElements = new List<Element>();
        ElementsInit();
    }

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
            SpawnChance = 15
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
            SpawnChance = 10
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
            SpawnChance = 5
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
            SpawnChance = 20
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
            SpawnChance = 15
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
            SpawnChance = 20
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
            SpawnChance = 15
        });
    }

    private bool CorrectChancesCheck()
    {
        int sum = 0;
        foreach(var el in listOfElements)
        {
            sum += el.SpawnChance;
        }
        return sum == sumOfChances;
    }

    // Используется метод из этой статьи: https://jonlabelle.com/snippets/view/csharp/pick-random-elements-based-on-probability
    // Важно задать шансы так, чтобы суммарно они были равны 100
    // Подумать, как ввести предупреждение об этом на этапе компиляции
    public Element GetRandomElement()
    {
        int roll = Random.Range(1, 100);

        int cumulativeChance = 0;

        for (int i = 0; i < listOfElements.Count; i++)
        {
            cumulativeChance += listOfElements[i].SpawnChance;

            if (roll < cumulativeChance)
                return listOfElements[i];
        }
        return null;
    }
}
