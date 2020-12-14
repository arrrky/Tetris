using System;

public interface IRotate
{
    event Action ElementWasRotated;

    void Rotate();
}
