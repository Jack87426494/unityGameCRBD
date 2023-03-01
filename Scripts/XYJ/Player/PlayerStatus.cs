using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("玩家最大血量")]
    public float maxHealth;
    [Header("玩家当前血量")]
    public float currentHealth;
    [Header("玩家子弹伤害")]
    public float bulletDamage;
    [Header("玩家子弹最大数量")]
    public float bulletMaxNum;
    [Header("玩家换弹cd")]
    public float updateBulletNumTime;
    [Header("玩家开火间隔")]
    public float fireCD;

}
