using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using LitJson;

public class BackEndSocial : MonoBehaviour
{
    public InputField userNickname;
    public void OnClickGetGamerIndateByNickname()
    {
        BackendReturnObject BRO = Backend.Social.GetGamerIndateByNickname(userNickname.text);

        if (BRO.IsSuccess())
        {
            var indate = "";
            JsonData data = BRO.GetReturnValuetoJSON();
            for (int i = 0; i < data["rows"].Count; i++)
            {
                indate = data["rows"][i]["inDate"]["S"].ToString();
                Debug.Log(indate);

            }
            Debug.Log(indate);
        }
        else {
            Debug.Log("서버 공통 에러 발생"+BRO.GetMessage());
        }
    }



}

