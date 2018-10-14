using Info;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public Info_Map mapInfo = new Info_Map();
    //inGame
    [SerializeField]
    private GameObject obj_player;
    [SerializeField]
    private GameObject[] obj_monster;
    [SerializeField]
    private GameObject[] obj_boss;
    [HideInInspector]
    public Player player;

    private void Awake()
    {
        instance = this;
        mapInfo = new Info_Map();
        DontDestroyOnLoad(gameObject);
    }

    #region public
    public void BackLobby()
    {
        if(!UserInfoManager.instance.isLobby)
        {
            mapInfo = null;
            LoadingSceneManager.LoadScene("Lobby");
        }
    }

    public void GoGame()
    {
        if(UserInfoManager.instance.isLobby)
        {
            LoadingGameManager.LoadGameScene("Map1");
        }
    }

    //inGame
    public void CreatePlayer()
    {
        int blockIndex = 0;
        GameObject prefab = Instantiate<GameObject>(obj_player, Grid.instance.tr_Player);
        player = prefab.AddComponent<Player>();
        player.transform.name = "Player";
        while (true)
        {
            blockIndex = Random.Range(0, Grid.instance.BlockList.Count);
            if (GameManager.instance.mapInfo.MapBlockInfo[blockIndex].height > 0)
            {
                break;
            }
        }
        player.transform.position = new Vector3(Grid.instance.BlockList[blockIndex].transform.position.x, GameManager.instance.mapInfo.MapBlockInfo[blockIndex].height, Grid.instance.BlockList[blockIndex].transform.position.z);
        Grid.instance.Camera.target = player.transform;
    }
    public void CreateMonster()
    {
        foreach(Info_Map.BlockInfo m in GameManager.instance.mapInfo.MapBlockInfo)
        {
            if (m.type == (int)Block.BlockType.MONSTER)
            {
                int random = Random.Range(0, obj_monster.Length);
                GameObject prefab = Instantiate<GameObject>(obj_monster[random], Grid.instance.tr_Monster);
                Monster monster = prefab.AddComponent<Monster>();
                monster.transform.name = string.Format("Monster({0})", (Block.BlockType)m.type);
                monster.transform.position = new Vector3(Grid.instance.BlockList[m.index - 1].transform.position.x, m.height, Grid.instance.BlockList[m.index - 1].transform.position.z);
            }
            else if (m.type == (int)Block.BlockType.BOSS)
            {
                int random = Random.Range(0, obj_boss.Length);
                GameObject prefab = Instantiate<GameObject>(obj_boss[random], Grid.instance.tr_Monster);
                Monster monster = prefab.AddComponent<Monster>();
                monster.transform.name = string.Format("Monster({0})", (Block.BlockType)m.type);
                monster.transform.position = new Vector3(Grid.instance.BlockList[m.index - 1].transform.position.x, m.height, Grid.instance.BlockList[m.index - 1].transform.position.z);
            }
        }
    }
    #endregion
}
