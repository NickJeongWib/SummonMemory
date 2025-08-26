using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class Equipment_Slot : MonoBehaviour
{
    public int SlotNum;

    [SerializeField] Inventory_UI Inventory_UI_Ref;
    public Inventory_UI Get_Inventory_UI_Ref { get => Inventory_UI_Ref; set => Inventory_UI_Ref = value; }
    [SerializeField] Button Open_Item_Info;
    [SerializeField] Mask Slot_Mask;
    [SerializeField] Image Item_Image;

    [SerializeField] Image Grade_Back_Image;
    [SerializeField] Mask Grade_Back_Mask;
    [SerializeField] Image Owner_Image;
    [SerializeField] GameObject OwnerObject;

    // �̹��� ���o
    public void Set_Image(Sprite _sprite, EQUIPMENT_GRADE _equipGrade, Item _item)
    {
        Item_Image.sprite = _sprite;
        Slot_Mask.showMaskGraphic = true;

        Grade_Back_Image.color = Inventory_UI_Ref.Get_Colors[(int)_equipGrade];
        Grade_Back_Mask.showMaskGraphic = true;

        if(_item.Get_OwnCharacter != null)
        {
            OwnerObject.SetActive(true);
            Owner_Image.sprite = _item.Get_OwnCharacter.Get_Icon_Img;
        }
        else
        {
            OwnerObject.SetActive(false);
        }
    }

    // �̹��� ���x
    public void Off_Image()
    {
        Slot_Mask.showMaskGraphic = false;
        Grade_Back_Mask.showMaskGraphic = false;
    }

    private void Start()
    {
        // ��ư ȣ�� ���
        Open_Item_Info.onClick.AddListener(Item_Info);
    }

    void Item_Info()
    {
        SoundManager.Inst.PlayUISound();

        Inventory_UI_Ref.On_Click_Open_ItemInfo(SlotNum);
    }
        
}
