using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class sunNight : MonoBehaviourPunCallbacks
{
    [SerializeField] private float nightFogDensity; // �� ������ Fog �е�
    public float dayFogDensity = 0.0f; // �� ������ Fog �е�
    [SerializeField] private float fogDensityCalc; // ������ ����
    public float currentFogDensity;
    [SerializeField]
    private GameObject dirLight;

    [SerializeField]
    private float secondPerRealTimeSecond; // ���� ���迡���� 100�� = ���� ������ 1��

    private bool isNight = false;
    private float dFogD = 0.0f;
    private float cFD;
    private Quaternion transX;

    private void Update()
    {
        // ��� �¾��� X �� �߽����� ȸ��. ���ǽð� 1�ʿ�  0.1f * secondPerRealTimeSecond ������ŭ ȸ��
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
        if (dirLight.transform.eulerAngles.x >= 170) // x �� ȸ���� 170 �̻��̸� ���̶�� �ϰ���
            isNight = true;
        else if (dirLight.transform.eulerAngles.x <= 10)  // x �� ȸ���� 10 ���ϸ� ���̶�� �ϰ���
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
