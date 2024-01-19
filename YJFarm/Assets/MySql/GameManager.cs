using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("LoginPanel")]
    public InputField ID;
    public InputField Pass;

    //public InputField NewID;
    //public InputField NewPass;

    public string LoginUrl;


    void Start()
    {
        LoginUrl = "localhost/login.php";
    }

    public void LoginBtn() 
    {

        StartCoroutine(LoginGo());
    
    }

    IEnumerator LoginGo() 
    {

        Debug.Log(ID.text);
        Debug.Log(Pass.text);

        WWWForm form = new WWWForm();
        form.AddField("username", ID.text);
        form.AddField("userpwd", Pass.text);

        WWW webRequest = new WWW(LoginUrl, form);


        yield return webRequest;

        Debug.Log(webRequest.text);
    
    }

    public void CreateAccountBtn() 
    {

    }

    void Update()
    {
        
    }
}
