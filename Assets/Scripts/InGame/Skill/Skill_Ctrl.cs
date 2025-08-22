using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill_Ctrl : MonoBehaviour
{
    // 텍스트 색상
    Color TextColor;

    #region Skill_End
    // 캐릭터가 공격 차례일 때
    public void TurnEnd()
    {
        bool allDead = true;

        for (int i = 0; i < InGame_Mgr.Inst.CurMonsters.Count; i++)
        {
            // 체력이 0이 아니면
            if (InGame_Mgr.Inst.CurMonsters[i].Get_CurHP > 0)
            {
                allDead = false;
                // 살아있는 애 발견했으니 굳이 더 볼 필요 없음
                break; 
            }
        }

        if (allDead)
        {
            InGame_Mgr.Inst.InGameState = INGAME_STATE.STAGE_END;
        }
        else
        {
            InGame_Mgr.Inst.InGameState = INGAME_STATE.TURN_END;
        }
    }

    public void MonTurnEnd()
    {
        bool allDead = true;

        for (int i = 0; i < InGame_Mgr.Inst.CharCtrl_List.Count; i++)
        {
            // 체력이 0이 아니면
            if (InGame_Mgr.Inst.CharCtrl_List[i].Get_CurHP > 0)
            {
                allDead = false;
                // 살아있는 애 발견했으니 굳이 더 볼 필요 없음
                break;
            }
        }

        if (allDead)
        {
            InGame_Mgr.Inst.InGameState = INGAME_STATE.STAGE_END;
        }
        else
        {
            InGame_Mgr.Inst.InGameState = INGAME_STATE.ENEMY_TURN_END;
        }
    }

    #endregion

    #region DamageCalc
    // 스킬 데미지 계산 공식
    float CalcDamage(float _atk, float _def, float _skillPower, ref bool _isCrit, float _criR = 0.5f, float _criD = 1.3f, float _elementMul = 1)
    {
        float baseDamage = _atk * _skillPower;

        // 방어력 보정
        float defenseFactor = _def / (_def + 100f);
        float afterDef = baseDamage * (1 - defenseFactor);

        // 속성 상성
        float afterElement = afterDef * _elementMul;

        // 치명타
        bool isCrit = UnityEngine.Random.value < _criR;
        _isCrit = isCrit;
        float critMul = isCrit ? (1f + _criD) : 1f;

        // 최종값 (최소 1 이상)
        return Mathf.Max(1f, afterElement * critMul);
    }
    #endregion

    // TODO ## Skill_Ctrl 몬스터에게 데미지 적용
    #region Monster Damage
    public void Damage_Point()
    {
        // Debug.Log(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.Get_CharName);
        int targetNum = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_SkillData.Get_TargetCount;

        // InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_SkillData.get
        float atk = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_Atk;
        float skillPower = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_SkillData.Get_Damage_Ratio;
        float criDamage = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CriD;
        float criRate = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CriR;

        // 속성 데미지 비율
        float _elementMul = 0;
        // 실제로 데미지를 적용한 몬스터 수
        int appliedCount = 0; 

        //  데미지 적용
        for (int i = 0; i < InGame_Mgr.Inst.CurMonsters.Count; i++)
        {
            // 이미 원하는 수만큼 적용했으면 중단
            if (appliedCount >= targetNum)
                break;

            // 해당 몬스터가 죽어있으면
            if (InGame_Mgr.Inst.CurMonsters[i].gameObject.activeSelf == false)
            {
                continue;
            }
        
            // 속성 데미지
            if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CharEle == CHAR_ELE.FIRE)
            {
                if(InGame_Mgr.Inst.CurMonsters[i].Get_MonEle == MONSTER_ELE.WIND)
                {
                    _elementMul = 2.0f;
                }
                else if (InGame_Mgr.Inst.CurMonsters[i].Get_MonEle == MONSTER_ELE.WATER)
                {
                    _elementMul = 0.8f;
                }
                else
                {
                    _elementMul = 1.0f;
                }

                TextColor = InGame_Mgr.Inst.TextColor[0];
            }
            else if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CharEle == CHAR_ELE.WATER)
            {
                if (InGame_Mgr.Inst.CurMonsters[i].Get_MonEle == MONSTER_ELE.FIRE)
                {
                    _elementMul = 2.0f;
                }
                else if (InGame_Mgr.Inst.CurMonsters[i].Get_MonEle == MONSTER_ELE.GROUND)
                {
                    _elementMul = 0.8f;
                }
                else
                {
                    _elementMul = 1.0f;
                }

                TextColor = InGame_Mgr.Inst.TextColor[1];
            }
            else if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CharEle == CHAR_ELE.WIND)
            {
                if (InGame_Mgr.Inst.CurMonsters[i].Get_MonEle == MONSTER_ELE.GROUND)
                {
                    _elementMul = 2.0f;
                }
                else if (InGame_Mgr.Inst.CurMonsters[i].Get_MonEle == MONSTER_ELE.FIRE)
                {
                    _elementMul = 0.8f;
                }
                else
                {
                    _elementMul = 1.0f;
                }

                TextColor = InGame_Mgr.Inst.TextColor[2];
            }
            else if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CharEle == CHAR_ELE.GROUND)
            {
                if (InGame_Mgr.Inst.CurMonsters[i].Get_MonEle == MONSTER_ELE.WATER)
                {
                    _elementMul = 2.0f;
                }
                else if (InGame_Mgr.Inst.CurMonsters[i].Get_MonEle == MONSTER_ELE.WIND)
                {
                    _elementMul = 0.8f;
                }
                else
                {
                    _elementMul = 1.0f;
                }

                TextColor = InGame_Mgr.Inst.TextColor[3];
            }

            bool _isCrit = false;
            float damage = CalcDamage(atk, InGame_Mgr.Inst.CurMonsters[i].Get_Def, skillPower, ref _isCrit, criDamage, criRate, _elementMul);
            float value = 0;
            // 데미지 적용
            InGame_Mgr.Inst.CurMonsters[i].TakeDamage(damage, ref value, _isCrit, TextColor);
            // UI 초기화
            InGame_Mgr.Inst.Get_ObjPool.MonStatUI_List[i].Set_HP(value);
            appliedCount++;
        }
    }
    #endregion

    // TODO ## Skill_Ctrl 캐릭터에게 데지미 적용
    #region CharacterDamage
    public void Mon_Damage_Point()
    {
        // Debug.Log(InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].MonName);
        int targetNum = InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].Get_TargetCount;

        float atk = InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].Get_Atk;
        float skillPower = InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].Get_Skill_Ratio;

        // 속성데미지 비율
        float _elementMul = 0;
        // 실제로 데미지를 적용한 몬스터 수
        int appliedCount = 0;


        //  데미지 적용
        for (int i = 0; i < InGame_Mgr.Inst.CharCtrl_List.Count; i++)
        {
            // 이미 원하는 수만큼 적용했으면 중단
            if (appliedCount >= targetNum)
                break;

            // 해당 몬스터가 죽어있으면
            if (InGame_Mgr.Inst.CharCtrl_List[i].gameObject.activeSelf == false)
            {
                continue;
            }

            // 속성 데미지 불 속성
            if (InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].Get_MonEle == MONSTER_ELE.FIRE)
            {
                if (InGame_Mgr.Inst.CharCtrl_List[i].Get_CharEle == CHAR_ELE.WIND)
                {
                    _elementMul = 2.0f;
                }
                else if (InGame_Mgr.Inst.CharCtrl_List[i].Get_CharEle == CHAR_ELE.WATER)
                {
                    _elementMul = 0.8f;
                }
                else
                {
                    _elementMul = 1.0f;
                }

                TextColor = InGame_Mgr.Inst.TextColor[0];
            }
            // 물 속성
            else if (InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].Get_MonEle == MONSTER_ELE.WATER)
            {
                if (InGame_Mgr.Inst.CharCtrl_List[i].Get_CharEle == CHAR_ELE.FIRE)
                {
                    _elementMul = 2.0f;
                }
                else if (InGame_Mgr.Inst.CharCtrl_List[i].Get_CharEle == CHAR_ELE.GROUND)
                {
                    _elementMul = 0.8f;
                }
                else
                {
                    _elementMul = 1.0f;
                }

                TextColor = InGame_Mgr.Inst.TextColor[1];
            }
            // 바람속성
            else if (InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].Get_MonEle == MONSTER_ELE.WIND)
            {
                if (InGame_Mgr.Inst.CharCtrl_List[i].Get_CharEle == CHAR_ELE.GROUND)
                {
                    _elementMul = 2.0f;
                }
                else if (InGame_Mgr.Inst.CharCtrl_List[i].Get_CharEle == CHAR_ELE.FIRE)
                {
                    _elementMul = 0.8f;
                }
                else
                {
                    _elementMul = 1.0f;
                }

                TextColor = InGame_Mgr.Inst.TextColor[2];
            }
            // 땅속성
            else if (InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].Get_MonEle == MONSTER_ELE.GROUND)
            {
                if (InGame_Mgr.Inst.CharCtrl_List[i].Get_CharEle == CHAR_ELE.WATER)
                {
                    _elementMul = 2.0f;
                }
                else if (InGame_Mgr.Inst.CharCtrl_List[i].Get_CharEle == CHAR_ELE.WIND)
                {
                    _elementMul = 0.8f;
                }
                else
                {
                    _elementMul = 1.0f;
                }

                TextColor = InGame_Mgr.Inst.TextColor[3];
            }

            bool _isCrit = false;
            float damage = CalcDamage(atk, InGame_Mgr.Inst.CharCtrl_List[i].Get_Def, skillPower, ref _isCrit, 0.5f, 1.3f, _elementMul);
            float value = 0;
            // 데미지 적용
            InGame_Mgr.Inst.CharCtrl_List[i].TakeDamage(damage, ref value, TextColor, true, _isCrit);
            // UI 초기화
            InGame_Mgr.Inst.Get_ObjPool.CharStatUI_List[i].Set_HP(value);

            appliedCount++;
        }
    }
    #endregion

    // TODO ##  Skill_Ctrl 캐릭터 버프 스킬
    #region Buff_Skill
    public void Buff_Skill()
    {
        // SP Hill이면
        if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_BuffType
            == BUFF_TYPE.SP_HILL)
        {
            #region SP_Hill
            SP_Hill();
            #endregion
        }
        else if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_BuffType
            == BUFF_TYPE.HILL)
        {
            #region Hill
            List<Character_Ctrl> charList = new List<Character_Ctrl>();

            for(int i = 0; i < InGame_Mgr.Inst.CharCtrl_List.Count; i++)
            {
                charList.Add(InGame_Mgr.Inst.CharCtrl_List[i]);
            }

            // 체력 제일 낮은 캐릭터 서치
            charList.Sort((a, b) => a.Get_CurHP.CompareTo(b.Get_CurHP));

            for (int i = 0; i < InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_TargetCount; i++)
            {
                float hillValue = charList[i].Get_MaxHP * InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Buff_Ratio;
                float value = 0;

                // 힐 적용
                charList[i].TakeDamage(hillValue, ref value, TextColor, false);
            }

            for(int i = 0; i < InGame_Mgr.Inst.CharCtrl_List.Count; i++)
            {
                // UI HP비율 계산
                float bar_Ratio = InGame_Mgr.Inst.CharCtrl_List[i].Get_CurHP / InGame_Mgr.Inst.CharCtrl_List[i].Get_MaxHP;
                InGame_Mgr.Inst.Get_ObjPool.CharStatUI_List[i].Set_HP(bar_Ratio);
            }
            #endregion
        }
        else if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_BuffType
            == BUFF_TYPE.DEF)
        {
            #region Def
            Buff_Apply(BUFF_TYPE.DEF);
            #endregion
        }
        else if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_BuffType
           == BUFF_TYPE.ATK)
        {
            #region Atk
            Buff_Apply(BUFF_TYPE.ATK);
            #endregion
        }
        else if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_BuffType
            == BUFF_TYPE.ALL_BUFF)
        {
            #region Atk
            Buff_Apply(BUFF_TYPE.ALL_BUFF);
            SP_Hill();
            #endregion
        }
    }

    void SP_Hill()
    {
        for (int i = InGame_Mgr.Inst.CurSP; i < (InGame_Mgr.Inst.CurSP +
                InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_SP_Hill_Count); i++)
        {
            // 15보다 커지면 break
            if (15 <= i)
                break;

            InGame_Mgr.Inst.SP_ChargeAnimator[i].Play("SP_UP");
        }

        InGame_Mgr.Inst.CurSP += Mathf.RoundToInt(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_SP_Hill_Count);

        // 최대치 15보다 커지면 15
        if (15 < InGame_Mgr.Inst.CurSP)
        {
            InGame_Mgr.Inst.CurSP = 15;
        }
    }

    void Buff_Apply(BUFF_TYPE _buffType)
    {
        InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].CanSkill = false;

        float buffValue = 0;
        // 1인 버프
        if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_TargetCount == 1)
        {
            // 버프 증가
            InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].TakeBuff(_buffType,
                InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Buff_Ratio,
                ref buffValue);

            // 아이콘 생성
            InGame_Mgr.Inst.Get_ObjPool.Get_BuffIcon(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex],
                InGame_Mgr.Inst.Get_ObjPool.CharStatUI_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_Buff_Tr,
                InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Skill_Icon,
                InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Buff_Time,
                buffValue, _buffType);
        }
        else // 아군 버프
        {
            for (int i = 0; i < InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_TargetCount; i++)
            {
                if (InGame_Mgr.Inst.CharCtrl_List.Count <= i)
                    break;

                InGame_Mgr.Inst.CharCtrl_List[i].TakeBuff(_buffType,
                    InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Buff_Ratio,
                    ref buffValue);

                // 아이콘 생성
                InGame_Mgr.Inst.Get_ObjPool.Get_BuffIcon(InGame_Mgr.Inst.CharCtrl_List[i],
                    InGame_Mgr.Inst.Get_ObjPool.CharStatUI_List[i].Get_Buff_Tr,
                    InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Skill_Icon,
                    InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Buff_Time,
                    buffValue, _buffType);
            }
        }
    }
    #endregion
}
