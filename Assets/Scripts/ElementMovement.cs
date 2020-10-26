using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementMovement : MonoBehaviour
{
    private int leftBorderRotateRestriction;
    private int rightBorderRotateRestriction;


    void Start()
    {        
        //InvokeRepeating("FallingDown", 1f, 1f);
    }

    

    //private IEnumerator FallingDownManual()
    //{
    //    while (true)
    //    {
    //        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
    //        FallingDown();
    //        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
    //       // yield return new WaitForSeconds(0.05f); // скорость падения при зажатой клавише (меньше - быстрее)   

    //    }
    //}



    public void FallingDown()
    {        
        // Во временную матрицу будем записывать поле с уже смещенным элементом
        int[,] tempMatrix = new int[PlayingFieldManager.Height, PlayingFieldManager.Width];
        PlayingFieldManager.FallenToTemp(tempMatrix);       

        // Меняем состояние в матрице-поле снизу вверх, иначе (при проходе сверху вниз) мы будем проходить по уже измененным элементам
        // и элемент "упадет" за один проход циклов
        for (int y = PlayingFieldManager.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < PlayingFieldManager.Width; x++)
            {
                if (y > 0)
                {
                    if (IsFallingElementAbove(x, y))
                    {
                        PlayingFieldManager.FallingToFallen();
                        return;
                    }
                }

                if (IsLastRow(x, y))
                {
                    PlayingFieldManager.FallingToFallen();
                    return;
                }

                // Смещение по вертикали, если ряд не последний
                if (!IsLastRow(x, y))
                {
                    if (PlayingFieldManager.playingFieldMatrix[y, x] == (int)PlayingFieldManager.FieldState.Falling)
                    {
                        tempMatrix[y + 1, x] = (int)PlayingFieldManager.FieldState.Falling;
                    }
                }
            }
        }
        PlayingFieldManager.topLeftPositionOfCurrentElement += new Vector2(0, 1);    
        WriteAndUpdate(tempMatrix);
    }

    // Если текущий элемент - упавший, а над ним - падающий
    private static bool IsFallingElementAbove(int x, int y)
    {
        return (PlayingFieldManager.playingFieldMatrix[y, x] == (int)PlayingFieldManager.FieldState.Fallen &&
                PlayingFieldManager.playingFieldMatrix[y - 1, x] == (int)PlayingFieldManager.FieldState.Falling);
    }

    // Если последний ряд
    private static bool IsLastRow(int x, int y)
    {
        return (y == PlayingFieldManager.Height - 1 &&
                PlayingFieldManager.playingFieldMatrix[y, x] == (int)PlayingFieldManager.FieldState.Falling);
    }    

    // Записываем новую (временную) матрицу в оригинальную и обновляем поле
    protected static void WriteAndUpdate(int[,] tempMatrix)
    {
        PlayingFieldManager.playingFieldMatrix = tempMatrix;
        PlayingFieldManager.FullRowCheck();
        PlayingFieldManager.UpdateThePlayingField();
    }

    // Методы вроде рабочие, если вскроются проблемы с объединенным методом - можно использовать их

    //private IEnumerator MoveToTheRightManual()
    //{
    //    while (true)
    //    {
    //        yield return new WaitUntil(() => Input.GetButtonDown("MoveToTheRight"));
    //        MoveToTheRight(PlayingFieldManager.playingFieldMatrix);
    //        yield return new WaitUntil(() => Input.GetButtonDown("MoveToTheRight"));
    //    }
    //}

    //private IEnumerator MoveToTheLeftManual()
    //{
    //    while (true)
    //    {
    //        yield return new WaitUntil(() => Input.GetButtonDown("MoveToTheLeft"));
    //        MoveToTheLeft(PlayingFieldManager.playingFieldMatrix);
    //        yield return new WaitUntil(() => Input.GetButtonDown("MoveToTheLeft"));
    //    }
    //}

    //private void MoveToTheRight(int[,] playingFieldMatrix)
    //{
    //    int[,] tempMatrix = new int[PlayingFieldManager.Height, PlayingFieldManager.Width];
    //    int direction = 1;
    //    for (int y = PlayingFieldManager.Height - 1; y > 0; y--)
    //    {
    //        for (int x = PlayingFieldManager.Width - 1; x >= 0; x--)
    //        {
    //            if (playingFieldMatrix[y, x] == (int)PlayingFieldManager.FieldState.Falling)
    //            {
    //                if (IsRightBorderNear(x))
    //                    return;
    //                tempMatrix[y, x + direction] = (int)PlayingFieldManager.FieldState.Falling;
    //            }
    //            FallenToTemp(tempMatrix);
    //        }
    //    }
    //    WriteAndUpdate(tempMatrix);
    //}

    //private void MoveToTheLeft(int[,] playingFieldMatrix)
    //{
    //    int[,] tempMatrix = new int[PlayingFieldManager.Height, PlayingFieldManager.Width];
    //    int direction = -1;
    //    for (int y = PlayingFieldManager.Height - 1; y > 0; y--)
    //    {
    //        for (int x = PlayingFieldManager.Width - 1; x >= 0; x--)
    //        {
    //            if (playingFieldMatrix[y, x] == (int)PlayingFieldManager.FieldState.Falling)
    //            {
    //                if (IsLeftBorderNear(x))
    //                    return;
    //                tempMatrix[y, x + direction] = (int)PlayingFieldManager.FieldState.Falling;
    //            }
    //            FallenToTemp(tempMatrix);
    //        }
    //    }
    //    WriteAndUpdate(tempMatrix);
    //}

    // Пока не удаляю, потому что могут пригодиться в проверках вращения

    private static bool IsLeftBorderNear(int x)
    {
        return x == 0;
    }

    private static bool IsRightBorderNear(int x)
    {
        return x == PlayingFieldManager.Width - 1;
    }

    private static bool IsOtherBlockNear(int x, int y, int direction)
    {
        return (PlayingFieldManager.playingFieldMatrix[y, x + direction] == (int)PlayingFieldManager.FieldState.Fallen);           
    }

    private delegate bool BorderCheck(int x);
    private static BorderCheck borderCheck = null;

    public void HorizontalMovement()
    {
        Debug.Log(PlayingFieldManager.topLeftPositionOfCurrentElement);

        leftBorderRotateRestriction = PlayingFieldManager.currentElementSize - 1;
        rightBorderRotateRestriction = PlayingFieldManager.Width - PlayingFieldManager.currentElementSize - 1;

        int[,] tempMatrix = new int[PlayingFieldManager.Height, PlayingFieldManager.Width];
        PlayingFieldManager.FallenToTemp(tempMatrix);
        int direction = 0;

        if (Input.GetButtonDown("MoveToTheRight"))
        {
            direction = 1;
            borderCheck = IsRightBorderNear;
        }
        if (Input.GetButtonDown("MoveToTheLeft"))
        {
            direction = -1;
            borderCheck = IsLeftBorderNear;
        }

        for (int y = PlayingFieldManager.Height - 1; y >= 0; y--)
        {
            for (int x = PlayingFieldManager.Width - 1; x >= 0; x--)
            {
                if (PlayingFieldManager.playingFieldMatrix[y, x] == (int)PlayingFieldManager.FieldState.Falling)
                {
                    if (borderCheck(x))
                        return;
                    if (IsOtherBlockNear(x, y, direction))
                        return;
                    tempMatrix[y, x + direction] = (int)PlayingFieldManager.FieldState.Falling;
                }
            }
        }
        WriteAndUpdate(tempMatrix);
       
        PlayingFieldManager.topLeftPositionOfCurrentElement += new Vector2(direction, 0);       
    }

    //private IEnumerator HorizontalMovementManual()
    //{
    //    while (true)
    //    {
    //        yield return new WaitUntil(() => Input.GetButtonDown("Horizontal"));
    //        HorizontalMovement(PlayingFieldManager.playingFieldMatrix);         
    //        //yield return new WaitUntil(() => Input.GetButtonDown("Horizontal"));
    //    }
    //}

}
