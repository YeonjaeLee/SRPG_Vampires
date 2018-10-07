using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUI : MonoBehaviour {

    [SerializeField]
    private Transform profile;

    #region base
    private void Awake()
    {
        profile.GetComponent<Button>().AddEvent(OnClick_Profile);
    }
    #endregion

    #region event
    private void OnClick_Profile()
    {
        //Lobby.instance.SetEvent("UIPopup_Profile", new Action(() => { UIManager.a.OpenPopup<UIPopup_ProfileEdit>(); }));
        UIManager.a.OpenPopup<UIPopup_Profile>();
    }
    #endregion
}
