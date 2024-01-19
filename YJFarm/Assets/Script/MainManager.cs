using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //포톤 컴포넌트
using Photon.Realtime; // 포톤 라이브러리

public class MainManager : MonoBehaviourPunCallbacks
{
    private GameObject userData;

    void Start()
    {
        userData = PhotonNetwork.Instantiate("Character2", new Vector3(-62, 21, -10), Quaternion.identity, 0);
        gameObject.GetComponent<ChatManager>().PlayerEnteredRoom();

        // 현재 방의 이름 조회
        var roomName = PhotonNetwork.CurrentRoom.Name;
       
        if(roomName != null)
            Debug.Log($"Joined room {roomName}");

        GetComponent<MoveToOtherRoomManager>().GetUserData(userData);
    }
}
