using System.Collections;
using UnityEngine;
using MiscTools;

public class RegisterManager : InputFieldController
{
    public void CallRegister()
    {
        StartCoroutine(RegisterRoutine());
    }

    private IEnumerator RegisterRoutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameInputField.text);
        form.AddField("password", passwordInputField.text);

        WWW www = new WWW(PlayerProfileController.linkToDB + "register.php", form);

        yield return www;

        if (www.text == "0")
        {
            Debug.Log("User succesfully created an account");
            Tools.LoadScene(Scenes.MainMenu);
        }
        else
        {
            Debug.Log("Account creation failed " + www.text);
        }
    }
}
