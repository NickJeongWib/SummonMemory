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

    // 이미지 출력o
    public void Set_Image(Sprite _sprite, EQUIPMENT_GRADE _equipGrade)
    {
        Item_Image.sprite = _sprite;
        Slot_Mask.showMaskGraphic = true;

        Grade_Back_Image.color = Inventory_UI_Ref.Get_Colors[(int)_equipGrade];
        Grade_Back_Mask.showMaskGraphic = true;
    }

    // 이미지 출력x
    public void Off_Image()
    {
        Slot_Mask.showMaskGraphic = false;
        Grade_Back_Mask.showMaskGraphic = false;
    }

    private void Start()
    {
        // 버튼 호출 대기
        Open_Item_Info.onClick.AddListener(Item_Info);
    }

    void Item_Info()
    {
        Inventory_UI_Ref.On_Click_Open_ItemInfo(SlotNum);
    }
        
}
