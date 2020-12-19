using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using MiscTools;

public class LoginManager : InputFieldController
{
    public event Action PlayerLoggedIn;

    public void CallLogin()
    {
        StartCoroutine(LoginRoutine());
    }

    private IEnumerator LoginRoutine()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("name", nameInputField.text));
        formData.Add(new MultipartFormDataSection("password", passwordInputField.text));

        UnityWebRequest www = UnityWebRequest.Post(PlayerProfileController.linkToDB + "login.php", formData);

        yield return www.SendWebRequest();

        if (www.downloadHandler.text[0] == '0')
        {
            Debug.Log("User succesfully logged in");      

            PlayerProfileController.Instance.PlayerProfile.Name = nameInputField.text;
            PlayerProfileController.Instance.PlayerProfile.MaxLevel = int.Parse(www.downloadHandler.text.Split('\t')[1]);
            PlayerProfileController.Instance.PlayerProfile.MaxScore = int.Parse(www.downloadHandler.text.Split('\t')[2]);

            PlayerLoggedIn?.Invoke();
            Tools.LoadScene(Scenes.MainMenu);
        }
        else
        {
            Debug.Log("Login failed. Error #" + www.downloadHandler.text);
        }
    }
}
