using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    [SerializeField]
    private Button backlobby;

    #region base
    private void Awake()
    {
        backlobby.AddEvent(OnClick_BackLobby);
    }
    #endregion

    #region event
    private void OnClick_BackLobby()
    {
        GameManager.instance.BackLobby();
    }
    #endregion
}
