using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHit
{
    /// <summary>
    /// �յ��˺�
    /// </summary>
    /// <param name="enemyData"></param>
    public void GetHit(float damage);
}
