using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHiter
{
    /// <summary>
    /// ����˺�
    /// </summary>
    /// <param name="collision"></param>
    public void TakeDamage(Collider2D collision);
}