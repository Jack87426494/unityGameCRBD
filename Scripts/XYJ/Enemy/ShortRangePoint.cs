using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangePoint : BaseMonsterPoint
{

    protected override void CreateWave()
    {
        base.CreateWave();
        //下一波怪的数量，由上一波怪计算得到
        enemyNum = enemyNum * 1.06f + 1.5f>enemyWaveMax?enemyWaveMax: enemyNum * 1.06f + 1.5f;//限制每波最多40个怪
        
    }

}
