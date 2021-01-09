using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Language
{
    English = 0,
    Russian = 1,
}

public class LocalizationManager : MonoBehaviour
{    
    [SerializeField] private TextAsset english;
    [SerializeField] private TextAsset russian;

    [SerializeField] private List<Text> allTextObjects;

    private Dictionary<string, string> ruLoc = new Dictionary<string, string>();
    private Dictionary<string, string> enLoc = new Dictionary<string, string>();

    public static Dictionary<string, string> dicToUse = new Dictionary<string, string>();

    private void Start()
    {
        FillTheDics();
        InitTextObjects(ChosenLanguage.Instance.Language.ToString());       
    }

    private void FillTheDics()
    {
        string[] tempRu = russian.text.Split('\t', '\n');        

        for (int i = 0; i < tempRu.Length; i += 2)
        {
            ruLoc.Add(tempRu[i], tempRu[i + 1]);
        }

        string[] tempEn = english.text.Split('\t', '\n');

        for (int i = 0; i < tempEn.Length; i += 2)
        {
            enLoc.Add(tempEn[i], tempEn[i + 1]);
        }
    }

    public void InitTextObjects(string language)
    {       
        switch (language)
        {
            case "english":
                dicToUse = enLoc;
                ChosenLanguage.Instance.Language = Language.English;
                break;

            case "russian":
                dicToUse = ruLoc;
                ChosenLanguage.Instance.Language = Language.Russian;
                break;
        }       

        for (int i = 0; i < allTextObjects.Count; i++)
        {
            if (dicToUse.ContainsKey(allTextObjects[i].name))
            {
                allTextObjects[i].text = dicToUse[allTextObjects[i].name];                
            }
        }       
    }
}
