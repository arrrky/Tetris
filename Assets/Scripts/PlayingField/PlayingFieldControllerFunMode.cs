using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingFieldControllerFunMode : PlayingFieldController, IPlayingFieldController
{
    public override int PlayingFieldHeight { get; } = 20;   
    public override int PlayingFieldWidth { get; } = 12;

    public override float PlayingFieldXShift { get; } = -5.5f;
   
}
