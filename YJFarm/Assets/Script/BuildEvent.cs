using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildEvent : MonoBehaviour
{
    public bool buildEvent = false;
    public bool isBuilt = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Build")
        {
            buildEvent = true;
            Debug.Log(buildEvent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Build")
        {
            buildEvent = false;
        }
    }

    public bool getBuildEvent()
    {
        return buildEvent;
    }
}
