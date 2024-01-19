using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
 
    private static LoadingController instance;

    public static LoadingController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingController>();
                if (obj != null)
                {
                    instance = obj;

                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    private static LoadingController Create()
    {
        return Instantiate(Resources.Load<LoadingController>("LoaingScene"));


    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);//실행 될 때, 이 오브젝트는 파괴 되지마라.

    }
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Image ProgressBar;

    private string loadSceneName;

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded; //씬이 끝나는 지점.시간
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        ProgressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;

        while(!op.isDone) 
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                ProgressBar.fillAmount = op.progress;
            }
            else 
            {
                timer += Time.unscaledDeltaTime;
                ProgressBar.fillAmount = Mathf.Lerp(0.9f,1f,timer);
                if(ProgressBar.fillAmount >=1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }

            }
        }
     
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <=1f) 
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f,1f,timer) : Mathf.Lerp(1f, 0f, timer);

        
        
        }
        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }






}







