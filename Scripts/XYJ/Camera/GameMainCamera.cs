using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypes
{
    //�̶�ģʽ
    Fix,
    //��ֵģʽ(�ȿ����)
    Lerp,
    //���ٲ�ֵģʽ
    UniformLerp,
    SceneView,
}
public class GameMainCamera : MonoBehaviour
{
    [Header("���������")]
    public CameraTypes cameraType;
    [Header("����ƶ��ٶ�")]
    public float speed;

    private Transform playerTransform;
    private float time;
    private float changeTime;
    private Vector3 startVector;
    private Vector3 targetVector;
    //���ԭ����z����ֵ
    private float z;


    
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        targetVector =new Vector3(playerTransform.position.x,playerTransform.position.y,
            transform.position.z);
        z = transform.position.z;
        
    }
    private void Update()
    {
        switch (cameraType)
        {
            case CameraTypes.Fix:
                transform.position = playerTransform.position+Vector3.forward*z;
                break;
            case CameraTypes.Lerp:
                transform.position = Vector2.Lerp(transform.position, playerTransform.position,
                    speed*Time.deltaTime);
                transform.position += Vector3.forward * z;
                break;
            case CameraTypes.UniformLerp:
                if(targetVector!=playerTransform.position)
                {
                    startVector = transform.position;
                    targetVector = playerTransform.position;
                    time = 0;
                }
                time += Time.deltaTime;
                transform.position = Vector2.Lerp(startVector, targetVector,
                    time);
                transform.position += Vector3.forward * z;
                break;
        }
    }

    /// <summary>
    /// �л���ͷ
    /// </summary>
    /// <param name="pos">��ͷ��λ��</param>
    public void ChangeLens(Transform targetTransfrom)
    {
        BasePlayerController.Instance.isDead = true;
        playerTransform = targetTransfrom;
        StartCoroutine(ChangeLensI());
        
    }

    private void ChangePlayer()
    {
        
    }

    IEnumerator ChangeLensI()
    {
        BasePlayerController.Instance.hxw_weapon.lockShoot = true;
        changeTime = 3f;
        Camera.main.orthographicSize = 3f;
        while (changeTime>0)
        {
            changeTime -= Time.deltaTime;
            yield return null;
        }
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Camera.main.orthographicSize = 7f;
        BasePlayerController.Instance.isDead = false;
        BasePlayerController.Instance.hxw_weapon.lockShoot = false;
        StopCoroutine(ChangeLensI());
    }
}