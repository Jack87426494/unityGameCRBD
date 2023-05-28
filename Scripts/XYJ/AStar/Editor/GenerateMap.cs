
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateMap
{
    //private static readonly Dictionary<Vector2Int, AStarNode> aStarNodes =
    //    new Dictionary<Vector2Int, AStarNode>();

    //地图的宽
    private static int mapW = 1000;
    //地图的高
    private static int mapH = 1000;
    //玩家位置
    private static Vector2Int playerPos;
    //阻挡物体
    private static GameObject[] blockObjs;

    /// <summary>
    /// 以玩家为中心生成地图
    /// </summary>
    /// <param name="x">横坐标格子个数</param>
    /// <param name="y">纵坐标格子个数</param>
    [MenuItem("GameTool/GenerateCenterMap")]
    public static void GenerateCenterMap()
    {
        //重新生成时把原来的地图清空
        AStarMgr.Instance.aStarNodes.Clear();


        AStarNode tmp_Node;
        Vector2Int tmp_Pos;
        //玩家
        Vector2 pos = GameObject.Find("Player").transform.position;
        playerPos = new Vector2Int(Convert.ToInt32(pos.x), Convert.ToInt32(pos.y));


        //生成所有的格子（不同格子的类型）
        for (int i = playerPos.x - mapW / 2; i < playerPos.x + mapW / 2; ++i)
        {
            for (int j = playerPos.y - mapH / 2; j < playerPos.y + mapH / 2; ++j)
            {
                //aStarNodes[i, j] = new AStarNode(i, j, Random.Range(0, 100) > 10 ? NodeType.Walkable : NodeType.Stop);
                tmp_Node = new AStarNode(i, j, NodeType.Walkable);
                tmp_Pos = new Vector2Int(i, j);
                //ShowColliderText(tmp_Pos);
                AStarMgr.Instance.aStarNodes.Add(tmp_Pos, tmp_Node);
            }
        }

        //设置阻挡
        blockObjs = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blockObjs.Length; i++)
        {
            tmp_Pos = new Vector2Int(Convert.ToInt32(blockObjs[i].transform.position.x),
              Convert.ToInt32(blockObjs[i].transform.position.y));
            AStarMgr.Instance.aStarNodes[tmp_Pos].nodeType = NodeType.Stop;
            Debug.Log("阻挡位置" + tmp_Pos.ToString());
            //ShowColliderText(tmp_Pos,true);
        }

    }
}
