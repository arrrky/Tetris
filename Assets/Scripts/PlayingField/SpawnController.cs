using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Elements elements;
    [SerializeField] private PlayingFieldController playingFieldController;
    [SerializeField] private NextElementFieldController nextElementFieldController;

    public const int SpawnPoint = 4;

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
        Element element = nextElementFieldController.NextElement;
        playingFieldController.CurrentElementColor = nextElementFieldController.NextElement.Color;

        playingFieldController.CurrentElementArray = element.Matrix;
        playingFieldController.CurrentElementSize = element.Matrix.GetLength(0);

        playingFieldController.TopLeftPositionOfCurrentElement = playingFieldController.TopLeftPositionDefault;

        for (int y = 0; y < element.Matrix.GetLength(0); y++)
        {
            for (int x = 0; x < element.Matrix.GetLength(1); x++)
            {
                if (playingFieldController.PlayingField.Matrix[y, x + SpawnPoint] == FieldState.Fallen)
                {
                    PlayerProfileController.Instance.CallSavePlayerData();
                    StartCoroutine(gameController.GameOverRoutine());                    
                }
                playingFieldController.PlayingField.Matrix[y, x + SpawnPoint] = element.Matrix[y, x];
            }
        }
        playingFieldController.UpdatePlayingFieldState(playingFieldController.PlayingField, playingFieldController.CurrentElementColor);
        SpawnNextElement();
    }

    private void SpawnNextElement()
    {
        nextElementFieldController.NextElement = elements.GetRandomElement();
        nextElementFieldController.ClearField(nextElementFieldController.NextElementField);
        SpawnElement(nextElementFieldController.NextElement.Matrix, nextElementFieldController.NextElementField);
        nextElementFieldController.UpdatePlayingFieldState(nextElementFieldController.NextElementField, nextElementFieldController.NextElement.Color);
    }
}
