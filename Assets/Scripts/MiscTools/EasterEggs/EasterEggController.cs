using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class EasterEggController : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject spritesParent;

    private bool buttonCheck = true;

    public void Button()
    {
        if (buttonCheck)
            ButtonOn();
        else
            ButtonOff();
        buttonCheck = !buttonCheck;
    }

    protected virtual void ButtonOn() => spritesParent.SetActive(true);
    protected virtual void ButtonOff() => spritesParent.SetActive(false);
}
