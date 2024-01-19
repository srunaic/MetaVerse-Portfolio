using UnityEngine;
using System.Collections.Generic;
using BackEnd;
using LitJson;

public class BackEndGameInfo : MonoBehaviour
{
    public void OnClickInsertData()
    {
        int charlocation = Random.Range(0,99); //��ġ���尪
        int charScore = Random.Range(0,9999);

        //�ڳ� ������ ��� �Ҷ� �Ѱ��� 
        Param param = new Param();

        param.Add("lo", charlocation);
        param.Add("Sc",charScore);

        Dictionary<string, int> state = new Dictionary<string, int>
        //��ųʸ� ĳ���� ���� ��..
        {
            {"Character",123}  
        };

        param.Add("CHstate" , state);//�Ķ���� state �� ������ ����.

        BackendReturnObject BRO = Backend.GameInfo.Insert("custom",param);//���� ���� ����.
        //GameSave ���̺�� ����.

        if (BRO.IsSuccess())
        {
            Debug.Log("indate:" + BRO.GetInDate());
        }
        else
        {
            switch(BRO.GetStatusCode()) 
            {
                case "404":
                    Debug.Log("�������� �ʴ� table �̸���.");
                    break;

                case "412":
                    Debug.Log("��Ȱ��ȭ �� ���̺��� ���");
                    break;

                case "413":
                    Debug.Log("�ϳ��� ���� �÷��� ������ 400kb �����Ͱ� �Ѵ� ���");
                    break;

                default:
                    Debug.Log("���� ���� ���� �߻�:"+ BRO.GetMessage());
                    break;
            }
        
        }
        }

    //------------------------------------------------------------
    //--������ ���� ��� ��Ȳ�� ���� OnClick���� ���� �ε� �ٲ��ָ��.

    //������ ���̺� ������ �޾ƿ���.

    public void OnClickGetTableList()
    {

        BackendReturnObject BRO = Backend.GameInfo.GetTableList();

        if (BRO.IsSuccess())
        {
            JsonData publics = BRO.GetReturnValuetoJSON()["publicTables"];
            //public ���
            Debug.Log("public ���̺�");

            foreach (JsonData row in publics)
            {
                Debug.Log(row.ToString());
            }

            //private ���.
            Debug.Log("private ���̺�");
            JsonData privates = BRO.GetReturnValuetoJSON()["privatesTables"];

            foreach (JsonData row in privates)
            {
                Debug.Log(row.ToString());
            }

        }
        else
        {
            Debug.Log("���� ���� ���� �߻�:" + BRO.GetMessage());
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
    //�ܰ躰(����Ʈ)�ҷ�����.
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
    //���� ���̺� Ư�� ���� ���� �ҷ�����
    public void OnClickGetPublicContentsByGamerIndate()
    {
        //�ش� ������ �г������� �ҷ��ü��� ����

        BackendReturnObject BRO = Backend.GameInfo.GetPublicContentsByGamerIndate("custom","indate��");

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
        //ReturnValue�� �����ϰ�, �����Ͱ� �ִ��� Ȯ���ض�.
        if (returnData != null)
        {
            Debug.Log("�����Ͱ� �����մϴ�.");

            //���� ���޹޴� ���
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
            Debug.Log("�����Ͱ� �����ϴ�.");
        }
    }
    //������ �ֱ� parsing
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
            Debug.Log("�������� �ʴ� Ű�Դϴ�.");
        }
        //���� ���� ���� �迭�� ����Ǿ� ���� ��� �Ʒ��� ���� Ű�� �����ϴ��� Ȯ����.
        if (data.Keys.Contains("CHstate"))
        {
            JsonData stateData = data["CHstate"][0];
            if (stateData.Keys.Contains("Character"))//������ �����Ϳ� ĳ���� ��ü�� �����ϰ�.
            {
                Debug.Log("Character:" + stateData["Character"][0]);

            }
            else
            {
              //������ �߻��� �� �ֱ� ������ �ѹ��� ������ Ȯ��.
                Debug.Log("�������� �ʴ� Ű�Դϴ�.");
            
            }

        
        
        }


    }

    void CheckError(BackendReturnObject BRO)
    {
        switch (BRO.GetStatusCode())
        {
            case "200":
                Debug.Log("�ش� ������ �����Ͱ� ���̺� �����ϴ�.");
            break;
            case "404":
                if (BRO.GetMessage().Contains("gamer no found"))
                {
                    Debug.Log("gamerIndate�� �������� gamer�� Indated�� ���");
                }
                else if (BRO.GetMessage().Contains("table not found"))
                {
                    Debug.Log("�������� �ʴ� ���̺�");
                }
                break;

            case "400":
                if (BRO.GetMessage().Contains("bad limit"))
                {
                    Debug.Log("limit ���� 100 �̻��� ���");

                }
                else if (BRO.GetMessage().Contains("bad table"))
                {
                    Debug.Log("��û�� �ڵ�� ���̺� �������ΰ� ���� �ʽ��ϴ�.");

                }
                break;
            case "412":
                Debug.Log("��Ȱ��ȭ�� ���̺��Դϴ�.");

                break;
             default:
                Debug.Log("���� ���� ���� �߻�:"+BRO.GetMessage());
                break;

        }
    }

  }

