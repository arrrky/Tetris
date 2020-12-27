using System;

public interface IElementRotation
{
    void ElementRotationInit(IPlayingFieldController playingFieldController);
    event Action ElementWasRotated;
    void Rotate();
}
