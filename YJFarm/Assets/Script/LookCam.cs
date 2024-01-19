using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCam : MonoBehaviour
{
    private Camera Cam;
    private Vector3 startScale;
    private float distance = 30;

    private void Start()
    {
        Cam = Camera.main;
        startScale = transform.localScale;
    }

    private void Update()
    {
        float dist = Vector3.Distance(Cam.transform.position, transform.position);
        Vector3 newScale = startScale * dist / distance;
        transform.localScale = newScale;
        
        transform.rotation = Cam.transform.rotation;
    }
}
