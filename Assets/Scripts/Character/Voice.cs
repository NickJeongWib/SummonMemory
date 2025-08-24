using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Voice
{
    // 캐릭터 선택 시 목소리
    [SerializeField] string SelectVoice_Path;
    public string Get_SelectVoice_Path { get => SelectVoice_Path; }

    // 스킬 사용 시 목소리
    [SerializeField] string UseSkillVoice_Path;
    public string Get_UseSkillVoice_Path { get => UseSkillVoice_Path; }

    // 초기화
    public Voice(string _selectPath, string _useSkillPath)
    {
        SelectVoice_Path = _selectPath;
        UseSkillVoice_Path = _useSkillPath;
    }
}
