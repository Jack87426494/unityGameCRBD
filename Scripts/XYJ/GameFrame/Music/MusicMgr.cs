using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseMgr<MusicMgr>
{
    //音效源对象池
    private List<AudioSource> soundSourcesList=new List<AudioSource>();
    //音效切片对象池
    private Dictionary<string,AudioClip> clipsDic=new Dictionary<string, AudioClip>();

    //背景音乐
    private AudioSource bkAudioSource;

    //背景音乐实例对象
    private GameObject bkMusicObj;
    //音效的实例对象
    private GameObject soundsObj;

    //背景音乐的大小
    private float bkMusicVoluem;
    //背景音乐开关
    private bool isOpenMusic;
    //音效的大小
    private float soundVolum;
    //音效的开关
    private bool isOpenSound;


    public MusicMgr()
    {
        //当音效源过多时自动清除音效源
        MonoMgr.Instance.AddUpdateListener(() =>{
            if(soundSourcesList.Count>100)
            {
                ClearSoundSourcesList(50);
            }
        });
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="clipPath">音效切片的路径</param>
    ///  <param name="isLoop">是否为循环播放</param>
    /// <param name="callBack">播放背景音乐后要做得事情</param>
    public void PlayBkAudioSource(string clipPath,bool isLoop=true,UnityAction<AudioSource> callBack=null)
    {

        //if (!isOpenMusic)
        //    return;
       

        if (bkMusicObj == null)
        {
            bkMusicObj = new GameObject("BkMusicObj");
            bkMusicObj.AddComponent<DontDestroyOnLoad>();
        }
            

        if (bkAudioSource == null)
            bkAudioSource = bkMusicObj.AddComponent<AudioSource>();

        //bkAudioSource.mute = !isOpenMusic;

        if (clipsDic.ContainsKey(clipPath))
        {
            
            bkAudioSource.clip = clipsDic[clipPath];
            //Debug.Log(bkAudioSource.clip);
            bkAudioSource.loop = isLoop;
            bkAudioSource.Play();
        } 
        else
        {
            ResLoadMgr.Instance.LoadAsyn<AudioClip>(clipPath, (audioClip) =>
            {
                //Debug.Log(audioClip);
                bkAudioSource.clip = audioClip;
                bkAudioSource.loop = isLoop;
                clipsDic.Add(clipPath,audioClip);
                bkAudioSource.Play();
            });
        }
        
        bkAudioSource.volume = bkMusicVoluem;
        if(callBack!=null)
        callBack(bkAudioSource);

        
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    /// <param name="callBack">暂停背景音乐播放后执行的函数</param>
    public void PauseBkAudioSource(UnityAction<AudioSource> callBack = null)
    {
        if(bkAudioSource!=null)
        bkAudioSource.Pause();

        callBack(bkAudioSource);
    }

    /// <summary>
    /// 停止背景音乐的播放
    /// </summary>
    /// <param name="callBack">停止背景音乐播放后执行的函数</param>
    public void StopBkAudioSource(UnityAction<AudioSource> callBack = null)
    {
        if (bkAudioSource != null)
            bkAudioSource.Stop();

        callBack(bkAudioSource);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clipPath">音效切片的位置</param>
    /// <param name="isLoop">是否为循环播放</param>
    /// <param name="audioGameObj">播放音乐的物体</param>
    /// <param name="callBack">播放音效后执行的函数</param>
    public void PlaySound(string clipPath, GameObject audioGameObj=null,float valumMul=1f, bool isLoop = false, UnityAction<AudioSource> callBack=null)
    {

        if (!isOpenSound)
            return;
        AudioSource audioSource;
        //用物体身上挂载的音效源的情况下
        if (audioGameObj!=null)
        {
            audioSource = audioGameObj.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = audioGameObj.AddComponent<AudioSource>();
            }
        }
        //用公共的音效源的情况下
        else
        {
            if (soundsObj == null)
            {
                soundsObj = new GameObject("Audios");
                soundsObj.AddComponent<DontDestroyOnLoad>();
            }
            audioSource = GameObject.Find("Audio")?.GetComponent<AudioSource>();
            for (int i = 0; i < soundSourcesList.Count; ++i)
            {
                if (!soundSourcesList[i].isPlaying)
                {
                    audioSource = soundSourcesList[i];
                    break;
                }
                
            }
            if(audioSource==null)
            {
                audioSource = soundsObj.AddComponent<AudioSource>();
                soundSourcesList.Add(audioSource);
            }
           
        }
       

        if(clipsDic.ContainsKey(clipPath))
        {
            audioSource.clip = clipsDic[clipPath];
            audioSource.Play();
        }
        else
        {
            ResLoadMgr.Instance.LoadAsyn<AudioClip>(clipPath,(audioClip) =>
            {
                audioSource.clip = audioClip;
                if(!clipsDic.ContainsKey(clipPath))
                clipsDic.Add(clipPath, audioClip);
                audioSource.Play();
            });
        }

        audioSource.volume = soundVolum* valumMul;
       

        audioSource.loop = isLoop;

        if(callBack!=null)
        callBack(audioSource);


    }

    /// <summary>
    /// 暂停所有的音效播放
    /// </summary>
    public void PauseAllSounds()
    {
        for (int i = 0; i < soundSourcesList.Count; i++)
        {
            soundSourcesList[i].Pause();
        }
    }
    /// <summary>
    /// 停止所有音效的播放
    /// </summary>
    public void StopAllSounds()
    {
        for (int i = 0; i < soundSourcesList.Count; i++)
        {
            soundSourcesList[i].Stop();
        }
    }
    /// <summary>
    /// 清除指定数量音效源
    /// </summary>
    /// <param name="soundCount">清除指定音效源的数量</param>
    public void ClearSoundSourcesList(int soundCount)
    {
        for (int i = soundSourcesList.Count-1; i > soundCount; i--)
        {
            if (!soundSourcesList[i].isPlaying)
            {
               GameObject.Destroy(soundSourcesList[i]);
                soundSourcesList.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// 清空音效切片对象池
    /// </summary>
    public void ClearClipsDic()
    {
        clipsDic.Clear();
    }

    /// <summary>
    /// 设置背景音乐源的开关
    /// </summary>
    /// <param name="isOpen">是否开始背景音乐</param>
    public void SetBkOpen(bool isOpen)
    {
        if (bkAudioSource != null)
            bkAudioSource.mute = !isOpen;
        isOpenMusic = isOpen;
    }

    /// <summary>
    /// 设置背景音乐的大小
    /// </summary>
    /// <param name="bkVolume"></param>
    public void SetBkVolume(float bkVolume)
    {
        bkMusicVoluem = bkVolume;
        if(bkAudioSource!=null)
        bkAudioSource.volume = bkVolume;
    }

    /// <summary>
    /// 设置音效的开关
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetSoundOpen(bool isOpen)
    {
        isOpenSound = isOpen;
    }

    /// <summary>
    /// 设置音效的大小
    /// </summary>
    /// <param name="valuem"></param>
    public void SetSoundValuem(float valuem)
    {
        soundVolum = valuem;

    }
}
