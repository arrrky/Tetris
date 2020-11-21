using UnityEngine;
using MiscTools;

public class Field : MonoBehaviour
{  
    public int Width { get; set; }    
    public int Height { get; set; }   
    public GameObject[,] Objects { get; set; }
    public SpriteRenderer[,] Sprites { get; set; }
    public FieldState[,] Matrix { get; set; }
}
