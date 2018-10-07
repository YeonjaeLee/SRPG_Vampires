using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPopup_OneBtn : UIPopup
{
	#region VARIABLES
	private Text _txtMsg;
	private Text _txtTitle;
	private Text _txtBtn;
    private Button _button;
    private Button _close;
    private Action _actOnClick;
	#endregion

	#region METHODS - reserved
    public override void Init(params object[] args)
    {
        base.Init(args);

		_txtMsg = transform.Find("Dialog/Text_Msg").GetComponent<Text>();//transform.Find<Text>("Dialog/Text_Msg");
        _txtTitle = transform.Find("Dialog/Text_Title").GetComponent<Text>();//transform.Find<Text>("Dialog/Text_Title");
		_txtBtn = transform.Find("Dialog/Button/Text").GetComponent<Text>(); //transform.Find<Text>("Dialog/Button/Text");
        _button = transform.Find("Dialog/Button").GetComponent<Button>();//transform.Find<Button>("Dialog/Button").AddEvent(OnClick_BtnConfirm);
        _button.onClick.AddListener(OnClick_BtnConfirm);
        _close = transform.Find("Dialog/close_button").GetComponent<Button>();//transform.Find<Button>("Dialog/close_button").AddEvent(OnClick_BtnConfirm);
        _close.onClick.AddListener(OnClick_BtnConfirm);
    }
	#endregion

	#region METHODS - public
    public static UIPopup_OneBtn Create(string msg_, string title_ = null, string btnMsg_ = null, Action actOnClick_ = null, Action actOnBack_ = null)
	{
        UIPopup_OneBtn popup=UIManager.a.OpenPopup<UIPopup_OneBtn>();

        popup._txtMsg.text = null == msg_ ? msg_ : string.Empty;
        popup._txtTitle.text = null == title_ ? title_ : "Error";//I2.Loc.ScriptLocalization.Get("Error");
        popup._txtBtn.text = null == btnMsg_ ? btnMsg_ : "OK";//I2.Loc.ScriptLocalization.Get("OK");
        popup._actOnClick = actOnClick_;
        popup.OnBackEvent = actOnBack_;
        return popup;
	}
	#endregion

	#region METHODS - event
	private void OnClick_BtnConfirm()
	{
		Close();

		if (null != _actOnClick)
		{
			_actOnClick();
		}
	}

	#endregion
}
