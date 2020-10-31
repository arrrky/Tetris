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

    public void SpawnRandomElement(FieldState[,] playingFieldMatrix)
    {    
        Element element = elements.GetRandomElement();
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

    //private Element GetRandomElement()
    //{
    //    return elements.listOfElements[Random.Range(0, elements.listOfElements.Count)];
    //}
}
