using System;
using UnityEngine;

public interface IElementMovement
{
    void ElementsMovementInit(GameController gameController);

    event Action LastRowOrElementsCollided;
    event Action<FieldState[,], Vector2> ElementMoved;
    void FallingDown();
    void HorizontalMovement();
}
