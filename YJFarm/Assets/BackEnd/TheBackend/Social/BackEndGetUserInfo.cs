using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class BackEndGetUserInfo : MonoBehaviour
{
    public void OnClickGetUserInfo()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();

        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetReturnValue());
            Debug.Log(BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString());
        }
        else
        {
            Debug.Log("서버 공통 에러 발생:"+ BRO.GetErrorCode());
        }
         
    }

}
