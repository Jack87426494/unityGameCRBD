using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleMap : MonoBehaviour
{
    [SerializeField] private Transform currentLeft;
    [SerializeField] private Transform currentLeft_Up;
    [SerializeField] private Transform currentLeft_Down;

    [SerializeField] private Transform currentMiddle;
    [SerializeField] private Transform currentMiddle_Up;
    [SerializeField] private Transform currentMiddle_Down;

    [SerializeField] private Transform currentRight;
    [SerializeField] private Transform currentRight_Up;
    [SerializeField] private Transform currentRight_Down;

    //private Transform[] leftTrans = new Transform[3];
    //private Transform[] horizontalMiddleTrans = new Transform[3];
    //private Transform[] rightTrans = new Transform[3];
    //private Transform[] upTrans = new Transform[3];
    //private Transform[] verticalMiddleTrans = new Transform[3];
    //private Transform[] downTrans = new Transform[3];

    private GameObject player;
    //private Transform originalMiddleMap;

    private void Awake()
    {
        HXW_InputMgr.GetInstance();

        player = GameObject.FindGameObjectWithTag("Player");
        if (Check() == false)
            return;
        
        //horizontalTransforms[0] = currentLeft;
        //horizontalTransforms[1] = currentMiddle;
        //horizontalTransforms[2] = currentRight;

        //verticalTransforms[0] = currentDown;
        //verticalTransforms[1] = currentMiddle;
        //verticalTransforms[2] = currentUp;

        //originalMiddleMap = currentMiddle;
    }

    private bool Check()
    {
        if (player == null)
            return false;
        if (currentLeft_Down == null || currentLeft == null || currentLeft_Up == null)
            return false;
        if (currentMiddle_Down == null || currentMiddle == null || currentMiddle_Up == null)
            return false;
        if (currentRight_Down == null || currentRight == null || currentRight_Up == null)
            return false;
        return true;
    }

    private void CheckRefreshMap()
    {
        if (Check() == false)
            return;
        Vector3 playerPos = player.transform.position;
        if(playerPos.x >= currentRight.position.x)
        {
            currentLeft.position = currentRight.position + new Vector3(240, 0, 0);
            Swap(ref currentRight, ref currentMiddle, ref currentLeft);
            currentLeft_Up.position = currentRight_Up.position + new Vector3(240, 0, 0);
            Swap(ref currentRight_Up, ref currentMiddle_Up, ref currentLeft_Up);
            currentLeft_Down.position = currentRight_Down.position + new Vector3(240, 0, 0);
            Swap(ref currentRight_Down, ref currentMiddle_Down, ref currentLeft_Down);
        }
        else if(playerPos.x <= currentLeft.position.x)
        {
            currentRight.position = currentLeft.position + new Vector3(-240, 0, 0);
            Swap(ref currentLeft, ref currentMiddle, ref currentRight);
            currentRight_Up.position = currentLeft_Up.position + new Vector3(-240, 0, 0);
            Swap(ref currentLeft_Up, ref currentMiddle_Up, ref currentRight_Up);
            currentRight_Down.position = currentLeft_Down.position + new Vector3(-240, 0, 0);
            Swap(ref currentLeft_Down, ref currentMiddle_Down, ref currentRight_Down);
        }
        
        if(playerPos.y >= currentMiddle_Up.position.y)
        {
            currentLeft_Down.position = currentLeft_Up.position + new Vector3(0, 180, 0);
            Swap(ref currentLeft_Up, ref currentLeft, ref currentLeft_Down);
            currentMiddle_Down.position = currentMiddle_Up.position + new Vector3(0, 180, 0);
            Swap(ref currentMiddle_Up, ref currentMiddle, ref currentMiddle_Down);
            currentRight_Down.position = currentRight_Up.position + new Vector3(0, 180, 0);
            Swap(ref currentRight_Up, ref currentRight, ref currentRight_Down);
        }
        else if(playerPos.y <= currentMiddle_Down.position.y)
        {
            currentLeft_Up.position = currentLeft_Down.position + new Vector3(0, -180, 0);
            Swap(ref currentLeft_Down, ref currentLeft, ref currentLeft_Up);

            currentMiddle_Up.position = currentMiddle_Down.position + new Vector3(0, -180, 0);
            Swap(ref currentMiddle_Down, ref currentMiddle, ref currentMiddle_Up);

            currentRight_Up.position = currentRight_Down.position + new Vector3(0, -180, 0);
            Swap(ref currentRight_Down, ref currentRight, ref currentRight_Up);
        }
    }

    private void Swap(ref Transform trans_1, ref Transform trans_Middle, ref Transform trans_2)
    {
        Transform temp = trans_2;
        trans_2 = trans_Middle;
        trans_Middle = trans_1;
        trans_1 = temp;
    }

    private void Update()
    {
        CheckRefreshMap();
    }
}
