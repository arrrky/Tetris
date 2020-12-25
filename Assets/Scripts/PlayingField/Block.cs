using UnityEngine;

public enum FieldState
{
    Empty = 0,
    Falling = 1,
    Fallen = 2
}

public class Block
{
    public GameObject Object { get; set; }
    public SpriteRenderer Sprite { get; set; }
    public FieldState State { get; set; }    
}


