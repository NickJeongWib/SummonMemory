using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class Character_Ctrl : MonoBehaviour
{
    Character character;
    public Character Get_character { get => character; }

    [Header("HP")]
    float MaxHP;
    public float Get_MaxHP { get => MaxHP; }
    [SerializeField] Image HP_Image;
    [SerializeField] float CurHP;
    public float Get_CurHP { get => CurHP; }

    [Header("CharStat")]
    public bool CanSkill = true;
    CHAR_ELE CharEle;
    public CHAR_ELE Get_CharEle { get => CharEle; }
    [SerializeField] float Atk;
    public float Get_Atk { get => Atk; set => Atk = value; }
    [SerializeField] float Def;
    public float Get_Def { get => Def; set => Def = value; }
    float CriD;
    public float Get_CriD { get => CriD; }
    float CriR;
    public float Get_CriR { get => CriR; }

    [SerializeField] Skill SkillData;
    public Skill Get_SkillData { get => SkillData; }
    [SerializeField] GameObject Skill_Prefab;
    public GameObject Get_Skill_Prefab { get => Skill_Prefab; }

    [SerializeField] Animator animator;
    [SerializeField] Animator Damage_Animator;

    [Header("UI")]
    [SerializeField] Text DamageText;

    protected virtual void Update()
    {
        
    }

    protected virtual void Start()
    {
        // 스킬 프리펩이 없다면
        if (Skill_Prefab == null)
        {
            // Debug.Log(SkillData.Get_Skill_Name);
            // 스킬 이펙트 동적생성
            GameObject skill_obj = Resources.Load<GameObject>(SkillData.Get_Skill_Prefab_Path);
            GameObject skill = Instantiate(skill_obj);
            Skill_Prefab = skill;
            Skill_Prefab.SetActive(false);

            Transform skill_Tr = GameObject.Find("Skill_Tr").transform;
            Skill_Prefab.transform.SetParent(skill_Tr);
        }
    }

    #region Stat_Init
    // 스탯 초기화
    public virtual void Set_Init(float _hp, float _atk, float _def, float _criD, float _criR, Skill _skillData, Character _char, CHAR_ELE _ele)
    {
        MaxHP = _hp;
        Atk = _atk;
        Def = _def;
        CriD = _criD;
        CriR = _criR;
        CurHP = MaxHP;

        CharEle = _ele;

        SkillData = _skillData;
        character = _char;
    }
    #endregion

    #region TakeDamage
    public void TakeDamage(float _value, ref float _hpBarvalue, Color _color, bool _isDamage = true, bool _isCrit = false)
    {
        if (_isDamage)
        {
            // 크리티컬 데미지 적용 시
            if (_isCrit)
            {
                Damage_Animator.gameObject.SetActive(true);
                Damage_Animator.Play("CriDamage");
            }
            else // 일반 데미지 적용 시
            {
                Damage_Animator.gameObject.SetActive(true);
                Damage_Animator.Play("Damage");
            }

            DamageText.color = _color;
            DamageText.text = $"-{_value.ToString("N0")}";

            // 체력 제거
            CurHP -= _value;
        }
        else
        {
            // 체력 추가
            CurHP += _value;

            // 최대체력 넘어가면 최대체력 고정
            if (MaxHP < CurHP)
                CurHP = MaxHP;

            DamageText.color = Color.green;
            DamageText.text = $"+{_value.ToString("N0")}";
        }

        // 체력바 조정
        HP_Image.fillAmount = CurHP / MaxHP;
        _hpBarvalue = HP_Image.fillAmount;

        if (CurHP <= 0)
        {
            CurHP = 0;
            animator.Play("Die");
        }
    }
    #endregion

    #region TakeBuff
    public void TakeBuff(BUFF_TYPE _buffType, float _buffRatio, ref float _buffValue)
    {
        // 방어력 버프 일 때
        if(_buffType == BUFF_TYPE.DEF)
        {
            // Debug.Log(character.Get_BaseDef * _buffRatio);
            // 기본 방어력의 _buffRatio%만큼 방어력 증가
            Def += character.Get_BaseDef * _buffRatio;
            
            _buffValue = character.Get_BaseDef * _buffRatio;
        }
        // 공격력 버프 일 때
        else if (_buffType == BUFF_TYPE.ATK || _buffType == BUFF_TYPE.ALL_BUFF)
        {
            // 기본 공격력의 _buffRatio%만큼 방어력 증가
            Atk += character.Get_BaseAtk * _buffRatio;

            _buffValue = character.Get_BaseAtk * _buffRatio;
        }
    }

    // 버프 제거
    public void EndBuff(BUFF_TYPE _buffType, float _buffValue)
    {
        // 버프 타입이 방어면 방어력 감소
        if (_buffType == BUFF_TYPE.DEF)
        {
            Def -= _buffValue;
        }
        // 공격력이면 공격력 감소
        else if (_buffType == BUFF_TYPE.ATK || _buffType == BUFF_TYPE.ALL_BUFF)
        {
            Atk -= _buffValue;
        }
    }
    #endregion
}
