using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float attackCD;
    [SerializeField] private float attackDamage;

    [SerializeField] private float attackRange;
    private Skill_Data skill_Data;
    private Collider2D hit2D;
    private Vector3 fireDir;
    private float timer;
    private SkillChange_Data skillChange_Data;
    void Start()
    {
        skillChange_Data = SkillManager.Instance.skillChange_Data;
        SkillManager.Instance.updateDataEvent += UpdateSkillData;
        skill_Data = GameDataMgr.Instance.GetSkill_Data("Ghost");
        attackCD = skill_Data.attackCD;
        attackRange = skill_Data.attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.RotateAround(transform.parent.position, Vector3.forward, 50 * Time.deltaTime);
        transform.right = Vector3.right;
        hit2D = Physics2D.OverlapCircle(transform.position, attackRange, 1 << LayerMask.NameToLayer("Enemy"));
        if(hit2D!=null&&timer>attackCD)
        {
            if(hit2D.transform.position.x<transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            timer = 0;
            fireDir = (hit2D.transform.position - transform.position).normalized;
            ObjectPoolMgr.Instance.GetObject(GhostBullet.prefabsPath, (obj) =>
            {
                
                GhostBullet bullet = obj.GetComponent<GhostBullet>();
                
                bullet.Fire(transform,fireDir);
            });
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void UpdateSkillData()
    {
        attackCD = skillChange_Data.ghostFireCD_Change * skill_Data.attackCD;
        attackRange = skillChange_Data.ghostAttackRange_Change * skill_Data.attackRange;
        if(skillChange_Data.ghostSkill3)
        {
            attackCD *= skillChange_Data.fireCD_Change;//¹¥ËÙ¼Ó³É
        }
    }

    private void OnDisable()
    {
        SkillManager.Instance.updateDataEvent -= UpdateSkillData;
    }
}
