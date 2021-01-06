using System;

public interface IElementRotation
{
    void ElementRotationInit(GameController gameController, IPlayingFieldController playingFieldController);
    event Action ElementWasRotated;
    void Rotate();
}
