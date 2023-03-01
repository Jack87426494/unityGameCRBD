using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetColorImg: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Sprite img;
    public Image imgNow;
    public Sprite imgSub;

    public void OnPointerEnter(PointerEventData eventData)
    {
        img = imgNow.sprite;
        imgNow.sprite = imgSub;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imgNow.sprite = img;
    }
}
