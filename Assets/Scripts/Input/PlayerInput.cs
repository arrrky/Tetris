using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private ElementMovement elementMovement;
    [SerializeField]
    private ElementRotation elementRotation;
    [SerializeField]
    private float manualFallingSpeed = 0.03f; // скорость падения при зажатой клавише (меньше - быстрее)     

    private void Start()
    {        
        StartCoroutine(FallingDownManual());
    }

    void Update()
    {
        CheckPlayerInput();
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
    }

    private IEnumerator FallingDownManual()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetButton("FallingDown"));
            elementMovement.FallingDown();
            yield return new WaitForSeconds(manualFallingSpeed);
        }
    }
}