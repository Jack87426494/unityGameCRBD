
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateMap
{
    //private static readonly Dictionary<Vector2Int, AStarNode> aStarNodes =
    //    new Dictionary<Vector2Int, AStarNode>();

    //��ͼ�Ŀ�
    private static int mapW = 1000;
    //��ͼ�ĸ�
    private static int mapH = 1000;
    //���λ��
    private static Vector2Int playerPos;
    //�赲����
    private static GameObject[] blockObjs;

    /// <summary>
    /// �����Ϊ�������ɵ�ͼ
    /// </summary>
    /// <param name="x">��������Ӹ���</param>
    /// <param name="y">��������Ӹ���</param>
    [MenuItem("GameTool/GenerateCenterMap")]
    public static void GenerateCenterMap()
    {
        //��������ʱ��ԭ���ĵ�ͼ���
        AStarMgr.Instance.aStarNodes.Clear();


        AStarNode tmp_Node;
        Vector2Int tmp_Pos;
        //���
        Vector2 pos = GameObject.Find("Player").transform.position;
        playerPos = new Vector2Int(Convert.ToInt32(pos.x), Convert.ToInt32(pos.y));


        //�������еĸ��ӣ���ͬ���ӵ����ͣ�
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

        //�����赲
        blockObjs = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blockObjs.Length; i++)
        {
            tmp_Pos = new Vector2Int(Convert.ToInt32(blockObjs[i].transform.position.x),
              Convert.ToInt32(blockObjs[i].transform.position.y));
            AStarMgr.Instance.aStarNodes[tmp_Pos].nodeType = NodeType.Stop;
            Debug.Log("�赲λ��" + tmp_Pos.ToString());
            //ShowColliderText(tmp_Pos,true);
        }

    }
}
