using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class Skill
{
    // ��ų �̸�
    [SerializeField] string Skill_Name;
    public string Get_Skill_Name { get => Skill_Name; }

    // ��ų ����
    [SerializeField] int Skill_Lv;
    public int Get_Skill_Lv { get => Skill_Lv; set => Skill_Lv = value; }

    // ��ų �Ӽ�
    [SerializeField] SKILL_TYPE SkillType;
    public SKILL_TYPE Get_SkillType { get => SkillType; }

    // ��� SP
    [SerializeField] int SP;
    public int Get_SP { get => SP; }

    // Ÿ���� ��
    [SerializeField] int TargetCount;
    public int Get_TargetCount { get => TargetCount; }

    // ������ ����
    [SerializeField] float Damage_Ratio;
    public float Get_Damage_Ratio { get => Damage_Ratio; }

    // �����
    [SerializeField] float DeBuff_Ratio;
    public float Get_DeBuff_Ratio { get => DeBuff_Ratio; }
    [SerializeField] DEBUFF_TYPE DeBuffType;
    public DEBUFF_TYPE Get_DeBuffType { get => DeBuffType; }

    // ����
    [SerializeField] float Buff_Ratio;
    public float Get_Buff_Ratio { get => Buff_Ratio; }
    [SerializeField] BUFF_TYPE BuffType;
    public BUFF_TYPE Get_BuffType { get => BuffType; }

    // ��ų����Ʈ ȸ�� ī��Ʈ
    [SerializeField] float SP_Hill_Count; 
    public float Get_SP_Hill_Count { get => SP_Hill_Count; }
    // ���� ���� ��
    [SerializeField] int Buff_Time;
    public int Get_Buff_Time { get => Buff_Time; }

    // ��ų ����
    [SerializeField] string Skill_Desc;
    public string Get_Skill_Desc { get => Skill_Desc; }

    // ��ų UI
    Sprite Skill_Icon_Sprite;
    public Sprite Get_Skill_Icon { get => Skill_Icon_Sprite; }

    [SerializeField] string SkillIcon_Path;
    public string Get_SkillIcon_Path { get => SkillIcon_Path; }

    [SerializeField] string SFX_Path;
    public string Get_SFX_Path { get => SFX_Path; }

    // ��ų ������ �ּ� & ������
    GameObject Skill_Prefab;
    public GameObject Get_Skill_Prefab { get => Skill_Prefab; }
    [SerializeField] string Skill_Prefab_Path;
    public string Get_Skill_Prefab_Path { get => Skill_Prefab_Path; }


    // -------------�⺻ ����---------------
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
        // ��ų����
        Skill_Name = _skillName;
        Skill_Lv = _lv;
        SkillType = _skillType;
        SP = _sp;
        TargetCount = _targetCount;
        Damage_Ratio = _damageRatio;

        //�����
        DeBuff_Ratio = _debuffRatio;
        DeBuffType = _debuffType;

        // ����
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

        // ------------- �⺻ ���� ���� �ʱ�ȭ -------------
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
    // ���� ���� ��ų ����
    public string Skill_Desc_Trans(string _desc, float _damage = 0, float _hill = 0, float _sp = 0, float _buff = 0, float _deBuff = 0)
    {
        string Replace_Desc = _desc
            .Replace("{_damage}", (_damage * 100).ToString("N0"))
            .Replace("{_hill}", (_hill * 100).ToString("N0"))
            .Replace("{_sp}", (_sp).ToString("N0"))
            .Replace("{_buff}", (_buff * 100).ToString("N0"))
            .Replace("{_deBuff}", (_deBuff * 100).ToString("N0"));

        // �ٹٲ� ��ȣ�� �ٲٱ�
        Replace_Desc = Replace_Desc.Replace("\\n", "\n");
        Replace_Desc = Replace_Desc.Replace("\\", "");

        return Replace_Desc;
    }

    // ���� ���� ��ų ����
    public string NextSkill_Desc_Trans(string _desc, float _damage = 0, float _hill = 0, float _sp = 0, float _buff = 0, float _deBuff = 0)
    {
        string Replace_Desc = _desc
            .Replace("{_damage}", "<color=orange>" + ((_damage + 0.05f) * 100).ToString("N0") + "</color>")
            .Replace("{_hill}", "<color=orange>" + ((_hill + 0.01f) * 100).ToString("N0") + "</color>")
            .Replace("{_sp}", "<color=orange>" + (_sp + 0.2f).ToString("N0") + "</color>")
            .Replace("{_buff}", "<color=orange>" + ((_buff + 0.01f) * 100).ToString("N0") + "</color>")
            .Replace("{_deBuff}", "<color=orange>" + ((_deBuff + 0.01f) * 100).ToString("N0") + "</color>");

        // �ٹٲ� ��ȣ�� �ٲٱ�
        Replace_Desc = Replace_Desc.Replace("\\n", "\n");
        Replace_Desc = Replace_Desc.Replace("\\", "");

        return Replace_Desc;
    }

    #endregion

    #region Increase_Ratio
    // ��ų ������ �� ���� ����
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
