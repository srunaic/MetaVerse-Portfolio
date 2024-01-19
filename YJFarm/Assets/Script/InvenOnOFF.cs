using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenOnOFF : MonoBehaviour
{
    [SerializeField]
    private GameObject InvenUI;

    private bool InvenUIOnOff;



    public void UIonOff()
    {

        if (InvenUIOnOff)
        {
            InvenUI.SetActive(false);
            InvenUIOnOff = false;

        }
        else
        {
            InvenUI.SetActive(true);
            InvenUIOnOff = true;

        }
    }
}
