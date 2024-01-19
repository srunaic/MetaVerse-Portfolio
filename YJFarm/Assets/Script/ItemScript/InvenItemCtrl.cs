using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenItemCtrl : MonoBehaviour
{
    Item item;

    private Button RemoveButton;
    public void RemoveItem() 
    {
        ItemManager.Instance.Remove(item);

        Destroy(gameObject);
    }
    public void AddItem(Item newItem)
    {
        item = newItem;
    }
}
