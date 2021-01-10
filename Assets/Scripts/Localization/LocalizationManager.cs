using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField] private List<Text> allTextObjects;  
    
    private void Start()
    {          
        InitTextObjects(ChosenLanguage.Instance.Language.ToString());       
    }      

    public void InitTextObjects(string language)
    {       
        switch (language)
        {
            case "english":
                ChosenLanguage.Instance.currentDictionary = ChosenLanguage.Instance.enLoc;
                ChosenLanguage.Instance.Language = Language.English;
                break;

            case "russian":
                ChosenLanguage.Instance.currentDictionary = ChosenLanguage.Instance.ruLoc;
                ChosenLanguage.Instance.Language = Language.Russian;
                break;
        }       

        for (int i = 0; i < allTextObjects.Count; i++)
        {
            if (ChosenLanguage.Instance.currentDictionary.ContainsKey(allTextObjects[i].name))
            {
                allTextObjects[i].text = ChosenLanguage.Instance.currentDictionary[allTextObjects[i].name];                
            }
        }       
    }
}