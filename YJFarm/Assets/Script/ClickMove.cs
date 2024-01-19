using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Utility;
using Photon.Pun;

public class ClickMove : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private PhotonView PV;
    [SerializeField]
    private Transform character;
    private Camera camera;
    private NavMeshAgent agent;
    private Vector3 pos;
    private Quaternion rot;
    private float speed = 10f;
    private Transform tr;
    private bool isMove;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    private void Start()
    {
        if (PV.IsMine) Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform;
    }

    void Update()
    {
        if (!PV.IsMine)
        {
            if((tr.position - pos).sqrMagnitude >= 5f * 5f)
            {
                tr.position = pos;
                tr.rotation = rot;
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, pos, Time.deltaTime * 5f);
                tr.rotation = Quaternion.Lerp(tr.rotation, rot, Time.deltaTime * 5f);
            }
        }

        if(PV.IsMine)
        {

            if (Input.GetMouseButton(1))
            {
            
                RaycastHit hit;
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    SetDestination(hit.point);
                }
            }
            LookMove();

       
        }
        
    }

    private void SetDestination(Vector3 dest)
    {
        agent.SetDestination(dest);
        isMove = true;
    }

    private void LookMove()
    {
        if(isMove)
        {
            //if(Vector3.Distance(destination, transform.position) <= 0.1f)
            if(agent.velocity.magnitude == 0.0f)
            {
                isMove = false;
                return;
            }

            //var dir = destination - transform.position;
            var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
            character.transform.forward += dir.normalized * Time.deltaTime * speed;
            //transform.position += dir.normalized * Time.deltaTime * speed;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            pos = (Vector3)stream.ReceiveNext();
            rot = (Quaternion)stream.ReceiveNext();
        }
    }

    public bool getIsMove()
    {
        return isMove;
    }
}
