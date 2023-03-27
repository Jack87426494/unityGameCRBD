using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerPoint : BaseMonsterPoint
{
    protected override void CreateWave()
    {
        base.CreateWave();
        enemyNum = (enemyNum*1.06f +0.5f) > enemyWaveMax ? enemyWaveMax : enemyNum * 1.06f + 0.5f;
    }
}
