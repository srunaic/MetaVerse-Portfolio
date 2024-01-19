using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPun
{
    public float speed = 8f;
    public float rotSpeed = 120f;
    public Transform tr;
    public Transform modelTr;
    public PhotonView PV;
    public Transform clothGroup;
    public GameObject shopCvs;

    private Vector3 currPos;
    private Quaternion currRot;

    [SerializeField]
    private Animator animator;


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //����� ������ 
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }

        //Ŭ���� ����� �޴� 
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }



    void Start()
    {
        tr = GetComponent<Transform>();
        animator = modelTr.GetComponent<Animator>();
        //if (PV.IsMine) Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform;
    }
    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (shopCvs.active == true) shopCvs.SetActive(false);
                else shopCvs.SetActive(true);
            }
            float amtMove = speed * Time.deltaTime;
            float amtRot = rotSpeed * Time.deltaTime;
            float ver = Input.GetAxis("Vertical");
            float ang = Input.GetAxis("Horizontal");
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("Run", true);
                ver *= 1.3f;
            }
            else animator.SetBool("Run", false);
            animator.SetFloat("Speed", ver);
            transform.Translate(Vector3.forward * ver * amtMove);
            transform.Rotate(Vector3.up * ang * amtRot);
        }
/*        else {
            if ((tr.position - currPos).sqrMagnitude >= 10.0f * 10.0f)
            {
                tr.position = currPos;
                tr.rotation = currRot;
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);

            }

        }*/
    }
}

