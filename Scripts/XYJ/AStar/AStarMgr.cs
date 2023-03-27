using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Convert = System.Convert;

/// <summary>
/// a星寻路管理器
/// </summary>
public class AStarMgr : BaseMgr<AStarMgr>
{
   
    //全地图的格子信息
    //private static AStarNode[,] aStarNodes;
    public Dictionary<Vector2Int, AStarNode> aStarNodes=
        new Dictionary<Vector2Int, AStarNode>();

    //地图的宽
    private static int mapW = 1000;
    //地图的高
    private static int mapH = 1000;
    //阻挡物体
    private static GameObject[] blockObjs;
    //玩家位置
    private static Vector2Int playerPos;
    //格子的边长
    private float gridlength = 1f;

    //开始列表
    private List<AStarNode> openList=new List<AStarNode>();
    //关闭列表
    private List<AStarNode> closeList=new List<AStarNode>();



    #region 以玩家为中心生成地图
    /// <summary>
    /// 以玩家为中心生成地图
    /// </summary>
    /// <param name="x">横坐标格子个数</param>
    /// <param name="y">纵坐标格子个数</param>
    //[MenuItem("GameTool/GenerateCenterMap")]
    public void GenerateCenterMap()
    {
        //重新生成时把原来的地图清空
        aStarNodes.Clear();


        AStarNode tmp_Node;
        Vector2Int tmp_Pos;
        //玩家
        Vector2 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
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
                aStarNodes.Add(tmp_Pos, tmp_Node);
            }
        }

        //设置阻挡
        blockObjs = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blockObjs.Length; i++)
        {
            tmp_Pos = new Vector2Int(Convert.ToInt32(blockObjs[i].transform.position.x),
              Convert.ToInt32(blockObjs[i].transform.position.y));
            aStarNodes[tmp_Pos].nodeType = NodeType.Stop;
            Debug.Log("阻挡位置" + tmp_Pos.ToString());
            //ShowColliderText(tmp_Pos,true);
        }

    }
    #endregion

    private Vector2Int tmp_Pos;

    /// <summary>
    /// 找到正确的路径
    /// </summary>
    /// <param name="startPos">开始的坐标</param>
    /// <param name="endPos">结束的坐标</param>
    /// <returns>路径</returns>
    public List<AStarNode> FindWay(Vector2 startPos, Vector2 endPos)
    {
        Debug.Log(aStarNodes.Count);

        #region 坐标系中的位置转化为格子坐标
        //endPos.x = aStarNodes.GetLength(0) / 2 + (endPos.x - startPos.x) / gridlength;
        //endPos.y = aStarNodes.GetLength(1) / 2 - (endPos.y - startPos.y) / gridlength;
        //startPos.x = 0;
        //startPos.y = 0;

        //startPos.x = aStarNodes.GetLength(0) / 2 + startPos.x / gridlength;
        //startPos.y = aStarNodes.GetLength(1) / 2 - startPos.y / gridlength;
        //endPos.x = aStarNodes.GetLength(0) / 2 + endPos.x / gridlength;
        //endPos.y = aStarNodes.GetLength(1) / 2 - endPos.y / gridlength;

        ////传入的坐标是否在寻路地图的范围之内
        //if (startPos.x < 0 || startPos.y < 0 || startPos.x >= mapW || startPos.y >= mapH ||
        //    endPos.x < 0 || endPos.y < 0 || endPos.x >= mapW || endPos.y >= mapH)
        //{
        //    Debug.Log("开始或结束点再寻路地图的范围之外");
        //    return Vector3.zero;
        //}
        #endregion

        //得到起点终点对应的格子
        tmp_Pos= new Vector2Int(Convert.ToInt32(startPos.x),
            Convert.ToInt32(startPos.y));
        if (!aStarNodes.ContainsKey(tmp_Pos))
        {
            Debug.Log("A星寻路起始超出地图范围");
            return null;
        }
        AStarNode startNode = aStarNodes[tmp_Pos];

        tmp_Pos = new Vector2Int(Convert.ToInt32(endPos.x),
            Convert.ToInt32(endPos.y));
        if (!aStarNodes.ContainsKey(tmp_Pos))
        {
            Debug.Log("A星寻路终点超出地图范围");
            return null;
        }
        AStarNode endNode = aStarNodes[tmp_Pos];

        //是否是障碍物
        if (startNode.nodeType == NodeType.Stop || endNode.nodeType == NodeType.Stop)
        {
            Debug.Log("开始或结束点是阻挡");
            return null;
        }



        //清空start身上多余的参数
        startNode.fatherNode = null;
        startNode.comsume = 0;
        startNode.startDis = 0;
        startNode.endDis = 0;

        //清空开启列表和关闭列表
        openList.Clear();
        closeList.Clear();

        //将开始结点加入开启列表
        closeList.Add(startNode);

        #region 写作废的代码舍不得删

        //    //判断开始结束点是否合法
        //    //是阻挡不合法
        //    //是边界点不合法
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

        //    //记录开始结束点的格子，并且初始化加入结束列表
        //    startNode = aStarNodes[startX, startY];
        //    endNode = aStarNodes[endX, endY];

        //    endList.Add(aStarNodes[startX,startY]);
        //    endList.Add(aStarNodes[endX, endY]);

        //    Caculate();

        //        return null;
        //}
        //private void Caculate()
        //{
        //    //寻找下一个位置的格子
        //    //八个方位
        //    //右
        //    endX = startX + 1;
        //    endY = startY;
        //    //是否是阻挡点，是不是边界点
        //    //是否在开始列表和结束列表中
        //    if (endX < aStarNodes.GetLength(1) &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //加入开始列表
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //右上
        //    endX = startX + 1;
        //    endY = startY+1;
        //    //是否是阻挡点，是不是边界点
        //    //是否在开始列表和结束列表中
        //    if (endX < aStarNodes.GetLength(1)&&endY<aStarNodes.GetLength(0)&&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //加入开始列表
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //上
        //    endX = startX;
        //    endY = startY + 1;
        //    //是否是阻挡点，是不是边界点
        //    //是否在开始列表和结束列表中
        //    if ( endY < aStarNodes.GetLength(0) &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //加入开始列表
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //左上
        //    endX = startX-1;
        //    endY = startY + 1;
        //    //是否是阻挡点，是不是边界点
        //    //是否在开始列表和结束列表中
        //    if (endY < aStarNodes.GetLength(0) &&endX>0&&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //加入开始列表
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //左
        //    endX = startX - 1;
        //    endY = startY;
        //    //是否是阻挡点，是不是边界点
        //    //是否在开始列表和结束列表中
        //    if ( endX > 0 &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //加入开始列表
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //左下
        //    endX = startX - 1;
        //    endY = startY - 1;
        //    //是否是阻挡点，是不是边界点
        //    //是否在开始列表和结束列表中
        //    if (endY >0 && endX > 0 &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //加入开始列表
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //下
        //    endX = startX ;
        //    endY = startY - 1;
        //    //是否是阻挡点，是不是边界点
        //    //是否在开始列表和结束列表中
        //    if ( endY > 0 &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //加入开始列表
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }
        //    //右下
        //    endX = startX + 1;
        //    endY = startY - 1;
        //    //是否是阻挡点，是不是边界点
        //    //是否在开始列表和结束列表中
        //    if (endY>0 && endX <aStarNodes.GetLength(0) &&
        //        !startList.Contains(aStarNodes[endX, endY]) && !endList.Contains(aStarNodes[endX, endY]) &&
        //        !(endX == 0 && endY == 4) &&
        //        !(endX == 4 && endY == 0) && !(endX == -4 && endY == -4))
        //    {
        //        //加入开始列表
        //        startList.Add(aStarNodes[endX, endY]);
        //        aStarNodes[endX, endY].fatherNode = aStarNodes[startX, startY];
        //    }

        //    nowleastNode = startList[0];
        //    //判断是不是路径消耗最小的点
        //    for (int i=1;i<startList.Count;i++)
        //    {

        //    }
        //    //找到后，加入结束列表，从开始列表中删除
        #endregion


        while(true)
        {
            //左上
            FindNearlyNodeToOpenList(startNode.x - 1, endNode.y - 1, 1.4f, startNode, endNode);
            //左
            FindNearlyNodeToOpenList(startNode.x - 1, endNode.y, 1, startNode, endNode);
            //左下
            FindNearlyNodeToOpenList(startNode.x - 1, endNode.y + 1, 1.4f, startNode, endNode);
            //下
            FindNearlyNodeToOpenList(startNode.x, endNode.y + 1, 1, startNode, endNode);
            //右下
            FindNearlyNodeToOpenList(startNode.x + 1, endNode.y + 1, 1.4f, startNode, endNode);
            //右
            FindNearlyNodeToOpenList(startNode.x + 1, endNode.y, 1, startNode, endNode);
            //右上
            FindNearlyNodeToOpenList(startNode.x + 1, endNode.y - 1, 1.4f, startNode, endNode);
            //上
            FindNearlyNodeToOpenList(startNode.x, endNode.y - 1, 1, startNode, endNode);


            if(openList.Count==0)
            {
                Debug.Log("死路");
                return null;
            }

            //按照路径消耗，排序开启列表排序
            openList.Sort(SortOpenList);

            //将开启列表中路径消耗最小的格子放入关闭列表
            closeList.Add(openList[0]);
            //下次循环寻路的起点
            startNode = openList[0];
            //在开启列表中删除找到的格子
            openList.RemoveAt(0);

            //如果开启列表中路径消耗最小的格子是寻路的终点就停止循环
            if(startNode==endNode)
            {
                List<AStarNode> retList = new List<AStarNode>();
                retList.Add(endNode);
                //得到路径
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
    /// 开启列表排序
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

    //临时格子
    AStarNode node;

    /// <summary>
    /// 将临近的点放入开启列表，并计算它的路径消耗
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void FindNearlyNodeToOpenList(int x, int y,float startDis,AStarNode fatherNode,AStarNode endNode)
    {
        //判断是够在地图范围内如果不在地图范围内就直接返回
        if(!aStarNodes.ContainsKey(new Vector2Int(x,y)))
        return;
        //if (x < playerPos.x-mapW/2 || x > playerPos.x + mapW / 2 ||
        //    y < playerPos.y - mapH / 2 || y > playerPos.y + mapH / 2)



        //得到格子
        node = aStarNodes[new Vector2Int(x,y)];

        //判断格子是够在开启或关闭列表中
        //判断格子是否是可通过的格子
        if (node == null || openList.Contains(node) || closeList.Contains(node) || 
            node.nodeType == NodeType.Stop)
            return;

        //计算格子的寻路消耗
        node.fatherNode = fatherNode;
        node.startDis = startDis + fatherNode.startDis;
        node.endDis =Mathf.Abs(endNode.x - node.x)+Mathf.Abs(endNode.y-node.y);
        node.comsume = node.startDis + node.endDis;

        //格子合法将加入开启列表
        openList.Add(node);
    }

    #region 在场景中实例化文本对象的方式展示方块位置以及、碰撞的信息
    //public static GameObject prefabText;
    //private static Transform prefabParent;
    //private static void ShowColliderText(Vector2Int curPos, bool isCollider=false)
    //{
    //    if (prefabText == null)
    //    {
    //        Debug.LogWarning("老铁你没有选择文本预制体");
    //        return;
    //    }
    //    prefabParent = GameObject.Find("Canvas").transform; //实例化展示碰撞信息的文本对象时所用到的父物体
    //    prefabText.name = curPos.x + "," + curPos.y;
    //    prefabText.GetComponent<Text>().text = curPos.ToString();
    //    GameObject.Instantiate(prefabText, curPos + new Vector2(0.5f, 0.5f), Quaternion.identity, prefabParent);
    //}
    #endregion
}
