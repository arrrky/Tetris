using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject parentOfBlocks;
    [SerializeField]
    private ScoreController scoreController;
    [SerializeField]
    private SpawnManager spawnManager;

    public PlayingField playingField;
    public int currentElementSize;
    public FieldState[,] currentElementArray;
    public Vector2 topLeftPositionDefault;
    public Vector2 topLeftPositionOfCurrentElement;
    public int fullRowsCount = 0;


    private void Start()
    {
        PlayingFieldInit();
        topLeftPositionDefault = new Vector2(SpawnManager.spawnPoint, 0);
        topLeftPositionOfCurrentElement = topLeftPositionDefault;

        LevelController.Instance.InitializeLevel();

        spawnManager.SpawnRandomElement(playingField);
    }

    private void PlayingFieldInit()
    {
        playingField = gameObject.AddComponent<PlayingField>();
        playingField.Height = 20;
        playingField.Width = 10;
        playingField.matrix = new FieldState[20, 10];
        playingField.field = new GameObject[20, 10];        
        playingField.FillThePlayingField(blockPrefab, parentOfBlocks, 0, -1);
        playingField.UpdateThePlayingField();
    }
}
