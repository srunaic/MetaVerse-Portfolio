using UnityEngine;
using BackEnd;

public class backEnd : MonoBehaviour
{
    private string testNickName = null;

    private static backEnd instance = null; //?????? db ???? ??.

    public static backEnd MyInstance { get => instance; set => instance = value; }
  
    //single tone
    void Awake()
    {
        if(instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        Backend.Initialize(HandleBackendCallback);
    }
    void HandleBackendCallback() 
    { 
        if (Backend.IsInitialized)
        {
            Debug.Log("백앤드 해쉬 정보 불러옴.");

            if (!Backend.Utils.GetGoogleHash().Equals(""))
                Debug.Log(Backend.Utils.GetGoogleHash());
      
            Debug.Log(Backend.Utils.GetServerTime());
        
        }
        else
        {
            Debug.LogError("오류");
        }
    }
    public void ShowErrorUI(BackendReturnObject backendReturn)
    {
        int statusCode = int.Parse(backendReturn.GetStatusCode());

        switch(statusCode) 
        {
            case 401:
                Debug.Log("ID or PW is wrong.");
                loginScript.errorTxt = "ID or PW is wrong";
                break;
            case 403:
                Debug.Log(backendReturn.GetErrorCode());
                break;
            case 404:
                Debug.Log("game not found,game (??(??)???? ?? ????????.)");
                loginScript.errorTxt = "Game not found";
                break;
            case 408:
                //???? ???? ????
                Debug.Log(backendReturn.GetMessage());
                break;
            case 409:
                Debug.Log("ID already exists.");
                loginScript.errorTxt = "ID already exists";
                break;
            case 410:
                Debug.Log("bad refreshToken, ?????? refreshToken ??????.");
                loginScript.errorTxt = "bad refreshToken";
                break;
            case 429:
                //?????? ?????? ?????? ??????.
                //???????? ??
                Debug.Log(backendReturn.GetMessage());
                loginScript.errorTxt = backendReturn.GetMessage();
                break;
            case 503:
                //?????? ???? ?? ??
                Debug.Log(backendReturn.GetMessage());
                loginScript.errorTxt = backendReturn.GetMessage();
                break;
            case 504:
                //???? ???? ???? (???? ????)
                Debug.Log(backendReturn.GetMessage());
                loginScript.errorTxt = backendReturn.GetMessage();
                break;
            default:
                loginScript.errorTxt = "First enter ID and PW";
                break;
        }
    }
    
    public void SetName(string nickName)
    {
        testNickName = nickName;
    }

    public string GetName()
    {
        return testNickName;
    }

    void Update()
    {
        Backend.AsyncPoll();
    }
}
