using UnityEngine;

public class Element
{
    public string Name { get; set; }
    public FieldState[,] Matrix { get; set; }
    public int SpawnChance { get; set; }
    public Color32 Color { get; set; }
}
