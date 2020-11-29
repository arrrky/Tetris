using System.Collections;
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
        WWWForm form = new WWWForm();
        form.AddField("name", nameInputField.text);
        form.AddField("password", passwordInputField.text);

        WWW www = new WWW("http://localhost/tetris/login.php", form);

        yield return www;

        if (www.text[0] == '0')
        {
            Debug.Log("User succesfully logged in");      

            PlayerProfileController.Instance.playerProfile.Name = nameInputField.text;
            PlayerProfileController.Instance.playerProfile.MaxLevel = int.Parse(www.text.Split('\t')[1]);
            PlayerProfileController.Instance.playerProfile.MaxScore = int.Parse(www.text.Split('\t')[2]);

            PlayerLoggedIn?.Invoke();
            Tools.LoadScene(Scenes.MainMenu);
        }
        else
        {
            Debug.Log("Login failed. Error #" + www.text);
        }
    }
}
