using Info;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoManager : MonoBehaviour {

    public static UserInfoManager instance;

    public Info_User userinfo
    {
        get
        {
            return info;
        }
    }
    private Info_User info = new Info_User();
    public bool isLobby;

    #region base
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region private
    private void onapplicationquit()
    {
        PlayerPrefs.SetString("user_name", info.user_name);
    }
    #endregion

    #region public
    public void Update_UserInfo()
    {
        info.user_id = Info_Player.user_id;
        info.user_name = Info_Player.user_name;
        PlayerPrefs.SetString("user_name", info.user_name);
    }
    #endregion
}
