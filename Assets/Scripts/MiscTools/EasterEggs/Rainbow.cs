using System.Collections;
using UnityEngine;
using MiscTools;
using UnityEngine.UI;

public class Rainbow : EasterEggsController
{
    [SerializeField] private Text title;

    private Color randomColor;    
    private Coroutine colorChangeRoutine;

    protected override void ButtonOn()
    {
        base.ButtonOn();
        colorChangeRoutine = StartCoroutine(ColorChange());
    }

    protected override void ButtonOff()
    {
        base.ButtonOff();
        StopCoroutine(colorChangeRoutine);
        title.color = Color.white;
    }

    private Color SetRandomRainbowColor()
    {
        do
        {
            randomColor = Tools.rainbowColors[Random.Range(0, Tools.rainbowColors.Length)];

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
