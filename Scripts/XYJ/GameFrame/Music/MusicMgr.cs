using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseMgr<MusicMgr>
{
    //��ЧԴ�����
    private List<AudioSource> soundSourcesList=new List<AudioSource>();
    //��Ч��Ƭ�����
    private Dictionary<string,AudioClip> clipsDic=new Dictionary<string, AudioClip>();

    //��������
    private AudioSource bkAudioSource;

    //��������ʵ������
    private GameObject bkMusicObj;
    //��Ч��ʵ������
    private GameObject soundsObj;

    //�������ֵĴ�С
    private float bkMusicVoluem;
    //�������ֿ���
    private bool isOpenMusic;
    //��Ч�Ĵ�С
    private float soundVolum;
    //��Ч�Ŀ���
    private bool isOpenSound;


    public MusicMgr()
    {
        //����ЧԴ����ʱ�Զ������ЧԴ
        MonoMgr.Instance.AddUpdateListener(() =>{
            if(soundSourcesList.Count>100)
            {
                ClearSoundSourcesList(50);
            }
        });
    }

    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="clipPath">��Ч��Ƭ��·��</param>
    ///  <param name="isLoop">�Ƿ�Ϊѭ������</param>
    /// <param name="callBack">���ű������ֺ�Ҫ��������</param>
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
    /// ��ͣ��������
    /// </summary>
    /// <param name="callBack">��ͣ�������ֲ��ź�ִ�еĺ���</param>
    public void PauseBkAudioSource(UnityAction<AudioSource> callBack = null)
    {
        if(bkAudioSource!=null)
        bkAudioSource.Pause();

        callBack(bkAudioSource);
    }

    /// <summary>
    /// ֹͣ�������ֵĲ���
    /// </summary>
    /// <param name="callBack">ֹͣ�������ֲ��ź�ִ�еĺ���</param>
    public void StopBkAudioSource(UnityAction<AudioSource> callBack = null)
    {
        if (bkAudioSource != null)
            bkAudioSource.Stop();

        callBack(bkAudioSource);
    }

    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="clipPath">��Ч��Ƭ��λ��</param>
    /// <param name="isLoop">�Ƿ�Ϊѭ������</param>
    /// <param name="audioGameObj">�������ֵ�����</param>
    /// <param name="callBack">������Ч��ִ�еĺ���</param>
    public void PlaySound(string clipPath, GameObject audioGameObj=null,float valumMul=1f, bool isLoop = false, UnityAction<AudioSource> callBack=null)
    {

        if (!isOpenSound)
            return;
        AudioSource audioSource;
        //���������Ϲ��ص���ЧԴ�������
        if (audioGameObj!=null)
        {
            audioSource = audioGameObj.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = audioGameObj.AddComponent<AudioSource>();
            }
        }
        //�ù�������ЧԴ�������
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
    /// ��ͣ���е���Ч����
    /// </summary>
    public void PauseAllSounds()
    {
        for (int i = 0; i < soundSourcesList.Count; i++)
        {
            soundSourcesList[i].Pause();
        }
    }
    /// <summary>
    /// ֹͣ������Ч�Ĳ���
    /// </summary>
    public void StopAllSounds()
    {
        for (int i = 0; i < soundSourcesList.Count; i++)
        {
            soundSourcesList[i].Stop();
        }
    }
    /// <summary>
    /// ���ָ��������ЧԴ
    /// </summary>
    /// <param name="soundCount">���ָ����ЧԴ������</param>
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
    /// �����Ч��Ƭ�����
    /// </summary>
    public void ClearClipsDic()
    {
        clipsDic.Clear();
    }

    /// <summary>
    /// ���ñ�������Դ�Ŀ���
    /// </summary>
    /// <param name="isOpen">�Ƿ�ʼ��������</param>
    public void SetBkOpen(bool isOpen)
    {
        if (bkAudioSource != null)
            bkAudioSource.mute = !isOpen;
        isOpenMusic = isOpen;
    }

    /// <summary>
    /// ���ñ������ֵĴ�С
    /// </summary>
    /// <param name="bkVolume"></param>
    public void SetBkVolume(float bkVolume)
    {
        bkMusicVoluem = bkVolume;
        if(bkAudioSource!=null)
        bkAudioSource.volume = bkVolume;
    }

    /// <summary>
    /// ������Ч�Ŀ���
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetSoundOpen(bool isOpen)
    {
        isOpenSound = isOpen;
    }

    /// <summary>
    /// ������Ч�Ĵ�С
    /// </summary>
    /// <param name="valuem"></param>
    public void SetSoundValuem(float valuem)
    {
        soundVolum = valuem;

    }
}
