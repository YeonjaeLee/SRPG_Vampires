using Info;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour {

    public int Index;
    public Info_Map.BlockInfo blockInfo;
    public enum BlockType
    {
        NONE = 0,
        NOMAL,
        MONSTER,
        BOSS,
        WALL,
        EXIT,
    }
    
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

            case BlockType.EXIT:
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
                int randomIndex = Random.Range(0, 2);
                if(randomIndex > 0)
                {
                    GameObject nomal_prefab = Resources.Load<GameObject>(Const.MAP_BLOCKTYPE_PATH_NOMAL);
                    GameObject nomal = Instantiate<GameObject>(nomal_prefab, transform.parent);
                    nomal.transform.position = new Vector3(nomal.transform.position.x + transform.position.x, blockInfo.height, nomal.transform.position.z + transform.position.z);
                }
                break;

            case BlockType.MONSTER:
                break;

            case BlockType.BOSS:
                break;

            case BlockType.WALL:
                GameObject wall_prefab = Resources.Load<GameObject>(Const.MAP_BLOCKTYPE_PATH_WALL);
                GameObject wall = Instantiate<GameObject>(wall_prefab, transform.parent);
                wall.transform.position = new Vector3(wall.transform.position.x + transform.position.x, blockInfo.height, wall.transform.position.z + transform.position.z);
                break;

            case BlockType.EXIT:
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
            GameManager.instance.player.PlayerMove(this);
        }
    }
    #endregion
}
