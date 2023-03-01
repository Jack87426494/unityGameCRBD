using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NodeType
{
    Walkable,
    Stop,
}

public class AStarNode
{
    //格子位次的坐标
    public int x;
    public int y;

    //父格子
    public AStarNode fatherNode;

    //格子的类型
    public NodeType nodeType;


    //寻路消耗
    public float comsume;
    //距离起点的距离
    public float startDis;
    //距离终点的距离
    public float endDis;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="x">格子位次横坐标</param>
    /// <param name="y">格子位次纵坐标</param>
    /// <param name="nodeType">格子的类型</param>
     public AStarNode(int x,int y,NodeType nodeType)
    {
        this.x = x;
        this.y = y;
        this.nodeType = nodeType;
    }
    ///// <summary>
    ///// 构造函数
    ///// </summary>
    ///// <param name="x">横坐标</param>s
    ///// <param name="y">纵坐标</param>
    ///// <param name="nodeType">格子的类型</param>
    ///// <param name="fatherNode">父格子</param>
    //public AStarNode(float x, float y, NodeType nodeType,AStarNode fatherNode)
    //{
    //    this.x = x;
    //    this.y = y;
    //    this.nodeType = nodeType;
    //    this.fatherNode = fatherNode;
    //}
}
