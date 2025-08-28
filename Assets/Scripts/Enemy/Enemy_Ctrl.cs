using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;


public class Enemy_Ctrl : MonoBehaviour
{
    public Monster_DB monster;

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
    public float Get_Def { get => Def; set => Def = value; }
    [SerializeField] int TargetCount;
    public int Get_TargetCount { get => TargetCount; }
    [SerializeField] float Skill_Ratio;
    public float Get_Skill_Ratio { get => Skill_Ratio; }
    DEBUFF_TYPE DebuffType;
    public DEBUFF_TYPE Set_DebuffType { set => DebuffType = value; }
    bool isCC;
    public bool Get_isCC;
    public List<string> DebuffSkill_List = new List<string>();
    public List<BuffIcon_UI> DeBuffIconList = new List<BuffIcon_UI>();

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



    // ���� �ʱ�ȭ
    public void Set_Stat(string _name, float _hp, float _atk, float _def, float _value, int _targetNum, float _skillRatio, MONSTER_ELE _ele, string _skillPath,
        Transform _skillTr, Monster_DB _monsterDB)
    {
        monster = _monsterDB;
        MonName = _name;

        // value : ���������� ���� ���� ���ġ
        // ���������� �ö� �� ���� ���� ��½�Ű�� ���� ���
        Atk = _atk + (_atk * _value);
        Def = _def + (_def * _value);

        MaxHP = _hp + (_hp * _value);

        Mon_Ele = _ele;
        CurHP = MaxHP;

        TargetCount = _targetNum;
        Skill_Ratio = _skillRatio;

        // ��ų ���� ����
        GameObject skill = Resources.Load<GameObject>(_skillPath);
       
        Skill_Prefab = Instantiate(skill);
        Skill_Prefab.transform.SetParent(_skillTr);
        Skill_Prefab.SetActive(false);
    }

    public void TakeDamage(float _damage, ref float _value, bool _isCrit, Color _color)
    {
        // ũ��Ƽ�� ������ ���� ��
        if(_isCrit)
        {
            Damage_Animator.gameObject.SetActive(true);
            Damage_Animator.Play("CriDamage");
        }
        else // �Ϲ� ������ ���� ��
        {
            Damage_Animator.gameObject.SetActive(true);
            Damage_Animator.Play("Damage");
        }

        DamageText.color = _color;
        DamageText.text = $"-{_damage.ToString("N0")}";

        // ü�� ����
        CurHP -= _damage;
        
        // ü�¹� ����
        HP_Image.fillAmount = CurHP / MaxHP;
        _value = HP_Image.fillAmount;

        if (CurHP <= 0)
        {
            CurHP = 0;
            animator.Play("Die");
        }
    }

    public void EndDeBuff(DEBUFF_TYPE _deBuffType, float _buffValue, string _skillName, BuffIcon_UI _buffIcon)
    {
        // ���� Ÿ���� ���� ���� ����
        if (_deBuffType == DEBUFF_TYPE.DEF)
        {
            Def += _buffValue;
        }
        // ���ݷ��̸� ���ݷ� ����
        else if (_deBuffType == DEBUFF_TYPE.STUNNED)
        {
            Get_isCC = false;
        }

        DebuffSkill_List.Remove(_skillName);


        for (int i = 0; i < DeBuffIconList.Count; i++)
        {
            if(DeBuffIconList[i] == _buffIcon)
            {
                DeBuffIconList.Remove(_buffIcon);
                break;
            }
        }
    }

    public void Skill_Use()
    {
        Skill_Prefab.SetActive(true);
    }
}
