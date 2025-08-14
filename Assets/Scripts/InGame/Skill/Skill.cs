using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class Skill
{
    [SerializeField] string Skill_Name;
    public string Get_Skill_Name { get => Skill_Name; }
    [SerializeField] int Skill_Lv;
    public int Get_Skill_Lv { get => Skill_Lv; set => Skill_Lv = value; }
    [SerializeField] SKILL_TYPE SkillType;
    [SerializeField] int SP;
    [SerializeField] int TargetCount;
    [SerializeField] float Damage_Ratio;

    // 디버프
    [SerializeField] float DeBuff_Ratio;
    [SerializeField] DEBUFF_TYPE DeBuffType;

    // 버프
    [SerializeField] float Buff_Ratio;
    [SerializeField] BUFF_TYPE BuffType;

    [SerializeField] int SP_Hill_Count; // 스킬포인트 회복 카운트
    [SerializeField] int Buff_Time;     // 버프 유지 턴

    [SerializeField] string Skill_Desc;

    // 스킬 UI
    public Sprite Skill_Icon_Sprite;
    public Sprite Get_Skill_Icon { get => Skill_Icon_Sprite; }

    [SerializeField] string SkillIcon_Path;

    // 스킬 프리펩 주소 & 프리펩
    public GameObject Skill_Prefab;
    public GameObject Get_Skill_Prefab { get => Skill_Prefab; }
    [SerializeField] string Skill_Prefab_Path;

    #region Constructor
    public Skill(string _skillName, int _lv, SKILL_TYPE _skillType, int _sp, int _targetCount, float _damageRatio, float _debuffRatio, DEBUFF_TYPE _debuffType, float _buffRatio, BUFF_TYPE _buffType,
        int _spHillCount, int _buffTime, string _skillDesc, string _iconPath, string _prefabPath)
    {
        // 스킬정보
        Skill_Name = _skillName;
        Skill_Lv = _lv;
        SkillType = _skillType;
        SP = _sp;
        TargetCount = _targetCount;
        Damage_Ratio = _damageRatio;

        //디버프
        DeBuff_Ratio = _debuffRatio;
        DeBuffType = _debuffType;

        // 버프
        Buff_Ratio = _buffRatio;
        BuffType = _buffType;
        SP_Hill_Count = _spHillCount;

        Buff_Time = _buffTime;

        // UI
        Skill_Desc = _skillDesc;
        SkillIcon_Path = _iconPath;

        // Prefab
        Skill_Prefab_Path = _prefabPath;

        Skill_Init(_skillDesc, _iconPath, _prefabPath);
    }

    public void Skill_Init(string _Desc, string _iconPath, string _prefabPath)
    {
        Skill_Icon_Sprite = Resources.Load<Sprite>(_iconPath);
        Skill_Prefab = Resources.Load<GameObject>(_prefabPath);
        Skill_Desc = _Desc;
    }

    #endregion
}
