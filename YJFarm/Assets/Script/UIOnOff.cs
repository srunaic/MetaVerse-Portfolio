using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnOff : MonoBehaviour
{
    [SerializeField]
    private GameObject FriendUI;
    [SerializeField]
    private GameObject FriendInsertUI;
    [SerializeField]
    private GameObject FriendRevokeUI;
    [SerializeField]
    private GameObject MultiPlayUI;
    [SerializeField]
    private GameObject FriendAcceptRejectUI;
    [SerializeField]
    private GameObject FriendDeleteUI;

    private bool FriendUIOnOff;
    private bool FriendInsertUIOnOff;
    private bool FriendRevokeUIOnOff;
    private bool MultiPlayUIOnOff;
    private bool FriendAcceptRejectUIOnOff;
    private bool FriendDeleteUIOnOff;

    public void UIFriendOnOff()
    {
        if(FriendUIOnOff)
        {
            FriendUI.SetActive(false);
            FriendUIOnOff = false;

            FriendInsertUI.SetActive(false);
            FriendInsertUIOnOff = false;

            FriendRevokeUI.SetActive(false);
            FriendRevokeUIOnOff = false;

            FriendAcceptRejectUI.SetActive(false);
            FriendAcceptRejectUIOnOff = false;

            FriendDeleteUI.SetActive(false);
            FriendDeleteUIOnOff = false;
        }
        else
        {
            FriendUI.SetActive(true);
            FriendUIOnOff = true;
        }
    }

    public void UIFriendInsertOnOff()
    {
        if (FriendInsertUIOnOff)
        {
            FriendInsertUI.SetActive(false);
            FriendInsertUIOnOff = false;
        }
        else
        {
            FriendInsertUI.SetActive(true);
            FriendInsertUIOnOff = true;
        }
    }

    public void UIFriendRevokeOnOff()
    {
        if(FriendRevokeUIOnOff)
        {
            FriendRevokeUI.SetActive(false);
            FriendRevokeUIOnOff = false;
        }
        else
        {
            FriendRevokeUI.SetActive(true);
            FriendRevokeUIOnOff = true;
        }
    }

    public void UIMultiPlayOnOff()
    {
        if (MultiPlayUIOnOff)
        {
            MultiPlayUI.SetActive(false);
            MultiPlayUIOnOff = false;
        }
        else
        {
            MultiPlayUI.SetActive(true);
            MultiPlayUIOnOff = true;
        }
    }

    public void UIFriendAcceptRejectOnOff()
    {
        if (FriendAcceptRejectUIOnOff)
        {
            FriendAcceptRejectUI.SetActive(false);
            FriendAcceptRejectUIOnOff = false;
        }
        else
        {
            FriendAcceptRejectUI.SetActive(true);
            FriendAcceptRejectUIOnOff = true;
        }
    }

    public void UIFriendDeleteOnOff()
    {
        if (FriendDeleteUIOnOff)
        {
            FriendDeleteUI.SetActive(false);
            FriendDeleteUIOnOff = false;
        }
        else
        {
            FriendDeleteUI.SetActive(true);
            FriendDeleteUIOnOff = true;
        }
    }
}
