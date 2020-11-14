using UnityEngine;
using System.Collections;
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
    [SerializeField]
    private ElementMovement elementMovement;

    private const float timeBeforeFirstSpawn = 2f;

    public const int spawnPoint = 4;

    private void Start()
    {
        StartCoroutine(DelayedSpawn());
        elementMovement.LastRowOrElementsCollide += SpawnRandomElement;
    }      

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(timeBeforeFirstSpawn);
        SpawnRandomElement();        
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

    public void SpawnRandomElement()
    {
        Element element = nextElementFieldController.nextElement;       
        playingFieldController.currentElementArray = element.Matrix;
        playingFieldController.currentElementSize = element.Matrix.GetLength(0);

        playingFieldController.topLeftPositionOfCurrentElement = playingFieldController.topLeftPositionDefault;      

        for (int y = 0; y < element.Matrix.GetLength(0); y++)
        {
            for (int x = 0; x < element.Matrix.GetLength(1); x++)
            {
                if (playingFieldController.playingField.Matrix[y, x + spawnPoint] == FieldState.Fallen)
                {
                    StartCoroutine(gameController.GameOver());
                }
                playingFieldController.playingField.Matrix[y, x + spawnPoint] = element.Matrix[y, x];
            }
        }
        playingFieldController.UpdateThePlayingField(playingFieldController.playingField);
        SpawnNextElement();        
    }

    private void SpawnNextElement()
    {
        nextElementFieldController.nextElement = elements.GetRandomElement();
        nextElementFieldController.ClearField(nextElementFieldController.nextElementField);
        SpawnElement(nextElementFieldController.nextElement.Matrix, nextElementFieldController.nextElementField);
        nextElementFieldController.UpdateThePlayingField(nextElementFieldController.nextElementField);
    }
}
