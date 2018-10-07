//using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using Newtonsoft.Json.Linq;


public class UIManager : Singleton<UIManager>
{
	#region CONSTANTS
	private const string TAG_CANVAS_KEY = "Canvas";
	private const string PATH_UI_PREFAB_POPUP = "UI/Prefab/Popup/";
	private const string POPUP_ONEBTN_NAME = "UIPopup_OneBtn";
	private const string POPUP_TWOBTN_NAME = "UIPopup_TwoBtn";
	#endregion

	#region PROPERTIES
	public bool IsSound = true;
	#endregion


	#region METHODS - reserved
	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		UpdateBackBtn();
	}
	#endregion

	public static string GetSpecialPopupName(int type) {
		switch (type)
		{
		case 1:
			return "UIPopup_Special";
		case 2:
			return "UIPopup_Seasonoffer";
		case 3:
			return "UIPopup_Rateus";
		default:
			throw new Exception("Unknown Type");
		}

	}

	#region METHODS - public
	public T OpenPopup<T>(params object[] args_) where T : UIPopup
	{

		T t = LoadPrefab<T>();
        GameObject overlay = Resources.Load<GameObject>("UI/Prefab/Popup/Popup_Overlay");
        if (t != null)
		{
            if(GetTopPopup() != null)
			    GetTopPopup().Hide();
			T Popup = Instantiate<T>(t, UIManager.a.GetCanvas(), false);
            GameObject ol = Instantiate<GameObject>(overlay, Popup.transform, false);
            ol.transform.SetAsFirstSibling();
            Popup.name = typeof(T).ToString();
			Popup.Init(args_);
			return Popup;
		}
		return null;

	}

	public UIPopup OpenPopup(string curPopup_, params object[] args_)
	{
		UIPopup t = LoadPrefab(curPopup_);
        GameObject overlay = Resources.Load<GameObject>("UI/Prefab/Popup/Popup_Overlay");
		if (t != null)
		{
            if (GetTopPopup() != null)
                GetTopPopup().Hide();
			UIPopup Popup = Instantiate<UIPopup>(t, UIManager.a.GetCanvas(), false);
            GameObject ol = Instantiate<GameObject>(overlay, Popup.transform, false);
            ol.transform.SetAsFirstSibling();
            Popup.name = curPopup_;
            Popup.Init(args_);
			return Popup;
		}
		return null;
	}

	public UIPopup_OneBtn OpenPopup_OneBtn(string msg_, string title_ = null, string btnMsg_ = null, Action actOnClick_ = null)
	{

		return UIPopup_OneBtn.Create(msg_, title_, btnMsg_, actOnClick_);

	}

	public UIPopup_TwoBtn OpenPopup_TwoBtn(string msg_, string title_ = null, string btnMsgLeft_ = null, string btnMsgRight_ = null, Action actOnClickLeft_ = null, Action actOnClickRight_ = null)
	{
		return UIPopup_TwoBtn.Create(msg_, title_, btnMsgLeft_, btnMsgRight_, actOnClickLeft_, actOnClickRight_);

	}

	public void ClosePopup()
	{
		if (GetTopPopup() != null)
		{
			GetTopPopup().Destroy();
			// 이전 팝업을 연다.
			//PrevPopupActive(true);
            StartCoroutine(prepopup());
		}
	}
    IEnumerator prepopup()
    {
        yield return new WaitForSeconds(0.05f);
        PrevPopupActive(true);
    }

	public T GetPopupInst<T>() where T : UIPopup
	{
		return GetCanvas().GetComponentInChildren<T>(true);
	}

	public T CreateObject<T>(T prefab_, Transform tmParnet_) where T : MonoBehaviour
	{
		T obj = GameObject.Instantiate<T>(prefab_);
		obj.transform.SetParent(tmParnet_, false);
		obj.transform.name = System.IO.Path.GetFileNameWithoutExtension(obj.name);
		obj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

		return obj;
	}

	public T CreateObject<T>(string path_, Transform parent_) where T : MonoBehaviour
	{
		T Obj = Resources.Load<T>(path_);
		if (null == Obj)
		{
			Debug.LogError("Path : " + path_ + ", CreateObject Failed");
			return null;
		}

		return GameObject.Instantiate<T>(Obj, parent_);
	}

	public bool IsActivePopup<T>() where T : UIPopup
	{
		T t = GetCanvas().GetComponentInChildren<T> ();
		return t != null;
	}

	public bool IsActivePopup()
	{
		return GetTopPopup () != null;
	}

	public void ClearPopupAll()
	{
		foreach (UIPopup p in GetCanvas().GetComponentsInChildren<UIPopup> (true)) {
			Destroy (p.gameObject);
		}
	}

	public RectTransform GetCanvas()
	{
		GameObject go = GameObject.FindGameObjectWithTag(TAG_CANVAS_KEY);
		if (null == go)
		{
			Debug.LogError("Canvas Tag Find Failed!!");
			return null;
		}

		return go.GetComponent<RectTransform>();;
	}

	public UIPopup GetTopPopup()
	{
		RectTransform canvasTr = UIManager.a.GetCanvas ();
		if (canvasTr.childCount>0)
			return canvasTr.GetChild(canvasTr.childCount-1).GetComponent<UIPopup>();
		return null;
	}

    public UIPopup GetPrePopup()
    {
        RectTransform canvasTr = UIManager.a.GetCanvas();
        if (canvasTr.childCount > 1)
            return canvasTr.GetChild(canvasTr.childCount - 2).GetComponent<UIPopup>();
        return null;
    }

    #endregion



    #region METHODS - private


    private void PrevPopupActive(bool isActive_)
	{
		UIPopup popup = GetTopPopup();
        if(popup != null)
        {
            popup.Show();
            //popup.transform.Show();
        }
	}

	private static T LoadPrefab<T>() where T : UIPopup
	{
		string Path = string.Concat(PATH_UI_PREFAB_POPUP, typeof(T).ToString());
		if (string.IsNullOrEmpty(Path))
		{
			Debug.LogError("PopupName Value Is Null!!");
			return null;
		}

		T Prefab = Resources.Load<T>(Path);
		if (null == Prefab)
		{
			Debug.LogError("Path : " + Path + ", Prefab Load Failed!!");
			return null;
		}

		return Prefab;
	}

	private UIPopup LoadPrefab(string popup_)
	{
		string Path = string.Concat(PATH_UI_PREFAB_POPUP, popup_);
		if (string.IsNullOrEmpty(Path))
		{
			Debug.LogError("PopupName Value Is Null!!");
			return null;
		}

		UIPopup Prefab = Resources.Load<UIPopup>(Path);
		if (null == Prefab)
		{
			Debug.LogError("Path : " + Path + ", Prefab Load Failed!!");
			return null;
		}

		return Prefab;
	}


	private void UpdateBackBtn()
	{
		if (false == Input.GetKeyDown(KeyCode.Escape))
			return;
		
		//if (GameManager.IsNetworkLoading)
		//	return;
		
		UIPopup popup=GetTopPopup ();
		if (popup != null) {
			if (popup.IsEnableBackBtn)
				popup.OnBackKeyEvent ();
		} else {
			//switch (GameManager.instance.CurrentState)
			//{
			//case GameManager.GameState.LOBBY: UIManager.ExitGameDialog(Application.Quit); break;
			//case GameManager.GameState.SLOT:
			//	{
			//		if (BaseSlotManager.a.GetState() == (int)BaseSlotManager.GameState.READY && !BaseSlotManager.a.IsFreeSpinMode)
			//			UIManager.ExitToLobbyDialog(GameManager.instance.BackToLobby);
			//	}
			//	break;
			//}
		}

	}
	#endregion

	#region DEFINED_POPUP
	//public static UIPopup Disconnected3Dialog()
	//{
	//	return a.OpenPopup_OneBtn(I2.Loc.ScriptLocalization.Get("DISCONNECTED3"), I2.Loc.ScriptLocalization.Get("Opps"), I2.Loc.ScriptLocalization.Get("REJOIN"), () =>
	//		{
	//			NetworkManager.AddActionLog(ActionLog.USER_ACTION_CATEGORY_DISCONNECT, ActionLog.USER_ACTION_DISCONNECT_REJOIN_BUTTON);
	//			GameManager.ResetGame();
	//		});
	//}
	//public static void JoinRoomFailedDialog(Action action)
	//{
	//	a.OpenPopup_OneBtn(I2.Loc.ScriptLocalization.Get("JOIN_ROOM_FAIL"), I2.Loc.ScriptLocalization.Get("Error"), I2.Loc.ScriptLocalization.Get("OK"), action);
	//}
	//public static void NovacancyDialog(Action action)
	//{
	//	a.OpenPopup_OneBtn(I2.Loc.ScriptLocalization.Get("NOVACANCY"), I2.Loc.ScriptLocalization.Get("No Vacancy"), I2.Loc.ScriptLocalization.Get("OK"), action);
	//}
	//public static void ExitGameDialog(Action action)
	//{
	//	a.OpenPopup_TwoBtn(I2.Loc.ScriptLocalization.Get("EXITGAME"), I2.Loc.ScriptLocalization.Get("Exit game"), I2.Loc.ScriptLocalization.Get("OK"),I2.Loc.ScriptLocalization.Get("NO"),  action);
	//}
	//public static void ExitToLobbyDialog(Action action)
	//{
	//	a.OpenPopup_TwoBtn(I2.Loc.ScriptLocalization.Get("To Lobby"), I2.Loc.ScriptLocalization.Get("TO_LOBBY"), I2.Loc.ScriptLocalization.Get("OK"),I2.Loc.ScriptLocalization.Get("NO"),  action);
	//}
	//public static void EmailFormatErrorDialog()
	//{
	//	a.OpenPopup_OneBtn(I2.Loc.ScriptLocalization.Get("EMAIL_CHECK"), I2.Loc.ScriptLocalization.Get("Error"), I2.Loc.ScriptLocalization.Get("OK"));
	//}
	//public static void DuplicateLoginDialog(Action<bool, Newtonsoft.Json.Linq.JToken> action)
	//{
	//	a.OpenPopup_TwoBtn(I2.Loc.ScriptLocalization.Get("ALREADY_CONNECTED"), I2.Loc.ScriptLocalization.Get("Warning"), I2.Loc.ScriptLocalization.Get("NO"), I2.Loc.ScriptLocalization.Get("YES"), () =>
	//		{
	//			Application.Quit();
	//		},
	//		() =>
	//		{
	//			NetworkManager.InitializeLogin(true, true, action);
	//		});

	//}
	//public static void DisconnectByDuplicate()
	//{
	//	a.OpenPopup_OneBtn(I2.Loc.ScriptLocalization.Get("ALREADY_CONNECTED"), I2.Loc.ScriptLocalization.Get("Warning"), I2.Loc.ScriptLocalization.Get("YES"), () =>
	//		{
	//			Debug.Log(" =================== Application.Quit() ==================== ");
	//			Application.Quit();
	//		});
	//}
	//public static void BuyErrorPopup(string message = "")
	//{
	//	string TitleMsg = I2.Loc.ScriptLocalization.Get("BUY_ERROR_TITLE");
	//	string InfoMsg = I2.Loc.ScriptLocalization.Get("BUY_ERROR_CONTENTS");
	//	UIManager.a.OpenPopup_OneBtn(InfoMsg + "\n" + message, TitleMsg);

	//}

	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void Dummy(string message = "This is Dummy")
	{
		UIManager.a.OpenPopup_OneBtn(message);
	}
	#endregion
}