using System;
using UnityEngine;

public interface IPlayingFieldController : IFieldController
{
    void PlayingFieldControllerInit(IMove elementMovement, IRotate elementRotation);

    int FullRowsCount { get; set; }    
    Vector2 TopLeftPositionOfCurrentElement { get; set; }       

    event Action ElementFell;
    event Action RowDeleted;
    
    void FallenToTemp(FieldState[,] tempMatrix);
    void FallingToFallen();    
    void FullRowCheck();
    void UpdateAfterMovement(FieldState[,] tempMatrix, Vector2 topLeftPointOfElementShift);
    void UpdateAfterRotation();  
}