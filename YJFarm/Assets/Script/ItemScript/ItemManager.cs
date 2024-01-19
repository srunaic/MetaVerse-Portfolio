using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; //포톤 컴포넌트
using Photon.Realtime; // 포톤 라이브러리

public class ItemManager : MonoBehaviourPunCallbacks
{
    public static ItemManager Instance;

    public List<Item> Items = new List<Item>();

    public Transform ItemContent; //슬롯의 부모임
    public GameObject InventoryItem;//인벤 아이템 슬롯

    public Toggle EnableRemove;

    public InvenItemCtrl[] InventoryItems;

    private void Awake()
    {
        Instance = this;
    }
    public void Add(Item item)
    {
        Items.Add(item);
    }
    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItem()//아이템 리스트를 인벤토리에 불러옴 
    {
        foreach (Transform item in ItemContent) //아이템 추가
        {
            Destroy(item.gameObject);
        }
        
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>(); //아이템 프리팹의 변수명이 이렇게 되야됨 안 그러면 nullreference 뜸.
            //아이템 프리팹 꼭 확인하시오.
            var itemicon = obj.transform.Find("ItemIcon").GetComponent<Image>();//아이템 아이콘 변수명
            var removeButton = obj.transform.Find("RemoveBtn").GetComponent<Button>();
            var aItemCount = obj.transform.Find("CountText").GetComponent<Text>();

            itemName.text = item.ItemName;
            itemicon.sprite = item.icon;
            aItemCount.text = item.count.ToString();

            if (EnableRemove.isOn)
                removeButton.gameObject.SetActive(true);
        }

        SetInventoryItems();

    }
    public void EnableItemRemove()
    {
        if (EnableRemove.isOn)
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveBtn").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveBtn").gameObject.SetActive(false);
            }
        }
    }

    public void SetInventoryItems()
    {
        InventoryItems = ItemContent.GetComponentsInChildren<InvenItemCtrl>();

        for (int i = 0; i < Items.Count; i++)
        {
            InventoryItems[i].AddItem(Items[i]);
        }

    }


}
