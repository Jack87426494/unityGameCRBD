using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormPoint : BaseMonsterPoint
{
    protected override void CreateWave()
    {
        base.CreateWave();
        enemyNum = (enemyNum * 1.06f + 0.3f) > enemyWaveMax ? enemyWaveMax : enemyNum * 1.06f + 0.3f;
    }
}
