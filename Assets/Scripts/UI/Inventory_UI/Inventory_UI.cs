
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    // 소비, 장비 인벤토리 게임오브젝트
    [SerializeField] GameObject Spend_Inventory_Slot;
    [SerializeField] GameObject Equip_Inventory_Slot;

    [Header("Spend_Item_UI")]
    [SerializeField] GameObject Spend_Btn_PopUp;
    [SerializeField] GameObject Spend_Select_Shine_BG;

    [Header("Equip_Item_UI")]
    [SerializeField] GameObject Equip_Btn_PopUp;
    [SerializeField] GameObject Equip_Select_Shine_BG;

    public void On_Click_Spend_Item_Btn()
    {
        // Spend_Inventory_Slot 비활성화 상태에 연결이 잘 되어있다면
        if (Spend_Inventory_Slot.activeSelf == false && Spend_Inventory_Slot != null)
        {
            Spend_Inventory_Slot.SetActive(true);
            Equip_Inventory_Slot.SetActive(false);
        }

        // Spend_Btn_PopUp 비활성화 상태에 연결이 잘 되어있다면
        if (Spend_Btn_PopUp.activeSelf == false && Spend_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(true);
            Equip_Btn_PopUp.SetActive(false);
        }

        // Spend_Select_Shine_BG 비활성화 상태에 연결이 잘 되어있다면
        if (Spend_Select_Shine_BG.activeSelf == false && Spend_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(true);
            Equip_Select_Shine_BG.SetActive(false);
        }
    }

    public void On_Click_Equip_Item_Btn()
    {
        // Spend_Inventory_Slot 비활성화 상태에 연결이 잘 되어있다면
        if (Equip_Inventory_Slot.activeSelf == false && Equip_Inventory_Slot != null)
        {
            Spend_Inventory_Slot.SetActive(false);
            Equip_Inventory_Slot.SetActive(true);
        }

        // Spend_Btn_PopUp 비활성화 상태에 연결이 잘 되어있다면
        if (Equip_Btn_PopUp.activeSelf == false && Equip_Btn_PopUp != null)
        {
            Spend_Btn_PopUp.SetActive(false);
            Equip_Btn_PopUp.SetActive(true);
        }

        // Spend_Select_Shine_BG 비활성화 상태에 연결이 잘 되어있다면
        if (Equip_Select_Shine_BG.activeSelf == false && Equip_Select_Shine_BG != null)
        {
            Spend_Select_Shine_BG.SetActive(false);
            Equip_Select_Shine_BG.SetActive(true);
        }
    }

}
