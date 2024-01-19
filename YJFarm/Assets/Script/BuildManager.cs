using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using BackEnd;
using LitJson;
using System;
using UnityEngine.UI;

public class BuildManager : MonoBehaviourPunCallbacks, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private GameObject Prefab;
    [SerializeField]
    private GameObject BuildInventory;
    [SerializeField]
    private GameObject mainManager;
    [SerializeField]
    private GameObject houseCount;
    [SerializeField]
    private GameObject BuildingCount;
    [SerializeField]
    private GameObject rabbithomeCount;
    public GameObject ListItems;

    private Camera camera;
    private GameObject PreViewPrefab;
    private GameObject[] BuildArea;
    private bool buildCount;

    private LayerMask build = 11;
    private float distance = 1000;
    private Vector3 buildingPosition;
    private string roomName = null;
    private string nickName = null;
    private int adamCount = 0;
    private int atomCount = 0;
    private int flameCount = 0;
    private string inDate;
    private GameObject smoke;

    private void Awake()
    {
        camera = Camera.main;
        BuildArea = GameObject.FindGameObjectsWithTag("BuildArea");
        BackendReturnObject BRO = Backend.BMember.GetUserInfo();
        nickName = $"{BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString()}";

        roomName = PhotonNetwork.CurrentRoom.Name;
        if (mainManager.GetComponent<PhotonView>().IsMine && roomName == nickName)
        {
            LoadBuildingPositionDB();
        }
        else
        {
            BuildInventory.SetActive(false);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PreViewPrefab = PhotonNetwork.Instantiate(this.Prefab.name, new Vector3(0,0,0), Quaternion.identity, 0);
        smoke = Instantiate(PreViewPrefab.GetComponent<EffectManager>().Smoke, PreViewPrefab.transform.position, Quaternion.identity);
        smoke.transform.parent = PreViewPrefab.transform;
        smoke.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        //Debug.Log(Input.mousePosition);
        // 미리보기
        RaycastHit hitData;
     
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hitData))
        {
            PreViewPrefab.transform.position = hitData.point;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        for (int i = 0; i < BuildArea.Length; i++)
        {
            if (BuildArea[i].GetComponent<BuildEvent>().getBuildEvent() == true
                && BuildArea[i].GetComponent<BuildEvent>().isBuilt == false)
            {
                buildCount = true;
                BuildArea[i].GetComponent<BuildEvent>().isBuilt = true;
                BuildArea[i].GetComponent<BuildEvent>().buildEvent = false;
                break;
            }
        }

        if (buildCount == false)
        {
            PhotonNetwork.Destroy(PreViewPrefab);
        }
        else
        {
            Vector3 objPositionList = PreViewPrefab.transform.position;

            buildingPosition = objPositionList;
            PreViewPrefab.gameObject.name = PreViewPrefab.gameObject.name.Replace("(Clone)", "");

            buildCount = false;
            if(DecreaseItemCount(this.Prefab.name, PreViewPrefab) == false)
            {
                return;
            }

            SaveBuildingPositionToDB(PreViewPrefab.gameObject.name, buildingPosition.ToString());
            smoke.SetActive(true);
            Destroy(smoke, 3.0f);
        }
        
        Debug.Log("Drag End");
    }

    private bool DecreaseItemCount(string BuildingName, GameObject PreViewPrefab)
    {
        Debug.Log("Decrease : " + BuildingName);
        string objName;

        switch (BuildingName)
        {
            case "house":
                objName = "adamantium";
                adamCount = LoadItemDB(objName, adamCount);

                if (adamCount <= 0)
                {
                    Destroy(PreViewPrefab);
                    return false;
                }

                adamCount--;

                Debug.Log("Decrease : " + adamCount);
                houseCount.GetComponent<Text>().text = adamCount.ToString();

                SaveItemDB(objName, adamCount);
                break;

            case "Building":
                objName = "Atom";
                atomCount = LoadItemDB(objName, atomCount);

                if (atomCount <= 0)
                {
                    Destroy(PreViewPrefab);
                    return false;
                }

                atomCount--;

                Debug.Log("Decrease : " + atomCount);
                BuildingCount.GetComponent<Text>().text = atomCount.ToString();

                SaveItemDB(objName, atomCount);
                break;

            case "rabbithome":
                objName = "Flame";
                flameCount = LoadItemDB(objName, flameCount);

                if (flameCount <= 0)
                {
                    Destroy(PreViewPrefab);
                    return false;
                }

                flameCount--;

                Debug.Log("Decrease : " + flameCount);
                rabbithomeCount.GetComponent<Text>().text = flameCount.ToString();

                SaveItemDB(objName, flameCount);
                break;

            default:
                break;
        }
        return true;
    }

    private int LoadItemDB(string objName, int objCount)
    {
        string[] select = { objName };
        //Debug.Log(objName);        

        Where where = new Where();
        var bro = Backend.GameData.GetMyData("Item", where, select);
        inDate = bro.GetInDate();

        var gameDataListJson = bro.GetReturnValuetoJSON();
        var rows = gameDataListJson["rows"];
        //Debug.Log(rows.Count);

        var user = rows[0];
        //Debug.Log(user);

        int dbCount = int.Parse(user[objName]["N"].ToString());
        objCount = dbCount;

        Debug.Log("DB 불러오기, objCount : " + objCount);
        return objCount;
    }

    private void SaveItemDB(string objName, int objCount)
    {
        Param param = new Param();
        param.Add(objName, objCount);
        BackendReturnObject BRO = Backend.GameData.UpdateV2("Item", inDate, Backend.UserInDate, param);
    }


    private void SaveBuildingPositionToDB(string objName, string objPosition)
    {
        Param param = new Param();
        param.Add(objName, objPosition);
        BackendReturnObject BRO = Backend.GameData.Insert("building", param);
        Debug.Log(BRO.ToString());
    }

    private void LoadBuildingPositionDB()
    {
        Debug.Log(Prefab.gameObject.name);
        string buildName = Prefab.gameObject.name;

        string[] select = { buildName };

        Where where = new Where();
        var bro = Backend.GameData.GetMyData("building", where, select);

        var gameDataListJson = bro.GetReturnValuetoJSON();
        var rows = gameDataListJson["rows"];

        Debug.Log(rows.Count);
        if(rows.Count == 0)
        {
            return;
        }

        for(int i = 0; i < rows.Count; i++)
        {
            var user = rows[i];

            if (user[buildName]["S"].ToString() == "float")
                continue;
            string buildingPosition = user[buildName]["S"].ToString();
            Vector3 newBuildingPosition = StringToVector3(buildingPosition);
            PreViewPrefab = PhotonNetwork.Instantiate(buildName, newBuildingPosition, Quaternion.identity, 0);
            RaycastHit hit;
            if (Physics.Raycast(PreViewPrefab.transform.position + new Vector3(0, 5, 0), Vector3.down, out hit, 10))
            {
                Debug.DrawRay(PreViewPrefab.transform.position + new Vector3(0, 5, 0), Vector3.down * 10, Color.blue, 20f);
                if(hit.transform.gameObject.tag == "BuildArea")
                {
                    hit.transform.gameObject.GetComponent<BuildEvent>().buildEvent = true;
                }
            }

            Debug.Log("DB 불러오기, objCount : " + buildingPosition);
            Debug.Log(user);
        }

        StartCoroutine("BuildAreaRefresh");
    }

    Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    IEnumerator BuildAreaRefresh()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < BuildArea.Length; i++)
        {
            if (BuildArea[i].GetComponent<BuildEvent>().buildEvent == true)
            {
                BuildArea[i].GetComponent<BuildEvent>().isBuilt = true;
                BuildArea[i].GetComponent<BuildEvent>().buildEvent = false;
                Debug.Log(BuildArea[i].name);
            }
        }
        BuildInventory.SetActive(false);
    }
}
