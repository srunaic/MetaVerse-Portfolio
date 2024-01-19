using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackEndNickNameCtrl : MonoBehaviour
{


    [SerializeField]
    private GameObject LoginOption; //닉네임 생성시 로그인 버튼 활성화 다시.
  
    [SerializeField]
    private GameObject NickNameUI;

    private bool isAppear = false;
    private bool isDisAppear = false;

    public void AppearLoginOp()
    {
        if (isAppear == false)
        {
            LoginOption.SetActive(true);
            isAppear = true;
        }
    }
    public void DisAppearNickNameOp()
    {
        if (isDisAppear == false)
        {
            NickNameUI.SetActive(false);
        }
    }

}
