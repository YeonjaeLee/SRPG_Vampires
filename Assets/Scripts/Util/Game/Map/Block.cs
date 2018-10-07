using Info;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour {

    public int Index;
    public static Block instance;   // 삭제 필요
    private Info_Map.BlockInfo blockInfo;
    private Material material;

    #region base
    private void Awake() {
        instance = this;
        transform.GetComponent<Renderer>().material.color = Color.gray;
    }
    #endregion

    #region public
    public void Setup(Info_Map.BlockInfo _blockInfo)
    {
        blockInfo = _blockInfo;
        Index = _blockInfo.index;
    }
    #endregion

    #region event
    private void OnMouseUp()
    {
        if (blockInfo.height == 0)
            return;

        if(!DragCameraMove.Drag && !EventSystem.current.IsPointerOverGameObject())
        {
            Grid.MovePlayer(transform);
        }
    }
    #endregion
}
