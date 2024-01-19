using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterAnim : MonoBehaviourPun
{
    [SerializeField]
    private PhotonView PV;
    [SerializeField]
    private GameObject Player;
    private Animator animator;
    private bool isWalk;
    private bool isRun;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {

    }

    void Update()
    {
        if(!PV.IsMine)
        {
            return;
        }

        if(PV.IsMine)
        {
            isWalk = Player.GetComponent<ClickMove>().getIsMove();
            animator.SetBool("isWalk", isWalk);

            isRun = Player.GetComponent<ClickMove>().getIsMove();
            animator.SetBool("isRun", isRun);

        }
    }
}
