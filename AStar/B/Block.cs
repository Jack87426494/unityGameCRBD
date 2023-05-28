using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public readonly Vector2Int position;
    public readonly bool isCollider;
    public readonly int pre_cost; //Ԥ��ɱ����ڳ�ʼ��ʱ����

    public Block pre_block;
    private int cur_cost; //��ǰ�ɱ����ڻ�֪��һ����λ��֮�����
    private int all_cost; //�ܳɱ����ڻ�֪��һ����λ��֮�����

    public bool isChecked = false; //��ǵ�ǰ�����Ƿ񱻼���

    public Block(Vector2Int position, Vector2Int endPos, bool isCollider)
    {
        this.position = position;
        cur_cost = 1000000; //����ǰ�ɱ���Ϊ�����ܴ�
        pre_cost = Math.Abs(position.x - endPos.x) + Math.Abs(position.y - endPos.y); //�����پ���
        this.isCollider = isCollider;
    }

    public void UpdateCurCost()
    {
        if (pre_block != null)
        {
            cur_cost = pre_block.cur_cost + 1; //������һ����ġ���ǰ�ɱ�����һ�õ��˷���ĵ�ǰ�ɱ�
        }
        else
        {
            cur_cost = 0;   
        }
    }

    public int GetAllCost()
    {
        UpdateCurCost();
        all_cost = cur_cost + pre_cost;
        return all_cost;
    }

    public string CostString()
    {
        return cur_cost + "/" + pre_cost + "/" + all_cost;
    }

}
