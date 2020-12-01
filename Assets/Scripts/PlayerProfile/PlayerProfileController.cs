using UnityEngine;
using System.Collections;

public class PlayerProfileController : MonoBehaviour
{
    public static PlayerProfileController Instance { get; set; }
    public PlayerProfile playerProfile;

    public const string linkToDB = "https://ivaltetris.000webhostapp.com/";

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
        if (playerProfile == null)
        {
            playerProfile = new PlayerProfile();
        }
    }

    public void CallSavePlayerData()
    {
        if (playerProfile.Name == null)
            return;
        StartCoroutine(SavePlayerDataRoutine());
    }

    private IEnumerator SavePlayerDataRoutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", playerProfile.Name);
        form.AddField("max_level", playerProfile.MaxLevel);
        form.AddField("max_score", playerProfile.MaxScore);

        WWW www = new WWW(linkToDB + "savedata.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Player profile saved");
        }
        else
        {
            Debug.Log("Save failed. Error #" + www.text);
        }
    }    
}
