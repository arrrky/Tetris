using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private ElementMovement elementMovement;
    [SerializeField] private ElementRotation elementRotation;

    private float timeOfButtonPress;

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