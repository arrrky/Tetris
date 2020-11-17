using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private Button play;
    [SerializeField] private Button quit;
    [SerializeField] private Text rainbowButton;
    [SerializeField] private Text title;
    [SerializeField] private GameObject sprRainbow;
    [SerializeField] private GameObject easterEgg;

    private Coroutine colorChangeRoutine;

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
        easterEgg.transform.position = new Vector3(-3, 3, 0);
        colorChangeRoutine = StartCoroutine(ColorChange());
        rainbowButton.text = "Turn rainbow OFF";
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

    private bool rainbowButtonCheck = false;
    private Coroutine slowComingOutRoutine;

    public void RainbowButton()
    {
        if (rainbowButtonCheck)
            RainbowButtonOn();
        else
            RainbowButtonOff();
    }

    private void RainbowButtonOn()
    {
        rainbowButton.text = "Turn rainbow OFF";
        sprRainbow.SetActive(true);            
        title.text = "TETRIS FORCE";
        IvanComingBack();
        rainbowButtonCheck = !rainbowButtonCheck;
    }

    private void RainbowButtonOff()
    {
        rainbowButton.text = "Turn rainbow ON";
        sprRainbow.SetActive(false);    
        title.text = "";
        IvanComingOut();
        rainbowButtonCheck = !rainbowButtonCheck;
    }

    private void IvanComingOut()
    {   
        // easterEgg.transform.position = new Vector3(30, 3, 0);
        slowComingOutRoutine = StartCoroutine(SlowComingOut());
    }

    private void IvanComingBack()
    {      
        easterEgg.transform.position = new Vector3(-3, 3, 0);
        StopCoroutine(slowComingOutRoutine);
    }

    private IEnumerator SlowComingOut()
    {
        while(easterEgg.transform.position.x < 30)
        {
            easterEgg.transform.position += new Vector3(1, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
