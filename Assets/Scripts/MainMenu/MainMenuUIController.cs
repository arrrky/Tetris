using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private Button play;
    [SerializeField] private Button quit;
    [SerializeField] private Text title;

    private Color randomColor;

    private Color32[] rainbowColors =
    {
        Color.red,
        new Color32(255, 128, 0, 255), // оранжевый
        Color.yellow,
        Color.green,
        new Color32(32, 72, 203, 255), // голубой
        Color.blue,
        new Color32(42, 7 ,49 , 255) // фиолетовый
    };

    // Start is called before the first frame update
    void Start()
    {       
        StartCoroutine(ColorChange());
    }

    public void PlayTheGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    private Color SetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private Color SetRandomRainbowColor()
    {
        do
        {
             randomColor = rainbowColors[Random.Range(0, rainbowColors.Length)];

        } while (randomColor == title.color);

        return randomColor;           
    }

    private IEnumerator ColorChange()
    {
        while (true)
        {
            title.color = SetRandomRainbowColor();
            yield return new WaitForSeconds(1f);
        }
    }
}
