using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("�ӵ����е��ٶ�")]
    public float moveSpeed;
    public float bulletSize;//�ӵ��Ĵ�С

    ////�Ƿ�ת
    //public bool isLeft;

    //private SpriteRenderer spriteRenderer;
    private void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, 2f);
    }

    private void FixedUpdate()
    {
        //if(!isLeft)
        //{
        //    transform.Translate(transform.right * Time.deltaTime * moveSpeed);
        //}
        //else
        //{
        //    spriteRenderer.flipX = true;
        //    transform.Translate(-transform.right * Time.deltaTime * moveSpeed);
        //}

        transform.Translate(transform.right * Time.deltaTime * moveSpeed,Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
