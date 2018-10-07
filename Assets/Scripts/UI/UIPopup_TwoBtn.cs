using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPopup_TwoBtn : UIPopup
{
	#region VARIABLES
	private Text _txtMsg;
	private Text _txtTitle;
	private Text _txtBtnLeft;
	private Text _txtBtnRight;
    private Button _buttonLeft;
    private Button _buttonRight;
    private Button _close;
    private Action _actOnClickLeft;
	private Action _actOnClickRight;
	#endregion


	#region METHODS - reserved
	protected override void Awake()
	{
		base.Awake();

		_txtMsg = transform.Find("Dialog/Text_Msg").GetComponent<Text>();//transform.Find<Text>("Dialog/Text_Msg");
        _txtTitle = transform.Find("Dialog/Text_Title").GetComponent<Text>();//transform.Find<Text>("Dialog/Text_Title");
        _txtBtnLeft = transform.Find("Dialog/Button_Left/Text").GetComponent<Text>(); //transform.Find<Text>("Dialog/Button_Left/Text");
		_txtBtnRight = transform.Find("Dialog/Button_Right/Text").GetComponent<Text>(); //transform.Find<Text>("Dialog/Button_Right/Text");
        _buttonLeft = transform.Find("Dialog/Button_Left").GetComponent<Button>();//transform.Find<Button>("Dialog/Button_Left").AddEvent(OnClick_BtnLeft);
        _buttonLeft.onClick.AddListener(OnClick_BtnLeft);
        _buttonRight = transform.Find("Dialog/Button_Right").GetComponent<Button>();//transform.Find<Button>("Dialog/Button_Right").AddEvent(OnClick_BtnRight);
        _buttonRight.onClick.AddListener(OnClick_BtnRight);
        _close = transform.Find("Dialog/close_button").GetComponent<Button>();//transform.Find<Button>("Dialog/close_button").AddEvent(OnClick_BtnRight);
        _close.onClick.AddListener(OnClick_BtnRight);
    }
	#endregion


	#region METHODS - base
	#endregion


	#region METHODS - public

    public static UIPopup_TwoBtn Create(string msg_, string title_ = null, string btnMsgLeft_ = null, string btnMsgRight_ = null, Action actOnClickLeft_ = null, Action actOnClickRight_ = null)
    {
        UIPopup_TwoBtn popup = UIManager.a.OpenPopup<UIPopup_TwoBtn>();
        popup._txtMsg.text = null == msg_ ? msg_ : string.Empty;
        popup._txtTitle.text = null == title_ ? title_ : "ERROR";// I2.Loc.ScriptLocalization.Get("ERROR");
        popup._txtBtnLeft.text = null == btnMsgLeft_ ? btnMsgLeft_ : "NO";// I2.Loc.ScriptLocalization.Get("NO");
        popup._txtBtnRight.text = null == btnMsgRight_ ? btnMsgRight_ : "OK";// I2.Loc.ScriptLocalization.Get("OK");
        popup._actOnClickLeft += actOnClickLeft_;
        popup._actOnClickRight += actOnClickRight_;
       
        return popup;
    }


	#endregion


	#region METHODS - event
	private void OnClick_BtnLeft()
	{
		Close();
		if (null != _actOnClickLeft)
		{
			_actOnClickLeft();
		}
		
	}

	private void OnClick_BtnRight()
	{
		Close();

		if (null != _actOnClickRight)
		{
			_actOnClickRight();
		}
		
	}

    #endregion
}
