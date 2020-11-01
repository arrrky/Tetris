using UnityEngine;
using System.Collections;
using MiscTools;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private PlayingFieldManager playingFieldManager;
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
                if (playingFieldMatrix[y, x + spawnPoint] == FieldState.Fallen)
                {
                    StartCoroutine(GameOver());
                }                    
                playingFieldMatrix[y, x + spawnPoint] = element.Matrix[y, x];
            }
        }
        playingFieldManager.UpdateThePlayingField();
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
