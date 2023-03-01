using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        //MusicMgr.Instance.PlayBkAudioSource("Music/begin");

        if (GameDataMgr.Instance == null)
        {

            new GameObject("GameDataMgr").AddComponent<GameDataMgr>();
        }
        //if(GameManager.Instance==null)
        //{
        //    new GameObject("GameManager").AddComponent<GameManager>();
        //}

        //if(MouseManager.Instance==null)
        //{
        //    new GameObject("MouseManager").AddComponent<MouseManager>();
        //}
        //if(SkillManager.Instance==null)
        //{
        //    new GameObject("SkillManager").AddComponent<SkillManager>();
        //}

        //AStarMgr.Instance.GenerateCenterMap();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //MusicMgr.Instance.PlayBkAudioSource("Music/battle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
