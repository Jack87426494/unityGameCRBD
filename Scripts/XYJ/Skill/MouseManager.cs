using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MouseManager : MonoBehaviour
{
    public Texture2D idelTexture, fireTexture, updateBulletTexture;
    private GameObject playerUIObj;
    private PlayerUI playerUI;
    private static MouseManager instance;
    public static MouseManager Instance=>instance;
    
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
       
        //DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        //idelTexture = Resources.Load<Texture2D>("Art/图标/准星普通");
        //fireTexture = Resources.Load<Texture2D>("Art/图标/准星射击");
        //updateBulletTexture = Resources.Load<Texture2D>("Art/图标/准星子弹耗尽");
        Cursor.SetCursor(idelTexture, new Vector2(16, 16), CursorMode.Auto);
        playerUIObj = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).gameObject;
        playerUI = playerUIObj.GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        SetCursorTexture();
    }

    void SetCursorTexture()
    {
        if(playerUI.isUpdateBullet)
        {
            Cursor.SetCursor(updateBulletTexture, new Vector2(16, 16), CursorMode.Auto);
            //Cursor.visible = false;
            return;
        }
        Cursor.visible = true;
        RaycastHit2D raycastHit2D;
        Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
        raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if(raycastHit2D.collider!=null)
        {
            switch (raycastHit2D.collider.tag)
            {
                case "Enemy":
                    {
                        Cursor.SetCursor(fireTexture, new Vector2(16, 16), CursorMode.Auto);
                        
                    }
                    break;
                default:
                    {
                        Cursor.SetCursor(idelTexture, new Vector2(16, 16), CursorMode.Auto);
                    }
                    
                    break;
            }
            
        }
        else
        {
            Cursor.SetCursor(idelTexture, new Vector2(16, 16), CursorMode.Auto);
        }

        
    }
}
