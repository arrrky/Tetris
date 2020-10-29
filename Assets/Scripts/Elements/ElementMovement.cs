using System;
using UnityEngine;
using MiscTools;

public class ElementMovement : MonoBehaviour
{
    [SerializeField]
    private PlayingFieldManager playingFieldManager;

    void Start()
    {
        InvokeRepeating("FallingDown", 1f, 1f);
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

    public void StopFallingDown()
    {
        CancelInvoke("FallingDown");
    }

    public void FallingDown()
    {
        // Во временную матрицу будем записывать поле с уже смещенным элементом
        FieldState[,] tempMatrix = new FieldState[playingFieldManager.Height, playingFieldManager.Width];
        playingFieldManager.FallenToTemp(tempMatrix);

        // Меняем состояние в матрице-поле снизу вверх, иначе (при проходе сверху вниз) мы будем проходить по уже измененным элементам
        // и элемент "упадет" за один проход циклов
        for (int y = playingFieldManager.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < playingFieldManager.Width; x++)
            {
                if (y > 0)
                {
                    if (IsFallingElementAbove(x, y))
                    {
                        playingFieldManager.FallingToFallen();
                        return;
                    }
                }

                if (IsLastRow(x, y))
                {
                    playingFieldManager.FallingToFallen();
                    return;
                }

                // Смещение по вертикали, если ряд не последний
                if (!IsLastRow(x, y))
                {
                    if (playingFieldManager.fieldMatrix[y, x] == FieldState.Falling)
                    {
                        tempMatrix[y + 1, x] = FieldState.Falling;
                    }
                }
            }
        }
        playingFieldManager.topLeftPositionOfCurrentElement += new Vector2(0, 1);
        WriteAndUpdate(tempMatrix);
    }

    // Если текущий элемент - упавший, а над ним - падающий
    private bool IsFallingElementAbove(int x, int y)
    {
        return (playingFieldManager.fieldMatrix[y, x] == FieldState.Fallen &&
                playingFieldManager.fieldMatrix[y - 1, x] == FieldState.Falling);
    }

    // Если последний ряд
    private bool IsLastRow(int x, int y)
    {
        return (y == playingFieldManager.Height - 1 &&
                playingFieldManager.fieldMatrix[y, x] == FieldState.Falling);
    }

    // Записываем новую (временную) матрицу в оригинальную и обновляем поле
    protected void WriteAndUpdate(FieldState[,] tempMatrix)
    {
        playingFieldManager.fieldMatrix = tempMatrix;
        playingFieldManager.FullRowCheck();
        playingFieldManager.UpdateThePlayingField();
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

    private bool IsLeftBorderNear(int x)
    {
        return x == 0;
    }

    private bool IsRightBorderNear(int x)
    {
        return x == playingFieldManager.Width - 1;
    }

    private bool IsOtherBlockNear(int x, int y, int direction)
    {
        return (playingFieldManager.fieldMatrix[y, x + direction] == FieldState.Fallen);
    }

    private Func<int, bool> borderCheck;    

    public void HorizontalMovement()
    {      
        FieldState[,] tempMatrix = new FieldState[playingFieldManager.Height, playingFieldManager.Width];
        playingFieldManager.FallenToTemp(tempMatrix);
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

        for (int y = playingFieldManager.Height - 1; y >= 0; y--)
        {
            for (int x = playingFieldManager.Width - 1; x >= 0; x--)
            {
                if (playingFieldManager.fieldMatrix[y, x] == FieldState.Falling)
                {
                    if (borderCheck(x))
                        return;
                    if (IsOtherBlockNear(x, y, direction))
                        return;
                    tempMatrix[y, x + direction] = FieldState.Falling;
                }
            }
        }
        WriteAndUpdate(tempMatrix);
        playingFieldManager.topLeftPositionOfCurrentElement += new Vector2(direction, 0);
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
