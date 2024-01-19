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
         //public GameObject[] GETchart;//프리팹 배열

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
            Debug.Log("회원가입 성공");

        }
        else
        {
            backEnd.MyInstance.ShowErrorUI(BRO);
            Debug.Log("회원가입 실패.");
        }
    }

    //동기화 로그인 방식

    /*public void OnClickLoading1() //로딩 방식 이렇게 하면 그냥 포톤 사용 안되고, main 으로 넘어가짐 
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
            //GETchart = GameObject.FindGameObjectsWithTag("Building"); //차트 프리팹 파일 불러오는데 태그로.

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
   
    //비동기 회원가입
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

    //비동기 로그인 방식
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

    //비동기 로그인 방식
    public void AutoLogin1()
    {
        BackendReturnObject backendReturnObject = Backend.BMember.LoginWithTheBackendToken();

        if (backendReturnObject.IsSuccess()==true)
        {
            Debug.Log("동기화 로그인 성공");
        }
        else
        {
            backEnd.MyInstance.ShowErrorUI(backendReturnObject);
            Debug.Log("동기화 로그인 실패");
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
                Debug.Log("비동기 로그인 성공");
            }
            else
            {
                backEnd.MyInstance.ShowErrorUI(bro);
                Debug.Log("비동기 로그인 실패");
            }

        });
    }

}
