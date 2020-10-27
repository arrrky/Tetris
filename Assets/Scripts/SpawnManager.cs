using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private PlayingFieldManager playingFieldManager;
    [SerializeField]
    private ElementsArrays elementsArrays;

    private List<int[,]> elements;

    public const int spawnPoint = 4;

    private void Awake()
    {
        elements = new List<int[,]> (elementsArrays.dicOfElements.Values);
    }

    public void SpawnElement(int[,] element, int[,] playingFieldMatrix)
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

    public void SpawnRandomElement(int[,] playingFieldMatrix)
    {
        // Временно, все равно надо писать свой рандомайзер с разными шансами выпадения
        int[,] element = GetRandomElement();
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

    private int[,] GetRandomElement()
    {
        return elements[Random.Range(0, elements.Count)];
    }
}
