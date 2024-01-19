using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using UnityEngine.SceneManagement;

public class BackEndAutoUI : MonoBehaviour
{
    public InputField idInput;
    public InputField pwInput;
    public GameObject NickNameUI;
    public GameObject LoginBtn;

    [SerializeField]
    private LobbyManager lobbyManager = null;
    [SerializeField]
    LoadManager loadManager = null;

    private bool isNickNameAppearTo = false;
         //public GameObject[] GETchart;//������ �迭

    [SerializeField]
    private Button GoToLobbyBtn;

    public void isAppearNickNameAppear()
    {
        if (isNickNameAppearTo == false)
        {
            LoginBtn.SetActive(false);
        }
    
    }
    public void OnClickSignUp1() 
    {
        BackendReturnObject BRO = Backend.BMember.CustomSignUp(idInput.text,pwInput.text,"ID,Pass."); 
        if (BRO.IsSuccess())
        {
            NickNameUI.SetActive(true);
            Debug.Log("ȸ������ ����");

        }
        else
        {
            backEnd.MyInstance.ShowErrorUI(BRO);
            Debug.Log("ȸ������ ����.");
        }
    }

    //����ȭ �α��� ���

    /*public void OnClickLoading1() //�ε� ��� �̷��� �ϸ� �׳� ���� ��� �ȵǰ�, main ���� �Ѿ�� 
    {
        LoadingScene.LoadScene("Main");
    }*/

        public void OnClickLogin1()
    {
        BackendReturnObject BRO = Backend.BMember.CustomLogin(idInput.text, pwInput.text); 

        if (BRO.IsSuccess())
        {
            Debug.Log("successfully logged in");

            //LoadingScene.LoadScene("Main");
            lobbyManager.Connect();
            loadManager.LoadOnClick();
            //GETchart = GameObject.FindGameObjectsWithTag("Building"); //��Ʈ ������ ���� �ҷ����µ� �±׷�.

        }
        else
        {
            backEnd.MyInstance.ShowErrorUI(BRO);
            Debug.Log("failed to log in");
        }
    }

    BackendReturnObject bro = new BackendReturnObject();
    bool isSuccess;
    bool isSuccess2;

    void Update()
    {
        if (isSuccess)
        {
            //Backend.BMember.SaveToken(bro); //<--?????? save ?????? ??????. null ?????? ?????? ?????? ???? ????
            isSuccess = false;
            bro.Clear();
        }
    }
   
    //�񵿱� ȸ������
    public bool OnClickSignUp2() 
    {
        int count = 0;
        Backend.BMember.CustomSignUp(idInput.text, pwInput.text, "Test2", (BRO) => {
        bro = BRO;
            isSuccess = BRO.IsSuccess();
            if (isSuccess) 
            {
                count = 1;
                Debug.Log("successfully signed up");
            }
            else
            {
                count = 1;
                backEnd.MyInstance.ShowErrorUI(BRO);
            }
        });

            while(count != 1)
        {
            if(count ==1)
            {
                break;
            }
        }
        return isSuccess;
    }

    //�񵿱� �α��� ���
    public bool OnClickLogin2()
    {
        int count = 0;
        Backend.BMember.CustomLogin(idInput.text, pwInput.text, (BRO) =>
        {
            isSuccess2 = BRO.IsSuccess();
            if (isSuccess2)
            {
                count = 1;
                Debug.Log("Successfully logged in");
                Debug.Log("1");
            }
            else
            {
                count = 1;
                backEnd.MyInstance.ShowErrorUI(BRO);
            }
        });
        while (count != 1)
        {
            if (count == 1)
            {
                Debug.Log("count == 1");
                break;
            }
        }
        return isSuccess2;
    }

    //�񵿱� �α��� ���
    public void AutoLogin1()
    {
        BackendReturnObject backendReturnObject = Backend.BMember.LoginWithTheBackendToken();

        if (backendReturnObject.IsSuccess()==true)
        {
            Debug.Log("����ȭ �α��� ����");
        }
        else
        {
            backEnd.MyInstance.ShowErrorUI(backendReturnObject);
            Debug.Log("����ȭ �α��� ����");
        }
    }

  
    public void AutoLogin2()
    {
        Backend.BMember.LoginWithTheBackendToken((callback) =>
        {
            bro = callback;
            isSuccess = callback.IsSuccess();

            if (isSuccess == true)
            {
                Debug.Log("�񵿱� �α��� ����");
            }
            else
            {
                backEnd.MyInstance.ShowErrorUI(bro);
                Debug.Log("�񵿱� �α��� ����");
            }

        });
    }

}
