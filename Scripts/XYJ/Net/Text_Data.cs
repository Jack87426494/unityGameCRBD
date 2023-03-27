using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Data : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextInfo textInfo = new TextInfo();
        textInfo.atk = 10;
        textInfo.playerData = new Player_Data();

        byte[] bytes=textInfo.Writing();
        textInfo.atk = 99;
        textInfo.Reading(bytes);
        Debug.Log(textInfo.atk);
        print(textInfo.hp);
        print(textInfo.playerData.playerName);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
