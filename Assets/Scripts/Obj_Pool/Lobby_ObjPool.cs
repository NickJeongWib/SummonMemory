using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_ObjPool : MonoBehaviour
{
    [SerializeField] Inventory_UI Inventory_UI_Ref;

    [Header("Inventory_Equipment_Slot")]
    [SerializeField] GameObject Equipment_Slot_Prefab;
    [SerializeField] Transform Equipment_SlotTr;

    [Header("Inventory_Spend_Slot")]
    [SerializeField] GameObject Spend_Slot_Prefab;
    [SerializeField] Transform Spend_SlotTr;

    // Start is called before the first frame update
    void Start()
    {
        // 인벤토리 장비슬롯 오브젝트 풀 생성
        for (int i = 0; i < Inventory_UI_Ref.Get_EquipmentCount; i++)
        {
            GameObject equipSlot = Instantiate(Equipment_Slot_Prefab);

            equipSlot.transform.localPosition = Vector3.zero;
            equipSlot.transform.localScale = Vector3.one;

            equipSlot.transform.SetParent(Equipment_SlotTr, false);
            equipSlot.GetComponent<Equipment_Slot>().SlotNum = i;
            equipSlot.GetComponent<Equipment_Slot>().Get_Inventory_UI_Ref = Inventory_UI_Ref;
            Inventory_UI_Ref.Get_EquipmentSlot_List.Add(equipSlot.GetComponent<Equipment_Slot>());
        }

        // 인벤토리 소비슬롯 오브젝트 풀 생성
        for (int i = 0; i < Inventory_UI_Ref.Get_SpendSlotCount; i++)
        {
            GameObject spendSlot = Instantiate(Spend_Slot_Prefab);

            spendSlot.transform.localPosition = Vector3.zero;
            spendSlot.transform.localScale = Vector3.one;

            spendSlot.transform.SetParent(Spend_SlotTr, false);
            spendSlot.GetComponent<Spend_Slot>().SlotNum = i;
            Inventory_UI_Ref.Get_SpendSlot_List.Add(spendSlot.GetComponent<Spend_Slot>());
        }
    }
}
