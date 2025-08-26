using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster_DB
{
    #region Var
    string Monster_Name;
    public string Get_Monster_Name { get => Monster_Name; }

    MONSTER_ELE MonsterEle;
    public MONSTER_ELE Get_MonsterEle { get => MonsterEle; }

    float MaxHP;
    public float Get_MaxHP { get => MaxHP; }

    float Mon_ATK;
    public float Get_Mon_ATK { get => Mon_ATK; }

    float Mon_DEF;
    public float Get_Mon_DEF { get => Mon_DEF; }

    int TargetCount;
    public int Get_TargetCount { get => TargetCount; }

    float Skill_Ratio;
    public float Get_Skill_Ratio { get => Skill_Ratio; }

    // 몬스터 프리펩
    string MonPrefab_Path;
    public string Get_MonPrefab_Path { get => MonPrefab_Path; }
    // 몬스터 스킬 프리펩
    string MonSkill_Path;
    public string Get_MonSkill_Path { get => MonSkill_Path; }
    // 몬스터 아이콘 주소
    string MonIcon_Path;
    public string Get_MonIcon_Path { get => MonIcon_Path; }
    // 몬스터 아이콘 스프라이트
    Sprite MonIcon_Sprite;
    public Sprite Get_MonIcon_Sprite { get => MonIcon_Sprite; }
    // 몬스터 일러스트 주소
    string MonIllust_Path;
    public string Get_MonIllust_Path { get => MonIllust_Path; }
    // 몬스트 일러스트 스프라이트
    Sprite MonIllust_Sprite;
    public Sprite Get_MonIllust_Sprite { get => MonIllust_Sprite; }

    // Skill_Sound
    // 스킬 사운드 주소
    string SFX_Path; 
    public string Get_SFX_Path { get => SFX_Path; }
    #endregion


    #region Constructor
    public Monster_DB(string _name, MONSTER_ELE _ele, float _hp, float _atk, float _def, int _targetNum, float _skillRatio,
        string _prefabPath, string _skillPath, string _iconPath, string _illustPath, string _sfxPath)
    {
        Monster_Name = _name;
        MonsterEle = _ele;
        MaxHP = _hp;
        Mon_ATK = _atk;
        Mon_DEF = _def;

        TargetCount = _targetNum;
        Skill_Ratio = _skillRatio;

        MonPrefab_Path = _prefabPath;
        MonSkill_Path = _skillPath;
        MonIcon_Path = _iconPath;
        MonIllust_Path = _illustPath;

        SFX_Path = _sfxPath;

        Resource_Load();
    }

    void Resource_Load()
    {
        // 이미지 리소스 로드
        MonIcon_Sprite = Resources.Load<Sprite>(MonIcon_Path);
        MonIllust_Sprite = Resources.Load<Sprite>(MonIllust_Path);
    }
    #endregion
}
