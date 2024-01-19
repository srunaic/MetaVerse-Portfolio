using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using BackEnd;

public class CallNickName : MonoBehaviourPunCallbacks
{
    private string nn;

    public void OtherPlayerNickName()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        nn = BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        if(photonView.IsMine)
            photonView.RPC("ReceiveNickName", RpcTarget.AllBuffered, nn);
        //ReceiveNickName(nn);
    }

    private void Start()
    {
        OtherPlayerNickName();
    }

    [PunRPC]
    private void ReceiveNickName(string nn)
    {
        gameObject.GetComponent<TextMesh>().text = nn;
    }
}
