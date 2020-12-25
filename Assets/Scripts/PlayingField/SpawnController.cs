using UnityEngine;
using Zenject;

public class SpawnController : MonoBehaviour
{
    private GameController gameController;
    private INextElementFieldController nextElementFieldController;
    private IPlayingFieldController playingFieldController;
    private Elements elements;

    public static int SpawnPoint = 4;
   
    public void SpawnControllerInit(GameController gameController, INextElementFieldController nextElementFieldController, IPlayingFieldController playingFieldController)
    {
        this.gameController = gameController;
        this.nextElementFieldController = nextElementFieldController;
        this.playingFieldController = playingFieldController;
    }

    private void Start()
    {
        EventsSetup();
    }

    [Inject]
    private void ElementsInit(Elements elements)
    {
        this.elements = elements;
    }

    private void EventsSetup()
    {
        nextElementFieldController.FirstElementSpawned += SpawnElement;
        gameController.GameStarted += SpawnRandomElement;
        playingFieldController.ElementFell += SpawnRandomElement;
    }

    public void SpawnElement(FieldState[,] element, Field field)
    {
        for (int y = 0; y < element.GetLength(0); y++)
        {
            for (int x = 0; x < element.GetLength(1); x++)
            {
                field.Blocks[y, x].State = element[y, x];
            }
        }
    }

    public void SpawnRandomElement()
    {      
        Element element = nextElementFieldController.NextElement;
        playingFieldController.CurrentElementColor = nextElementFieldController.NextElement.Color;

        playingFieldController.CurrentElementArray = element.Matrix;
        playingFieldController.CurrentElementSize = element.Matrix.GetLength(0);

        playingFieldController.TopLeftPositionOfCurrentElement = new Vector2(SpawnPoint, 0);

        for (int y = 0; y < element.Matrix.GetLength(0); y++)
        {
            for (int x = 0; x < element.Matrix.GetLength(1); x++)
            {
                if (playingFieldController.Field.Blocks[y, x + SpawnPoint].State == FieldState.Fallen)
                {
                    PlayerProfileController.Instance.CallSavePlayerData();
                    StartCoroutine(gameController.GameOverRoutine());                    
                }
                playingFieldController.Field.Blocks[y, x + SpawnPoint].State = element.Matrix[y, x];
            }
        }
        playingFieldController.UpdatePlayingFieldState(playingFieldController.Field, playingFieldController.CurrentElementColor);
        SpawnNextElement();
    }

    private void SpawnNextElement()
    {
        nextElementFieldController.NextElement = elements.GetRandomElement();
        nextElementFieldController.ClearField(nextElementFieldController.Field);
        SpawnElement(nextElementFieldController.NextElement.Matrix, nextElementFieldController.Field);
        nextElementFieldController.UpdatePlayingFieldState(nextElementFieldController.Field, nextElementFieldController.NextElement.Color);
    }
}
