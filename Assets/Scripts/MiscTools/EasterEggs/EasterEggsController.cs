using System.Collections;
using UnityEngine;

public class EasterEggsController : MonoBehaviour
{
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject easterEgg;
    [SerializeField] private GameObject button;

    private bool buttonCheck = true;
    private Coroutine slowComingOutRoutine;

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
        EasterEggComingOut();        
    }

    private void EasterEggComingOut()
    {
        slowComingOutRoutine = StartCoroutine(SlowComingOut());
    }

    private IEnumerator SlowComingOut()
    {
        button.SetActive(false);
        while (easterEgg.transform.position.x < 30)
        {
            easterEgg.transform.position += new Vector3(1, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }
        while (easterEgg.transform.position.x > -4)
        {
            easterEgg.transform.position += new Vector3(-1, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }
        button.SetActive(true);
    }
}
