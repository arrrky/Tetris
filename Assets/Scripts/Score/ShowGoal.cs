using UnityEngine;
using UnityEngine.UI;

public class ShowGoal : MonoBehaviour
{
    [SerializeField]
    private Text goalText;
    [SerializeField]
    private ScoreController scoreController;

    void Start()
    {
        goalText.text = $"Goal: {scoreController.Goal.ToString()}";
    }   
}
