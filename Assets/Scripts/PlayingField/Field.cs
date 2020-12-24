using UnityEngine;

public enum FieldState
{
    Empty = 0,
    Falling = 1,
    Fallen = 2
}

public class Field
{  
    public int Width { get; set; }    
    public int Height { get; set; }   
    public GameObject[,] Objects { get; set; }
    public SpriteRenderer[,] Sprites { get; set; }
    public FieldState[,] Matrix { get; set; }
    
    public Field(int width, int height)
    {
        Width = width;
        Height = height;
        Objects = new GameObject[Width, Height];
        Sprites = new SpriteRenderer[Width, Height];
        Matrix = new FieldState[Width, Height];
    }
}
