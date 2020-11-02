using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class NextElementWindow : MonoBehaviour
{
    private Field nextElement;

    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject parentOfBlocks;

    private void Start()
    {
        nextElement = new Field
        {
            Width = 4,
            Height = 4,
            matrix = new FieldState[4, 4],
            field = new GameObject[4, 4],
        };

        nextElement.FillThePlayingField(blockPrefab,parentOfBlocks);
        nextElement.UpdateThePlayingField();

    }

    private void ShowState()
    {
        Debug.Log(nextElement);
    }




}
