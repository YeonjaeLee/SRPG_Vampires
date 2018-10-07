using Info;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour {

    [SerializeField]
    private Slider progressBar;
    private bool isInfoLoad = false;

    private void Awake()
    {
        StartCoroutine(GetPlayerInfo());
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync("Lobby");
        op.allowSceneActivation = false;
        
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress >= 0.9f)
            {
                if(isInfoLoad)
                {
                    progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);

                    if (progressBar.value == 1.0f)
                    {
                        op.allowSceneActivation = true;
                    }
                }
            }
            else
            {
                progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);
                if (progressBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
        }
    }

    IEnumerator GetPlayerInfo()
    {
        if(!PlayerPrefs.HasKey("user_id"))
        {
            PlayerPrefs.SetString("user_id", SystemInfo.deviceUniqueIdentifier);
            PlayerPrefs.SetString("user_name", "NON_NAME");
        }
        Info_Player.user_id = PlayerPrefs.GetString("user_id");
        Info_Player.user_name = PlayerPrefs.GetString("user_name");
        yield return null;
        isInfoLoad = true;
        UserInfoManager.instance.Update_UserInfo();
    }

}
