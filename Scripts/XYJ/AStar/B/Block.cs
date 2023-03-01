using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public readonly Vector2Int position;
    public readonly bool isCollider;
    public readonly int pre_cost; //预测成本，在初始化时更新

    public Block pre_block;
    private int cur_cost; //当前成本，在获知上一方块位置之后更新
    private int all_cost; //总成本，在获知上一方块位置之后更新

    public bool isChecked = false; //标记当前方块是否被检测过

    public Block(Vector2Int position, Vector2Int endPos, bool isCollider)
    {
        this.position = position;
        cur_cost = 1000000; //将当前成本设为尽可能大
        pre_cost = Math.Abs(position.x - endPos.x) + Math.Abs(position.y - endPos.y); //曼哈顿距离
        this.isCollider = isCollider;
    }

    public void UpdateCurCost()
    {
        if (pre_block != null)
        {
            cur_cost = pre_block.cur_cost + 1; //根据上一方块的“当前成本”加一得到此方块的当前成本
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
