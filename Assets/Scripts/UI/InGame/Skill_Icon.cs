using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Icon : MonoBehaviour
{
    // ĳ���Ͱ� ��ų�� ����� �� �ִ��� �˱� ����
    [SerializeField] Character_Ctrl InGame_Char;
    // UI �̹���
    [SerializeField] Image Characet_Icon;
    [SerializeField] Image Skill_On_Frame;

    // ��ų ������ UI �ʱ�ȭ �� ĳ���� ��Ʈ�ѷ� ���۰� �Բ� �ʱ�ȭ
    public void Set_Character_UI(Sprite _sprite, Material _mat, Character_Ctrl _ctrl)
    {
        InGame_Char = _ctrl;
        Characet_Icon.sprite = _sprite;
        Skill_On_Frame.material = _mat;
    }
}
