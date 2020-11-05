using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class Elements : MonoBehaviour
{    
    private List<Element> listOfElements;
    private const int sumOfChances = 100;

    // Нужно инициализировать список элементов ДО спауна первого элемента
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

    // Используется метод из этой статьи: https://jonlabelle.com/snippets/view/csharp/pick-random-elements-based-on-probability
    // Важно задать шансы так, чтобы суммарно они были равны 100 (для корректной работы метода)
    // Метод будет работать в любом случае, просто шанс выпадения элемента не будет статистически верен
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
    
    private void ElementsSortByChance()
    {        
        var sortedElements =
            from element in listOfElements
            orderby element.SpawnChance
            select element;      

        listOfElements = sortedElements.ToList();
    }
}
