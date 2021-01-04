using UnityEngine;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour
{
    [SerializeField] protected InputField nameInputField;
    [SerializeField] protected InputField passwordInputField;
    [SerializeField] protected Button submitButton;
    [SerializeField] protected GameObject messageText;
    [SerializeField] protected GameObject messageOKButton;
    [SerializeField] protected GameObject background;

    protected Text message;

    private void Awake()
    {
        message = messageText.GetComponent<Text>();
    }

    public void VerifyInput()
    {
        submitButton.interactable = (nameInputField.text.Length >= 5 && passwordInputField.text.Length >= 8);
        // сделать более сложную проверку?
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TabBetweenInputFields();
        }
    }

    private void TabBetweenInputFields()
    {
        if (nameInputField.isFocused)
        {
            passwordInputField.Select();
        }
        if (passwordInputField.isFocused)
        {
            nameInputField.Select();
        }
    }

    protected void ShowMessage()
    {
        messageText.SetActive(true);
        messageOKButton.SetActive(true);
        background.SetActive(true);
    }
}
