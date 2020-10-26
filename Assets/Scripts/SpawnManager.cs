using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Элемент будет спавниться или в самом верхнем ряду, или даже выходя за верхнюю границу (посмотреть как лучше)
    // Точка спавна будет примерно посередине, и с этого значения будет заполняться матрица-поле

    public static int spawnPoint = 4;
    public int currentElementSize;


    void Start()
    {

    }

    public static void SpawnElement(int[,] element, int[,] playingFieldMatrix)
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

    public static void SpawnRandomElement(int[,] playingFieldMatrix)
    {  
        int[,] element = ElementsArrays.elementsList[Random.Range(0, ElementsArrays.elementsList.Count)];
        PlayingFieldManager.currentElementArray = element;
        PlayingFieldManager.currentElementSize = element.GetLength(0);

        PlayingFieldManager.topLeftPositionOfCurrentElement = PlayingFieldManager.topLeftPositionDefault;
        //ElementsArrays.elementsArrays.TryGetValue("J", out int[,] element);

        for (int y = 0; y < element.GetLength(0); y++)
        {
            for (int x = 0; x < element.GetLength(1); x++)
            {
                playingFieldMatrix[y, x + spawnPoint] = element[y, x];
            }
        }
        PlayingFieldManager.UpdateThePlayingField();
    }   
}
