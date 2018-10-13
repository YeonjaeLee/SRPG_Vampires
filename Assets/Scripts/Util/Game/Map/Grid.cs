using Info;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    [SerializeField]
    private Transform Map;
    [SerializeField]
    private FollowCam Camera;
    [SerializeField]
    private GameObject[] obj_map;
    [SerializeField]
    private GameObject obj_player;

    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public float nodeRadius;
    Node[,] grid;
    private Vector3 worldBottomLeft;

    float nodeDiameter;
    public int gridSizeX, gridSizeY;

    //[Header("[블록]")]
    public static List<GameObject> BlockList = new List<GameObject>();
    public enum BlockType
    {
        NONE = 0,
        NOMAL,
        MONSTER,
        BOSS,
        WALL,
    }

    public static Unit player;

    void Awake()
    {
        BlockList = new List<GameObject>();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = gridSizeY = Mathf.RoundToInt(Mathf.Sqrt(GameManager.instance.mapInfo.MapBlockInfo.Count) / nodeDiameter);
        CreateGrid();
        SetMap();
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        int blockIndex = 0;
        GameObject prefab = Instantiate<GameObject>(obj_player, transform);
        player = prefab.AddComponent<Unit>();
        player.transform.name = "Player";
        while(true)
        {
            blockIndex = Random.Range(0, BlockList.Count);
            if(GameManager.instance.mapInfo.MapBlockInfo[blockIndex].height > 0)
            {
                break;
            }
        }
        player.transform.position = new Vector3(BlockList[blockIndex].transform.position.x, GameManager.instance.mapInfo.MapBlockInfo[blockIndex].height, BlockList[blockIndex].transform.position.z);
        Camera.target = player.transform;
    }

    public void SetMap()
    {
        for (int i = 0; i < BlockList.Count; i++)
        {
            float y = GameManager.instance.mapInfo.MapBlockInfo[i].height;
            BlockList[i].transform.position = new Vector3(BlockList[i].transform.position.x, y / 2, BlockList[i].transform.position.z);
            BlockList[i].transform.localScale = new Vector3(BlockList[i].transform.localScale.x, y, BlockList[i].transform.localScale.z);
            switch (GameManager.instance.mapInfo.MapBlockInfo[i].type)
            {
                case (int)BlockType.NONE:
                    BlockList[i].transform.localScale = new Vector3(BlockList[i].transform.localScale.x, 0.1f, BlockList[i].transform.localScale.z);
                    break;

                case (int)BlockType.NOMAL:
                    break;

                case (int)BlockType.MONSTER:
                    break;

                case (int)BlockType.BOSS:
                    break;

                case (int)BlockType.WALL:
                    break;
            }
        }
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        worldBottomLeft = transform.position - Vector3.right * gridSizeX / 2 - Vector3.forward * gridSizeY / 2;

        int index = 0;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
                
                GameObject cube = Instantiate<GameObject>(obj_map[GameManager.instance.mapInfo.MapBlockInfo[index].type], transform);
                switch (GameManager.instance.mapInfo.MapBlockInfo[index].type)
                {
                    case (int)BlockType.NOMAL:
                        break;

                    case (int)BlockType.MONSTER:
                        break;

                    case (int)BlockType.BOSS:
                        break;

                    case (int)BlockType.WALL:
                        break;
                }
                cube.transform.parent = Map;
                cube.transform.position = worldPoint;
                cube.AddComponent<Block>();
                Block.instance.Setup(GameManager.instance.mapInfo.MapBlockInfo[index]);
                BlockList.Add(cube);
                index++;
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        int curblockindex = node.gridX * gridSizeX + node.gridY * 1;
        for (int x = -1; x <= 1; x++)
        {
            if ((node.gridX == 0 && x == -1) || (node.gridX == gridSizeX && x == 1))
                continue;
            for (int y = -1; y <= 1; y++)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y) || ((node.gridY == 0 && y == -1) || (node.gridY == gridSizeY && y == 1)))
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                int blockindex = checkX * gridSizeX + checkY * 1;

                if (blockindex >= BlockList.Count)
                    continue;
                if (1f < Mathf.Abs(BlockList[blockindex].transform.localScale.y - BlockList[curblockindex].transform.localScale.y) || BlockList[blockindex].transform.localScale.y <= 0.5f)
                {
                    continue;
                }
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridSizeX / 2) / gridSizeX;
        float percentY = (worldPosition.z + gridSizeY / 2) / gridSizeY;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }


    public Vector3 WorldPointFromNode(Node node)
    {
        Node Node = node;
        int x = Node.gridX;
        int y = Node.gridY;
        int roundrad = Mathf.RoundToInt(nodeRadius);
        int posx = Mathf.RoundToInt(worldBottomLeft.x) + (x * roundrad);
        int posy = Mathf.RoundToInt(worldBottomLeft.y) + (y * roundrad);
        Vector3 pos = new Vector3(posx, 0, posy);
        return pos;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, 1, gridSizeY));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
