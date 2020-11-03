using UnityEngine;
using MiscTools;

public class Field : MonoBehaviour
{ 
    private int width;
    private int height;

    public int Width
    {
        get
        {
            return width;
        }
        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }
        set
        {
            height = value;
        }
    }

    public GameObject[,] field; 
    public FieldState[,] matrix;

    public void FillThePlayingField(GameObject blockPrefab, GameObject parentOfBlocks, int xShift, int yShift)
    {
        // Из-за разницы в нумерации элементов матрицы-поля и отсчета координат в Unity удобнее инициализировать объекты именно таким образом.
        // Поэтому 'y' кооордината инстанирования имеет вид height - y - yShift (где Shift - смещение по осям),
        // чтобы блоки заполнялись сверху вниз (как в матрице-поле).

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                field[y, x] = Instantiate(blockPrefab, new Vector3(x + xShift, Height - y + yShift, 0), Quaternion.identity, parentOfBlocks.transform);
            }
        }
    }   

    /// <summary>
    /// Обновление состояния игрового поля
    /// </summary>
    public void UpdateThePlayingField()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                field[y, x].SetActive(matrix[y, x] != FieldState.Empty);
            }
        }
    } 
}
