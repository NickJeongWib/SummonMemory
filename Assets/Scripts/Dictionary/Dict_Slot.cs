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

    public void Set_UI_Refresh(bool _isExist, Character _character = null)
    {
        LockImage.SetActive(!_isExist);

        if (Slot_Char == null)
        {
            Slot_Char = _character;
            // CharImage.sprite = Slot_Char.Get_Normal_Img;
            CharImage.sprite = Slot_Char.Get_Icon_Img;
        }

        Select_Btn.interactable = _isExist;

        if (_isExist)
        {
            CharImage.color = Color.white;
        }
        else
        {
            CharImage.color = LockColor;
        }
    }

    public void On_Click_Slot()
    {
        DictionaryCtrl_Ref.Show_Char_Info(Slot_Char);
    }
}
