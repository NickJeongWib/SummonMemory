using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class Skill
{
    // 스킬 이름
    [SerializeField] string Skill_Name;
    public string Get_Skill_Name { get => Skill_Name; }

    // 스킬 레벨
    [SerializeField] int Skill_Lv;
    public int Get_Skill_Lv { get => Skill_Lv; set => Skill_Lv = value; }

    // 스킬 속성
    [SerializeField] SKILL_TYPE SkillType;
    public SKILL_TYPE Get_SkillType { get => SkillType; }

    // 사용 SP
    [SerializeField] int SP;
    public int Get_SP { get => SP; }

    // 타켓팅 수
    [SerializeField] int TargetCount;
    public int Get_TargetCount { get => TargetCount; }

    // 데미지 비율
    [SerializeField] float Damage_Ratio;
    public float Get_Damage_Ratio { get => Damage_Ratio; }

    // 디버프
    [SerializeField] float DeBuff_Ratio;
    public float Get_DeBuff_Ratio { get => DeBuff_Ratio; }
    [SerializeField] DEBUFF_TYPE DeBuffType;
    public DEBUFF_TYPE Get_DeBuffType { get => DeBuffType; }

    // 버프
    [SerializeField] float Buff_Ratio;
    public float Get_Buff_Ratio { get => Buff_Ratio; }
    [SerializeField] BUFF_TYPE BuffType;
    public BUFF_TYPE Get_BuffType { get => BuffType; }

    // 스킬포인트 회복 카운트
    [SerializeField] float SP_Hill_Count; 
    public float Get_SP_Hill_Count { get => SP_Hill_Count; }
    // 버프 유지 턴
    [SerializeField] int Buff_Time;
    public int Get_Buff_Time { get => Buff_Time; }

    // 스킬 설명
    [SerializeField] string Skill_Desc;
    public string Get_Skill_Desc { get => Skill_Desc; }

    // 스킬 UI
    Sprite Skill_Icon_Sprite;
    public Sprite Get_Skill_Icon { get => Skill_Icon_Sprite; }

    [SerializeField] string SkillIcon_Path;
    public string Get_SkillIcon_Path { get => SkillIcon_Path; }

    [SerializeField] string SFX_Path;
    public string Get_SFX_Path { get => SFX_Path; }

    // 스킬 프리펩 주소 & 프리펩
    GameObject Skill_Prefab;
    public GameObject Get_Skill_Prefab { get => Skill_Prefab; }
    [SerializeField] string Skill_Prefab_Path;
    public string Get_Skill_Prefab_Path { get => Skill_Prefab_Path; }


    // -------------기본 공격---------------
    [SerializeField] float NormalAtk_Ratio;
    public float Get_NormalAtk_Ratio { get => NormalAtk_Ratio; }
    [SerializeField] string NormalAtk_Desc;
    public string Get_NormalAtk_Desc { get => NormalAtk_Desc; }
    [SerializeField] int NormalAtk_Lv;
    public int Get_NormalAtk_Lv { get => NormalAtk_Lv; }

    #region Constructor
    public Skill(string _skillName, int _lv, SKILL_TYPE _skillType, int _sp, int _targetCount, float _damageRatio, float _debuffRatio, DEBUFF_TYPE _debuffType, float _buffRatio, BUFF_TYPE _buffType,
        int _spHillCount, int _buffTime, string _skillDesc, string _iconPath, string _prefabPath, float _normalAtkRatio, string _normalAtkDesc, string _sfxPath)
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

        SFX_Path = _sfxPath;

        Skill_Init(_skillDesc, _iconPath, _prefabPath);

        // ------------- 기본 공격 변수 초기화 -------------
        NormalAtk_Ratio = _normalAtkRatio;
        NormalAtk_Desc = _normalAtkDesc;
        NormalAtk_Lv = 0;
    }

    public void Skill_Init(string _Desc, string _iconPath, string _prefabPath)
    {
        Skill_Icon_Sprite = Resources.Load<Sprite>(_iconPath);
        Skill_Prefab = Resources.Load<GameObject>(_prefabPath);
        Skill_Desc = _Desc;
    }

    public void Resource_Path_Init()
    {
        Skill_Icon_Sprite = Resources.Load<Sprite>(SkillIcon_Path);
        Skill_Prefab = Resources.Load<GameObject>(Skill_Prefab_Path);
    }
    #endregion

    #region Skill_Desc
    // 현재 레벨 스킬 설명
    public string Skill_Desc_Trans(string _desc, float _damage = 0, float _hill = 0, float _sp = 0, float _buff = 0, float _deBuff = 0)
    {
        string Replace_Desc = _desc
            .Replace("{_damage}", (_damage * 100).ToString("N0"))
            .Replace("{_hill}", (_hill * 100).ToString("N0"))
            .Replace("{_sp}", (_sp).ToString("N0"))
            .Replace("{_buff}", (_buff * 100).ToString("N0"))
            .Replace("{_deBuff}", (_deBuff * 100).ToString("N0"));

        // 줄바꿈 기호로 바꾸기
        Replace_Desc = Replace_Desc.Replace("\\n", "\n");
        Replace_Desc = Replace_Desc.Replace("\\", "");

        return Replace_Desc;
    }

    // 다음 레벨 스킬 설명
    public string NextSkill_Desc_Trans(string _desc, float _damage = 0, float _hill = 0, float _sp = 0, float _buff = 0, float _deBuff = 0)
    {
        string Replace_Desc = _desc
            .Replace("{_damage}", "<color=orange>" + ((_damage + 0.05f) * 100).ToString("N0") + "</color>")
            .Replace("{_hill}", "<color=orange>" + ((_hill + 0.01f) * 100).ToString("N0") + "</color>")
            .Replace("{_sp}", "<color=orange>" + (_sp + 0.2f).ToString("N0") + "</color>")
            .Replace("{_buff}", "<color=orange>" + ((_buff + 0.01f) * 100).ToString("N0") + "</color>")
            .Replace("{_deBuff}", "<color=orange>" + ((_deBuff + 0.01f) * 100).ToString("N0") + "</color>");

        // 줄바꿈 기호로 바꾸기
        Replace_Desc = Replace_Desc.Replace("\\n", "\n");
        Replace_Desc = Replace_Desc.Replace("\\", "");

        return Replace_Desc;
    }

    #endregion

    #region Increase_Ratio
    // 스킬 레벨업 시 증가 비율
    public void Increase_Ratio(float _damage = 0, float _sp = 0, float _buff = 0, float _deBuff = 0)
    {
        if (Skill_Lv == 20)
            return;

        Skill_Lv++;

        Damage_Ratio += _damage;
        SP_Hill_Count += _sp;
        Buff_Ratio += _buff;
        DeBuff_Ratio += _deBuff;
    }

    public void Increase_NormalRatio(float _damage = 0)
    {
        if (NormalAtk_Lv == 20)
            return;

        NormalAtk_Lv++;
        NormalAtk_Ratio += _damage;
    }
    #endregion
}
