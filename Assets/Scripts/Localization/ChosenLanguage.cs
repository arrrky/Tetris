using UnityEngine;

public class ChosenLanguage : MonoBehaviour
{
    public static ChosenLanguage Instance { get; set; }
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
}
