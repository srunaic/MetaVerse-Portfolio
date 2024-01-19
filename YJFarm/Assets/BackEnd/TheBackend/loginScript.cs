using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loginScript : MonoBehaviour
{
    public BackEndAutoUI backEndAutoUI;
   //public UITransitionManager uITransitionManager;
    public InputField idInput;
    public Text message;

    // Start is called before the first frame update
    void Start()
    {

    }

    public static string errorTxt;

    public void login()
    {
        bool s = backEndAutoUI.OnClickLogin2();
        if (s)
        {
            StartCoroutine(moveFurther());
        }
        else
        {
            StartCoroutine(errorMessage());
        }
        Debug.Log(s);
    }

    public void suignUp()
    {
        bool s = backEndAutoUI.OnClickSignUp2();
        if (s)
        {
            errorTxt = "Successfully signed up";
            StartCoroutine(errorMessage());
        }
        else
        {
            StartCoroutine(errorMessage());
        }
        Debug.Log(s);
    }


    public IEnumerator errorMessage()
    {
        yield return new WaitForSeconds(0.5f);
        message.text = errorTxt;
        yield return new WaitForSeconds(3f);
        message.text = "";
    }

    public IEnumerator moveFurther()
    {

        message.text = idInput.text+" 님 환영합니다!";
        yield return new WaitForSeconds(3f);
        //uITransitionManager.updateCamera(1);
    }
}