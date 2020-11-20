using System;
using UnityEngine;
using MiscTools;

public class ElementMovement : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private PlayingFieldController playingFieldController;   
    [SerializeField] private ScoreController scoreController;

    private Field playingField;

    public event Action LastRowOrElementsCollide;

    private void Start()
    {
        playingField = playingFieldController.playingField;
        InvokeRepeating(nameof(FallingDown), 1f, LevelController.Instance.FallingDownAutoSpeed);
        gameController.GameOver += StopFallingDown;
        gameController.NextLevel += StopFallingDown;
    }   

    public void FallingDown()
    {
        // Во временную матрицу будем записывать поле с уже смещенным элементом
        FieldState[,] tempMatrix = new FieldState[playingField.Height, playingField.Width];
        playingFieldController.FallenToTemp(tempMatrix);

        // Меняем состояние в матрице-поле снизу вверх, иначе (при проходе сверху вниз) мы будем проходить по уже измененным элементам
        // и элемент "упадет" за один проход циклов
        for (int y = playingField.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < playingField.Width; x++)
            {
                if (y > 0)
                {
                    if (IsFallingElementAboveFallen(x, y) || IsLastRow(x, y))
                    {
                        LastRowOrElementsCollide?.Invoke();
                        return;
                    }
                }

                if (playingField.Matrix[y, x] == FieldState.Falling)
                {
                    tempMatrix[y + 1, x] = FieldState.Falling;
                }
            }
        }
        playingFieldController.TopLeftPositionOfCurrentElement += new Vector2(0, 1);
        playingFieldController.WriteAndUpdate(tempMatrix);
    }

    public void StopFallingDown()
    {
        CancelInvoke(nameof(FallingDown));
    }

    private bool IsFallingElementAboveFallen(int x, int y)
    {
        return (playingField.Matrix[y, x] == FieldState.Fallen &&
                playingField.Matrix[y - 1, x] == FieldState.Falling);
    }

    private bool IsLastRow(int x, int y)
    {
        return (y == playingField.Height - 1 &&
                playingField.Matrix[y, x] == FieldState.Falling);
    }

    private bool IsLeftBorderNear(int x) => x == 0;
    private bool IsRightBorderNear(int x) => x == playingField.Width - 1;

    private bool IsOtherBlockNear(int x, int y, int direction)
    {
        return (playingField.Matrix[y, x + direction] == FieldState.Fallen);
    }

    private Func<int, bool> borderCheck;

    public void HorizontalMovement()
    {
        FieldState[,] tempMatrix = new FieldState[playingField.Height, playingField.Width];
        playingFieldController.FallenToTemp(tempMatrix);
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
        playingFieldController.WriteAndUpdate(tempMatrix);
        playingFieldController.TopLeftPositionOfCurrentElement += new Vector2(direction, 0);
    }
}
