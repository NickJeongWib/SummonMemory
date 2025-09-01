using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dict_Slot : MonoBehaviour
{
    Character Slot_Char;
    public Character Get_Slot_Char { get => Slot_Char; }
    [SerializeField] Button Select_Btn;
    [SerializeField] Image CharImage;
    [SerializeField] GameObject LockImage;
    [SerializeField] Color LockColor;
    Dictionary_Ctrl DictionaryCtrl_Ref;
    public Dictionary_Ctrl Set_DictionaryCtrl_Ref { set => DictionaryCtrl_Ref = value; }

    // UI이미지 초기화
    public void Set_UI_Refresh(bool _isExist, Character _character = null)
    {
        // Lock 이미지 설정
        LockImage.SetActive(!_isExist);

        // 캐릭터가 없다면
        if (Slot_Char == null)
        {
            // Slot_Char의 캐릭터는 매개변수로 받은 _character가 된다.
            Slot_Char = _character;
            // 슬롯 아이콘은 매개변수로 받은 캐릭터의 아이콘 이미지
            CharImage.sprite = Slot_Char.Get_Icon_Img;
        }

        // 선택할 수 있는 버튼의 상호작용 토글을 매개변수로 받은 _isExist로 적용
        Select_Btn.interactable = _isExist;

        // 존재 한다면
        if (_isExist)
        {
            // 슬롯의 이미지를 원본 색상
            CharImage.color = Color.white;
        }
        else
        {
            // 슬롯의 이미지를 어둡게
            CharImage.color = LockColor;
        }
    }

    // 슬롯을 누를 시
    public void On_Click_Slot()
    {
        SoundManager.Inst.PlayUISound();

        GameManager.Inst.SelectCharacter = Slot_Char;

        DictionaryCtrl_Ref.Show_Char_Info(Slot_Char);
    }
}
