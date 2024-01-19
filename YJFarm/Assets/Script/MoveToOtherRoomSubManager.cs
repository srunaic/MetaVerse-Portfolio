using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToOtherRoomSubManager : MonoBehaviour
{
    [SerializeField]
    GameObject JoinRoomManager = null;
    // Start is called before the first frame update
    void Start()
    {
        JoinRoomManager.GetComponent<MoveToOtherRoomManager>().ConnectToOtherRoom();
    }
}
