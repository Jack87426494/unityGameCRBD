using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelText : TheBasePanel
{
    protected override void OnClick(string name)
    {
        switch (name)
        {
            case "beginBtn":
                print(name);
                break;
        }
    }

    
}