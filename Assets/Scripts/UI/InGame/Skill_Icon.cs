using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Icon : MonoBehaviour
{
    // 캐릭터가 스킬을 사용할 수 있는지 알기 위해
    [SerializeField] Character_Ctrl InGame_Char;
    // UI 이미지
    [SerializeField] Image Characet_Icon;
    [SerializeField] Image Skill_On_Frame;

    // 스킬 아이콘 UI 초기화 및 캐릭터 컨트롤러 시작과 함께 초기화
    public void Set_Character_UI(Sprite _sprite, Material _mat, Character_Ctrl _ctrl)
    {
        InGame_Char = _ctrl;
        Characet_Icon.sprite = _sprite;
        Skill_On_Frame.material = _mat;
    }
}
