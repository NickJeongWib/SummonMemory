using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class BuffIcon_UI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] BUFF_TYPE BuffType;
    [SerializeField] DEBUFF_TYPE DeBuffType;

    [SerializeField] Character_Ctrl CharCtrl;
    [SerializeField] Character_Ctrl UseSkill_Char;
    public Character_Ctrl Get_Character_Ctrl { get => CharCtrl; }
    [SerializeField] Enemy_Ctrl EnemyCtrl;
    public Enemy_Ctrl Get_EnemyCtrl { get => EnemyCtrl; }

    [SerializeField] int Skill_Turn;
    public int Set_Skill_Turn { set => Skill_Turn = value; }
    [SerializeField] float BuffValue;

    [SerializeField] Image Skill_Icon;
    [SerializeField] Text Skill_Turn_Txt;

    Skill Skill_Ref;
    string SkillName;

    // 스킬 버프 UI 초기화
    public void Set_Skill_UI(Character_Ctrl _useChar, Character_Ctrl _charCtrl, Transform _parent, Sprite _sprite, int _turn, float _buffValue, BUFF_TYPE _buffType, Skill _skill)
    {
        UseSkill_Char = _useChar;
        CharCtrl = _charCtrl;
        BuffValue = _buffValue;
        this.transform.SetParent(_parent, false);
        Skill_Icon.sprite = _sprite;
        BuffType = _buffType;

        Skill_Turn = _turn;
        Skill_Turn_Txt.text = Skill_Turn.ToString();
        Skill_Ref = _skill;
    }

    // 디버퍼 스킬 UI 초기화
    public void Set_DeBuff_UI(Enemy_Ctrl _enemyCtrl, Transform _parent, Sprite _sprite, int _turn, DEBUFF_TYPE _deBuffType, string _skillName, float _buffValue = 0, Skill _skill = null)
    {
        EnemyCtrl = _enemyCtrl;
        this.transform.SetParent(_parent, false);
        Skill_Icon.sprite = _sprite;
        DeBuffType = _deBuffType;
        BuffValue = _buffValue;

        Skill_Turn = _turn;
        Skill_Turn_Txt.text = Skill_Turn.ToString();
        SkillName = _skillName;
        Skill_Ref = _skill;
    }

    // 적용 턴 재설정
    public void Set_ApplyTurn_Text(int _turn)
    {
        Skill_Turn = _turn;
        Skill_Turn_Txt.text = Skill_Turn.ToString();
    }

    public void Turn_Decreased()
    {
        // 턴 감소
        Skill_Turn--;
        // 텍스트로 출력
        Skill_Turn_Txt.text = Skill_Turn.ToString();

        if (Skill_Turn <= 0)
        {
            // 캐릭터가 버프를 받고 있다면
            if(CharCtrl != null)
            {
                // 현재 아이콘의 버프 타입 전달
                CharCtrl.EndBuff(BuffType, BuffValue);
                // 버프가 끝났으니 버프를 사용하는 캐릭터는 스킬을 쓸 수 있도록
                UseSkill_Char.CanSkill = true;
            }

            // 몬스터가 버프를 받고 있다면
            if (EnemyCtrl != null)
            {
                EnemyCtrl.EndDeBuff(DeBuffType, BuffValue, SkillName, this);
            }

            // 오브젝트 풀에서 다시 재사용 하기 위해 변수들 모두 초기화
            BuffType = BUFF_TYPE.NONE;
            DeBuffType = DEBUFF_TYPE.NONE;
            CharCtrl = null;
            UseSkill_Char = null;
            EnemyCtrl = null;
            Skill_Turn = 0;
            BuffValue = 0;
            Skill_Icon.sprite = null;
            Skill_Turn_Txt.text = "";
            SkillName = "";
            Skill_Ref = null;

            this.gameObject.SetActive(false);
        }
    }

    // 버프 아이콘 눌렀을 때 스킬 설명 나올 수 있도록
    public void OnPointerDown(PointerEventData eventData)
    {
        InGame_Mgr.Inst.Show_Skill_Desc(true, this, Skill_Ref, this.transform, Skill_Icon.sprite);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InGame_Mgr.Inst.Show_Skill_Desc(false);
    }
}
