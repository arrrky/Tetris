using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    ElementMovement elementMovement;
    [SerializeField]
    ElementRotate elementRotate;

    private float inputTime = 0f;
    private float delayBeforeAction = 1f;

    private void Start()
    {
        StartCoroutine(FallingDownManual());
    }

    void Update()
    {
        if (Input.anyKey)
        {
            CheckPlayerInput();
        }
    }

    private void CheckPlayerInput()
    {
        if (Input.GetButtonDown("MoveToTheRight") || Input.GetButtonDown("MoveToTheLeft"))
        {
            elementMovement.HorizontalMovement();
        }
        //if (Input.GetButtonDown("FallingDown"))
        //{
        //    elementMovement.FallingDown();
        //    inputTime = Time.time;
        //    if (Time.time - inputTime > delayBeforeAction)
        //    {
        //        do
        //        {
        //            elementMovement.FallingDown();
        //        } while (Input.GetButtonDown("FallingDown"));
        //    }
        //}
        if (Input.GetButtonDown("Rotate"))
        {
            elementRotate.RotateTest();
        }
    }

    private IEnumerator FallingDownManual()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetButton("FallingDown"));
            elementMovement.FallingDown();
            yield return new WaitForSeconds(0.03f); // скорость падения при зажатой клавише (меньше - быстрее)   
        }
    }
}