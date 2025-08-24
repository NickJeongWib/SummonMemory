using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Voice
{
    // ĳ���� ���� �� ��Ҹ�
    [SerializeField] string SelectVoice_Path;
    public string Get_SelectVoice_Path { get => SelectVoice_Path; }

    // ��ų ��� �� ��Ҹ�
    [SerializeField] string UseSkillVoice_Path;
    public string Get_UseSkillVoice_Path { get => UseSkillVoice_Path; }

    // �ʱ�ȭ
    public Voice(string _selectPath, string _useSkillPath)
    {
        SelectVoice_Path = _selectPath;
        UseSkillVoice_Path = _useSkillPath;
    }
}
