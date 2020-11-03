using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class NextElement : MonoBehaviour
{
    private Field nextElement;

    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject parentOfBlocks;

    private const int heightOfField = 4;
    private const int widthOfField = 4;
    private const int xShift = 15;
    private const int yShift = 3;

    private void Start()
    {
        nextElement = new Field
        {
            Height = heightOfField,
            Width = widthOfField,
            matrix = new FieldState[heightOfField, widthOfField],
            field = new GameObject[heightOfField, widthOfField],
        };
        nextElement.FillThePlayingField(blockPrefab, parentOfBlocks, xShift, yShift);
        nextElement.UpdateThePlayingField();
    }
}
