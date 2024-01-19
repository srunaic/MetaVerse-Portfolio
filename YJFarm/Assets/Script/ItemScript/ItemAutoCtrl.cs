using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAutoCtrl : MonoBehaviour
{
    public GameObject ItemPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            ItemPickup.GetComponent<ItemPickUp>();
        }
    }
}
