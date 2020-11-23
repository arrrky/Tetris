using UnityEngine;
using MiscTools;

public class EasterEggController : MonoBehaviour
{
    [SerializeField] private GameObject sprite; 
    [SerializeField] private GameObject button;

    private bool buttonCheck = true;   

    public void Button()
    {
        if (buttonCheck)
            ButtonOn();
        else
            ButtonOff();
        buttonCheck = !buttonCheck;
    }

    protected virtual void ButtonOn()
    {
        sprite.SetActive(true);
    }

    protected virtual void ButtonOff()
    {
        sprite.SetActive(false);             
    }   
}
