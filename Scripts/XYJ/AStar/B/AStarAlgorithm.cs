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

    private int maxStep = 1000; //�����������

    private void Awake()
    {
        Vector2 temp = GameObject.Find("hero").transform.position;
        startPos = new Vector2Int(Convert.ToInt32(temp.x), Convert.ToInt32(temp.y));
        temp = GameObject.Find("end").transform.position;
        endPos = new Vector2Int(Convert.ToInt32(temp.x), Convert.ToInt32(temp.y));

        //ȷ�������յ�
        Debug.Log("startPos: " + startPos + ", endPos: " + endPos);
    }

    // Start is called before the first frame update
    void Start()
    {
        //1����ȡ��ͼ��ײ��Ϣ��Ϣ
        MapManager.Instance.ReadMap(startPos, endPos);

        //2��������㡢�յ�͵�ͼ��Ϣ��ʹ��A*�㷨����·��
        shortestPath = FindPath(startPos, endPos, MapManager.Instance.GetMap());

        //3��OnDrawGizmosSelected()������������ӻ�
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    List<Block> boundaryBlock = new List<Block>(); //�����Ե��̽��������
    public List<Vector2Int> FindPath(Vector2Int startPos, Vector2Int endPos, Dictionary<Vector2Int, Block> map)
    {
        List<Vector2Int> path = new List<Vector2Int>(); //��������·��

        Block curBlock = map[startPos];
        curBlock.isChecked = true;
        curBlock.UpdateCurCost();

        int searchCount = 0;
        //��������������ǰ��Ϊ�յ�飬���������
        while (curBlock.position != endPos && searchCount < maxStep)
        {
            //�ռ���Χδ�����ķ���
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

            boundaryBlock.Sort(new ComparerBlock()); //�Կɵ���ı߽緽���������

            curBlock = boundaryBlock[0];
            curBlock.isChecked = true;
            boundaryBlock.Remove(boundaryBlock[0]);

            searchCount++;

            ShowCost(curBlock);
        }

        //��·����ӵ�path
        while (curBlock != null)
        {
            path.Add(curBlock.position);
            curBlock = curBlock.pre_block;
        }

        path.Reverse(); //��ת·��������㵽�յ��˳��������
        DebugPath(path); //��·����Ϣ�ڿ���̨���
        return path;
    }

    private void DebugPath(List<Vector2Int> path)
    {
        int i = 1;
        foreach (Vector2Int pos in path)
        {
            Debug.Log("*���շ���·�� " + i + ":" + pos);
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
            Debug.LogWarning("�Ҳ����ı�����");
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
    /// ����·��
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

    //����һ����������ڲ���
    public class ComparerBlock : IComparer<Block>
    {
        public int Compare(Block x, Block y)
        {
            int offset = x.GetAllCost() - y.GetAllCost();
            return offset;
        }
    }
}
