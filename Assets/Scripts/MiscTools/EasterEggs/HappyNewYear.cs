using System.Collections;
using UnityEngine;
using MiscTools;
using UnityEngine.UI;
using System.Linq;

public class HappyNewYear : EasterEggController
{
    [SerializeField] private Text title;
    [SerializeField] private AudioClip christmasSong;
    [SerializeField] private AudioSource audioSource;

    private Color32 randomColor;    
    private Coroutine colorChangeRoutine;
    private readonly float colorChangeTime = 1f;

    private void Start()
    {        
        audioSource.clip = christmasSong;
    }

    protected override void ButtonOn()
    {
        base.ButtonOn();

        colorChangeRoutine = StartCoroutine(ColorChange());
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        audioSource.mute = false;
    }

    protected override void ButtonOff()
    {
        base.ButtonOff();

        StopCoroutine(colorChangeRoutine);
        title.color = Color.white;
        audioSource.mute = true;
    }

    private Color32 SetRandomRainbowColor()
    {
        // Чтобы не показывались 2 одинаковых цвета подряд
        do
        {
            randomColor = Tools.rainbowColors.ElementAt(Random.Range(0, Tools.rainbowColors.Count)).Value;

        } while (randomColor == title.color);

        return randomColor;
    }

    private IEnumerator ColorChange()
    {
        while (true)
        {
            title.color = SetRandomRainbowColor();
            yield return new WaitForSeconds(colorChangeTime);
        }
    }
}