using System;
using UnityEngine;

public interface INextElementFieldController : IFieldController
{
    void NextElementFieldControllerInit();
    event Action<FieldState[,], Field> FirstElementSpawned;        
    Element NextElement { get; set; }
}
