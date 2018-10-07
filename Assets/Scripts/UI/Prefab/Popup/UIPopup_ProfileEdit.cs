using Info;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup_ProfileEdit : UIPopup
{
    #region VASIABLE
    [SerializeField]
    private Text _id;
    [SerializeField]
    private InputField _name;
    [SerializeField]
    private Button close;
    #endregion

    #region base
    protected override void Awake()
    {
        close.AddEvent(OnClick_Close);
    }
    public override void Init(params object[] args_)
    {
        base.Init(args_);
        SetUI();
    }
    #endregion

    #region private
    private void SetUI()
    {
        _id.text = string.Format("ID : {0}", UserInfoManager.instance.userinfo.user_id);
        _name.text = UserInfoManager.instance.userinfo.user_name;
    }
    #endregion

    #region event
    private void OnClick_Close()
    {
        Info_Player.user_name = _name.text;
        UserInfoManager.instance.Update_UserInfo();
        Close();
    }
    #endregion
}
