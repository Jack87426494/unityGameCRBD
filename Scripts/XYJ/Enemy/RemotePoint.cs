using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePoint : BaseMonsterPoint
{
    protected override void CreateWave()
    {
        base.CreateWave();
        enemyNum = enemyNum*1.06f > enemyWaveMax ? enemyWaveMax : enemyNum * 1.06f;
    }
}
