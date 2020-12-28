using UnityEngine;

public class FieldController : MonoBehaviour, IFieldController
{
    private GameObject blockPrefab;
    private GameObject blocksParent;

    #region PROPERTIES
    public Field Field { get; set; }
    public FieldState[,] CurrentElementArray { get; set; }
    public Color32 CurrentElementColor { get; set; }
    public int CurrentElementSize { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public float FieldXShift { get; set; }
    public float FieldYShift { get; set; }
    #endregion

    public void FieldControllerInit(GameObject blockPrefab, GameObject blocksParent)
    {
        this.blockPrefab = blockPrefab;
        this.blocksParent = blocksParent;       
    }

    public void FieldInit()
    {
        Field = new Field(Width, Height);       
        FillTheField(Field, FieldXShift, FieldYShift);
        UpdatePlayingFieldState(Field, CurrentElementColor);
    }

    public void FillTheField(Field field, float xShift, float yShift)
    {
        // Из-за разницы в нумерации элементов матрицы-поля и отсчета координат в Unity удобнее инициализировать объекты именно таким образом.
        // Поэтому 'y' кооордината инстанирования имеет вид height - y - yShift (где Shift - смещение по осям),
        // чтобы блоки заполнялись сверху вниз (как в матрице-поле).

        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Objects[y, x] = Instantiate(blockPrefab, new Vector3(x + xShift, field.Height - y + yShift, 0), Quaternion.identity, blocksParent.transform);
                field.Sprites[y, x] = field.Objects[y, x].GetComponent<SpriteRenderer>();
            }
        }
    }

    public void UpdatePlayingFieldState(Field field, Color32 elementColor)
    {
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Objects[y, x].SetActive(field.Matrix[y, x] != FieldState.Empty);

                if (field.Matrix[y, x] == FieldState.Moving)
                {
                    field.Sprites[y, x].color = elementColor;
                }
            }
        }
    }    

    public void ClearField(Field field)
    {
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Matrix[y, x] = FieldState.Empty;
            }
        }
    }     
}
