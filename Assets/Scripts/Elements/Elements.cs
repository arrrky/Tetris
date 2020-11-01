using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class Elements : MonoBehaviour
{    
    private List<Element> listOfElements;
    private const int sumOfChances = 100;
    
    void Start()
    {
        listOfElements = new List<Element>();
        ElementsInitialization();
        ElementsSortByChance();
        AreChancesCorrect();
    }

    #region Инициализация элементов
    private void ElementsInitialization()
    {
        listOfElements.Add(new Element
        {
            Name = "O",
            Matrix = Tools.ConvertToFieldState(new int[,]
            {
                 {1,1},
                 {1,1}
            }),
            SpawnChance = 10
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
            SpawnChance = 10
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
            SpawnChance = 15
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
            SpawnChance = 20
        });
    }
    #endregion

    private bool AreChancesCorrect()
    {
        int sum = 0;
        foreach(var el in listOfElements)
        {
            sum += el.SpawnChance;
        }
        Debug.Log($"Summary spawn chances of elements: {sum}");
        return sum == sumOfChances;
    }

    // Используется метод из этой статьи: https://jonlabelle.com/snippets/view/csharp/pick-random-elements-based-on-probability
    // Важно задать шансы так, чтобы суммарно они были равны 100 (для корректной работы метода)
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

    // Возможно сортировка по шансу необходима для корректной работы выбора рандомного элемента
    // Обдумать!!!
    private void ElementsSortByChance()
    {        
        var elementsQuery =
            from element in listOfElements
            orderby element.SpawnChance
            select element;      

        listOfElements = elementsQuery.ToList();
    }
}
