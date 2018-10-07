using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LoadingGameManager : MonoBehaviour
{ // 사용: LoadingGameManager.LoadScene("Map1");
    public static string nextGameScene;

    [SerializeField]
    Image progressBar;

    private void Awake()
    {
        StartCoroutine(LoadGameScene());
    }

    public static void LoadGameScene(string sceneName)
    {
        nextGameScene = sceneName;
        SceneManager.LoadScene("LoadingGame");
    }

    IEnumerator LoadGameScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextGameScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f)
                {
                    //Application.LoadLevel(nextGameScene);
                    Application.LoadLevelAdditiveAsync("GameUI");
                    UserInfoManager.instance.isLobby = false;
                    yield return new WaitWhile(() => GameManager.instance.mapInfo == null);
                    op.allowSceneActivation = true;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
        }
    }
}
