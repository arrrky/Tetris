using UnityEngine;
using MiscTools;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Elements elements;
    [SerializeField] private PlayingFieldController playingFieldController;
    [SerializeField] private NextElementFieldController nextElementFieldController;   

    public const int SPAWN_POINT = 4;

    private void Start()
    {
        gameController.GameStarted += SpawnRandomElement;
        playingFieldController.ElementFell += SpawnRandomElement;
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
        playingFieldController.currentElementColor = nextElementFieldController.nextElement.Color;

        playingFieldController.currentElementArray = element.Matrix;
        playingFieldController.currentElementSize = element.Matrix.GetLength(0);

        playingFieldController.TopLeftPositionOfCurrentElement = playingFieldController.topLeftPositionDefault;     
        

        for (int y = 0; y < element.Matrix.GetLength(0); y++)
        {
            for (int x = 0; x < element.Matrix.GetLength(1); x++)
            {
                if (playingFieldController.playingField.Matrix[y, x + SPAWN_POINT] == FieldState.Fallen)
                {
                    StartCoroutine(gameController.GameOverRoutine());
                }
                playingFieldController.playingField.Matrix[y, x + SPAWN_POINT] = element.Matrix[y, x];
            }
        }
        playingFieldController.UpdateThePlayingField(playingFieldController.playingField, playingFieldController.currentElementColor);
        SpawnNextElement();        
    }

    private void SpawnNextElement()
    {
        nextElementFieldController.nextElement = elements.GetRandomElement();     
        nextElementFieldController.ClearField(nextElementFieldController.nextElementField);
        SpawnElement(nextElementFieldController.nextElement.Matrix, nextElementFieldController.nextElementField);
        nextElementFieldController.UpdateThePlayingField(nextElementFieldController.nextElementField, nextElementFieldController.nextElement.Color);
    }
}
