using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class BackEndChart : MonoBehaviour
{
    public void OnClickGetChartAndSave()
    {
        BackendReturnObject BRO = Backend.Chart.GetChartAndSave();

        if (BRO.IsSuccess())
        {
            Debug.Log("불러오기 완료");
            Debug.Log(BRO);

            JsonData rows = BRO.GetReturnValuetoJSON()["rows"];
            string ChartName, ChartContents;

            for (int i = 0; i < rows.Count; i++)
            {
                ChartName = rows[i]["chartName"][0].ToString();
                //프리팹 저장 정보 불러오기.
                ChartContents = PlayerPrefs.GetString(ChartName);
                Debug.Log(string.Format("{0}\n{1}", ChartName, ChartContents)); //특수 문자 사용 할것이냐 방식

                GetPlayerPrefs(ChartName);

            }


        }

        else 
        {
            Debug.Log("서버 공통 에러 발생:" + BRO.GetMessage());
        }



    }

    void GetPlayerPrefs(string chartName)
    {
        string chartString = PlayerPrefs.GetString(chartName);

        JsonData chartJson = JsonMapper.ToObject(chartString)["rows"][1];
        Debug.Log(chartJson["name"][0]); //이름이란 차트를 들고오겠다.


    }

    //프리팹 리스트 받아오기.

    public void OnClickGetChartList()
    {
        BackendReturnObject BRO = Backend.Chart.GetChartList();
        if (BRO.IsSuccess())
        {
            Debug.Log("받아오기 완료");
            Debug.Log(BRO);

            JsonData rows = BRO.GetReturnValuetoJSON()["rows"];

            for (int i = 0; i < rows.Count; i++)
            {
                JsonData data = rows[i];
                Debug.Log("차트 이름:" + data["charName"][0]);
                Debug.Log("차트 설명:" + data["chartExplain"][0]);
                Debug.Log("차트 id" + data["selectedChartFileId"][0]);
            }
        }
        else 
        {
            Debug.Log("서버 공통 에러 발생:"+BRO.GetMessage());
        }
    }
    public void OnClickGetChartContents()
    {
        BackendReturnObject BRO = Backend.Chart.GetChartContents("46176"); //차트 아이디 Chart ID. 프리팹의 정보를 지닌 id 
        //서버에서 이 아이디로 받아오면 됨.

        if (BRO.IsSuccess())
        {
            JsonData rows = BRO.GetReturnValuetoJSON()["rows"];

            for (int i = 0; i < rows.Count; i++)
            {
                Debug.Log("아이템 이름:" + rows[i]["name"][0]);
                //더 추가하기 ...
            }
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "400":

                    Debug.Log("올바르지 못한 { uuid|id } 를 입력한 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생:"+BRO.GetMessage());
                    break;

            }
          
        }
    }
}
