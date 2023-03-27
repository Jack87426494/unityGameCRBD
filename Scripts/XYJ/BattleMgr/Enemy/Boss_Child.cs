using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Child : ShortRangeEnemy
{
    public override int GetExcelIndex()
    {
        return 6;
    }

    public override bool IsOriginalRight()
    {
        return false;
    }

}
