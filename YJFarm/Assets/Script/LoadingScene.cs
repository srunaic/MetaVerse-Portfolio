using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LoadingScene : MonoBehaviourPunCallbacks
{
    static string nextScene;

    [SerializeField]
    private Image progressbar;

    public void Start()
    {
          PhotonNetwork.AutomaticallySyncScene = true;
          StartCoroutine(LoadSceneProgress());   
    }
    public static void LoadScene(string sceneName)
    {
            nextScene = sceneName;
            SceneManager.LoadScene("LoadingScene"); 
    }

    IEnumerator LoadSceneProgress()
    {
           AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;
            float timer = 0f;

            while (!op.isDone)
            {
               yield return new WaitForSeconds(0.001f);

                if (op.progress < 0.9f)
                {
                    progressbar.fillAmount = op.progress;
                }
                else
                {
                    timer += Time.unscaledDeltaTime;
                    progressbar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

                    if (progressbar.fillAmount >= 1f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }

                }
            }

        }
    
    
}



