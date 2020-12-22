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

        //// Звук - TODO
        //if (Input.GetButton("VolumeLevel+"))
        //{
        //    AudioController.Instance.IncreaseVolumeLevel();
        //}
        //if (Input.GetButton("VolumeLevel-"))
        //{
        //    AudioController.Instance.DecreaseVolumeLevel();
        //}
        //if (Input.GetButtonDown("RandomSong"))
        //{
        //    AudioController.Instance.PlayRandomSong();
        //}
        //if (Input.GetButtonDown("Mute"))
        //{
        //    AudioController.Instance.MuteUnmute();
        //}
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