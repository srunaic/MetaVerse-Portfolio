using UnityEngine;
using System.Collections.Generic;
using BackEnd;
using LitJson;

public class BackEndGameInfo : MonoBehaviour
{
    public void OnClickInsertData()
    {
        int charlocation = Random.Range(0,99); //위치저장값
        int charScore = Random.Range(0,9999);

        //뒤끝 서버와 통신 할때 넘겨줌 
        Param param = new Param();

        param.Add("lo", charlocation);
        param.Add("Sc",charScore);

        Dictionary<string, int> state = new Dictionary<string, int>
        //딕셔너리 캐릭터 상태 값..
        {
            {"Character",123}  
        };

        param.Add("CHstate" , state);//파라미터 state 로 서버에 저장.

        BackendReturnObject BRO = Backend.GameInfo.Insert("custom",param);//게임 정보 저장.
        //GameSave 테이블로 저장.

        if (BRO.IsSuccess())
        {
            Debug.Log("indate:" + BRO.GetInDate());
        }
        else
        {
            switch(BRO.GetStatusCode()) 
            {
                case "404":
                    Debug.Log("존재하지 않는 table 이름임.");
                    break;

                case "412":
                    Debug.Log("비활성화 된 테이블인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 열에 컬럼의 집합이 400kb 데이터가 넘는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생:"+ BRO.GetMessage());
                    break;
            }
        
        }
        }

    //------------------------------------------------------------
    //--위에는 저장 방식 상황에 따라 OnClick으로 저장 로드 바꿔주면됨.

    //데이터 테이블 리스드 받아오기.

    public void OnClickGetTableList()
    {

        BackendReturnObject BRO = Backend.GameInfo.GetTableList();

        if (BRO.IsSuccess())
        {
            JsonData publics = BRO.GetReturnValuetoJSON()["publicTables"];
            //public 방식
            Debug.Log("public 테이블");

            foreach (JsonData row in publics)
            {
                Debug.Log(row.ToString());
            }

            //private 방식.
            Debug.Log("private 테이블");
            JsonData privates = BRO.GetReturnValuetoJSON()["privatesTables"];

            foreach (JsonData row in privates)
            {
                Debug.Log(row.ToString());
            }

        }
        else
        {
            Debug.Log("서버 공통 에러 발생:" + BRO.GetMessage());
        }

    }
    public void OnClickPublicContents()
    {
        BackendReturnObject BRO = Backend.GameInfo.GetMyPublicContents("custom",0);

        if (BRO.IsSuccess())
        {
            GetGameInfo(BRO.GetReturnValuetoJSON());
        }
        else
        {
            CheckError(BRO);
        }
    
    }
    //단계별(리스트)불러오기.
    string firstKey = string.Empty;
    public void OnClickPublicContentsNext()
    {
        BackendReturnObject BRO;

        if(firstKey == null)
        {
            BRO = Backend.GameInfo.GetPublicContents("custom",1);

        }

        else
        {
            BRO = Backend.GameInfo.GetPublicContents("custom",firstKey,1);

        }

        if (BRO.IsSuccess())
        {
            firstKey = BRO.FirstKeystring();
            GetGameInfo(BRO.GetReturnValuetoJSON());

        }
        else
        {
            CheckError(BRO);
        }
    }
    //공개 테이블 특정 유저 정보 불러오기
    public void OnClickGetPublicContentsByGamerIndate()
    {
        //해당 유저의 닉네임으로 불러올수도 있음

        BackendReturnObject BRO = Backend.GameInfo.GetPublicContentsByGamerIndate("custom","indate값");

        if (BRO.IsSuccess())
        {
            GetGameInfo(BRO.GetReturnValuetoJSON());
        }
        else
        {
            CheckError(BRO);
        }
    }
    void GetGameInfo(JsonData returnData)
    {
        //ReturnValue가 존재하고, 데이터가 있는지 확인해라.
        if (returnData != null)
        {
            Debug.Log("데이터가 존재합니다.");

            //열로 전달받는 경우
            if (returnData.Keys.Contains("rows"))
            {
                JsonData rows = returnData["rows"];
                for (int i = 0; i < rows.Count; i++)
                {
                    GetData(rows[i]);

                }

            }
            else if (returnData.Keys.Contains("row"))
            {
                JsonData row = returnData["rows"];
                GetData(row[0]);

            }

        }
        else 
        {
            Debug.Log("데이터가 없습니다.");
        }
    }
    //데이터 넣기 parsing
    void GetData(JsonData data)
    {
        var score = data["score"][0];
        var location = data["lo"][0];

        Debug.Log("score:"+score);
        Debug.Log("lo:"+location);

        if (data.Keys.Contains("lo"))
        {
            Debug.Log("lo" + data["lo"][0]);
        }
        else 
        {
            Debug.Log("존재하지 않는 키입니다.");
        }
        //현재 상태 값이 배열로 저장되어 있을 경우 아래와 같이 키가 존재하는지 확인함.
        if (data.Keys.Contains("CHstate"))
        {
            JsonData stateData = data["CHstate"][0];
            if (stateData.Keys.Contains("Character"))//저장할 데이터에 캐릭터 객체를 포함하고.
            {
                Debug.Log("Character:" + stateData["Character"][0]);

            }
            else
            {
              //에러가 발생할 수 있기 때문에 한번더 유무를 확인.
                Debug.Log("존재하지 않는 키입니다.");
            
            }

        
        
        }


    }

    void CheckError(BackendReturnObject BRO)
    {
        switch (BRO.GetStatusCode())
        {
            case "200":
                Debug.Log("해당 유저의 데이터가 테이블에 없습니다.");
            break;
            case "404":
                if (BRO.GetMessage().Contains("gamer no found"))
                {
                    Debug.Log("gamerIndate가 존재하지 gamer의 Indated인 경우");
                }
                else if (BRO.GetMessage().Contains("table not found"))
                {
                    Debug.Log("존재하지 않는 테이블");
                }
                break;

            case "400":
                if (BRO.GetMessage().Contains("bad limit"))
                {
                    Debug.Log("limit 값이 100 이상인 경우");

                }
                else if (BRO.GetMessage().Contains("bad table"))
                {
                    Debug.Log("요청한 코드와 테이블 공개여부가 맞지 않습니다.");

                }
                break;
            case "412":
                Debug.Log("비활성화된 테이블입니다.");

                break;
             default:
                Debug.Log("서버 공통 에러 발생:"+BRO.GetMessage());
                break;

        }
    }

  }

