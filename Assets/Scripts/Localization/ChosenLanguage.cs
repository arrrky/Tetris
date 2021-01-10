using UnityEngine;
using System.Collections.Generic;
public enum Language
{
    English = 0,
    Russian = 1,
}

public class ChosenLanguage : MonoBehaviour
{
    public static ChosenLanguage Instance { get; set; }

    private Dictionary<TextAsset, Dictionary<string, string>> textAssetsAndLocs = new Dictionary<TextAsset, Dictionary<string, string>>();

    [SerializeField] private TextAsset english;
    [SerializeField] private TextAsset russian;

    public Dictionary<string, string> ruLoc = new Dictionary<string, string>();
    public Dictionary<string, string> enLoc = new Dictionary<string, string>();
    public Dictionary<string, string> currentDictionary = new Dictionary<string, string>();

    public Language Language = Language.English;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FillTextAssetsAndLocs();
        FillTheDics(textAssetsAndLocs);
        currentDictionary = enLoc;
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
}