using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

    public static Lobby instance;

    #region VASIABLE
    public bool isLoginEvent = true;
    [SerializeField]
    private Button btn_game;

    Queue<GameEvent> GameEventList = new Queue<GameEvent>();
    Action endEvent = null;
    public struct GameEvent
    {
        public string popupType;
        public object[] args_;

        public GameEvent(string _popupType, params object[] _args_)
        {
            popupType = _popupType;
            args_ = _args_;
        }
    }
    //public delegate void DeleEvent(Action ac);
    //public DeleEvent _deleEvent;
    #endregion

    #region base
    private void Awake()
    {
        instance = this;

        UserInfoManager.instance.isLobby = true;
        Lobby.instance.SetEvent();

        btn_game.onClick.AddListener(OnClick_GoGame);
    }
    #endregion

    #region public
    private void SetEventList(GameEvent ev) // 0:일반 팝업, 1:event 팝업, args_[0]:afterWork
    {
        if (string.IsNullOrEmpty(ev.popupType))
            return;
        GameEventList.Enqueue(ev);
    }

    public void SetEvent(string popuptype = null, Action afterWork = null)
    {
        if(string.IsNullOrEmpty(popuptype))
        {
            if (isLoginEvent && MenuSetting.st.isProfilePopup)
            {
                GameEvent ev = new GameEvent("UIPopup_Profile");
                SetEventList(ev);
            }

            if(isLoginEvent && MenuSetting.st.isProfileEditPopup)
            {
                GameEvent ev = new GameEvent("UIPopup_ProfileEdit");
                SetEventList(ev);
            }

            if (isLoginEvent && MenuSetting.st.isLobbyPopup)
            {
                GameEvent ev = new GameEvent("UIPopup_Sample", new Action(() => { UIManager.a.OpenPopup<UIPopup_Profile>(); }));
                SetEventList(ev);
            }
        }

        if (!string.IsNullOrEmpty(popuptype))
        {
            GameEvent ev = new GameEvent(popuptype);
            SetEventList(ev);
        }

        if (afterWork != null)
            endEvent = afterWork;

        isLoginEvent = false;
        NextEvent();
    }

    public void NextEvent()
    {
        if(GameEventList.Count == 0)
        {
            GameEventList.Clear();
            if(endEvent != null)
            {
                endEvent();
                endEvent = null;
            }
        }
        else
        {
            GameEvent ev = GameEventList.Dequeue();
            UIManager.a.OpenPopup(ev.popupType, ev.args_).OnBackEvent += NextEvent;
        }
    }
    #endregion

    #region event
    public void OnClick_GoGame()
    {
        GameManager.instance.GoGame();
    }
    #endregion
}
