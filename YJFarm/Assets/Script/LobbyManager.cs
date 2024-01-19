using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Photon.Pun; //포톤 컴포넌트
using Photon.Realtime; // 포톤 라이브러리
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button joinButton; // 룸 접속 버튼
    [SerializeField]
    private GameObject BackEndUI;
    private string gameVersion = "1"; // 게임버전

    // 게임 실행과 동시에 마스터 서버 접속 시도
    private void Start()
    {
        // 게임 버전 설정
        PhotonNetwork.GameVersion = gameVersion;
        // 설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        // 룸 접속 버튼 잠시 비활성화
        //joinButton.interactable = false;
        // 접속 시도 중임을 텍스트로 표시
        //connectionInfoText.text = "마스터 서버에 접속중...";

    }
    // 마스터 서버 접속 성공 시 자동 실행
    public override void OnConnectedToMaster()
    {
        // 룸 접속 버튼 활성화
        //joinButton.interactable = true;
        // 접속 정보 표시
        //connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
    }

    // 마스터 서버 접속 실패 시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 룸 접속 버튼 비활성화
        //joinButton.interactable = false;
        // 접속 정보 표시
        //connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
        // 마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 접속 시도
    public void Connect()
    { 

        // 중복 접속 방지
        joinButton.interactable = false;
        BackEndUI.SetActive(false);

        // 마스터 서버에 접속중이라면
        if (PhotonNetwork.IsConnected)
        {

            // 룸 접속 실행
            //connectionInfoText.text = "룸에 접속...";
            //PhotonNetwork.JoinRandomRoom();

            BackendReturnObject BRO = Backend.BMember.GetUserInfo();
            string nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";

            // 처음 서버에 접속한 후에는 본인 이름의 방 생성 시도
            PhotonNetwork.CreateRoom(nickname, new RoomOptions { MaxPlayers = 4 });
            Debug.Log(nickname + "이라는 이름의 방이 만들어졌습니다.");
        }
    
        else
        {
            // 접속 중이 아니면 접속 시도
            //connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
            // 마스터 서버로의 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 방 참가에 실패했을 경우, 본인 이름의 방 생성 시도
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        string nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";

        PhotonNetwork.CreateRoom(nickname, new RoomOptions { MaxPlayers = 4 });
        Debug.Log(nickname + "이라는 이름의 방이 만들어졌습니다.");
    }

    // 방 생성에 실패했을 경우, 본인 이름의 방으로 접속 시도
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방이 이미 있습니다. 다시 입장합니다.");
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        string nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";

        PhotonNetwork.JoinRoom(nickname);
    }
  
    //룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        // 접속 상태 표시
        // connectionInfoText.text = "방 참가 성공";
        // 모든 룸 참가자가 씬을 로드함

        PhotonNetwork.LoadLevel("Main");
    }

}
