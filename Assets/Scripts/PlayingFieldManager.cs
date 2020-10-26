using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingFieldManager : MonoBehaviour
{
    // Вопрос о целесообразности использования этого enum'a остается открытым
    // Вроде так оно более читаемо, и понятно что такое 0, 1 и 2 
    // С другой стороны в методах приходится постоянно кастить их к int'ам
    // Пока останется комбинированный вариант
    public enum FieldState
    {
        Empty = 0,
        Falling = 1,
        Fallen = 2
    }

    private static int width = 10;
    private static int height = 20;

    public static int Width
    {
        get
        {
            return width;
        }
    }

    public static int Height
    {
        get
        {
            return height;
        }
    }

    // Матрица-поле заполняется 3-мя типами значений-состояний: 0 - пустое место, 1 - двигающийся блок, 2 - упавший блок
    public static int[,] playingFieldMatrix = new int[Height, Width];
    private static GameObject[,] playingField = new GameObject[Height, Width];
    public static int[,] currentElementArray = null;
    public static int currentElementSize;
    public static Vector2 topLeftPositionDefault = new Vector2(SpawnManager.spawnPoint, 0);
    public static Vector2 topLeftPositionOfCurrentElement;

    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject parentOfBlocks;

    void Start()
    {
        FillThePlayingField();
        topLeftPositionOfCurrentElement = topLeftPositionDefault;        
        //CreateRandomFieldState();   
        SpawnManager.SpawnRandomElement(playingFieldMatrix);
        //SpawnManager.SpawnElement(ElementsArrays.elementsArrays["O"], playingFieldMatrix);
        
    }

    void Update()
    {
    }

    void FillThePlayingField()
    {
        // Из-за разницы в нумерации элементов матрицы-поля и отсчета координат в Unity удобнее инициализировать объекты именно таким образом.
        // Поэтому 'y' кооордината инстанирования имеет вид height - y - 1, чтобы блоки заполнялись сверху вниз (как в матрице-поле).

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                playingField[y, x] = Instantiate(blockPrefab, new Vector3(x, height - y - 1, 0), Quaternion.identity, parentOfBlocks.transform);
            }
        }
    }

    // Тестовый метод, потом можно выпилить
    void CreateRandomFieldState()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                playingFieldMatrix[y, x] = Random.Range(0, 2);
            }
        }
    }

    // Обновление состояния поля
    public static void UpdateThePlayingField()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (playingFieldMatrix[y, x] > 0) // т.е. блок или движется, или уже упал
                    playingField[y, x].SetActive(true);
                else
                    playingField[y, x].SetActive(false);
            }
        }
    }

    // Меняем состояние элементов с 1 на 2 (падающие на упавшие)
    public static void FallingToFallen()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (playingFieldMatrix[y, x] > 0)
                    playingFieldMatrix[y, x] = (int)FieldState.Fallen;
            }
        }
        SpawnManager.SpawnRandomElement(playingFieldMatrix);
        //SpawnManager.SpawnElement(ElementsArrays.elementsArrays["O"], playingFieldMatrix);
    }

    public static void FullRowCheck()
    {
        for (int y = Height - 1; y >= 0; y--)
        {
            int rowSum = 0;
            for (int x = Width - 1; x >= 0; x--)
            {
                rowSum += playingFieldMatrix[y, x];
            }
            if (rowSum == Width * 2) // в ряду должно быть 10 (Width) элементов со значением 2 (Fallen)
                DeleteFullRow(y);
        }        
    }

    public static void DeleteFullRow(int rowNumber)
    {
        for (int x = Width - 1; x >= 0; x--)
        {
            playingFieldMatrix[rowNumber, x] = 0;
        }

        // Смещаем вниз на все элементы над уничтоженным рядом
        for (int y = rowNumber - 1; y >= 0; y--)
        {            
            for (int x = Width - 1; x >= 0; x--)
            {
                // Проверка, чтобы опускать падающий элемент
                if (playingFieldMatrix[y, x] == (int)FieldState.Falling)
                    return;
                playingFieldMatrix[y + 1, x] = playingFieldMatrix[y, x];
            }     
        }       
    }

    // Если перед смещением в оригинальной матрице уже были упавшие элементы - запишем их во временную матрицу
    public static void FallenToTemp(int[,] tempMatrix)
    {
        for (int y = PlayingFieldManager.Height - 1; y > 0; y--)
        {
            for (int x = PlayingFieldManager.Width - 1; x >= 0; x--)
            {
                if (PlayingFieldManager.playingFieldMatrix[y, x] == (int)PlayingFieldManager.FieldState.Fallen)
                {
                    tempMatrix[y, x] = (int)PlayingFieldManager.FieldState.Fallen;
                }
            }
        }
    }
}
