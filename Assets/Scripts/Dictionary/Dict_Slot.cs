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

    // UI�̹��� �ʱ�ȭ
    public void Set_UI_Refresh(bool _isExist, Character _character = null)
    {
        // Lock �̹��� ����
        LockImage.SetActive(!_isExist);

        // ĳ���Ͱ� ���ٸ�
        if (Slot_Char == null)
        {
            // Slot_Char�� ĳ���ʹ� �Ű������� ���� _character�� �ȴ�.
            Slot_Char = _character;
            // ���� �������� �Ű������� ���� ĳ������ ������ �̹���
            CharImage.sprite = Slot_Char.Get_Icon_Img;
        }

        // ������ �� �ִ� ��ư�� ��ȣ�ۿ� ����� �Ű������� ���� _isExist�� ����
        Select_Btn.interactable = _isExist;

        // ���� �Ѵٸ�
        if (_isExist)
        {
            // ������ �̹����� ���� ����
            CharImage.color = Color.white;
        }
        else
        {
            // ������ �̹����� ��Ӱ�
            CharImage.color = LockColor;
        }
    }

    // ������ ���� ��
    public void On_Click_Slot()
    {
        SoundManager.Inst.PlayUISound();

        GameManager.Inst.SelectCharacter = Slot_Char;

        DictionaryCtrl_Ref.Show_Char_Info(Slot_Char);
    }
}
