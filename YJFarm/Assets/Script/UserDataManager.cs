using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    [SerializeField]
    private GameObject everyPanel = null;

    float chatHeight = 0;

    public void SaveUserSettings()
    {
        chatHeight = everyPanel.GetComponent<RectTransform>().sizeDelta.y;
    }
}
