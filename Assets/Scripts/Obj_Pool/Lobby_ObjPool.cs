using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_ObjPool : MonoBehaviour
{
    [SerializeField] CharacterList_UI CharacterListUI_Ref;
    [SerializeField] Lobby_Manager LobbyManagerRef;
    [SerializeField] Store_Manager StoreManagerRef;
    [SerializeField] Inventory_UI Inventory_UI_Ref;

    [Header("Inventory_Equipment_Slot")]
    [SerializeField] GameObject Equipment_Slot_Prefab;
    [SerializeField] Transform Equipment_SlotTr;

    [Header("Inventory_Spend_Slot")]
    [SerializeField] GameObject Spend_Slot_Prefab;
    [SerializeField] Transform Spend_SlotTr;

    [Header("---Store_Item_Slot---")]
    [SerializeField] GameObject StoreSlot_Prefab;
    [SerializeField] Transform CurrencyTr;
    [SerializeField] GameObject TicketSlot_Prefab;
    [SerializeField] Transform TicketTr;

    [Header("---EquimentSlot---")]
    [SerializeField] GameObject EquipmentSlot_Prefab;
    [SerializeField] Transform EquipmentSlotTr;

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

            GameObject equipSlot = Instantiate(EquipmentSlot_Prefab);
            equipSlot.transform.SetParent(EquipmentSlotTr, false);
            equipSlot.GetComponent<EquipSlot>().SlotNum = i;
            equipSlot.GetComponent<EquipSlot>().Set_InventoryUI_Ref = Inventory_UI_Ref;
            equipSlot.GetComponent<EquipSlot>().Set_CharacterListUI_Ref = CharacterListUI_Ref;
            Inventory_UI_Ref.Get_EquipSlot_List.Add(equipSlot.GetComponent<EquipSlot>());
        }

        // 상점 다이아 슬롯 오브젝트 풀 생성
        for (int i = 0; i < Store_List.CurrencyList.Count; i++)
        {
            GameObject currencySlot = Instantiate(StoreSlot_Prefab);
            currencySlot.GetComponent<Store_Slot_Init>().StoreManager_Ref = this.StoreManagerRef;
            currencySlot.GetComponent<Store_Slot_Init>().StoreItemInfo = Store_List.CurrencyList[i];
            currencySlot.GetComponent<Store_Slot_Init>().Init_UI();
            currencySlot.transform.SetParent(CurrencyTr, false);
        }

        // 상점 티켓 슬롯 오브젝트 풀 생성
        for (int i = 0; i < Store_List.TicketList.Count; i++)
        {
            GameObject ticketSlot = Instantiate(TicketSlot_Prefab);
            ticketSlot.GetComponent<Store_Slot_Init>().StoreManager_Ref = this.StoreManagerRef;
            ticketSlot.GetComponent<Store_Slot_Init>().StoreItemInfo = Store_List.TicketList[i];
            ticketSlot.GetComponent<Store_Slot_Init>().Init_UI();
            ticketSlot.transform.SetParent(TicketTr, false);
        }
        // for (int i = 0;)
    }
}
