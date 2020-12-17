using System;
using UnityEngine;

public interface IPlayingFieldController
{
    FieldState[,] CurrentElementArray { get; set; }
    Color32 CurrentElementColor { get; set; }
    int CurrentElementSize { get; set; }
    int FullRowsCount { get; set; }
    Field PlayingField { get; set; }
    Vector2 TopLeftPositionOfCurrentElement { get; set; }

    int PlayingFieldHeight { get; }
    int PlayingFieldWidth { get; }

    float PlayingFieldXShift { get; }
    float PlayingFieldYShift { get; }

    event Action ElementFell;
    event Action RowDeleted;

    void ClearField(Field field);
    void FallenToTemp(FieldState[,] tempMatrix);
    void FallingToFallen();
    void FillTheField(Field field, float xShift, float yShift);
    void FullRowCheck();
    void UpdateAfterMovement(FieldState[,] tempMatrix, Vector2 topLeftPointOfElementShift);
    void UpdateAfterRotation();
    void UpdatePlayingFieldState(Field field, Color32 elementColor);
}