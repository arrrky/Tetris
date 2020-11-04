using System;
using UnityEngine;
using MiscTools;

public class ElementMovement : MonoBehaviour
{    
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private SpawnManager spawnManager;
    [SerializeField]
    private ScoreController scoreController;

    private Field playingField;

    private void Start()
    {
        playingField = gameController.playingField;
        InvokeRepeating("FallingDown", 1f, LevelController.Instance.FallingDownAutoSpeed);
    }    

    public void StopFallingDown()
    {
        CancelInvoke("FallingDown");
    }

    public void FallingDown()
    {
        // Во временную матрицу будем записывать поле с уже смещенным элементом
        FieldState[,] tempMatrix = new FieldState[playingField.Height, playingField.Width];
        gameController.FallenToTemp(tempMatrix);

        // Меняем состояние в матрице-поле снизу вверх, иначе (при проходе сверху вниз) мы будем проходить по уже измененным элементам
        // и элемент "упадет" за один проход циклов
        for (int y = playingField.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < playingField.Width; x++)
            {
                if (y > 0)
                {
                    if (IsFallingElementAbove(x, y))
                    {
                        gameController.FallingToFallen();
                        spawnManager.SpawnRandomElement(gameController.playingField);
                        return;
                    }
                }

                if (IsLastRow(x, y))
                {
                    gameController.FallingToFallen();
                    spawnManager.SpawnRandomElement(gameController.playingField);
                    return;
                }

                // Смещение по вертикали, если ряд не последний
                if (!IsLastRow(x, y))
                {
                    if (playingField.Matrix[y, x] == FieldState.Falling)
                    {
                        tempMatrix[y + 1, x] = FieldState.Falling;
                    }
                }
            }
        }
        gameController.topLeftPositionOfCurrentElement += new Vector2(0, 1);
        gameController.WriteAndUpdate(tempMatrix);
    }
   
    private bool IsFallingElementAbove(int x, int y)
    {
        return (playingField.Matrix[y, x] == FieldState.Fallen &&
                playingField.Matrix[y - 1, x] == FieldState.Falling);
    }
   
    private bool IsLastRow(int x, int y)
    {
        return (y == playingField.Height - 1 &&
                playingField.Matrix[y, x] == FieldState.Falling);
    }     

    private bool IsLeftBorderNear(int x)
    {
        return x == 0;
    }

    private bool IsRightBorderNear(int x)
    {
        return x == playingField.Width - 1;
    }

    private bool IsOtherBlockNear(int x, int y, int direction)
    {
        return (playingField.Matrix[y, x + direction] == FieldState.Fallen);
    }

    private Func<int, bool> borderCheck;    

    public void HorizontalMovement()
    {      
        FieldState[,] tempMatrix = new FieldState[playingField.Height, playingField.Width];
        gameController.FallenToTemp(tempMatrix);
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

        for (int y = playingField.Height - 1; y >= 0; y--)
        {
            for (int x = playingField.Width - 1; x >= 0; x--)
            {
                if (playingField.Matrix[y, x] == FieldState.Falling)
                {
                    if (borderCheck(x))
                        return;
                    if (IsOtherBlockNear(x, y, direction))
                        return;
                    tempMatrix[y, x + direction] = FieldState.Falling;
                }
            }
        }
        gameController.WriteAndUpdate(tempMatrix);
        gameController.topLeftPositionOfCurrentElement += new Vector2(direction, 0);
    }
}
