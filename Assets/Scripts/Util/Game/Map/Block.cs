using Info;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour {

    public int Index;
    public static Block instance;   // 삭제 필요
    public Info_Map.BlockInfo blockInfo;
    private Material material;
    public enum BlockType
    {
        NONE = 0,
        NOMAL,
        MONSTER,
        BOSS,
        WALL,
    }

    #region base
    private void Awake() {
        instance = this;
        //transform.GetComponent<Renderer>().material.color = Color.gray;
    }
    #endregion

    #region public
    public void Setup(Info_Map.BlockInfo _blockInfo)
    {
        blockInfo = _blockInfo;
        Index = _blockInfo.index;
        InitUI();
        SetUpUI();
    }
    #endregion

    #region private
    private void InitUI()
    {
        float y = blockInfo.height;
        transform.position = new Vector3(transform.position.x, y / 2, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        switch ((BlockType)blockInfo.type)
        {
            case BlockType.NONE:
                transform.localScale = new Vector3(transform.localScale.x, 0.1f, transform.localScale.z);
                break;

            case BlockType.NOMAL:
                break;

            case BlockType.MONSTER:
                break;

            case BlockType.BOSS:
                break;

            case BlockType.WALL:
                break;
        }
    }

    private void SetUpUI()
    {
        switch ((BlockType)blockInfo.type)
        {
            case BlockType.NONE:
                break;

            case BlockType.NOMAL:
                break;

            case BlockType.MONSTER:
                break;

            case BlockType.BOSS:
                break;

            case BlockType.WALL:
                break;
        }
    }
    #endregion

    #region event
    private void OnMouseUp()
    {
        if (blockInfo.height == 0)
            return;

        if(!DragCameraMove.Drag && !EventSystem.current.IsPointerOverGameObject())
        {
            Grid.player.PlayerMove(this);
        }
    }
    #endregion
}
