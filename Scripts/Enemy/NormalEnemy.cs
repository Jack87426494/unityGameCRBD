using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class NormalEnemy : BaseEnemy
{
    //��һ֡��xֵ
    private float originalX;
    //Ŀǰ��һ֡��xֵ
    private float nowX;

    ////���ٲ�ֵ����
    //private float nowtime;
    //private float start;
    //private float end;

    protected override void Start()
    {
        base.Start();
        originalX = transform.position.x;
    }

   
    private void FixedUpdate()
    {
        nowX = transform.position.x;
        if (nowX < originalX)
            sr.flipX = true;
        if (nowX > originalX)
            sr.flipX = false;

        //time += Time.deltaTime*10;
        //if(end!=transform.position.x)
        //{
        //    start = originalX;
        //    end = transform.position.x;
        //}
        

        
        //if (Mathf.Abs(transform.position.x - originalX) > 0.8f)
            
    }

    protected override void Update()
    {
        base.Update();
        //originalX = nowX;

        //��������
        originalX = Mathf.Lerp(originalX, transform.position.x, Time.deltaTime*3.5f);
    }
    protected override void FollowPlayer()
    {
        base.FollowPlayer();
    }
}
