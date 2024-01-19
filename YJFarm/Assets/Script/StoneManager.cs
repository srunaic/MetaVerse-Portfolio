using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StoneManager : MonoBehaviour
{

    public void DestroySelf()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
