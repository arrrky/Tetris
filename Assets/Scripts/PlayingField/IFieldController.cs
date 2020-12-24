using UnityEngine;

public interface IFieldController
{
    void FieldControllerInit(GameObject blockPrefab, GameObject blocksParent);    

    Field Field { get; set; }
    FieldState[,] CurrentElementArray { get; set; }
    Color32 CurrentElementColor { get; set; }
    float FieldXShift { get; set; } 
    float FieldYShift { get; set; } 
    int CurrentElementSize { get; set; }
    int Height { get; }
    int Width { get; }

    void FillTheField(Field field, float xShift, float yShift);
    void ClearField(Field field);
    void UpdatePlayingFieldState(Field field, Color32 elementColor);
}
