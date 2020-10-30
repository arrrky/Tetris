using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private PlayingFieldManager playingFieldManager;
    [SerializeField]
    private Elements elements; 

    public const int spawnPoint = 4;    

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
        Element element = GetRandomElement();
        playingFieldManager.currentElementArray = element.Matrix;
        playingFieldManager.currentElementSize = element.Matrix.GetLength(0);

        playingFieldManager.topLeftPositionOfCurrentElement = playingFieldManager.topLeftPositionDefault;      

        for (int y = 0; y < element.Matrix.GetLength(0); y++)
        {
            for (int x = 0; x < element.Matrix.GetLength(1); x++)
            {
                playingFieldMatrix[y, x + spawnPoint] = element.Matrix[y, x];
            }
        }
        playingFieldManager.UpdateThePlayingField();
    }   

    private Element GetRandomElement()
    {
        return elements.listOfElements[Random.Range(0, elements.listOfElements.Count)];
    }
}
