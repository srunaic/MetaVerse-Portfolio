using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using BackEnd;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button sendBtn;
    [SerializeField]
    private Text everyChatLog;
    [SerializeField]
    private InputField input;
    [SerializeField]
    private Text systemChatLog;
    [SerializeField]
    private Scrollbar scrollBar;

    private ScrollRect scroll_rect = null;

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !input.isFocused) SendButtonOnClicked();
    }

    public void SendButtonOnClicked()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        if (input.text.Equals("")) { Debug.Log("Empty"); return; }
        string msg = string.Format(" [{0}] {1}", BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString(), input.text);
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);
        input.ActivateInputField();
        input.text = "";
        StartCoroutine(MoveToRecentLog());
    }

    public void PlayerEnteredRoom()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        string msg = "<color=blue>" + string.Format(" [{0}] {1}", BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString(), "님이 참가하셨습니다.") + "</color>";
        photonView.RPC("ReceiveSystem", RpcTarget.All, msg);
        input.ActivateInputField();
        input.text = "";
    }

    public void PlayerLeftRoom()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        string msg = "<color=red>" + string.Format(" [{0}] {1}", BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString(), "님이 나가셨습니다.") + "</color>";
        photonView.RPC("ReceiveSystem", RpcTarget.All, msg);
        input.ActivateInputField();
        input.text = "";
    }

    public void FriendInsertSuccess()
    {
        string msg = " <color=yellow>" + "친구 요청을 성공적으로 보냈습니다." + "</color>";
        systemChatLog.text += "\n" + msg;
    }

    public void MaxSendFriendRequestError()
    {
        string msg = " <color=yellow>" + "더 이상 친구 요청을 할 수 없습니다." + "</color>";
        systemChatLog.text += "\n" + msg;
    }

    public void MaxRequestFriendError()
    {
        string msg = " <color=yellow>" + "받는 사람의 친구 목록이 꽉 차있습니다." + "</color>";
        systemChatLog.text += "\n" + msg;
    }

    public void AgainFriendInsertError()
    {
        string msg = " <color=yellow>" + "이미 친구 요청을 보냈습니다." + "</color>";
        systemChatLog.text += "\n" + msg;
    }

    public void DefaultFriendError()
    {
        string msg = " <color=yellow>" + "대상을 찾을 수 없습니다." + "</color>";
        systemChatLog.text += "\n" + msg;
    }

    public void FriendRevokeSuccess()
    {
        string msg = " <color=yellow>" + "친구 요청을 취소 하였습니다." + "</color>";
        systemChatLog.text += "\n" + msg;
    }

    public void FriendAcceptSuccess()
    {
        string msg = " <color=yellow>" + "친구 요청을 수락 하였습니다." + "</color>";
        systemChatLog.text += "\n" + msg;
    }

    public void FriendRejectSuccess()
    {
        string msg = " <color=yellow>" + "친구 요청을 거절 하였습니다." + "</color>";
        systemChatLog.text += "\n" + msg;
    }

    public void FriendDeleteSuccess()
    {
        string msg = " <color=yellow>" + "친구를 삭제 하였습니다." + "</color>";
        systemChatLog.text += "\n" + msg;
    }

    [PunRPC]
    private void ReceiveMsg(string msg)
    {
        everyChatLog.text += "\n" + msg;
        // scroll_rect.verticalNormalizedPosition = 0.0f;
    }

    [PunRPC]
    private void ReceiveSystem(string msg)
    {
        everyChatLog.text += "\n" + msg;
    }

    private IEnumerator MoveToRecentLog()
    {
        yield return new WaitForSeconds(0.1f);
        scrollBar.value = 0.0f;
    }
}
