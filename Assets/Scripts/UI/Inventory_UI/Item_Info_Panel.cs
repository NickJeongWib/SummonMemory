using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class Item_Info_Panel : MonoBehaviour
{
    #region Variable
    [SerializeField] Animator animator;
    [SerializeField] Inventory_UI Inventory_UI_Ref;
    [SerializeField] CharacterList_UI CharacterListUI_Ref;
    [SerializeField] Lobby_Manager LobbyMangerRef;

    [SerializeField] Image Item_Image;
    [SerializeField] Image Item_Back;

    // [SerializeField] Color[] Item_Grade_Colors;
    [SerializeField] Sprite[] Grade_Sprites;
    public Sprite[] Get_Grade_Sprites { get => Grade_Sprites; set => Grade_Sprites = value; }

    [Header("---Spend_Item---")]
    [SerializeField] GameObject Spend_Item_Root;
    public GameObject Get_Spend_Item_Root { get => Spend_Item_Root; }
    [SerializeField] Text Spend_Item_Name;
    [SerializeField] Text Spend_Item_Type;
    [SerializeField] Text Spend_Item_Ex;
    [SerializeField] Text Spend_Item_Des;

    [Header("---Equipment_Item---")]
    [SerializeField] Character_Equipment Character_Equipment_Ref;
    [SerializeField] Image Item_Grade;
    [SerializeField] GameObject Equipment_Item_Root;
    public GameObject Get_Equipment_Item_Root { get => Equipment_Item_Root; }
    [SerializeField] Text Equipment_Item_Name;
    [SerializeField] Text Equipment_Base_Option;
    // 캐릭터 랜덤 옵션
    [SerializeField] Text[] Equipment_Random_Options;

    [SerializeField] GameObject Decomposition_Btn;
    public GameObject Get_Decomposition_Btn { get => Decomposition_Btn; }
    [SerializeField] GameObject EquipBtn_Obj;
    [SerializeField] GameObject Change_Btn;
    [SerializeField] GameObject Equip_Btn;
    [SerializeField] GameObject UnEquip_Btn;

    [SerializeField] GameObject OwnCharRoot;
    [SerializeField] Image Character_Illust;
    [SerializeField] Text OwnChar_Text;
    [SerializeField] Image[] ItemOptionGradeImage;

    Item CurrentItem;
    public Item Get_CurrentItem { get => CurrentItem; set => CurrentItem = value; }

    [Header("---UpGrading---")]
    [SerializeField] GameObject ButtonRoot;
    [SerializeField] ItemUpGrade ItemUpGrade_Ref;
    [SerializeField] GameObject InventoryListRoot;
    [SerializeField] GameObject UpgradePanel;
    [SerializeField] Transition_Fade Transition;

    [Header("---Decomposition---")]
    [SerializeField] GameObject ItemDecom_Info_Panel;
    [SerializeField] GameObject DecomError_Info_Panel;
    [SerializeField] GameObject Decom_GetItem_Info_Panel;
    [SerializeField] Text Get_Decom_Amount_Text;
    #endregion

    // -------------

    public void On_Click_Close_ItemInfo()
    {
        SoundManager.Inst.PlayUISound();

        if (CurrentItem != null)
            CurrentItem = null;
        animator.Play("Pop_Down_Item_Info");
    }

    // 오브젝트 비활성화
    public void ActiveF_Panel()
    {
        this.gameObject.SetActive(false);
    }

    #region Upgrade_PopUP
    public void On_Click_UpgradePanel()
    {
        SoundManager.Inst.PlayUISound();

        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }

        if (InventoryListRoot.activeSelf)
        {
            InventoryListRoot.SetActive(false);
        }

        ItemUpGrade_Ref.Get_SelectItem = CurrentItem;

        // 장착아이템 강화단계가 9라면
        if (ItemUpGrade_Ref.Get_SelectItem.Get_Item_Lv >= 9)
        {
            ButtonRoot.SetActive(true);
        }
        else
        {
            ButtonRoot.SetActive(false);
        }

        ItemUpGrade_Ref.On_Click_UpgradeRoot();
        Transition.gameObject.SetActive(true);
        UpgradePanel.SetActive(true);
    }

    public void ButtonRoot_Active(bool _isOn)
    {
        ButtonRoot.SetActive(_isOn);
    }

    #endregion

    #region Iventory_PopUP
    // 아이템 정보 UI 초기화
    public void ItemInfo_Refresh(INVENTORY_TYPE _invenType, int _num)
    {
        if (_invenType == INVENTORY_TYPE.EQUIPMENT)
        {
            Equipment_Item_Root.SetActive(true);
            OwnCharRoot.SetActive(true);
            Spend_Item_Root.SetActive(false);
            EquipBtn_Obj.SetActive(false);
            Decomposition_Btn.SetActive(true);
            Refresh_Equipment(_num);

            // 선택한 아이템
            CurrentItem = UserInfo.Equip_Inventory[_num];
        }
        else if (_invenType == INVENTORY_TYPE.SPEND) // 소모 아이템
        {
            Spend_Item_Root.SetActive(true);
            EquipBtn_Obj.SetActive(false);
            Equipment_Item_Root.SetActive(false);
            OwnCharRoot.SetActive(false);
            Refresh_Spend(_num);
        }
        else // 강화아이템
        {
            Spend_Item_Root.SetActive(true);
            EquipBtn_Obj.SetActive(false);
            Equipment_Item_Root.SetActive(false);
            OwnCharRoot.SetActive(false);
            Refresh_Upgrade(_num);
        }
    }

    void Refresh_Upgrade(int _num)
    {
        Item_Back.gameObject.SetActive(false);

        Spend_Item_Name.text = $"{UserInfo.Upgrade_Inventory[_num].Get_Item_Name}";
        Spend_Item_Ex.text = $"{UserInfo.InventoryDict[UserInfo.Upgrade_Inventory[_num].Get_Item_Name].Get_Amount}개 보유";
        Spend_Item_Type.text = $"강화아이템";
        Item_Image.sprite = UserInfo.Upgrade_Inventory[_num].Get_Item_Image;

        UserInfo.Upgrade_Inventory[_num].Get_Item_Desc = UserInfo.Upgrade_Inventory[_num].Get_Item_Desc.Replace("\\n", "\n");
        UserInfo.Upgrade_Inventory[_num].Get_Item_Desc = UserInfo.Upgrade_Inventory[_num].Get_Item_Desc.Replace("\\", "");

        Spend_Item_Des.text = UserInfo.Upgrade_Inventory[_num].Get_Item_Desc;
    }

    void Refresh_Spend(int _num)
    {
        Item_Back.gameObject.SetActive(false);

        Spend_Item_Name.text = $"{UserInfo.Spend_Inventory[_num].Get_Item_Name}";
        Spend_Item_Ex.text = $"{UserInfo.InventoryDict[UserInfo.Spend_Inventory[_num].Get_Item_Name].Get_Amount}개 보유";
        Spend_Item_Type.text = $"소모아이템";
        Item_Image.sprite = UserInfo.Spend_Inventory[_num].Get_Item_Image;

        UserInfo.Spend_Inventory[_num].Get_Item_Desc = UserInfo.Spend_Inventory[_num].Get_Item_Desc.Replace("\\n", "\n");
        UserInfo.Spend_Inventory[_num].Get_Item_Desc = UserInfo.Spend_Inventory[_num].Get_Item_Desc.Replace("\\", "");

        Spend_Item_Des.text = UserInfo.Spend_Inventory[_num].Get_Item_Desc;
    }

    void Refresh_Equipment(int _num)
    {
        Equipment_Item_Name.text = $"{UserInfo.Equip_Inventory[_num].Get_Item_Name} +{UserInfo.Equip_Inventory[_num].Get_Item_Lv}";

        Item_Back.gameObject.SetActive(true);
        // 등급에 따른 UI 효과 차별
        Item_Back.color = Inventory_UI_Ref.Get_Colors[(int)UserInfo.Equip_Inventory[_num].Get_Equipment_Grade];
        Item_Grade.sprite = Grade_Sprites[(int)UserInfo.Equip_Inventory[_num].Get_Equipment_Grade];

        // 아이템 이미지 교체
        Item_Image.sprite = UserInfo.Equip_Inventory[_num].Get_Item_Image;

        // 아이템을 장착한 캐릭터가 존재한다면
        if (UserInfo.Equip_Inventory[_num].Get_OwnCharacter != null)
        {
            OwnCharRoot.SetActive(true);
            UserInfo.Get_Square_Image(Character_Illust, UserInfo.Equip_Inventory[_num].Get_OwnCharacter);
        }
        else
        {
            OwnCharRoot.SetActive(false);
        }

        Refresh_EquipmentOption_Refresh(UserInfo.Equip_Inventory[_num]);

        // UI Text 효과 텍스트 출력
        Base_Option(UserInfo.Equip_Inventory[_num].Get_Item_Atk, UserInfo.Equip_Inventory[_num].Get_Item_DEF,
            UserInfo.Equip_Inventory[_num].Get_Item_HP, UserInfo.Equip_Inventory[_num].Get_Item_CRI_RATE,
            UserInfo.Equip_Inventory[_num].Get_Item_CRI_DMG);
    }
    #endregion

    #region EquipItemInfo
    // 장비 정보 열람
    public void Open_Equip_Info(Item _item, bool _isChange = false)
    {
        SoundManager.Inst.PlayUISound();

        Equipment_Item_Root.SetActive(true);
        EquipBtn_Obj.SetActive(true);
        Spend_Item_Root.SetActive(false);
        // Chanage_Equipment(_isChange, _item);
        Chanage_Equipment(_item);
        Refresh_Equipment(_item);
    }

    // void Chanage_Equipment(bool _isChange, Item _item)
    void Chanage_Equipment(Item _item)
    {
        // 장착 캐릭터가 없다면
        if (_item.Get_OwnCharacter == null)
        {
            Change_Btn.SetActive(false);
            UnEquip_Btn.SetActive(false);
            Equip_Btn.SetActive(true);
        }
        else // 장착된 캐릭터가 있다면
        {
            Change_Btn.SetActive(true);
            UnEquip_Btn.SetActive(true);
            Equip_Btn.SetActive(false);
        }
    }

    void Refresh_Equipment(Item _item)
    {
        Equipment_Item_Name.text = $"{_item.Get_Item_Name} +{_item.Get_Item_Lv}";

        Item_Back.gameObject.SetActive(true);
        // 등급에 따른 UI 효과 차별
        Item_Back.color = Inventory_UI_Ref.Get_Colors[(int)_item.Get_Equipment_Grade];
        Item_Grade.sprite = Grade_Sprites[(int)_item.Get_Equipment_Grade];

        // 아이템 이미지 교체
        Item_Image.sprite = _item.Get_Item_Image;

        // 아이템을 장착한 캐릭터가 존재한다면
        if (_item.Get_OwnCharacter != null)
        {
            OwnCharRoot.SetActive(true);
            UserInfo.Get_Square_Image(Character_Illust, _item.Get_OwnCharacter);
        }
        else
        {
            OwnCharRoot.SetActive(false);
        }

        Refresh_EquipmentOption_Refresh(CurrentItem);

        for (int i = 0; i < CurrentItem.Get_EquipmentOption.Length; i++)
        {
            ItemOptionGradeImage[i].sprite = Grade_Sprites[(int)CurrentItem.Get_EquipmentOptionGrade[i]];
        }

        // UI Text 효과 텍스트 출력
        Base_Option(_item.Get_Item_Atk, _item.Get_Item_DEF,
            _item.Get_Item_HP, _item.Get_Item_CRI_RATE,
            _item.Get_Item_CRI_DMG);
    }
    #region ItemInfo_POP_UP_Text_Refresh
    void Refresh_EquipmentOption_Refresh(Item _item)
    {
        int OptionLv = 0;
        for (int i = 0; i < Equipment_Random_Options.Length; i++)
        {
            OptionLv += 3;
            if (_item.Get_EquipmentOption[i] == EQUIPMENT_OPTION.NONE)
            {
                Equipment_Random_Options[i].text = $"강화 {OptionLv} 달성 시 무작위 능력치 추가";
            }
            else if (_item.Get_EquipmentOption[i] == EQUIPMENT_OPTION.ATK_INT || _item.Get_EquipmentOption[i] == EQUIPMENT_OPTION.DEF_INT ||
                _item.Get_EquipmentOption[i] == EQUIPMENT_OPTION.HP_INT)
            {
                Equipment_Random_Options[i].text = $"{_item.Get_Option_KorString(_item.Get_EquipmentOption[i])} +{_item.Get_OptionValue[i].ToString("N0")}";
            }
            else
            {
                Equipment_Random_Options[i].text = $"{_item.Get_Option_KorString(_item.Get_EquipmentOption[i])} +{(_item.Get_OptionValue[i] * 100).ToString("N1")}%";
            }
        }
    }
    #endregion

    public void Set_ChangeBtn(bool _isOn)
    {
        SoundManager.Inst.PlayUISound();
        Change_Btn.SetActive(_isOn);
    }

    public void Set_EquipBtn(bool _isOn)
    {
        SoundManager.Inst.PlayUISound();
        Equip_Btn.SetActive(_isOn);
    }
    #endregion

    #region Text_Refresh
    void Base_Option(float _atk, float _def, float _hp, float _criR, float _criD)
    {
        // 텍스트 초기화
        Equipment_Base_Option.text = "";

        if (_atk != 0)
        {
            Equipment_Base_Option.text = $"공격력 +{_atk.ToString("N0")}\n";
        }
        
        if (_def != 0)
        {
            Equipment_Base_Option.text += $"방어력 +{_def.ToString("N0")}\n";
        }

        if (_hp != 0)
        {
            Equipment_Base_Option.text += $"체력 +{_hp.ToString("N0")}\n";
        }

        if (_criR != 0)
        {
            Equipment_Base_Option.text += $"치명확률 +{(_criR * 100.0f).ToString("N1")}%\n";
        }

        if (_criD != 0)
        {
            Equipment_Base_Option.text += $"치명피해 +{(_criD * 100.0f).ToString("N1")}%\n";
        }
    }
    #endregion

    #region Item_Equip_UnEquip
    public void On_Click_EquipBtn()
    {
        SoundManager.Inst.PlayUISound();

        if (CurrentItem == null)
            return;

        // 로딩창 켜주기
        DataNetwork_Mgr.Inst.LoadingPanel.gameObject.SetActive(true);

        Inventory_UI_Ref.EquipSlots.SetActive(false);
        CurrentItem.Get_isEquip = true;

        if (CurrentItem.Get_OwnCharacter != null)
        {
            // 선택된 아이템을 장착하고 있는 캐릭터의 능력치를 뺴줌
            CurrentItem.EquipOption_Stat_Calc(false);
            CurrentItem.Get_OwnCharacter.Refresh_Char_Equipment_State(false, CurrentItem.Get_EquipType);
            // SlotItemInfo.Get_OwnCharacter.Get_EquipItems[(int)SlotItemInfo.Get_EquipType] = null;
        }

        // 선택된 캐릭터가 있을 시
        if (GameManager.Inst.Get_SelectChar != null)
        {
            // 선택된 캐릭터의 현재 선택 아이템타입의 장비를 착용하고 있다면
            if (GameManager.Inst.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType] != null)
            {
                GameManager.Inst.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType].Get_isEquip = false;
                GameManager.Inst.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType].EquipOption_Stat_Calc(false);
                // 장착해제
                GameManager.Inst.Get_SelectChar.Refresh_Char_Equipment_State(false, CurrentItem.Get_EquipType);

                // 선택된 캐릭터에서 해제
                // GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)SlotItemInfo.Get_EquipType].Get_OwnCharacter = null;
            }

            // 장착 캐릭터 등록
            GameManager.Inst.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType] = CurrentItem;
            GameManager.Inst.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType].Get_isEquip = true;
            CurrentItem.Get_OwnCharacter = GameManager.Inst.Get_SelectChar;
            CurrentItem.Set_EquipCharName = CurrentItem.Get_OwnCharacter.Get_CharName;

            // 장비 옵션값 전달 
            CurrentItem.EquipOption_Stat_Calc(true);
            // 능력치 전달
            GameManager.Inst.Get_SelectChar.Refresh_Char_Equipment_State(true, CurrentItem.Get_EquipType);
        }

        GameManager.Inst.Get_SelectChar.TestState();

        // 장비타입에 맞는 장비가 하나도 없을 때 다른 장비타입의 정보가 남아있는걸 방지
        for (int i = 0; i < Inventory_UI_Ref.Get_EquipSlot_List.Count; i++)
        {
            Inventory_UI_Ref.Get_EquipSlot_List[i].Get_SlotItemInfo = null;
        }

        if (CharacterListUI_Ref != null)
            CharacterListUI_Ref.Refresh_EquipItem_Image();

        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CHARLIST);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.EQUIP_ITEM_INVENTORY);

        Refresh_Equipment_Slot();

        LobbyMangerRef.Refresh_User_CombatPower();
        On_Click_Close_ItemInfo();
        CurrentItem = null;
    }

    public void On_Click_UnEquipBtn()
    {
        SoundManager.Inst.PlayUISound();

        if (CurrentItem.Get_OwnCharacter == null)
            return;

        // 로딩창 켜주기
        DataNetwork_Mgr.Inst.LoadingPanel.gameObject.SetActive(true);

        // GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType] = null;
        CurrentItem.EquipOption_Stat_Calc(false);

        if (CurrentItem.Get_OwnCharacter.Get_EquipItems[(int)CurrentItem.Get_EquipType] != null)
        {
            CurrentItem.Get_OwnCharacter.Refresh_Char_Equipment_State(false, CurrentItem.Get_EquipType);
        }

        Character_Equipment_Ref.Refresh_List_UI((int)CurrentItem.Get_EquipType);

        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.EQUIP_ITEM_INVENTORY);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CHARLIST);

        On_Click_Close_ItemInfo();

        CharacterListUI_Ref.Refresh_EquipItem_Image();

        Refresh_Equipment_Slot();

        // 해제라는 행동을 했으니 선택된 장비를 없애준다
        CurrentItem = null;
        LobbyMangerRef.Refresh_User_CombatPower();
        // GameManager.Instance.Get_SelectChar.TestState();

    }
    #endregion

    #region Upgrade_Scene_First_Enter
    public void UpgradeScene_Enter_Init()
    {
        ItemUpGrade_Ref.On_Click_UpgradeRoot();
    }
    #endregion

    #region Item_Decomposition
    // 아이템 분해 안내 문구
    public void On_Click_Item_Decomposition()
    {
        SoundManager.Inst.PlayUISound();
        ItemDecom_Info_Panel.SetActive(true);
    }

    int Crystal_Amount;
    int Powder_Amount;
    // 아이템 분해 확인
    public void On_Click_Decomposition_Select()
    {
        SoundManager.Inst.PlayUISound();

        if (CurrentItem.Get_OwnCharacter != null)
        {
            DecomError_Info_Panel.SetActive(true);
            ItemDecom_Info_Panel.SetActive(false);
            return;
        }
        // 로딩창 켜주기
        DataNetwork_Mgr.Inst.LoadingPanel.gameObject.SetActive(true);


        Crystal_Amount = (CurrentItem.Get_Item_Lv + 1);
        Powder_Amount = 2 * (CurrentItem.Get_Item_Lv + 1);

        if (CurrentItem.Get_EquipType == EQUIP_TYPE.WEAPON)
        {
            UserInfo.Weapon_Equipment.Remove(CurrentItem);
            #region Test
            for (int i = 0; i < UserInfo.Weapon_Equipment.Count; i++)
            {
                Debug.Log($"WEAP {i} : {UserInfo.Weapon_Equipment[i].Get_Item_Name}");
            }
            #endregion
        }
        else if (CurrentItem.Get_EquipType == EQUIP_TYPE.UPPER)
        {
            UserInfo.Upper_Equipment.Remove(CurrentItem);
            #region Test
            for (int i = 0; i < UserInfo.Upper_Equipment.Count; i++)
            {
                Debug.Log($"UPPET {i} : {UserInfo.Upper_Equipment[i].Get_Item_Name}");
            }
            #endregion
        }
        else if (CurrentItem.Get_EquipType == EQUIP_TYPE.HELMET)
        {
            UserInfo.Helmet_Equipment.Remove(CurrentItem);
            #region Test
            for (int i = 0; i < UserInfo.Helmet_Equipment.Count; i++)
            {
                Debug.Log($"HELMET {i} : {UserInfo.Helmet_Equipment[i].Get_Item_Name}");
            }
            #endregion
        }
        else if (CurrentItem.Get_EquipType == EQUIP_TYPE.ACCESSORY)
        {
            UserInfo.Accessory_Equipment.Remove(CurrentItem);
            #region Test
            for (int i = 0; i < UserInfo.Accessory_Equipment.Count; i++)
            {
                Debug.Log($"ACCE {i} : {UserInfo.Accessory_Equipment[i].Get_Item_Name}");
            }
            #endregion
        }
        else
        {
            UserInfo.Glove_Equipment.Remove(CurrentItem);
            #region Test
            for (int i = 0; i < UserInfo.Glove_Equipment.Count; i++)
            {
                Debug.Log($"GLOVE {i} : {UserInfo.Glove_Equipment[i].Get_Item_Name}");
            }
            #endregion
        }

        UserInfo.Equip_Inventory.Remove(CurrentItem);
        CurrentItem = null;

        ItemDecom_Info_Panel.SetActive(false);
        this.gameObject.SetActive(false);

        // 분해보상 획득
        Calc_Get_Decom_Item();
        // UI 분해 보상 열기
        Open_Get_DecomItem();
        // 인벤토리 초기화
        Refresh_Equipment_Slot();

        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.EQUIP_ITEM_INVENTORY);

        #region Test
        //for (int i = 0; i < UserInfo.Equip_Inventory.Count; i++)
        //{
        //    Debug.Log($"{i} EQUIP : {UserInfo.Equip_Inventory[i].Get_Item_Name}");
        //}
        #endregion
    }

    // 분해 취소
    public void Close_Decomposition()
    {
        if (CurrentItem != null)
            CurrentItem = null;
    }

    void Open_Get_DecomItem()
    {
        Decom_GetItem_Info_Panel.SetActive(true);
        Get_Decom_Amount_Text.text = $"";

        // 분해보상을 받았으니 0개로 초기화
        Crystal_Amount = 0;
        Powder_Amount = 0;
    }

    void Calc_Get_Decom_Item()
    {
        for (int i = 0; i < Item_List.Upgrade_Item_List.Count; i++)
        {
            // 보상 획득
            if (Item_List.Upgrade_Item_List[i].Get_Item_Name == "재련 수정")
            {
                UserInfo.Add_Inventory_Item(Item_List.Upgrade_Item_List[i], Crystal_Amount);
            }
            else if (Item_List.Upgrade_Item_List[i].Get_Item_Name == "재련 가루")
            {
                UserInfo.Add_Inventory_Item(Item_List.Upgrade_Item_List[i], Powder_Amount);
            }
        }

        Inventory_UI_Ref.Upgrade_Slot_Refresh();

        #region Test
        //for (int i = 0; i < UserInfo.Upgrade_Inventory.Count; i++)
        //{
        //    Debug.Log($"{UserInfo.Upgrade_Inventory[i].Get_Item_Name} " + UserInfo.Upgrade_Inventory[i].Get_Amount);
        //}
        #endregion
    }

    #region UI_Refresh - Overlap_Code
    public void Refresh_Equipment_Slot()
    {
        int count = 0;

        for (int i = 0; i < UserInfo.Equip_Inventory.Count; i++)
        {
            count = i;
            Inventory_UI_Ref.Get_EquipmentSlot_List[i].Set_Image(UserInfo.Equip_Inventory[i].Get_Item_Image, UserInfo.Equip_Inventory[i].Get_Equipment_Grade, UserInfo.Equip_Inventory[i]);
        }

        // 위에 구문에서 아이템이 삭제되고 카운트가 0이되면 이미지가 계속 남아 있기에 제거하기 위한 IF문
        if (UserInfo.Equip_Inventory.Count <= 0)
        {
            Inventory_UI_Ref.Get_EquipmentSlot_List[0].Off_Image();
        }

        for (int i = count + 1; i < Inventory_UI_Ref.Get_EquipmentSlot_List.Count; i++)
        {
            Inventory_UI_Ref.Get_EquipmentSlot_List[i].Off_Image();
        }
    }
    #endregion
    #endregion
}

