using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static SignUpScript;

public class SignUpScript : MonoBehaviour
{
    public string signUpURL = "https://binusgat.rf.gd/unity-api-test/api/auth/signup.php";

    public void OnSignUpButtonClick()
    {
        string email = emailField.text;
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;

        if (password != confirmPassword)
        {
            errorPrompt.text = "<Passwords do not match!>";
            return;
        }
        else errorPrompt.text = null;

        StartCoroutine(SendSignUpRequest(email, password));
    }

    IEnumerator SendSignUpRequest(string email, string password)
    {
        UnityWebRequest request = new UnityWebRequest(signUpURL, "POST");
        string json = JsonUtility.ToJson(new SignUpData
        {
            email = email,
            password = password,
            createDate = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
        });
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("User-Agent", "UnityWebRequest");

        yield return request.SendWebRequest();

        Debug.Log("Server Response: " + request.downloadHandler.text);
    }

    [System.Serializable]
    public class SignUpData
    {
        public string email;
        public string password;
        public string createDate;
    }

    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField confirmPasswordField;
    [SerializeField] private Text errorPrompt;
}
