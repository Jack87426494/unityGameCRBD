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
    //����λ�ε�����
    public int x;
    public int y;

    //������
    public AStarNode fatherNode;

    //���ӵ�����
    public NodeType nodeType;


    //Ѱ·����
    public float comsume;
    //�������ľ���
    public float startDis;
    //�����յ�ľ���
    public float endDis;

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="x">����λ�κ�����</param>
    /// <param name="y">����λ��������</param>
    /// <param name="nodeType">���ӵ�����</param>
     public AStarNode(int x,int y,NodeType nodeType)
    {
        this.x = x;
        this.y = y;
        this.nodeType = nodeType;
    }
    ///// <summary>
    ///// ���캯��
    ///// </summary>
    ///// <param name="x">������</param>s
    ///// <param name="y">������</param>
    ///// <param name="nodeType">���ӵ�����</param>
    ///// <param name="fatherNode">������</param>
    //public AStarNode(float x, float y, NodeType nodeType,AStarNode fatherNode)
    //{
    //    this.x = x;
    //    this.y = y;
    //    this.nodeType = nodeType;
    //    this.fatherNode = fatherNode;
    //}
}
