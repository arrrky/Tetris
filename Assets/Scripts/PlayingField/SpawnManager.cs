using UnityEngine;
using MiscTools;

public class SpawnManager : MonoBehaviour
{   
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private Elements elements;   

    private Field playingField;

    public const int spawnPoint = 4;

    private void Start()
    {
        playingField = gameController.playingField;
    }

    public void SpawnElement(FieldState[,] element, Field playingField)
    {        
        for (int y = 0; y < element.GetLength(0); y++)
        {
            for (int x = 0; x < element.GetLength(1); x++)
            {
                playingField.Matrix[y, x + spawnPoint] = element[y, x];
            }
        }
    }  

    public void SpawnRandomElement(Field playingField)
    {    
        Element element = elements.GetRandomElement();        
        gameController.currentElementArray = element.Matrix;
        gameController.currentElementSize = element.Matrix.GetLength(0);

        gameController.topLeftPositionOfCurrentElement = gameController.topLeftPositionDefault;      

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
        gameController.UpdateThePlayingField(playingField);
    }       
}
