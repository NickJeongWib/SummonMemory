using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAtk_Ctrl : MonoBehaviour
{
    [SerializeField] Transform TargetTr;
    [SerializeField] float MoveSpeed;
    [SerializeField] Skill_Ctrl SkillCtrl_Ref;

    private void Update()
    {
        // 타켓에 거의 도달했다면 오브젝트 비활성화 및 피격 판정
        if((TargetTr.position - this.transform.position).magnitude <= 0.8f)
        {
            float atk = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_Atk;
            float normalAtkPower = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_SkillData.Get_NormalAtk_Ratio;
            float criDamage = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CriD;
            float criRate = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CriR;
            
            // 데미지 적용
            SkillCtrl_Ref.NormalAtk(TargetTr.GetComponent<Enemy_Ctrl>(), atk, normalAtkPower, criDamage, criRate);

            // 전투종료
            SkillCtrl_Ref.TurnEnd();

            this.gameObject.SetActive(false);
        }

        // 타켓이 없다면 이동 x
        if(TargetTr != null)
        {
            Vector2 dir = (TargetTr.position - transform.position).normalized;
            transform.position += (Vector3)dir * MoveSpeed * Time.deltaTime;
        }
    }

    public void Set_Target(Transform _Tr)
    {
        TargetTr = _Tr;
    }
}
