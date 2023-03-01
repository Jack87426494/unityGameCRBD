using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AStarAlgorithm : MonoBehaviour
{
    List<Vector2Int> shortestPath = new List<Vector2Int>();

    private Vector2Int startPos;
    private Vector2Int endPos;

    private int maxStep = 1000; //最大搜索步数

    private void Awake()
    {
        Vector2 temp = GameObject.Find("hero").transform.position;
        startPos = new Vector2Int(Convert.ToInt32(temp.x), Convert.ToInt32(temp.y));
        temp = GameObject.Find("end").transform.position;
        endPos = new Vector2Int(Convert.ToInt32(temp.x), Convert.ToInt32(temp.y));

        //确定起点和终点
        Debug.Log("startPos: " + startPos + ", endPos: " + endPos);
    }

    // Start is called before the first frame update
    void Start()
    {
        //1、读取地图碰撞信息信息
        MapManager.Instance.ReadMap(startPos, endPos);

        //2、根据起点、终点和地图信息，使用A*算法搜索路径
        shortestPath = FindPath(startPos, endPos, MapManager.Instance.GetMap());

        //3、OnDrawGizmosSelected()将搜索结果可视化
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    List<Block> boundaryBlock = new List<Block>(); //保存边缘待探索的区域
    public List<Vector2Int> FindPath(Vector2Int startPos, Vector2Int endPos, Dictionary<Vector2Int, Block> map)
    {
        List<Vector2Int> path = new List<Vector2Int>(); //保存最终路径

        Block curBlock = map[startPos];
        curBlock.isChecked = true;
        curBlock.UpdateCurCost();

        int searchCount = 0;
        //结束条件：若当前块为终点块，则结束查找
        while (curBlock.position != endPos && searchCount < maxStep)
        {
            //收集周围未遍历的方块
            foreach (Vector2Int neighbor in Neighbors(curBlock.position))
            {
                Block block = map[neighbor];
                if (!block.isCollider && !block.isChecked && !boundaryBlock.Contains(block))
                {
                    if (block.pre_block == null)
                    {
                        block.pre_block = curBlock;
                    }
                    block.UpdateCurCost();
                    boundaryBlock.Add(block);
                }
            }

            //DebugList(boundaryBlock);

            boundaryBlock.Sort(new ComparerBlock()); //对可到达的边界方块进行排序

            curBlock = boundaryBlock[0];
            curBlock.isChecked = true;
            boundaryBlock.Remove(boundaryBlock[0]);

            searchCount++;

            ShowCost(curBlock);
        }

        //将路径添加到path
        while (curBlock != null)
        {
            path.Add(curBlock.position);
            curBlock = curBlock.pre_block;
        }

        path.Reverse(); //反转路径，以起点到终点的顺序进行输出
        DebugPath(path); //将路径信息在控制台输出
        return path;
    }

    private void DebugPath(List<Vector2Int> path)
    {
        int i = 1;
        foreach (Vector2Int pos in path)
        {
            Debug.Log("*最终返回路径 " + i + ":" + pos);
            i++;
        }
    }

    private static void ShowCost(Block curBlock)
    {
        string blockName = curBlock.position.x + "," + curBlock.position.y + "(Clone)";
        string str = "\n" + curBlock.CostString();
        //Debug.Log(str);
        Text textGO = GameObject.Find(blockName).GetComponent<Text>();
        if(textGO != null) textGO.text += str;
        else
        {
            Debug.LogWarning("找不到文本对象");
        }
    }

    private static void DebugList(List<Block> list)
    {
        Debug.Log("list count:" + list.Count);
        foreach(Block block in list)
        {
            Debug.Log(block.position + " ");
        }
    }

    public Vector2Int[] Neighbors(Vector2Int pos)
    {
        Vector2Int[] neighbors = new Vector2Int[4];
        neighbors[0] = pos + new Vector2Int(0, 1);
        neighbors[1] = pos + new Vector2Int(0, -1);
        neighbors[2] = pos + new Vector2Int(1, 0);
        neighbors[3] = pos + new Vector2Int(-1, 0);

        return neighbors;
    }

    /// <summary>
    /// 绘制路径
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for(int i = 1; i < shortestPath.Count; i++)
        {
            Vector3 pos_1 = new Vector3(shortestPath[i - 1].x + 0.5f, shortestPath[i - 1].y + 0.5f, 0);
            Vector3 pos_2 = new Vector3(shortestPath[i].x + 0.5f, shortestPath[i].y + 0.5f, 0);
            Gizmos.DrawLine(pos_1, pos_2);
        }
    }

    //定义一个排序规则，内部类
    public class ComparerBlock : IComparer<Block>
    {
        public int Compare(Block x, Block y)
        {
            int offset = x.GetAllCost() - y.GetAllCost();
            return offset;
        }
    }
}
