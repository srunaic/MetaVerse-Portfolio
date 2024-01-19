using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Photon.Pun; //���� ������Ʈ
using Photon.Realtime; // ���� ���̺귯��
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button joinButton; // �� ���� ��ư
    [SerializeField]
    private GameObject BackEndUI;
    private string gameVersion = "1"; // ���ӹ���

    // ���� ����� ���ÿ� ������ ���� ���� �õ�
    private void Start()
    {
        // ���� ���� ����
        PhotonNetwork.GameVersion = gameVersion;
        // ������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

        // �� ���� ��ư ��� ��Ȱ��ȭ
        //joinButton.interactable = false;
        // ���� �õ� ������ �ؽ�Ʈ�� ǥ��
        //connectionInfoText.text = "������ ������ ������...";

    }
    // ������ ���� ���� ���� �� �ڵ� ����
    public override void OnConnectedToMaster()
    {
        // �� ���� ��ư Ȱ��ȭ
        //joinButton.interactable = true;
        // ���� ���� ǥ��
        //connectionInfoText.text = "�¶��� : ������ ������ �����";
    }

    // ������ ���� ���� ���� �� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        // �� ���� ��ư ��Ȱ��ȭ
        //joinButton.interactable = false;
        // ���� ���� ǥ��
        //connectionInfoText.text = "�������� : ������ ������ ������� ����\n���� ��õ� ��...";
        // ������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // �� ���� �õ�
    public void Connect()
    { 

        // �ߺ� ���� ����
        joinButton.interactable = false;
        BackEndUI.SetActive(false);

        // ������ ������ �������̶��
        if (PhotonNetwork.IsConnected)
        {

            // �� ���� ����
            //connectionInfoText.text = "�뿡 ����...";
            //PhotonNetwork.JoinRandomRoom();

            BackendReturnObject BRO = Backend.BMember.GetUserInfo();
            string nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";

            // ó�� ������ ������ �Ŀ��� ���� �̸��� �� ���� �õ�
            PhotonNetwork.CreateRoom(nickname, new RoomOptions { MaxPlayers = 4 });
            Debug.Log(nickname + "�̶�� �̸��� ���� ����������ϴ�.");
        }
    
        else
        {
            // ���� ���� �ƴϸ� ���� �õ�
            //connectionInfoText.text = "�������� : ������ ������ ������� ����\n���� ��õ� ��...";
            // ������ �������� ������ �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // �� ������ �������� ���, ���� �̸��� �� ���� �õ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        string nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";

        PhotonNetwork.CreateRoom(nickname, new RoomOptions { MaxPlayers = 4 });
        Debug.Log(nickname + "�̶�� �̸��� ���� ����������ϴ�.");
    }

    // �� ������ �������� ���, ���� �̸��� ������ ���� �õ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("���� �̹� �ֽ��ϴ�. �ٽ� �����մϴ�.");
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        string nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";

        PhotonNetwork.JoinRoom(nickname);
    }
  
    //�뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        // ���� ���� ǥ��
        // connectionInfoText.text = "�� ���� ����";
        // ��� �� �����ڰ� ���� �ε���

        PhotonNetwork.LoadLevel("Main");
    }

}
