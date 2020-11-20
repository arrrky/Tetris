using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private ElementMovement elementMovement;
    [SerializeField] private ElementRotation elementRotation;      
    [SerializeField] [Range(10f, 1000f)]
    private float manualFallingSpeed = 500f; // скорость падения при зажатой клавише (меньше - быстрее)       

    void Update()
    {
        CheckPlayerInput();
    }

    private void FixedUpdate()
    {
        CheckManualFallingDown();
    }

    private void CheckPlayerInput()
    {
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameController.GamePause();
        }        
    }

    private float timeOfPress;

    private void CheckManualFallingDown()
    {
        if (Input.GetButton("FallingDown"))
        {
            if (Time.time - timeOfPress > Time.deltaTime)
            {
                elementMovement.FallingDown();
                timeOfPress = Time.time;
            }
        }
    }    
}