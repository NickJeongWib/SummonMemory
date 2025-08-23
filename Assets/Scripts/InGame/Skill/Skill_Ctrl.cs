using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill_Ctrl : MonoBehaviour
{
    // �ؽ�Ʈ ����
    Color TextColor;

    #region Skill_End
    // ĳ���Ͱ� ���� ������ ��
    public void TurnEnd()
    {
        bool allDead = true;

        for (int i = 0; i < InGame_Mgr.Inst.CurMonsters.Count; i++)
        {
            // ü���� 0�� �ƴϸ�
            if (InGame_Mgr.Inst.CurMonsters[i].Get_CurHP > 0)
            {
                allDead = false;
                // ����ִ� �� �߰������� ���� �� �� �ʿ� ����
                break; 
            }
        }

        if (allDead)
        {
            InGame_Mgr.Inst.InGameState = INGAME_STATE.STAGE_END;

            // ���� ���ۿ� ���õ� UI Canvas ���ֱ�
            for(int i = 0; i < InGame_Mgr.Inst.UI_Canvas.Length; i++)
            {
                InGame_Mgr.Inst.UI_Canvas[i].SetActive(false);
            }
            // ���� Ŭ���� ���� Ȱ��ȭ
            InGame_Mgr.Inst.StageClear_Root.SetActive(true);

            // �������� Ŭ���� ������ ����
            UserInfo.StageClear[GameManager.Inst.StageIndex] = true;
            DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.STAGE);
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
            // ü���� 0�� �ƴϸ�
            if (InGame_Mgr.Inst.CharCtrl_List[i].Get_CurHP > 0)
            {
                allDead = false;
                // ����ִ� �� �߰������� ���� �� �� �ʿ� ����
                break;
            }
        }

        if (allDead)
        {
            InGame_Mgr.Inst.InGameState = INGAME_STATE.STAGE_END;

            // ���� ���ۿ� ���õ� UI Canvas ���ֱ�
            for (int i = 0; i < InGame_Mgr.Inst.UI_Canvas.Length; i++)
            {
                InGame_Mgr.Inst.UI_Canvas[i].SetActive(false);
            }
            // ���� Ŭ���� ���� Ȱ��ȭ
            InGame_Mgr.Inst.StageFail_Root.SetActive(true);
        }
        else
        {
            InGame_Mgr.Inst.InGameState = INGAME_STATE.ENEMY_TURN_END;
        }
    }

    #endregion

    #region DamageCalc
    // ��ų ������ ��� ����
    public float CalcDamage(float _atk, float _def, float _skillPower, ref bool _isCrit, float _criR = 0.5f, float _criD = 1.3f, float _elementMul = 1)
    {
        float baseDamage = _atk * _skillPower;

        // ���� ����
        float defenseFactor = _def / (_def + 100f);
        float afterDef = baseDamage * (1 - defenseFactor);

        // �Ӽ� ��
        float afterElement = afterDef * _elementMul;

        // ġ��Ÿ
        bool isCrit = UnityEngine.Random.value < _criR;
        _isCrit = isCrit;
        float critMul = isCrit ? (1f + _criD) : 1f;

        // ������ (�ּ� 1 �̻�)
        return Mathf.Max(1f, afterElement * critMul);
    }
    #endregion

    float ShakeTime = 0.55f;
    float ShakePower = 0.25f;

    // TODO ## Skill_Ctrl.cs ���Ϳ��� ������ ����
    #region Monster Damage
    public void Damage_Point()
    {
        StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(ShakeTime, ShakePower));

        // Debug.Log(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.Get_CharName);
        int targetNum = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_SkillData.Get_TargetCount;

        // InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_SkillData.get
        float atk = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_Atk;
        float skillPower = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_SkillData.Get_Damage_Ratio;
        float criDamage = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CriD;
        float criRate = InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CriR;

        // �Ӽ� ������ ����
        float _elementMul = 0;
        // ������ �������� ������ ���� ��
        int appliedCount = 0; 

        //  ������ ����
        for (int i = 0; i < InGame_Mgr.Inst.CurMonsters.Count; i++)
        {
            // �̹� ���ϴ� ����ŭ ���������� �ߴ�
            if (appliedCount >= targetNum)
                break;

            // �ش� ���Ͱ� �׾�������
            if (InGame_Mgr.Inst.CurMonsters[i].gameObject.activeSelf == false)
            {
                continue;
            }
        
            // �Ӽ� ������
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
            // ������ ����
            InGame_Mgr.Inst.CurMonsters[i].TakeDamage(damage, ref value, _isCrit, TextColor);
            // UI �ʱ�ȭ
            InGame_Mgr.Inst.Get_ObjPool.MonStatUI_List[i].Set_HP(value);
            appliedCount++;
        }
    }
    #endregion

    // TODO ## Skill_Ctrl.cs ĳ���Ϳ��� ������ ����
    #region CharacterDamage
    public void Mon_Damage_Point()
    {

        int targetNum = InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].Get_TargetCount;

        float atk = InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].Get_Atk;
        float skillPower = InGame_Mgr.Inst.CurMonsters[InGame_Mgr.Inst.Get_MonTurnIndex].Get_Skill_Ratio;

        // �Ӽ������� ����
        float _elementMul = 0;
        // ������ �������� ������ ���� ��
        int appliedCount = 0;


        //  ������ ����
        for (int i = 0; i < InGame_Mgr.Inst.CharCtrl_List.Count; i++)
        {
            // �̹� ���ϴ� ����ŭ ���������� �ߴ�
            if (appliedCount >= targetNum)
                break;

            // �ش� ���Ͱ� �׾�������
            if (InGame_Mgr.Inst.CharCtrl_List[i].gameObject.activeSelf == false)
            {
                continue;
            }

            // �Ӽ� ������ �� �Ӽ�
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
            // �� �Ӽ�
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
            // �ٶ��Ӽ�
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
            // ���Ӽ�
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
            // ������ ����
            InGame_Mgr.Inst.CharCtrl_List[i].TakeDamage(damage, ref value, TextColor, true, _isCrit);
            // UI �ʱ�ȭ
            InGame_Mgr.Inst.Get_ObjPool.CharStatUI_List[i].Set_HP(value);

            appliedCount++;
        }
    }
    #endregion

    #region Set_elementMul
    public void NormalAtk(Enemy_Ctrl _enemyCtrl, float _atk, float _atkPower, float _criDamage, float _criRate)
    {
        // �Ӽ� ������ ����
        float _elementMul = 0;

        // �Ӽ� ������
        if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_CharEle == CHAR_ELE.FIRE)
        {
            if (_enemyCtrl.Get_MonEle == MONSTER_ELE.WIND)
            {
                _elementMul = 2.0f;
            }
            else if (_enemyCtrl.Get_MonEle == MONSTER_ELE.WATER)
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
            if (_enemyCtrl.Get_MonEle == MONSTER_ELE.FIRE)
            {
                _elementMul = 2.0f;
            }
            else if (_enemyCtrl.Get_MonEle == MONSTER_ELE.GROUND)
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
            if (_enemyCtrl.Get_MonEle == MONSTER_ELE.GROUND)
            {
                _elementMul = 2.0f;
            }
            else if (_enemyCtrl.Get_MonEle == MONSTER_ELE.FIRE)
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
            if (_enemyCtrl.Get_MonEle == MONSTER_ELE.WATER)
            {
                _elementMul = 2.0f;
            }
            else if (_enemyCtrl.Get_MonEle == MONSTER_ELE.WIND)
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
        float damage = CalcDamage(_atk, _enemyCtrl.Get_Def, _atkPower, ref _isCrit, _criDamage, _criRate, _elementMul);
        float value = 0;
        // ������ ����
        _enemyCtrl.TakeDamage(damage, ref value, _isCrit, TextColor);

        int index = 0;
        for(int i = 0; i < InGame_Mgr.Inst.CurMonsters.Count; i++)
        {
            if(_enemyCtrl == InGame_Mgr.Inst.CurMonsters[i])
            {
                index = i;
                break;
            }
        }
        // UI �ʱ�ȭ
        InGame_Mgr.Inst.Get_ObjPool.MonStatUI_List[index].Set_HP(value);
    }
    #endregion

    // TODO ##  Skill_Ctrl.cs ĳ���� ���� ��ų
    #region Buff_Skill
    public void Buff_Skill()
    {
        // SP Hill�̸�
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

            // ü�� ���� ���� ĳ���� ��ġ
            charList.Sort((a, b) => a.Get_CurHP.CompareTo(b.Get_CurHP));

            int hillCount = 0;
            int charIndex = 0;

            // �� ����
            while (hillCount < InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_TargetCount)
            {
                // �׾����� ���� ����
                if (charList[charIndex].Get_CurHP <= 0)
                {
                    // ���� ĳ����
                    charIndex++;
                    continue;
                }
                   

                float hillValue = charList[charIndex].Get_MaxHP * InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Buff_Ratio;
                float value = 0;

                // �� ����
                charList[charIndex].TakeDamage(hillValue, ref value, TextColor, false);
                hillCount++;
                charIndex++;
            }

            for(int i = 0; i < InGame_Mgr.Inst.CharCtrl_List.Count; i++)
            {
                // UI HP���� ���
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
    // TODO ##  Skill_Ctrl.cs SP ȸ��
    void SP_Hill()
    {
        for (int i = InGame_Mgr.Inst.CurSP; i < (InGame_Mgr.Inst.CurSP +
                InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_SP_Hill_Count); i++)
        {
            // 15���� Ŀ���� break
            if (15 <= i)
                break;

            InGame_Mgr.Inst.SP_ChargeAnimator[i].Play("SP_UP");
        }

        InGame_Mgr.Inst.CurSP += Mathf.RoundToInt(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_SP_Hill_Count);

        // �ִ�ġ 15���� Ŀ���� 15
        if (15 < InGame_Mgr.Inst.CurSP)
        {
            InGame_Mgr.Inst.CurSP = 15;
        }
    }

    void Buff_Apply(BUFF_TYPE _buffType)
    {
        InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].CanSkill = false;

        float buffValue = 0;
        // 1�� ����
        if (InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_TargetCount == 1)
        {
            // ���� ����
            InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].TakeBuff(_buffType,
                InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Buff_Ratio,
                ref buffValue);

            // ������ ����
            InGame_Mgr.Inst.Get_ObjPool.Get_BuffIcon(InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex],
                InGame_Mgr.Inst.Get_ObjPool.CharStatUI_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_Buff_Tr,
                InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Skill_Icon,
                InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Buff_Time,
                buffValue, _buffType);
        }
        else // �Ʊ� ����
        {
            for (int i = 0; i < InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_TargetCount; i++)
            {
                if (InGame_Mgr.Inst.CharCtrl_List.Count <= i)
                    break;

                InGame_Mgr.Inst.CharCtrl_List[i].TakeBuff(_buffType,
                    InGame_Mgr.Inst.CharCtrl_List[InGame_Mgr.Inst.CurTurnCharIndex].Get_character.SkillData.Get_Buff_Ratio,
                    ref buffValue);

                // ������ ����
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
