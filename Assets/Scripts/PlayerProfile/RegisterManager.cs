﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiscTools;
using UnityEngine.Networking;

public class RegisterManager : InputFieldController
{
    public void CallRegister()
    {
        StartCoroutine(RegisterRoutine());
    }

    private IEnumerator RegisterRoutine()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("name", nameInputField.text));
        formData.Add(new MultipartFormDataSection("password", passwordInputField.text));

        UnityWebRequest www = UnityWebRequest.Post(PlayerProfileController.linkToDB + "register.php", formData);

        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Account creation failed " + www.downloadHandler.text);
        }
        if (www.downloadHandler.text == "0")
        {
            Debug.Log("User succesfully created an account");
            Tools.LoadScene(Scenes.MainMenu);
        }

        //WWWForm form = new WWWForm();
        //form.AddField("name", nameInputField.text);
        //form.AddField("password", passwordInputField.text);

        //WWW www = new WWW(PlayerProfileController.linkToDB + "register.php", form);        

        //yield return www;

        //if (www.text == "0")
        //{
        //    Debug.Log("User succesfully created an account");
        //    Tools.LoadScene(Scenes.MainMenu);
        //}
        //else
        //{
        //    Debug.Log("Account creation failed " + www.text);
        //}
    }
}
