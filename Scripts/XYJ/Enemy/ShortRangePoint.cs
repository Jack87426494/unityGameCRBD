using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangePoint : BaseMonsterPoint
{

    protected override void CreateWave()
    {
        base.CreateWave();
        //��һ���ֵ�����������һ���ּ���õ�
        enemyNum = enemyNum * 1.06f + 1.5f>enemyWaveMax?enemyWaveMax: enemyNum * 1.06f + 1.5f;//����ÿ�����40����
        
    }

}
