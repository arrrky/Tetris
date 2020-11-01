using System;
using UnityEngine;
using MiscTools;

public class ElementMovement : MonoBehaviour
{
    [SerializeField]
    private PlayingFieldManager playingFieldManager;

    void Start()
    {
        InvokeRepeating("FallingDown", 1f, LevelController.Instance.FallingDownAutoSpeed);
    }    

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
   
    private bool IsFallingElementAbove(int x, int y)
    {
        return (playingFieldManager.fieldMatrix[y, x] == FieldState.Fallen &&
                playingFieldManager.fieldMatrix[y - 1, x] == FieldState.Falling);
    }
   
    private bool IsLastRow(int x, int y)
    {
        return (y == playingFieldManager.Height - 1 &&
                playingFieldManager.fieldMatrix[y, x] == FieldState.Falling);
    }
    
    protected void WriteAndUpdate(FieldState[,] tempMatrix)
    {
        playingFieldManager.fieldMatrix = tempMatrix;
        playingFieldManager.FullRowCheck();
        playingFieldManager.UpdateThePlayingField();
    }

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
}
