using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Convert = System.Convert;

/// <summary>
/// a��Ѱ·������
/// </summary>
public class AStarMgr : BaseMgr<AStarMgr>
{
   
    //ȫ��ͼ�ĸ�����Ϣ
    //private static AStarNode[,] aStarNodes;
    public Dictionary<Vector2Int, AStarNode> aStarNodes=
        new Dictionary<Vector2Int, AStarNode>();

    //��ͼ�Ŀ�
    private static int mapW = 1000;
    //��ͼ�ĸ�
    private static int mapH = 1000;
    //�赲����
    private static GameObject[] blockObjs;
    //���λ��
    private static Vector2Int playerPos;
    //���ӵı߳�
    private float gridlength = 1f;

    //��ʼ�б�
    private List<AStarNode> openList=new List<AStarNode>();
    //�ر��б�
    private List<AStarNode> closeList=new List<AStarNode>();



    #region �����Ϊ�������ɵ�ͼ
    /// <summary>
    /// �����Ϊ�������ɵ�ͼ
    /// </summary>
    /// <param name="x">��������Ӹ���</param>
    /// <param name="y">��������Ӹ���</param>
    //[MenuItem("GameTool/GenerateCenterMap")]
    public void GenerateCenterMap()
    {
        //��������ʱ��ԭ���ĵ�ͼ���
        aStarNodes.Clear();


        AStarNode tmp_Node;
        Vector2Int tmp_Pos;
        //���
        Vector2 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
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
                aStarNodes.Add(tmp_Pos, tmp_Node);
            }
        }

        //�����赲
        blockObjs = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blockObjs.Length; i++)
        {
            tmp_Pos = new Vector2Int(Convert.ToInt32(blockObjs[i].transform.position.x),
              Convert.ToInt32(blockObjs[i].transform.position.y));
            aStarNodes[tmp_Pos].nodeType = NodeType.Stop;
            Debug.Log("�赲λ��" + tmp_Pos.ToString());
            //ShowColliderText(tmp_Pos,true);
        }

    }
    #endregion

    private Vector2Int tmp_Pos;

    /// <summary>
    /// �ҵ���ȷ��·��
    /// </summary>
    /// <param name="startPos">��ʼ������</param>
    /// <param name="endPos">����������</param>
    /// <returns>·��</returns>
    public List<AStarNode> FindWay(Vector2 startPos, Vector2 endPos)
    {
        Debug.Log(aStarNodes.Count);

        #region ����ϵ�е�λ��ת��Ϊ��������
        //endPos.x = aStarNodes.GetLength(0) / 2 + (endPos.x - startPos.x) / gridlength;
        //endPos.y = aStarNodes.GetLength(1) / 2 - (endPos.y - startPos.y) / gridlength;
        //startPos.x = 0;
        //startPos.y = 0;

        //startPos.x = aStarNodes.GetLength(0) / 2 + startPos.x / gridlength;
        //startPos.y = aStarNodes.GetLength(1) / 2 - startPos.y / gridlength;
        //endPos.x = aStarNodes.GetLength(0) / 2 + endPos.x / gridlength;
        //endPos.y = aStarNodes.GetLength(1) / 2 - endPos.y / gridlength;

        ////����������Ƿ���Ѱ·��ͼ�ķ�Χ֮��
        //if (startPos.x < 0 || startPos.y < 0 || startPos.x >= mapW || startPos.y >= mapH ||
        //    endPos.x < 0 || endPos.y < 0 || endPos.x >= mapW || endPos.y >= mapH)
        //{
        //    Debug.Log("��ʼ���������Ѱ·��ͼ�ķ�Χ֮��");
        //    return Vector3.zero;
        //}
        #endregion

        //�õ�����յ��Ӧ�ĸ���
        tmp_Pos= new Vector2Int(Convert.ToInt32(startPos.x),
            Convert.ToInt32(startPos.y));
        if (!aStarNodes.ContainsKey(tmp_Pos))
        {
            Debug.Log("A��Ѱ·��ʼ������ͼ��Χ");
            return null;
        }
        AStarNode startNode = aStarNodes[tmp_Pos];

        tmp_Pos = new Vector2Int(Convert.ToInt32(endPos.x),
            Convert.ToInt32(endPos.y));
        if (!aStarNodes.ContainsKey(tmp_Pos))
        {
            Debug.Log("A��Ѱ·�յ㳬����ͼ��Χ");
            return null;
        }
        AStarNode endNode = aStarNodes[tmp_Pos];

        //�Ƿ����ϰ���
        if (startNode.nodeType == NodeType.Stop || endNode.nodeType == NodeType.Stop)
        {
            Debug.Log("��ʼ����������赲");
            return null;
        }



        //���start���϶���Ĳ���
        startNode.fatherNode = null;
        startNode.comsume = 0;
        startNode.startDis = 0;
        startNode.endDis = 0;

        //��տ����б�͹ر��б�
        openList.Clear();
        closeList.Clear();

        //����ʼ�����뿪���б�
        closeList.Add(startNode);

        #region д���ϵĴ����᲻��ɾ

        //    //�жϿ�ʼ�������Ƿ�Ϸ�
        //    //���赲���Ϸ�
        //    //�Ǳ߽�㲻�Ϸ�
        //    startX = Mathf.RoundToInt(startPos.x/gridlength);
        //    if (startX > 0)
        //        startX += aStarNodes.GetLength(1);
        //    else
        //    {
        //        startX = -startX + aStarNodes.GetLength(1);
        //    }
        //    startY = Mathf.RoundToInt(startPos.y/gridlength);
        //    if (startY > 0)
        //        startY += aStarNodes.GetLength(0);
        //    else
        //    {
        //        startY = -startY + aStarNodes.GetLength(0);
        //    }

        //    if (startPos.x < -mapW / 2||startPos.x>mapW/2||startPos.y<-mapH/2||startPos.y>mapH/2||(startX == 0&& startY == 4)||
        //        (startX == 4&& startY == 0)||(startX == -4&& startY == -4))
        //    {
        //        return null;
        //    }
        //    endX = Mathf.RoundToInt(endPos.x / gridlength);
        //    if (endX > 0)
        //        endX += aStarNodes.GetLength(1);
        //    else
        //    {
        //        endX = -endX + aStarNodes.GetLength(1);
        //    }
        //    endY = Mathf.RoundToInt(endPos.y / gridlength);
        //    if (endY > 0)
        //        endY += aStarNodes.GetLength(0);
        //    else
        //    {
        //        endY = -endY + aStarNodes.GetLength(0);
        //    }
        //    if (endPos.x < -mapW / 2 || endPos.x > mapW / 2 || endPos.y < -mapH / 2 || endPos.y > mapH / 2 || 
        //        (endX == 0 && endY == 4) ||
        //        (endX == 4 && endY == 0) || (endX == -4 && endY == -4))
        //    {
        //        return null;
        //    }

        //    //��¼��ʼ������ĸ��ӣ����ҳ�ʼ����������б�
        //    startNode = aStarNodes[startX, startY];
        //    endNode = aStarNodes[endX, endY];

        //    endList.Add(aStarNodes[startX,startY]);
        //    endList.Add(aStarNodes[endX, endY]);

        //    Caculate();

        //        return null;
        //}
        //private void Caculate()
        //{
        //    //Ѱ����һ��λ�õĸ���
        //    //�˸���λ
        //    //��
        //    endX = startX + 1;
        //    endY = startY;
        //    //�Ƿ����赲�㣬�ǲ��Ǳ߽��
        //    //�Ƿ��ڿ�ʼ�б�ͽ����б���
        //    if (endX < aStarNodes.GetLength(1) &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //���뿪ʼ�б�
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //����
        //    endX = startX + 1;
        //    endY = startY+1;
        //    //�Ƿ����赲�㣬�ǲ��Ǳ߽��
        //    //�Ƿ��ڿ�ʼ�б�ͽ����б���
        //    if (endX < aStarNodes.GetLength(1)&&endY<aStarNodes.GetLength(0)&&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //���뿪ʼ�б�
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //��
        //    endX = startX;
        //    endY = startY + 1;
        //    //�Ƿ����赲�㣬�ǲ��Ǳ߽��
        //    //�Ƿ��ڿ�ʼ�б�ͽ����б���
        //    if ( endY < aStarNodes.GetLength(0) &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //���뿪ʼ�б�
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //����
        //    endX = startX-1;
        //    endY = startY + 1;
        //    //�Ƿ����赲�㣬�ǲ��Ǳ߽��
        //    //�Ƿ��ڿ�ʼ�б�ͽ����б���
        //    if (endY < aStarNodes.GetLength(0) &&endX>0&&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //���뿪ʼ�б�
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //��
        //    endX = startX - 1;
        //    endY = startY;
        //    //�Ƿ����赲�㣬�ǲ��Ǳ߽��
        //    //�Ƿ��ڿ�ʼ�б�ͽ����б���
        //    if ( endX > 0 &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //���뿪ʼ�б�
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //����
        //    endX = startX - 1;
        //    endY = startY - 1;
        //    //�Ƿ����赲�㣬�ǲ��Ǳ߽��
        //    //�Ƿ��ڿ�ʼ�б�ͽ����б���
        //    if (endY >0 && endX > 0 &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //���뿪ʼ�б�
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //��
        //    endX = startX ;
        //    endY = startY - 1;
        //    //�Ƿ����赲�㣬�ǲ��Ǳ߽��
        //    //�Ƿ��ڿ�ʼ�б�ͽ����б���
        //    if ( endY > 0 &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //���뿪ʼ�б�
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //����
        //    endX = startX + 1;
        //    endY = startY - 1;
        //    //�Ƿ����赲�㣬�ǲ��Ǳ߽��
        //    //�Ƿ��ڿ�ʼ�б�ͽ����б���
        //    if (endY>0 && endX <aStarNodes.GetLength(0) &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //���뿪ʼ�б�
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }

        //    nowleastNode = startList[0];
        //    //�ж��ǲ���·��������С�ĵ�
        //    for (int i=1;i<startList.Count;i++)
        //    {

        //    }
        //    //�ҵ��󣬼�������б��ӿ�ʼ�б���ɾ��
        #endregion


        while(true)
        {
            //����
            FindNearlyNodeToOpenList(startNode.x - 1, endNode.y - 1, 1.4f, startNode, endNode);
            //��
            FindNearlyNodeToOpenList(startNode.x - 1, endNode.y, 1, startNode, endNode);
            //����
            FindNearlyNodeToOpenList(startNode.x - 1, endNode.y + 1, 1.4f, startNode, endNode);
            //��
            FindNearlyNodeToOpenList(startNode.x, endNode.y + 1, 1, startNode, endNode);
            //����
            FindNearlyNodeToOpenList(startNode.x + 1, endNode.y + 1, 1.4f, startNode, endNode);
            //��
            FindNearlyNodeToOpenList(startNode.x + 1, endNode.y, 1, startNode, endNode);
            //����
            FindNearlyNodeToOpenList(startNode.x + 1, endNode.y - 1, 1.4f, startNode, endNode);
            //��
            FindNearlyNodeToOpenList(startNode.x, endNode.y - 1, 1, startNode, endNode);


            if(openList.Count==0)
            {
                Debug.Log("��·");
                return null;
            }

            //����·�����ģ��������б�����
            openList.Sort(SortOpenList);

            //�������б���·��������С�ĸ��ӷ���ر��б�
            closeList.Add(openList[0]);
            //�´�ѭ��Ѱ·�����
            startNode = openList[0];
            //�ڿ����б���ɾ���ҵ��ĸ���
            openList.RemoveAt(0);

            //��������б���·��������С�ĸ�����Ѱ·���յ��ֹͣѭ��
            if(startNode==endNode)
            {
                List<AStarNode> retList = new List<AStarNode>();
                retList.Add(endNode);
                //�õ�·��
                while(endNode.fatherNode!=null)
                {
                    retList.Add(endNode.fatherNode);
                    endNode = endNode.fatherNode;
                }
                retList.Reverse();
                return retList;
            }

        }
        
    }

    /// <summary>
    /// �����б�����
    /// </summary>
    /// <param name="front"></param>
    /// <param name="back"></param>
    /// <returns></returns>
    private int SortOpenList(AStarNode front,AStarNode back)
    {

        if (front.comsume >= back.comsume)
            return 1;
        else
            return -1;
    }

    //��ʱ����
    AStarNode node;

    /// <summary>
    /// ���ٽ��ĵ���뿪���б�����������·������
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void FindNearlyNodeToOpenList(int x, int y,float startDis,AStarNode fatherNode,AStarNode endNode)
    {
        //�ж��ǹ��ڵ�ͼ��Χ��������ڵ�ͼ��Χ�ھ�ֱ�ӷ���
        if(!aStarNodes.ContainsKey(new Vector2Int(x,y)))
        return;
        //if (x < playerPos.x-mapW/2 || x > playerPos.x + mapW / 2 ||
        //    y < playerPos.y - mapH / 2 || y > playerPos.y + mapH / 2)



        //�õ�����
        node = aStarNodes[new Vector2Int(x,y)];

        //�жϸ����ǹ��ڿ�����ر��б���
        //�жϸ����Ƿ��ǿ�ͨ���ĸ���
        if (node == null || openList.Contains(node) || closeList.Contains(node) || 
            node.nodeType == NodeType.Stop)
            return;

        //������ӵ�Ѱ·����
        node.fatherNode = fatherNode;
        node.startDis = startDis + fatherNode.startDis;
        node.endDis =Mathf.Abs(endNode.x - node.x)+Mathf.Abs(endNode.y-node.y);
        node.comsume = node.startDis + node.endDis;

        //���ӺϷ������뿪���б�
        openList.Add(node);
    }

    #region �ڳ�����ʵ�����ı�����ķ�ʽչʾ����λ���Լ�����ײ����Ϣ
    //public static GameObject prefabText;
    //private static Transform prefabParent;
    //private static void ShowColliderText(Vector2Int curPos, bool isCollider=false)
    //{
    //    if (prefabText == null)
    //    {
    //        Debug.LogWarning("������û��ѡ���ı�Ԥ����");
    //        return;
    //    }
    //    prefabParent = GameObject.Find("Canvas").transform; //ʵ����չʾ��ײ��Ϣ���ı�����ʱ���õ��ĸ�����
    //    prefabText.name = curPos.x + "," + curPos.y;
    //    prefabText.GetComponent<Text>().text = curPos.ToString();
    //    GameObject.Instantiate(prefabText, curPos + new Vector2(0.5f, 0.5f), Quaternion.identity, prefabParent);
    //}
    #endregion
}
