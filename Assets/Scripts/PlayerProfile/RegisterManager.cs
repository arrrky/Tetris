using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class RegisterManager : InputFieldController
{   
    public void CallRegister()
    {
        StartCoroutine(RegisterRoutine());
        ShowMessage();
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
            message.text = "Account creation failed " + www.downloadHandler.text;            
        }
        if (www.downloadHandler.text == "0")
        {
            message.text = "User succesfully created an account";                      
        }       
    }   
}