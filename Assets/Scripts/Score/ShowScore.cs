using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private ScoreController scoreController;
    
    void Start()
    {
        scoreText.text = $"Score: {scoreController.Score.ToString()}";
    }

    public void UpdateScore()
    {
        scoreText.text = $"Score: {scoreController.Score.ToString()}";
    }   
}
