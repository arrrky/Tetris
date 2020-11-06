﻿using UnityEngine;
using MiscTools;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private Elements elements;
    [SerializeField]
    private PlayingFieldController playingFieldController;
    [SerializeField]
    private NextElementFieldController nextElementFieldController;    

    public const int spawnPoint = 4;   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnElement(elements.GetRandomElement().Matrix, nextElementFieldController.nextElementField);
            nextElementFieldController.UpdateThePlayingField(nextElementFieldController.nextElementField);
        }
    }

    public void SpawnElement(FieldState[,] element, Field field)
    {        
        for (int y = 0; y < element.GetLength(0); y++)
        {
            for (int x = 0; x < element.GetLength(1); x++)
            {
                field.Matrix[y, x] = element[y, x];
            }
        }
    }  

    public void SpawnRandomElement(Field playingField)
    {    
        Element element = elements.GetRandomElement();        
        playingFieldController.currentElementArray = element.Matrix;
        playingFieldController.currentElementSize = element.Matrix.GetLength(0);

        playingFieldController.topLeftPositionOfCurrentElement = playingFieldController.topLeftPositionDefault;      

        for (int y = 0; y < element.Matrix.GetLength(0); y++)
        {
            for (int x = 0; x < element.Matrix.GetLength(1); x++)
            {
                if (playingField.Matrix[y, x + spawnPoint] == FieldState.Fallen)
                {
                    StartCoroutine(gameController.GameOver());
                }
                playingField.Matrix[y, x + spawnPoint] = element.Matrix[y, x];
            }
        }
        playingFieldController.UpdateThePlayingField(playingField);
    }           
}
