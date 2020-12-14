using System;
using UnityEngine;

public interface IMove
{
    event Action LastRowOrElementsCollided;
    event Action<FieldState[,], Vector2> ElementMoved;
    void FallingDown();
    void HorizontalMovement();
}
