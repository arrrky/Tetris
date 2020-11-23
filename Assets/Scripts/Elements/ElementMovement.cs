using System;
using UnityEngine;

public class ElementMovement : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private ScoreController scoreController;

    private Field playingField;

    private Func<int, bool> BorderCheck;

    public event Action LastRowOrElementsCollided;
    public event Action<FieldState[,], Vector2> ElementMoved;

    private void Start()
    {
        playingField = FindObjectOfType<PlayingFieldController>().PlayingField;

        InvokeRepeating(nameof(FallingDown), 1f, LevelController.Instance.FallingDownAutoSpeed);

        gameController.GameOver += StopFallingDown;
        gameController.NextLevel += StopFallingDown;
    }

    public void FallingDown()
    {
        // Во временную матрицу будем записывать поле с уже смещенным элементом
        FieldState[,] tempMatrix = new FieldState[playingField.Height, playingField.Width];

        // Меняем состояние в матрице-поле снизу вверх, иначе (при проходе сверху вниз) мы будем проходить по уже измененным элементам
        // и элемент "упадет" за один проход циклов
        for (int y = playingField.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < playingField.Width; x++)
            {
                if (IsFallingElementAboveFallen(x, y) || IsLastRow(x, y))
                {
                    LastRowOrElementsCollided?.Invoke();
                    return;
                }


                if (playingField.Matrix[y, x] == FieldState.Falling)
                {
                    tempMatrix[y + 1, x] = FieldState.Falling;
                }
            }
        }
        ElementMoved?.Invoke(tempMatrix, new Vector2(0, 1));
    }

    public void HorizontalMovement()
    {
        FieldState[,] tempMatrix = new FieldState[playingField.Height, playingField.Width];
        int direction = 0;

        if (Input.GetButtonDown("MoveToTheRight"))
        {
            direction = 1;
            BorderCheck = IsRightBorderNear;
        }
        if (Input.GetButtonDown("MoveToTheLeft"))
        {
            direction = -1;
            BorderCheck = IsLeftBorderNear;
        }

        for (int y = playingField.Height - 1; y >= 0; y--)
        {
            for (int x = playingField.Width - 1; x >= 0; x--)
            {
                if (playingField.Matrix[y, x] == FieldState.Falling)
                {
                    if (BorderCheck(x))
                        return;
                    if (IsOtherBlockNear(x, y, direction))
                        return;
                    tempMatrix[y, x + direction] = FieldState.Falling;
                }
            }
        }
        ElementMoved?.Invoke(tempMatrix, new Vector2(direction, 0));
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
}
