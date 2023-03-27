using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; set; }

    private static readonly Dictionary<Vector2Int, Block> map = new Dictionary<Vector2Int, Block>();

    private void Awake()
    {
        //单例模式
        if(Instance!=null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// 根据起始点和终点更新地图上的碰撞信息
    /// </summary>
    /// <param name="startPos">起始点</param>
    /// <param name="endPos">终点</param>
    public void ReadMap(Vector2Int startPos, Vector2Int endPos)
    {
        int detectRange = 20;
        while (endPos.x < startPos.x - detectRange + 1 || endPos.x > startPos.x + detectRange - 1
            || endPos.y < startPos.y - detectRange + 1 || endPos.y > startPos.y + detectRange - 1)
        {
            //保证目标点在被检测的地图范围内
            detectRange *= 2;
        }
        ReadMap(startPos, endPos, detectRange);
    }

    /// <summary>
    /// 根据中心点和检测范围更新地图上的碰撞信息
    /// </summary>
    /// <param name="startPos">以该点为中心对周围的区块进行检测</param>
    /// <param name="detectRange">检测范围</param>
    private void ReadMap(Vector2Int startPos, Vector2Int endPos, int detectRange)
    {
        int x = startPos.x;
        int y = startPos.y;
        for (int i = x - detectRange; i<x+detectRange; i++)
        {
            for (int j = y - detectRange; j < y + detectRange; j++)
            {
                Vector2Int curPos = new Vector2Int(i, j);
                bool isCollider = DetectCollider(curPos);

                Block block = new Block(curPos, endPos, isCollider);
                map.Add(curPos, block);

                ShowColliderText(curPos, isCollider);
            }
        }
    }

    /// <summary>
    /// 检测单点是否有碰撞体
    /// </summary>
    /// <param name="orignal">待检测的位置</param>
    /// <returns>有碰撞体返回true，否则返回false</returns>
    private bool DetectCollider(Vector2 orignal)
    {
        orignal = orignal + new Vector2(0.5f, 0.5f); //位置往右上偏移0.5个单位
        Collider2D collider = Physics2D.OverlapPoint(orignal); 
        if (collider != null) return true;
        return false;
    }

    /// <summary>
    /// 获取地图方块信息
    /// </summary>
    /// <returns></returns>
    public Dictionary<Vector2Int, Block> GetMap()
    {
        return map;
    }

    /// <summary>
    /// 在场景中实例化文本对象的方式展示方块位置以及、碰撞的信息
    /// </summary>
    public GameObject prefabText;
    private Transform prefabParent;
    private void ShowColliderText(Vector2 curPos, bool isCollider)
    {
        if (prefabText == null)
        {
            Debug.LogWarning("老铁你没有选择文本预制体");
            return;
        }
        prefabParent = GameObject.Find("Canvas").transform; //实例化展示碰撞信息的文本对象时所用到的父物体
        prefabText.name = curPos.x + "," + curPos.y;
        prefabText.GetComponent<Text>().text = curPos.ToString();
        Instantiate(prefabText, curPos + new Vector2(0.5f, 0.5f), Quaternion.identity, prefabParent);
    }
}
