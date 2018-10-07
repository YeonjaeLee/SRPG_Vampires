using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup_Sample : UIPopup {

    #region VASIABLE
    [SerializeField] private Text _id, _name;
    [SerializeField]
    private Button close;

    private Action _afterWork = delegate { };
    #endregion

    #region base
    protected override void Awake()
    {
        close.AddEvent(OnClick_Close);
    }
    public override void Init(params object[] args_)
    {
        base.Init(args_);
        if(args_ != null)
        {
            _afterWork = args_[0] as Action;
        }
        SetUI();
    }
    #endregion

    #region private
    private void SetUI()
    {
        _id.text = string.Format("ID : {0}", UserInfoManager.instance.userinfo.user_id);
        _name.text = string.Format("NAME : {0}", UserInfoManager.instance.userinfo.user_name);
    }
    #endregion

    #region event
    private void OnClick_Close()
    {
        if (_afterWork != null)
        {
            Lobby.instance.SetEvent(null, _afterWork);
            _afterWork = null;
        }
        Close();
    }
    #endregion
}
