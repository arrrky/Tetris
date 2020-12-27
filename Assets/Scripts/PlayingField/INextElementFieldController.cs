using System;
using UnityEngine;

public interface INextElementFieldController : IFieldController
{
    void NextElementFieldControllerInit(Elements elements);
    event Action<FieldState[,], Field> FirstElementSpawned;        
    Element NextElement { get; set; }
}
