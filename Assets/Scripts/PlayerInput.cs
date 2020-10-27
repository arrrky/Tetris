using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private ElementMovement elementMovement;
    [SerializeField]
    private ElementRotation elementRotate;

    private float manualFallingSpeed; // скорость падения при зажатой клавише (меньше - быстрее)  

    public float ManualFallingSpeed
    {
        get
        {
            return manualFallingSpeed;
        }
        set
        {
            manualFallingSpeed = value;
        }
    }

    private void Start()
    {
        manualFallingSpeed = 0.03f;
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
            elementRotate.Rotate();
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