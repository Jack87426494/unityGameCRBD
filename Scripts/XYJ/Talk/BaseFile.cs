using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
public class BaseFile: MonoBehaviour
{
    public string fileInfo = "";

    [Header("��Ҿ����Զ�ĵƹ���ʾ����")]
    public float LithtRad=10f;
    [Header("��Ҿ����Զ������ʾͼ��ͼ�ʰ����")]
    public float getDis=3f;
    [Header("������ʾ")]
    public GameObject tipObj;
    [Header("��־����ţ�һ�㲻�ģ�����������ڿ�����־˵��Ӧ��һ�仰")]
    public int index=0;

    protected Collider2D col2D;

    private UnityEngine.Rendering.Universal.Light2D light2d;

    private SpriteRenderer sr;
    //��ֽͨ��sprite
    private Sprite spriteNormal;
    //����ֽ��sprite
    private Sprite spriteHighLight;
    //��Һ��ļ��ľ���
    private float dis;
    //�Ƿ����ڹۿ�
    private bool isLook;


    private void Awake()
    {
        TryGetComponent(out light2d);
        TryGetComponent(out sr);
        spriteNormal= Resources.Load<Sprite>("Items/Paper/NormalPaper");
        spriteHighLight= Resources.Load<Sprite>("Items/Paper/HighLightPaper");
        tipObj.SetActive(false);
        isLook = false;
    }

    protected virtual void Update()
    {
        if (isLook)
            return;

        col2D = Physics2D.OverlapCircle(transform.position, LithtRad, 1 << LayerMask.NameToLayer("Player"));
        if(col2D != null&& col2D.CompareTag("Player"))
        {
            dis = Vector3.Distance(transform.position, col2D.transform.position);
            light2d.intensity = Mathf.Min(0.15f / dis, 0.15f);
            sr.sprite = spriteHighLight;
            if(dis<getDis)
            {
                tipObj.SetActive(true);
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    isLook = true;
                    tipObj.SetActive(false);
                    light2d.intensity = 0f;
                    Time.timeScale = 0f;
                    HXW_UIManager.Instance.ShowPanelAsync<InfoPanel>(InfoPanel.prefabsPath, PanelLayer.Max, everyTimeCallBack: (panel) =>
                    {
                        BasePlayerController.Instance.isDead = true;
                        panel.LookOverAction += LookOver;
                        panel.SetInfo(fileInfo, index);
                    });
                    
                }
            }
            else
            {
                tipObj.SetActive(false);
                sr.sprite = spriteNormal;
            }
        }
        else
        {
            light2d.intensity = 0f;
            sr.sprite = spriteNormal;
        }
    }

    public void LookOver()
    {
        isLook = false;
    }
}
