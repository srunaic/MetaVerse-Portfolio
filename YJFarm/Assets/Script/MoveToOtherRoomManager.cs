using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using BackEnd;

public class MoveToOtherRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text nameInputField = null;

    public GameObject _backEnd;

    private Player roomMaster = null;
    private Player roomGuest = null;
    private bool isLeavingRoom = false;

    public void GetUserData(GameObject userData)
    {
        if (userData.GetComponent<PhotonView>().Owner.IsMasterClient)
        {
            roomMaster = userData.GetComponent<PhotonView>().Owner;
        }
        else
        {
            roomGuest = userData.GetComponent<PhotonView>().Owner;
            roomMaster = GetComponent<PhotonView>().Owner;
        }
    }

    public void JoinToFriendRoom()
    {
        if(nameInputField.text == null)
        {
            Debug.Log("이름이 입력되지 않았습니다.");
            return;
        }

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("방을 떠났습니다.");

            _backEnd = backEnd.MyInstance.gameObject;
            _backEnd.GetComponent<backEnd>().SetName(nameInputField.text);
           
             //PhotonNetwork.LoadLevel("MoveToOtherRoomScene");

            StartCoroutine("WaitAndMoveOtherRoom");
        }

        else
        {
            // 접속 중이 아니면 접속 시도
            //connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
            // 마스터 서버로의 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Update()
    {
        if (roomGuest == null)
        {
            return;
        }

        if (GetComponent<PhotonView>().Owner != roomMaster && isLeavingRoom == false)
        {
            ExitRoomAndJoinMyRoom();
            isLeavingRoom = true;
        }
    }

    IEnumerator WaitAndMoveOtherRoom()
    {
        yield return new WaitForSeconds(3.0f);
        ConnectToOtherRoom();
    }

    public override void OnJoinedRoom()
    {
        // 접속 상태 표시
        // connectionInfoText.text = "방 참가 성공";
        // 모든 룸 참가자가 씬을 로드함

            PhotonNetwork.LoadLevel("Main");

        //LoadingScene.LoadScene("Main"); //개체 참조.

    }

    public void ConnectToOtherRoom()
    {
        _backEnd = backEnd.MyInstance.gameObject;
        
        //PhotonNetwork.GameVersion = "1";
        //PhotonNetwork.ConnectUsingSettings();
        Debug.Log("포톤 서버에 연결중입니다.");

        // 친구 or 나의 이름으로 룸 접속 실행
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(_backEnd.GetComponent<backEnd>().GetName());
            Debug.Log(_backEnd.GetComponent<backEnd>().GetName() + "의 방으로 이동합니다");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("방이 없습니다.");
    }

    public void ExitRoomAndJoinMyRoom()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        string nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";
        
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("방을 나가고 본인 방으로 돌아갑니다.");
            PhotonNetwork.LeaveRoom();
            Debug.Log("방을 떠났습니다.");

            _backEnd = backEnd.MyInstance.gameObject;
            _backEnd.GetComponent<backEnd>().SetName(nickname);
            StartCoroutine("WaitAndMoveOtherRoom");
        }
    }

    // 방 생성에 실패했을 경우, 본인 이름의 방으로 접속 시도
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방이 이미 있습니다. 다시 입장합니다.");
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        string nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";

        PhotonNetwork.JoinRoom(nickname);
    }

    // 방 참가에 실패했을 경우, 본인 이름의 방 생성 시도
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        string nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";
        
        PhotonNetwork.CreateRoom(nickname, new RoomOptions { MaxPlayers = 4 });
        Debug.Log(nickname + "이라는 이름의 방이 만들어졌습니다.");
         
    }
}