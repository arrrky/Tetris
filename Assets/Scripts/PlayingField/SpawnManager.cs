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
    private GameObject gameOverText;
    [SerializeField]
    private ElementMovement elementMovement;
    [SerializeField]
    private GameObject playerInput;
    [SerializeField]
    private GameObject levelController;

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
                    StartCoroutine(GameOver());
                }
                playingField.Matrix[y, x + spawnPoint] = element.Matrix[y, x];
            }
        }
        gameController.UpdateThePlayingField(playingField);
    }   

    //private Element GetRandomElement()
    //{
    //    return elements.listOfElements[Random.Range(0, elements.listOfElements.Count)];
    //}

    private IEnumerator GameOver()
    {
        gameOverText.SetActive(true);
        elementMovement.StopFallingDown();
        playerInput.SetActive(false);

        LevelController.Instance.Reset();

        yield return new WaitForSeconds(3f);
        Tools.CurrentSceneReload();
    }
}
