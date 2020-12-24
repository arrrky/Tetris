using System;
using UnityEngine;

public interface INextElementFieldController : IFieldController
{
    event Action<FieldState[,], Field> FirstElementSpawned;        
    Element NextElement { get; set; }
}
