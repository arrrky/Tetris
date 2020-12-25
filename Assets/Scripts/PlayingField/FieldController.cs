using UnityEngine;

public class FieldController : MonoBehaviour, IFieldController
{
    private GameObject blockPrefab;
    private GameObject blocksParent;    

    public Field Field { get; set; }
    public FieldState[,] CurrentElementArray { get; set; }
    public Color32 CurrentElementColor { get; set; }
    public int CurrentElementSize { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public float FieldXShift { get; set; }
    public float FieldYShift { get; set; }


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
                field.Blocks[y, x].Object = Instantiate(blockPrefab, new Vector3(x + xShift, field.Height - y + yShift, 0), Quaternion.identity, blocksParent.transform);
                field.Blocks[y, x].Sprite = field.Blocks[y, x].Object.GetComponent<SpriteRenderer>();
            }
        }
    }

    public void UpdatePlayingFieldState(Field field, Color32 elementColor)
    {
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Blocks[y, x].Object.SetActive(field.Blocks[y, x].State != FieldState.Empty);

                if (field.Blocks[y, x].State == FieldState.Falling)
                {
                    field.Blocks[y, x].Sprite.color = elementColor;
                }
            }
        }
    }

    /// <summary>
    /// Записываем временную матрицу в актуальную
    /// </summary>
    /// <param name="tempMatrix"></param>
    protected void TempToActual(FieldState[,] tempMatrix)
    {
        for (int y = Field.Height - 1; y > 0; y--)
        {
            for (int x = Field.Width - 1; x >= 0; x--)
            {
                Field.Blocks[y, x].State = tempMatrix[y, x];
            }
        }
    }

    public void ClearField(Field field)
    {
        for (int y = 0; y < field.Height; y++)
        {
            for (int x = 0; x < field.Width; x++)
            {
                field.Blocks[y, x].State = FieldState.Empty;
            }
        }
    }     
}
