using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashLight : MonoBehaviour
{
    private Vector2 vector2;
    private float angle;
    private Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vector2 = mousePos -
            transform.position;
        angle = Vector2.Angle(Vector2.up, vector2);
        if (mousePos.x > transform.position.x)
        {
            angle = -angle;
        }

        transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
