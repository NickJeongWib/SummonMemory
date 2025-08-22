using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;


public class Enemy_Ctrl : MonoBehaviour
{
    public string MonName;

    [Header("HP")]
    float MaxHP;
    [SerializeField] Image HP_Image;
    [SerializeField] float CurHP;
    public float Get_CurHP { get => CurHP; }

    [Header("EnemyStat")]
    MONSTER_ELE Mon_Ele;
    public MONSTER_ELE Get_MonEle { get => Mon_Ele; }
    [SerializeField] float Atk;
    public float Get_Atk { get => Atk; }
    [SerializeField] float Def;
    public float Get_Def { get => Def; }
    [SerializeField] int TargetCount;
    public int Get_TargetCount { get => TargetCount; }
    [SerializeField] float Skill_Ratio;
    public float Get_Skill_Ratio { get => Skill_Ratio; }


    [Header("Skill")]
    [SerializeField] GameObject Skill_Prefab;
    public GameObject Get_Skill_Prefab { get => Skill_Prefab; }

    [Header("Animator")]
    [SerializeField] Animator animator;
    [SerializeField] Animator Damage_Animator;

    [Header("UI")]
    [SerializeField] Sprite Icon;
    public Sprite Get_Icon { get => Icon; }
    [SerializeField] Text DamageText;


    // 스탯 초기화
    public void Set_Stat(string _name, float _hp, float _atk, float _def, float _value, int _targetNum, float _skillRatio, MONSTER_ELE _ele, string _skillPath,
        Transform _skillTr)
    {
        MonName = _name;

        // value : 스테이지에 따른 스탯 상승치
        // 스테이지가 올라갈 때 마다 스탯 상승시키기 위한 계산
        Atk = _atk + (_atk * _value);
        Def = _def + (_def * _value);

        MaxHP = _hp + (_hp * _value);

        Mon_Ele = _ele;
        CurHP = MaxHP;

        TargetCount = _targetNum;
        Skill_Ratio = _skillRatio;

        // 스킬 동적 생성
        GameObject skill = Resources.Load<GameObject>(_skillPath);
       
        Skill_Prefab = Instantiate(skill);
        Skill_Prefab.transform.SetParent(_skillTr);
        Skill_Prefab.SetActive(false);
    }

    public void TakeDamage(float _damage, ref float _value, bool _isCrit, Color _color)
    {
        // 크리티컬 데미지 적용 시
        if(_isCrit)
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
        DamageText.text = $"-{_damage.ToString("N0")}";

        // 체력 제거
        CurHP -= _damage;
        
        // 체력바 조정
        HP_Image.fillAmount = CurHP / MaxHP;
        _value = HP_Image.fillAmount;

        if (CurHP <= 0)
        {
            CurHP = 0;
            animator.Play("Die");
        }
    }

    public void Skill_Use()
    {
        Skill_Prefab.SetActive(true);
    }
}
