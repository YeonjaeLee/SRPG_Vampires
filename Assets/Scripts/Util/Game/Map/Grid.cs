using Info;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public static Grid instance;
    [SerializeField]
    private GameObject[] obj_map;
    [SerializeField]
    private Transform tr_Map;
    public FollowCam Camera;
    public Transform tr_Monster;
    public Transform tr_Player;

    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public float nodeRadius;
    Node[,] grid;
    private Vector3 worldBottomLeft;

    float nodeDiameter;
    public int gridSizeX, gridSizeY;
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    
    public List<Block> BlockList = new List<Block>();

    private void Awake()
    {
        instance = this;
        BlockList = new List<Block>();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = gridSizeY = Mathf.RoundToInt(Mathf.Sqrt(GameManager.instance.mapInfo.MapBlockInfo.Count) / nodeDiameter);
        CreateGrid();
        GameManager.instance.CreatePlayer();
        GameManager.instance.CreateMonster();
    }

    private void CreateGrid()
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
                Block cubeBlock = cube.AddComponent<Block>();
                cube.transform.parent = tr_Map;
                cube.transform.position = worldPoint;
                cubeBlock.Setup(GameManager.instance.mapInfo.MapBlockInfo[index]);
                BlockList.Add(cubeBlock);
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
                if (1f < Mathf.Abs(BlockList[blockindex].blockInfo.height - BlockList[curblockindex].blockInfo.height) || BlockList[blockindex].blockInfo.height <= 0)
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
