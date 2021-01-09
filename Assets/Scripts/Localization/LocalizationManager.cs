using System.Text.RegularExpressions;
using System.Collections;
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
    private List<TextAsset> textAssets = new List<TextAsset>();

    private Dictionary<TextAsset, Dictionary<string, string>> textAssetsAndLocs = new Dictionary<TextAsset, Dictionary<string, string>>();

    [SerializeField] private TextAsset english;
    [SerializeField] private TextAsset russian;

    [SerializeField] private List<Text> allTextObjects;
   
    private Dictionary<string, string> ruLoc = new Dictionary<string, string>();
    private Dictionary<string, string> enLoc = new Dictionary<string, string>();

    public static Dictionary<string, string> currentDictionary = new Dictionary<string, string>();

    private void Start()
    {
        FillTextAssetsAndLocs();
        FillTheDics(textAssetsAndLocs);
        //FillTheDictionaries();
        InitTextObjects(ChosenLanguage.Instance.Language.ToString());       
    }

    private void FillTextAssetsAndLocs()
    {
        textAssetsAndLocs.Add(english, enLoc);
        textAssetsAndLocs.Add(russian, ruLoc);
    }

    private void FillTheDics(Dictionary<TextAsset, Dictionary<string, string>> textAssetsAndLocs)
    {
        foreach (var item in textAssetsAndLocs)
        {
            string[] tempLoc = item.Key.text.Split('\t', '\n');

            for (int i = 0; i < tempLoc.Length; i += 2)
            {
                item.Value.Add(tempLoc[i], tempLoc[i + 1]);
            }
        }        
    }    

    public void InitTextObjects(string language)
    {       
        switch (language)
        {
            case "english":
                currentDictionary = enLoc;
                ChosenLanguage.Instance.Language = Language.English;
                break;

            case "russian":
                currentDictionary = ruLoc;
                ChosenLanguage.Instance.Language = Language.Russian;
                break;
        }       

        for (int i = 0; i < allTextObjects.Count; i++)
        {
            if (currentDictionary.ContainsKey(allTextObjects[i].name))
            {
                allTextObjects[i].text = currentDictionary[allTextObjects[i].name];                
            }
        }       
    }
}
