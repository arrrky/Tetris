using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private PlayingFieldManager playingFieldManager;
    [SerializeField]
    private ElementsArrays elementsArrays;

    private List<int[,]> tempElements;
    private List<FieldState[,]> listOfElements;

    public const int spawnPoint = 4;

    private void Awake()
    {
        // Временное решение, связанное с полным переходом на матрицы из enum'ов
        tempElements = new List<int[,]>(elementsArrays.dicOfElements.Values);
        listOfElements = new List<FieldState[,]>();
        foreach(var elements in tempElements)
        {
            listOfElements.Add(ElementsArrays.ConvertToFieldState(elements));
        }        
    }

    public void SpawnElement(FieldState[,] element, FieldState[,] playingFieldMatrix)
    {
        
        for (int y = 0; y < element.GetLength(0); y++)
        {
            for (int x = 0; x < element.GetLength(1); x++)
            {
                playingFieldMatrix[y, x + spawnPoint] = element[y, x];
            }
        }
    }

    //public static IEnumerator SpawnRandomElement(int[,] playingFieldMatrix)
    //{
    //    while (true)
    //    {
    //        int[,] element = ElementsArrays.elementsList[Random.Range(0, ElementsArrays.elementsList.Count)];
    //        for (int y = 0; y < element.GetLength(0); y++)
    //        {
    //            for (int x = 0; x < element.GetLength(1); x++)
    //            {
    //                playingFieldMatrix[y, x + spawnPoint] = element[y, x];
    //            }
    //        }
    //        PlayingFieldManager.DrawThePlayingField();
    //        yield return new WaitForSeconds(1f);
    //    }
    //}

    public void SpawnRandomElement(FieldState[,] playingFieldMatrix)
    {             
        // Временно, все равно надо писать свой рандомайзер с разными шансами выпадения
        FieldState[,] element = GetRandomElement();
        playingFieldManager.currentElementArray = element;
        playingFieldManager.currentElementSize = element.GetLength(0);

        playingFieldManager.topLeftPositionOfCurrentElement = playingFieldManager.topLeftPositionDefault;      

        for (int y = 0; y < element.GetLength(0); y++)
        {
            for (int x = 0; x < element.GetLength(1); x++)
            {
                playingFieldMatrix[y, x + spawnPoint] = element[y, x];
            }
        }
        playingFieldManager.UpdateThePlayingField();
    }   

    private FieldState[,] GetRandomElement()
    {
        return listOfElements[Random.Range(0, tempElements.Count)];
    }
}
