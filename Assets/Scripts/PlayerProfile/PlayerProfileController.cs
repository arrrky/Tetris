using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerProfileController : MonoBehaviour
{
    public static PlayerProfileController Instance { get; set; }

    private PlayerProfile playerProfile;
    public PlayerProfile PlayerProfile { get => playerProfile; set => playerProfile = value; }

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
        if (PlayerProfile == null)
        {
            PlayerProfile = new PlayerProfile();
        }
    }

    public void CallSavePlayerData()
    {
        if (PlayerProfile.Name == null)
            return;
        StartCoroutine(SavePlayerDataRoutine());
    }

    private IEnumerator SavePlayerDataRoutine()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

        formData.Add(new MultipartFormDataSection("name", PlayerProfile.Name));
        formData.Add(new MultipartFormDataSection("max_level", PlayerProfile.MaxLevel.ToString()));
        formData.Add(new MultipartFormDataSection("max_score", PlayerProfile.MaxScore.ToString()));

        UnityWebRequest www = UnityWebRequest.Post(linkToDB + "savedata.php", formData);

        yield return www.SendWebRequest();

        if (www.downloadHandler.text == "0")
        {
            Debug.Log("Player profile saved");
        }
        else
        {
            Debug.Log("Save failed. Error #" + www.downloadHandler.text);
        }
    }    
}
