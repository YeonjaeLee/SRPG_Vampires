using Info;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public Info_Map mapInfo = new Info_Map();

    private void Awake()
    {
        instance = this;
        mapInfo = new Info_Map();
        DontDestroyOnLoad(gameObject);
    }

    #region public
    public void BackLobby()
    {
        if(!UserInfoManager.instance.isLobby)
        {
            mapInfo = null;
            LoadingSceneManager.LoadScene("Lobby");
        }
    }

    public void GoGame()
    {
        if(UserInfoManager.instance.isLobby)
        {
            LoadingGameManager.LoadGameScene("Map1");
        }
    }
    #endregion
}
