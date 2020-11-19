using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private ElementMovement elementMovement;
    [SerializeField] private ElementRotation elementRotation;      
    [SerializeField] [Range(10f, 1000f)]
    private float manualFallingSpeed = 500f; // скорость падения при зажатой клавише (меньше - быстрее)     

    private void Start()
    {        
       // StartCoroutine(FallingDownManual());
    }

    void Update()
    {
        CheckPlayerInput();
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("FallingDown"))
            elementMovement.FallingDown();
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

    private IEnumerator FallingDownManual()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetButton("FallingDown"));
            elementMovement.FallingDown();
            yield return new WaitForSecondsRealtime(1 / manualFallingSpeed);
        }
    }    
}