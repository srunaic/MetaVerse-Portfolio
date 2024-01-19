using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //���� ������Ʈ
using Photon.Realtime; // ���� ���̺귯��

public class MainManager : MonoBehaviourPunCallbacks
{
    private GameObject userData;

    void Start()
    {
        userData = PhotonNetwork.Instantiate("Character2", new Vector3(-62, 21, -10), Quaternion.identity, 0);
        gameObject.GetComponent<ChatManager>().PlayerEnteredRoom();

        // ���� ���� �̸� ��ȸ
        var roomName = PhotonNetwork.CurrentRoom.Name;
       
        if(roomName != null)
            Debug.Log($"Joined room {roomName}");

        GetComponent<MoveToOtherRoomManager>().GetUserData(userData);
    }
}
