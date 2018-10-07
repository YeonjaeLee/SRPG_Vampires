using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup_Profile : UIPopup
{

    #region VASIABLE
    [SerializeField]
    Text _id, _name;
    [SerializeField]
    private Button edit, close;
    #endregion

    #region base
    protected override void Awake()
    {
        edit.AddEvent(OnClick_Edit);
        close.AddEvent(OnClick_Close);
    }

    public override void Init(params object[] args_)
    {
        base.Init(args_);
        SetUI();
    }

    public override void Show(params object[] args_)
    {
        base.Show(args_);
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
        Close();
    }

    private void OnClick_Edit()
    {
        UIManager.a.OpenPopup<UIPopup_ProfileEdit>();
    }
    #endregion
}