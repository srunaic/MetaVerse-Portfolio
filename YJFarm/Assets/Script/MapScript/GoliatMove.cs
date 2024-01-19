using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;


public class GoliatMove : MonoBehaviour
{
    //[SerializeField]
    //private PhotonView PV;
    [SerializeField]
    private GameObject Map;

    private Animator MapAnimator;
    private bool run;



    void Awake()
    {
        MapAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
        /*if (!PV.IsMine)
        {
            return;
        }*/

        /*if (PV.IsMine)
        {*/
            MapAnimator.SetBool("run", run);

       //}
    }
}
