using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using BackEnd;
using LitJson;
using System;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviourPunCallbacks
{
    public GameObject houseCount;
    public GameObject BuildingCount;
    public GameObject rabbithomeCount;

    public GameObject thisCount;

    public Item Item;
    //public GameObject ListItems;//ItemManager 게임 오브젝트 참조하고.

    private string objName = null;
    private int objCount = 0;
    private PhotonView PV;
    private string inDate;
    private GameObject mainManager;

    // private bool isSuccess;

    // private string nn;

    void Awake()
    {
        mainManager = GameObject.Find("MainManager");
        PV = GetComponent<PhotonView>();
        objName = this.gameObject.name.Replace("(Clone)", "");
        Backend.GameData.GetTableList();
        FindText();
        SearchThisCount();
        LoadItemDB();
        thisCount.GetComponent<Text>().text = objCount.ToString();
    }

    private void FindText()
    {
        houseCount = mainManager.GetComponent<UIManager>().houseText;
        BuildingCount = mainManager.GetComponent<UIManager>().BuildingText;
        rabbithomeCount = mainManager.GetComponent<UIManager>().rabbithomeText;
    }

    private void SearchThisCount()
    {
        Debug.Log("Search" + objName);
        switch (objName)
        {
            case "adamantium":
                thisCount = houseCount;
                break;
            case "Atom":
                thisCount = BuildingCount;
                break;
            case "Flame":
                thisCount = rabbithomeCount;
                break;

            default:
                break;
        }
    }

    private void StayAlone()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    private void AddItems(PhotonView playerViewID) 
    {
        if(playerViewID.IsMine)
        {
            ItemManager.Instance.Add(Item);//추가
            //ListItems.GetComponent<ItemManager>().ListItem();//매니저에서 아이템을 들고온다를 참조.
        }
    }

    private void ItemNameCount(PhotonView playerViewID)
    {
        if(playerViewID.IsMine)
        {
            LoadItemDB();
            objCount++;
            thisCount.GetComponent<Text>().text = objCount.ToString();

            //ListItems.GetComponent<ItemManager>().ListItem();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PhotonView>().Owner.IsMasterClient)
        {
            StayAlone();
            //AddItems(col.gameObject.GetPhotonView());
            ItemNameCount(col.gameObject.GetPhotonView());
            SaveItemDB();
        }
    }

    private void SaveItemDB()
    {
        Param param = new Param();
        param.Add(objName, objCount);
        BackendReturnObject BRO = Backend.GameData.UpdateV2("Item", inDate, Backend.UserInDate, param);
        Debug.Log(BRO.ToString());
    }

    private void LoadItemDB()
    {
        string[] select = { objName };
        //Debug.Log(objName);        
        
        Where where = new Where();
        var bro = Backend.GameData.GetMyData("Item", where, select);
        inDate = bro.GetInDate();

        var gameDataListJson = bro.GetReturnValuetoJSON();
        var rows = gameDataListJson["rows"];
        //Debug.Log(rows.Count);

        if(rows.Count <= 0)
        {
            return;
        }

        var user = rows[0];
        //Debug.Log("user" + user);
        int dbCount = int.Parse(user[objName]["N"].ToString());
        objCount = dbCount;

        Debug.Log("DB 불러오기, objCount : " + objCount);
        //Debug.Log(dbCount.ToString());
    }
}
