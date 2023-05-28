using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HXW_Tools
{
    public static IEnumerator DelayAction(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        if (action != null)
            action.Invoke();
    }

}
