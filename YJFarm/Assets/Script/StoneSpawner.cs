using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject stone;

    private bool isSpawning = false;

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Stone", transform.position, transform.rotation, 0).transform.parent = this.transform;
        }
    }

    private void Update()
    {
        if(transform.Find("Stone(Clone)") == null && isSpawning == false)
        {
            StartCoroutine("SpawnStone");
            isSpawning = true;
        }
    }

    IEnumerator SpawnStone()
    {
        yield return new WaitForSeconds(3f);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Stone", transform.position, transform.rotation, 0).transform.parent = this.transform;
        }
        isSpawning = false;
    }
}
