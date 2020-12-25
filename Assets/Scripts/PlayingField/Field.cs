public class Field
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Block[,] Blocks { get; set; }

    public Field(int width, int height)
    {
        Width = width;
        Height = height;

        Blocks = new Block[Height, Width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Blocks[y, x] = new Block();
            }
        }
    }
}
