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

    [Header("Inventory_Upgrade_Slot")]
    [SerializeField] GameObject Upgrad_Slot_Prefab;
    [SerializeField] Transform Upgrad_SlotTr;

    [Header("---EquimentSlot---")]
    [SerializeField] GameObject EquipmentSlot_Prefab;
    [SerializeField] Transform EquipmentSlotTr;

    [Header("---Store_Item_Slot---")]
    [SerializeField] GameObject StoreSlot_Prefab;
    [SerializeField] Transform CurrencyTr;
    [SerializeField] GameObject TicketSlot_Prefab;
    [SerializeField] Transform TicketTr;
    [SerializeField] GameObject R_Book_Prefab;
    [SerializeField] Transform R_BookTr;
    [SerializeField] GameObject SR_Book_Prefab;
    [SerializeField] Transform SR_BookTr;
    [SerializeField] GameObject SSR_Book_Prefab;
    [SerializeField] Transform SSR_BookTr;

    // Start is called before the first frame update
    void Start()
    {
        #region Inventory_Slot
        // 인벤토리 장비 슬롯 팝업 오브젝트 풀 생성
        for (int i = 0; i < Inventory_UI_Ref.Get_EquipmentCount; i++)
        {
            GameObject equipSlot = Instantiate(Equipment_Slot_Prefab);

            equipSlot.transform.SetParent(Equipment_SlotTr, false);
            equipSlot.GetComponent<Equipment_Slot>().SlotNum = i;
            equipSlot.GetComponent<Equipment_Slot>().Get_Inventory_UI_Ref = Inventory_UI_Ref;
            Inventory_UI_Ref.Get_EquipmentSlot_List.Add(equipSlot.GetComponent<Equipment_Slot>());
        }

        // 인벤토리 소비슬롯 오브젝트 풀 생성
        for (int i = 0; i < Inventory_UI_Ref.Get_SpendSlotCount; i++)
        {
            GameObject spendSlot = Instantiate(Spend_Slot_Prefab);

            spendSlot.transform.SetParent(Spend_SlotTr, false);
            spendSlot.GetComponent<Spend_Slot>().SlotNum = i;
            Inventory_UI_Ref.Get_SpendSlot_List.Add(spendSlot.GetComponent<Spend_Slot>());
        }
        // 인벤토리 장비 슬롯 오브젝트 풀 생성
        for (int i = 0; i < Inventory_UI_Ref.Get_EquipmentCount; i++)
        {
            GameObject equipSlot = Instantiate(EquipmentSlot_Prefab);
            equipSlot.transform.SetParent(EquipmentSlotTr, false);
            equipSlot.GetComponent<EquipSlot>().SlotNum = i;
            equipSlot.GetComponent<EquipSlot>().Set_InventoryUI_Ref = Inventory_UI_Ref;
            equipSlot.GetComponent<EquipSlot>().Set_CharacterListUI_Ref = CharacterListUI_Ref;
            Inventory_UI_Ref.Get_EquipSlot_List.Add(equipSlot.GetComponent<EquipSlot>());
        }

        for (int i = 0; i < Inventory_UI_Ref.Get_UpgradeSlotCount; i++)
        {
            GameObject upgradeSlot = Instantiate(Upgrad_Slot_Prefab);

            upgradeSlot.transform.SetParent(Upgrad_SlotTr, false);
            upgradeSlot.GetComponent<Upgrade_Slot>().SlotNum = i;
            Inventory_UI_Ref.Get_UpgradeSlot_List.Add(upgradeSlot.GetComponent<Upgrade_Slot>());
        }
        #endregion

        #region Store_Slot
        // 상점 다이아 슬롯 오브젝트 풀 생성
        Create_Slot(StoreSlot_Prefab, Store_List.CurrencyList, CurrencyTr);

        // 상점 티켓 슬롯 오브젝트 풀 생성
        Create_Slot(TicketSlot_Prefab, Store_List.TicketList, TicketTr);

        // 상점 R_Book 슬롯 오브젝트 풀 생성
        Create_Slot(R_Book_Prefab, Store_List.R_BookList, R_BookTr);

        // 상점  SR_Book  슬롯 오브젝트 풀 생성
        Create_Slot(SR_Book_Prefab, Store_List.SR_BookList, SR_BookTr);

        // 상점 SR_Book 슬롯 오브젝트 풀 생성
        Create_Slot(SSR_Book_Prefab, Store_List.SSR_BookList, SSR_BookTr);
        #endregion
    }

    // 상점 (하, 중, 고)소환서 창 슬롯 생성하기 위한 함수
    void Create_Slot(GameObject _prefab, List<Store_Item> _list, Transform _parent)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            GameObject slot = Instantiate(_prefab);
            slot.GetComponent<Store_Slot_Init>().StoreManager_Ref = this.StoreManagerRef;
            slot.GetComponent<Store_Slot_Init>().StoreItemInfo = _list[i];
            slot.GetComponent<Store_Slot_Init>().Init_UI();
            slot.transform.SetParent(_parent, false);
        }
    }
}
