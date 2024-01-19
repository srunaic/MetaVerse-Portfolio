using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CommunicationManager : MonoBehaviourPunCallbacks
{
    private Camera camera;
    private PhotonView PV;
    private bool isEnteredComZone = false;
    [SerializeField]
    private GameObject[] itemList;
    [SerializeField]
    private GameObject Axe;
    private bool isEndMotion = false;
    private GameObject recentObject;

    // Start is called before the first frame update
    void Awake()
    {
        camera = Camera.main;
        PV = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetMouseButtonDown(0) && isEnteredComZone)
            {
                CommunicationStart();
            }

            if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Axe") &&
               this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f &&
               this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f &&
               isEndMotion == true)
            {
                isEndMotion = false;
                CommunicationEnd();
                photonView.RPC("inactiveAxe", RpcTarget.All);
                recentObject.GetComponent<StoneManager>().DestroySelf();
            }
        }
    }

    void CommunicationStart()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Debug.Log(hit.transform.tag);
            if(hit.transform.tag == "CommunicationObject" && PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Anim Start");
                recentObject = hit.transform.gameObject;
                photonView.RPC("activeAxe", RpcTarget.All);
                this.GetComponent<Animator>().SetTrigger("AxeAction");
                isEndMotion = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CommunicationObject" && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("In");
            isEnteredComZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CommunicationObject" && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Out");
            isEnteredComZone = false;
        }
    }
    
    private void CommunicationEnd()
    {
        int randomNum = Random.Range(9, 11);
        Debug.Log("randomNum : " + randomNum);
        if (randomNum <= 9)
        {
            Debug.Log("No Item");
        }
        else
        {
            Debug.Log("Item dropped");
            int randomItem = Random.Range(0, 3);
            Debug.Log(itemList[randomItem].name);
            PhotonNetwork.Instantiate(itemList[randomItem].name, recentObject.transform.position + new Vector3(0, 3, 0), Quaternion.identity, 0);
        }
    }

    [PunRPC]
    private void activeAxe()
    {
        Axe.SetActive(true);

    }

    [PunRPC]
    private void inactiveAxe()
    {
        Axe.SetActive(false);
    }
}
