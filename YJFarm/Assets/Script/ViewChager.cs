using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using Photon.Pun;
using BackEnd;

public class ViewChager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject buildViewPosition;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject BuildInventory;

    private bool isBuildView = false;
    private string nickname = null;
    private string roomName = null;

    private void Awake()
    {
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        nickname = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";
    }

    public void ChangeView()
    {
        roomName = PhotonNetwork.CurrentRoom.Name;
    
        if(photonView.IsMine && roomName == nickname)
        {
            if(isBuildView == false)
            {
                BuildInventory.SetActive(true);
                isBuildView = true;
                mainCamera.GetComponent<SmoothFollow>().enabled = false;
                mainCamera.transform.position = buildViewPosition.transform.position;
                mainCamera.transform.rotation = buildViewPosition.transform.rotation;
            }

            else if(isBuildView)
            {
                BuildInventory.SetActive(false);
                isBuildView = false;
                mainCamera.GetComponent<SmoothFollow>().enabled = true;
            }
        }
    }
}
