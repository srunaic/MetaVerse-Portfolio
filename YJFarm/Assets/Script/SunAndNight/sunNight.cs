using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class sunNight : MonoBehaviourPunCallbacks
{
    [SerializeField] private float nightFogDensity; // 밤 상태의 Fog 밀도
    public float dayFogDensity = 0.0f; // 낮 상태의 Fog 밀도
    [SerializeField] private float fogDensityCalc; // 증감량 비율
    public float currentFogDensity;
    [SerializeField]
    private GameObject dirLight;

    [SerializeField]
    private float secondPerRealTimeSecond; // 게임 세계에서의 100초 = 현실 세계의 1초

    private bool isNight = false;
    private float dFogD = 0.0f;
    private float cFD;
    private Quaternion transX;

    private void Update()
    {
        // 계속 태양을 X 축 중심으로 회전. 현실시간 1초에  0.1f * secondPerRealTimeSecond 각도만큼 회전
        dirLight.transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        cFD = currentFogDensity;
        transX = dirLight.transform.rotation;

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("GetFogDensity", RpcTarget.AllBuffered, cFD, transX);
        }

        DayFogDensityMove();
    }

    private void DayFogDensityMove()
    {
        if (dirLight.transform.eulerAngles.x >= 170) // x 축 회전값 170 이상이면 밤이라고 하겠음
            isNight = true;
        else if (dirLight.transform.eulerAngles.x <= 10)  // x 축 회전값 10 이하면 낮이라고 하겠음
            isNight = false;

        if (isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;

            }
        }
        else
        {
            if (currentFogDensity >= dFogD)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;

            }

        }
    }

    [PunRPC]
    private void GetFogDensity(float cFD, Quaternion transX)
    {
        currentFogDensity = cFD;
        dirLight.transform.rotation = transX;
    }
}
