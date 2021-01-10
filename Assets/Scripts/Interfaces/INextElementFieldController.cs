using System;

public interface INextElementFieldController : IFieldController
{
    void NextElementFieldControllerInit(ElementsList elements);
    event Action<FieldState[,], Field> FirstElementSpawned;        
    Element NextElement { get; set; }
}