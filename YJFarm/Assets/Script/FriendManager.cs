using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using LitJson;
//using Photon.Realtime;
//using Photon.Pun;

public class FriendManager : MonoBehaviour
{
    [SerializeField]
    private InputField InsertInput;
    [SerializeField]
    private InputField RevokeInput;
    [SerializeField]
    private InputField AcceptInput;
    [SerializeField]
    private InputField DeleteInput;
    [SerializeField]
    private Text FriendGetSentRequestList;
    [SerializeField]
    private Text RequestList;
    [SerializeField]
    private Text FriendList;

    // 친구 요청 
    public void OnClickRequestFriend()
    {
        var friendText = OnClickGetGamerIndateByNickname(InsertInput);
        BackendReturnObject BRO = Backend.Social.Friend.RequestFriend(friendText);

        if (BRO.IsSuccess())
        {
            gameObject.GetComponent<ChatManager>().FriendInsertSuccess();
            Debug.Log("친구요청 완료");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "412":
                    if (BRO.GetMessage().Contains("maxSendFriendRequest"))
                    {
                        gameObject.GetComponent<ChatManager>().MaxSendFriendRequestError();
                        Debug.Log("보내는 사람의 request 가 꽉 찬 경우");
                    }
                    else if (BRO.GetMessage().Contains("maxRequestFriend"))
                    {
                        gameObject.GetComponent<ChatManager>().MaxRequestFriendError();
                        Debug.Log("받는 사람의 request 가 꽉 찬 경우");
                    }
                    break;

                case "409":
                    gameObject.GetComponent<ChatManager>().AgainFriendInsertError();
                    Debug.Log("이미 친구요청한 사람에게 다시 요청한 경우");
                    break;

                default:
                    gameObject.GetComponent<ChatManager>().DefaultFriendError();
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                    break;
            }
        }
    }

    // 친구 요청 철회
    public void OnClickRevokeSentRequest()
    {
        var friendText = OnClickGetGamerIndateByNickname(RevokeInput);
        BackendReturnObject BRO = Backend.Social.Friend.RevokeSentRequest(friendText);

        if (BRO.IsSuccess())
        {
            OnClickGetSentRequestList();
            gameObject.GetComponent<ChatManager>().FriendRevokeSuccess();
            Debug.Log("철회 완료");
        }
        else
        {
            gameObject.GetComponent<ChatManager>().DefaultFriendError();
            Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
        }
    }

    // 친구요청을 보낸 리스트
    public void OnClickGetSentRequestList()
    {
        BackendReturnObject BRO = Backend.Social.Friend.GetSentRequestList();

        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetReturnValuetoJSON());
            FriendGetSentRequestList.text = "";

            JsonData data = BRO.GetReturnValuetoJSON();
            for (int i = 0; i < data["rows"].Count; i++)
            {
                var friendData = data["rows"][i]["nickname"]["S"].ToString();
                Debug.Log(friendData);
                FriendGetSentRequestList.text += "\n" + " " + friendData;
            }
        }
        else
        {
            gameObject.GetComponent<ChatManager>().DefaultFriendError();
            Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
        }
    }

    // 친구요청을 받은 리스트
    public void OnClickRequestList()
    {
        BackendReturnObject BRO = Backend.Social.Friend.GetReceivedRequestList();

        if (BRO.IsSuccess())
        {
            Debug.Log(BRO.GetReturnValuetoJSON());
            RequestList.text = "";

            JsonData data = BRO.GetReturnValuetoJSON();
            for (int i = 0; i < data["rows"].Count; i++)
            {
                var friendData = data["rows"][i]["nickname"]["S"].ToString();
                Debug.Log(friendData);
                RequestList.text += "\n" + " " + friendData;
            }
        }
        else
        {
            gameObject.GetComponent<ChatManager>().DefaultFriendError();
            Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
        }
    }

    // 친구 요청 수락
    public void OnClickAcceptFriend()
    {
        var friendText = OnClickGetGamerIndateByNickname(AcceptInput);
        BackendReturnObject BRO = Backend.Social.Friend.AcceptFriend(friendText);

        if (BRO.IsSuccess())
        {
            gameObject.GetComponent<ChatManager>().FriendAcceptSuccess();
            OnClickRequestList();
            OnClickGetFriendList();
            Debug.Log("친구요청 수락 완료");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "412":
                    if (BRO.GetMessage().Contains("maxSendFriendRequest"))
                    {
                        Debug.Log("보내는 사람의 request 가 꽉 찬 경우");
                        gameObject.GetComponent<ChatManager>().MaxSendFriendRequestError();
                    }
                    else if (BRO.GetMessage().Contains("maxRequestFriend"))
                    {
                        Debug.Log("받는 사람의 request 가 꽉 찬 경우");
                        gameObject.GetComponent<ChatManager>().MaxRequestFriendError();
                    }
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                    gameObject.GetComponent<ChatManager>().DefaultFriendError();
                    break;
            }
        }
    }

    // 친구 요청 거절
    public void OnClickRejectFriend()
    {
        var friendText = OnClickGetGamerIndateByNickname(AcceptInput);
        BackendReturnObject BRO = Backend.Social.Friend.RejectFriend(friendText);

        if (BRO.IsSuccess())
        {
            gameObject.GetComponent<ChatManager>().FriendRejectSuccess();
            OnClickRequestList();
            Debug.Log("친구 요청 거절 완료");
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
            gameObject.GetComponent<ChatManager>().DefaultFriendError();
        }
    }

    // 친구리스트 보기
    public void OnClickGetFriendList()
    {
        BackendReturnObject BRO = Backend.Social.Friend.GetFriendList();

        if (BRO.IsSuccess())
        {
            FriendList.text = "";

            JsonData data = BRO.GetReturnValuetoJSON();
            for (int i = 0; i < data["rows"].Count; i++)
            {
                var friendData = data["rows"][i]["nickname"]["S"].ToString();
                Debug.Log(friendData);
                FriendList.text += "\n" + " " + friendData;
            }
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
            gameObject.GetComponent<ChatManager>().DefaultFriendError();
        }
    }

    // 친구 끊기
    public void OnClickBreakFriend()
    {
        var friendText = OnClickGetGamerIndateByNickname(DeleteInput);
        BackendReturnObject BRO = Backend.Social.Friend.BreakFriend(friendText);

        if (BRO.IsSuccess())
        {
            OnClickGetFriendList();
            gameObject.GetComponent<ChatManager>().FriendDeleteSuccess();
            Debug.Log("친구 끊기 완료");
        }
        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
            gameObject.GetComponent<ChatManager>().DefaultFriendError();
        }
    }

    // 친구추가를 위한 indate 값 변경
    private string OnClickGetGamerIndateByNickname(InputField input)
    {
        var indate = "";
        BackendReturnObject BRO = Backend.Social.GetGamerIndateByNickname(input.text);

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();
            for (int i = 0; i < data["rows"].Count; i++)
            {
                indate = data["rows"][i]["inDate"]["S"].ToString();
                Debug.Log(indate);
            }
        }
        else
        {
            Debug.Log("서버 공통 에러 발생" + BRO.GetMessage());
        }
        return indate;
    }
}