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
        //����ģʽ
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
    /// ������ʼ����յ���µ�ͼ�ϵ���ײ��Ϣ
    /// </summary>
    /// <param name="startPos">��ʼ��</param>
    /// <param name="endPos">�յ�</param>
    public void ReadMap(Vector2Int startPos, Vector2Int endPos)
    {
        int detectRange = 20;
        while (endPos.x < startPos.x - detectRange + 1 || endPos.x > startPos.x + detectRange - 1
            || endPos.y < startPos.y - detectRange + 1 || endPos.y > startPos.y + detectRange - 1)
        {
            //��֤Ŀ����ڱ����ĵ�ͼ��Χ��
            detectRange *= 2;
        }
        ReadMap(startPos, endPos, detectRange);
    }

    /// <summary>
    /// �������ĵ�ͼ�ⷶΧ���µ�ͼ�ϵ���ײ��Ϣ
    /// </summary>
    /// <param name="startPos">�Ըõ�Ϊ���Ķ���Χ��������м��</param>
    /// <param name="detectRange">��ⷶΧ</param>
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
    /// ��ⵥ���Ƿ�����ײ��
    /// </summary>
    /// <param name="orignal">������λ��</param>
    /// <returns>����ײ�巵��true�����򷵻�false</returns>
    private bool DetectCollider(Vector2 orignal)
    {
        orignal = orignal + new Vector2(0.5f, 0.5f); //λ��������ƫ��0.5����λ
        Collider2D collider = Physics2D.OverlapPoint(orignal); 
        if (collider != null) return true;
        return false;
    }

    /// <summary>
    /// ��ȡ��ͼ������Ϣ
    /// </summary>
    /// <returns></returns>
    public Dictionary<Vector2Int, Block> GetMap()
    {
        return map;
    }

    /// <summary>
    /// �ڳ�����ʵ�����ı�����ķ�ʽչʾ����λ���Լ�����ײ����Ϣ
    /// </summary>
    public GameObject prefabText;
    private Transform prefabParent;
    private void ShowColliderText(Vector2 curPos, bool isCollider)
    {
        if (prefabText == null)
        {
            Debug.LogWarning("������û��ѡ���ı�Ԥ����");
            return;
        }
        prefabParent = GameObject.Find("Canvas").transform; //ʵ����չʾ��ײ��Ϣ���ı�����ʱ���õ��ĸ�����
        prefabText.name = curPos.x + "," + curPos.y;
        prefabText.GetComponent<Text>().text = curPos.ToString();
        Instantiate(prefabText, curPos + new Vector2(0.5f, 0.5f), Quaternion.identity, prefabParent);
    }
}
