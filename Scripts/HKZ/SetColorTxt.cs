using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SetColorTxt : MonoBehaviour, IPointerEnterHandler  
{


    public void OnPointerEnter(PointerEventData eventData)
    {
        MusicMgr.Instance.PlaySound("Music/Audio/BtnChoose",valumMul:0.2f);
    }


}
