using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoSorting : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxColl;
    private string defaultObstacleLayer = "Obstacle";

    //if (boxColl == null)
    //{
    //    boxColl = GetComponentInChildren<BoxCollider2D>();
    //    if (boxColl == null)
    //        boxColl.gameObject.AddComponent<BoxCollider2D>();
    //}
    private void Update()
    {
        if (sr == null)
        {
            sr = GetComponentInChildren<SpriteRenderer>();
            if (sr == null)
                sr.gameObject.AddComponent<SpriteRenderer>();
        }

        if(sr.sortingLayerName != defaultObstacleLayer)
            sr.sortingLayerName = defaultObstacleLayer;
        if (sr != null)
            sr.sortingOrder = -(int)(transform.position.y * 100);
    }
}

