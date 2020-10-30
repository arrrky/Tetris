namespace MiscTools
{
    public enum FieldState
    {
        Empty = 0,
        Falling = 1,
        Fallen = 2
    }

    public class Tools
    {
        public static FieldState[,] ConvertToFieldState(int[,] element)
        {
            FieldState[,] temp = new FieldState[element.GetLength(0), element.GetLength(1)];

            for (int y = 0; y < element.GetLength(0); y++)
                for (int x = 0; x < element.GetLength(1); x++)
                    temp[y, x] = (FieldState)element[y, x];
            return temp;
        }
    }
}