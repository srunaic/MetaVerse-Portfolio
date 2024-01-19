using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SizeControllerManager : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private GameObject everyPanel;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3(transform.position.x, Input.mousePosition.y, 10.0f);
        mousePosition.y -= 78;

        if (mousePosition.y >= 50 && mousePosition.y <= 500)
            everyPanel.GetComponent<RectTransform>().sizeDelta = mousePosition;
    }

    /*
    public void OnEndDrag(PointerEventData eventData)
    {
        float chatBoxPosition = everyPanel.GetComponent<RectTransform>().sizeDelta.y;
        Param param = new Param();

        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        string nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";

        BackendReturnObject bro = Backend.Social.GetGamerIndateByNickname(nickname);
        string gamerIndate = bro.Rows()[0]["inDate"]["S"].ToString();

        param.Add("size", chatBoxPosition);
        Backend.GameData.UpdateV2("tableName", gamerIndate, Backend.UserInDate, param);
        Debug.Log("Updated!");
    }
    */
}
