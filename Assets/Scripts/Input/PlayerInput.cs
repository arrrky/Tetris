using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameController gameController;    

    private IElementRotation elementRotation;
    private IElementMovement elementMovement;
    private float timeOfButtonPress;

    private void Awake()
    {
        elementMovement = gameController.ElementMovement;
        elementRotation = gameController.ElementRotation;
    }

    private void Update()
    {
        CheckPlayerInput();
    }

    private void FixedUpdate()
    {
        CheckManualFallingDown();
    }

    private void CheckPlayerInput()
    {
        // Управление элементом
        if (Input.GetButtonDown("MoveToTheRight") || Input.GetButtonDown("MoveToTheLeft"))
        {
            elementMovement.HorizontalMovement();
        }
        if (Input.GetButtonDown("Rotate"))
        {
            elementRotation.Rotate();
        }
        if(Input.GetButtonDown("MoveDown"))
        {
            elementMovement.FallingDown();
        }

        // Интерфейс / меню
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameController.GamePause();
        }
    }

    private void CheckManualFallingDown()
    {
        if (Input.GetButton("FallingDown"))
        {
            if (Time.time - timeOfButtonPress > Time.deltaTime)
            {
                elementMovement.FallingDown();
                timeOfButtonPress = Time.time;
            }
        }
    }    
}