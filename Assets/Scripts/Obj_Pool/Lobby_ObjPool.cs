using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_ObjPool : MonoBehaviour
{
    [SerializeField] Quest_UI QuestUI_Ref;
    [SerializeField] CharacterList_UI CharacterListUI_Ref;
    [SerializeField] Lobby_Manager LobbyManagerRef;
    [SerializeField] Store_Manager StoreManagerRef;
    [SerializeField] Inventory_UI Inventory_UI_Ref;
    [SerializeField] Dictionary_Ctrl DictionaryCtrl_Ref;

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

    [Header("---Quest_Slot---")]
    [SerializeField] GameObject QuestSlot_Prefab;
    [SerializeField] Transform QuestSlot_Tr;

    [Header("---Dictionary_Slot---")]
    [SerializeField] GameObject DictSlot_Prefab;
    [SerializeField] Transform R_Dict_Tr;
    [SerializeField] Transform SR_Dict_Tr;
    [SerializeField] Transform SSR_Dict_Tr;

    [Header("---Profile_Slot---")]
    [SerializeField] GameObject Profile_Prefab;
    [SerializeField] Transform Profile_Tr;

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
            spendSlot.GetComponent<Spend_Slot>().Set_Inventory_UI = Inventory_UI_Ref;
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
            upgradeSlot.GetComponent<Upgrade_Slot>().Set_Inventory_UI = Inventory_UI_Ref;
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

        #region Dictionary_Slot
        // R 슬롯
        for (int i = 0; i < Character_List.R_Char.Count; i++)
        {
            GameObject dictSlot = Instantiate(DictSlot_Prefab);
            dictSlot.transform.SetParent(R_Dict_Tr, false);
            dictSlot.GetComponent<Dict_Slot>().Set_DictionaryCtrl_Ref = this.DictionaryCtrl_Ref;

            // 캐릭터가 이미 존재하는지 여부에 따라 슬롯 초기화
            if (UserInfo.UserCharDict.ContainsKey(Character_List.R_Char[i].Get_CharName))
            {
                dictSlot.GetComponent<Dict_Slot>().Set_UI_Refresh(true, Character_List.R_Char[i]);
            }
            else
            {
                dictSlot.GetComponent<Dict_Slot>().Set_UI_Refresh(false, Character_List.R_Char[i]);
            }

            DictionaryCtrl_Ref.R_Slot.Add(dictSlot.GetComponent<Dict_Slot>());
        }

        for (int i = 0; i < Character_List.SR_Char.Count; i++)
        {
            GameObject dictSlot = Instantiate(DictSlot_Prefab);
            dictSlot.transform.SetParent(SR_Dict_Tr, false);
            dictSlot.GetComponent<Dict_Slot>().Set_DictionaryCtrl_Ref = this.DictionaryCtrl_Ref;

            // 캐릭터가 이미 존재하는지 여부에 따라 슬롯 초기화
            if (UserInfo.UserCharDict.ContainsKey(Character_List.SR_Char[i].Get_CharName))
            {
                dictSlot.GetComponent<Dict_Slot>().Set_UI_Refresh(true, Character_List.SR_Char[i]);
            }
            else
            {
                dictSlot.GetComponent<Dict_Slot>().Set_UI_Refresh(false, Character_List.SR_Char[i]);
            }

            DictionaryCtrl_Ref.SR_Slot.Add(dictSlot.GetComponent<Dict_Slot>());
        }

        for (int i = 0; i < Character_List.SSR_Char.Count; i++)
        {
            GameObject dictSlot = Instantiate(DictSlot_Prefab);
            dictSlot.transform.SetParent(SSR_Dict_Tr, false);
            dictSlot.GetComponent<Dict_Slot>().Set_DictionaryCtrl_Ref = this.DictionaryCtrl_Ref;

            // 캐릭터가 이미 존재하는지 여부에 따라 슬롯 초기화
            if (UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[i].Get_CharName))
            {
                dictSlot.GetComponent<Dict_Slot>().Set_UI_Refresh(true, Character_List.SSR_Char[i]);
            }
            else
            {
                dictSlot.GetComponent<Dict_Slot>().Set_UI_Refresh(false, Character_List.SSR_Char[i]);
            }

            DictionaryCtrl_Ref.SSR_Slot.Add(dictSlot.GetComponent<Dict_Slot>());
        }
        #endregion

        #region Quest_Slot
        // 퀘스트 슬롯 생성
        for (int i = 0; i < Quest_List.QuestList.Count; i++)
        {
            GameObject questSlot = Instantiate(QuestSlot_Prefab);

            questSlot.transform.SetParent(QuestSlot_Tr, false);
            questSlot.GetComponent<Quest_Slot>().Set_QuestUI_Ref = QuestUI_Ref;
            questSlot.GetComponent<Quest_Slot>().Set_UI(Quest_List.QuestList[i].Get_RewardType, Quest_List.QuestList[i].Get_Reward_Img,
                Quest_List.QuestList[i].Get_QuestTitle, Quest_List.QuestList[i].Get_QuestDesc, Quest_List.QuestList[i].Get_RewardAmount);
        }
        #endregion

        #region Profile_Slot
        for (int i = 0; i < Character_List.R_Char.Count; i++)
        {
            GameObject profileSlot = Instantiate(Profile_Prefab);
            profileSlot.transform.SetParent(Profile_Tr, false);
            profileSlot.GetComponent<Profile_Slot>().Set_LobbyMgr(LobbyManagerRef);
            profileSlot.GetComponent<Profile_Slot>().Set_Change_Char_Profile(Character_List.R_Char[i].Get_CharName,
            Character_List.R_Char[i].Get_Lobby_Sprite(), Character_List.R_Char[i].Get_Profile_Sprite(), Character_List.R_Char[i].Get_Icon_Img);
            LobbyManagerRef.UserInfo_ProfileList.Add(profileSlot.GetComponent<Profile_Slot>());
        }
        for (int i = 0; i < Character_List.SR_Char.Count; i++)
        {
            GameObject profileSlot = Instantiate(Profile_Prefab);
            profileSlot.transform.SetParent(Profile_Tr, false);
            profileSlot.GetComponent<Profile_Slot>().Set_LobbyMgr(LobbyManagerRef);
            profileSlot.GetComponent<Profile_Slot>().Set_Change_Char_Profile(Character_List.SR_Char[i].Get_CharName,
            Character_List.SR_Char[i].Get_Lobby_Sprite(), Character_List.SR_Char[i].Get_Profile_Sprite(), Character_List.SR_Char[i].Get_Icon_Img);
            LobbyManagerRef.UserInfo_ProfileList.Add(profileSlot.GetComponent<Profile_Slot>());
        }
        for (int i = 0; i < Character_List.SSR_Char.Count; i++)
        {
            GameObject profileSlot = Instantiate(Profile_Prefab);
            profileSlot.transform.SetParent(Profile_Tr, false);
            profileSlot.GetComponent<Profile_Slot>().Set_LobbyMgr(LobbyManagerRef);
            profileSlot.GetComponent<Profile_Slot>().Set_Change_Char_Profile(Character_List.SSR_Char[i].Get_CharName,
            Character_List.SSR_Char[i].Get_Lobby_Sprite(), Character_List.SSR_Char[i].Get_Profile_Sprite(), Character_List.SSR_Char[i].Get_Icon_Img);
            LobbyManagerRef.UserInfo_ProfileList.Add(profileSlot.GetComponent<Profile_Slot>());
        }
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
