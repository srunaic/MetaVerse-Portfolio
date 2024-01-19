using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class flashLight1 : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PhotonView PV;
    [SerializeField]
    private Light flash;

    private float count = 0;
    private bool light = false;

    private void Start()
    {
        flash.GetComponent<Light>().enabled = light;
    }

    private void Update()
    {
        if(PV.IsMine)
        {
            Light();
            PV.RPC("ReceiveLight", RpcTarget.AllBuffered, light);
        }
    }

    private void Light()
    {
        if (count == 0 && Input.GetKeyDown(KeyCode.B))
        {
            light = true;
            flash.GetComponent<Light>().enabled = light;

            count = 1;
        }
        else if(count == 1 && Input.GetKeyDown(KeyCode.B))
        {
            light = false;
            flash.GetComponent<Light>().enabled = light;

            count = 0;
        }
    }

    [PunRPC]
    private void ReceiveLight(bool light)
    {
        flash.GetComponent<Light>().enabled = light;
    }
}
