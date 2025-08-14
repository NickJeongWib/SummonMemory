using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Ctrl : MonoBehaviour
{
    Character character;
    public Character Get_character { get => character; }

    [Header("HP")]
    float MaxHP;
    [SerializeField] Image HP_Image;
    [SerializeField] float CurHP;

    [SerializeField] Skill SkillData;
    [SerializeField] GameObject Skill_Prefab;
    public GameObject Get_Skill_Prefab { get => Skill_Prefab; }

    protected virtual void Update()
    {
        
    }

    protected virtual void Start()
    {
        // ��ų �������� ���ٸ�
        if (Skill_Prefab == null)
        {
            // Debug.Log(SkillData.Get_Skill_Name);
            // ��ų ����Ʈ ��������
            GameObject skill = Instantiate(SkillData.Get_Skill_Prefab);
            Skill_Prefab = skill;
            Skill_Prefab.SetActive(false);
        }
    }

    #region HP_Set
    // ü�� �ʱ�ȭ
    public virtual void Set_Init(float _hp, Skill _skillData, Character _char)
    {
        MaxHP = _hp;
        CurHP = MaxHP;

        SkillData = _skillData;
        character = _char;
    }
    #endregion
}
