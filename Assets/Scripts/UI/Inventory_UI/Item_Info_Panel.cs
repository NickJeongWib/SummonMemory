using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class Item_Info_Panel : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Inventory_UI Inventory_UI_Ref;
    [SerializeField] CharacterList_UI CharacterListUI_Ref;

    [SerializeField] Image Item_Image;
    [SerializeField] Image Item_Back;

    // [SerializeField] Color[] Item_Grade_Colors;
    [SerializeField] Sprite[] Grade_Sprites;

    [Header("---Spend_Item---")]
    [SerializeField] GameObject Spend_Item_Root;
    public GameObject Get_Spend_Item_Root { get => Spend_Item_Root; }
    [SerializeField] Text Spend_Item_Name;
    [SerializeField] Text Spend_Item_Type;
    [SerializeField] Text Spend_Item_Ex;
    [SerializeField] Text Spend_Item_Des;

    [Header("---Equipment_Item---")]
    [SerializeField] Image Item_Grade;
    [SerializeField] GameObject Equipment_Item_Root;
    public GameObject Get_Equipment_Item_Root { get => Equipment_Item_Root; }
    [SerializeField] Text Equipment_Item_Name;
    [SerializeField] Text Equipment_Base_Option;

    [SerializeField] Text Equipment_Random_Option_1;
    [SerializeField] Text Equipment_Random_Option_2;
    [SerializeField] Text Equipment_Random_Option_3;

    [SerializeField] GameObject EquipBtn_Obj;
    [SerializeField] GameObject Change_Btn;
    [SerializeField] GameObject Equip_Btn;
    [SerializeField] GameObject UnEquip_Btn;

    [SerializeField] GameObject OwnCharRoot;
    [SerializeField] Image Character_Illust;
    [SerializeField] Text OwnChar_Text;

    Item CurrentItem;
    public Item Get_CurrentItem { get => CurrentItem; set => CurrentItem = value; }

    public void On_Click_Close_ItemInfo()
    {
        if (CurrentItem != null)
            CurrentItem = null;
        animator.Play("Pop_Down_Item_Info");
    }

    // 오브젝트 비활성화
    public void ActiveF_Panel()
    {
        this.gameObject.SetActive(false);
    }

    #region Iventory_PopUP
    // 아이템 정보 UI 초기화
    public void ItemInfo_Refresh(INVENTORY_TYPE _invenType, int _num)
    {
        if (_invenType == INVENTORY_TYPE.EQUIPMENT)
        {
            Equipment_Item_Root.SetActive(true);
            Spend_Item_Root.SetActive(false);
            EquipBtn_Obj.SetActive(false);
            Refresh_Equipment(_num);
        }
        else if (_invenType == INVENTORY_TYPE.SPEND)
        {
            Equipment_Item_Root.SetActive(false);
            Spend_Item_Root.SetActive(true);

            Refresh_Spend();
        }
        else
        {

        }
    }

    void Refresh_Spend()
    {
        Item_Back.gameObject.SetActive(false);
    }

    void Refresh_Equipment(int _num)
    {
        Equipment_Item_Name.text = UserInfo.Equip_Inventory[_num].Get_Item_Name;

        /*
        Debug.Log($"{UserInfo.Equip_Inventory[_num].Get_Item_Atk}, {UserInfo.Equip_Inventory[_num].Get_Item_DEF}," +
            $"{UserInfo.Equip_Inventory[_num].Get_Item_HP}, {UserInfo.Equip_Inventory[_num].Get_Item_CRI_RATE}," +
            $"{UserInfo.Equip_Inventory[_num].Get_Item_CRI_DMG}");
        */
        

        Item_Back.gameObject.SetActive(true);
        // 등급에 따른 UI 효과 차별
        Item_Back.color = Inventory_UI_Ref.Get_Colors[(int)UserInfo.Equip_Inventory[_num].Get_Equipment_Grade];
        Item_Grade.sprite = Grade_Sprites[(int)UserInfo.Equip_Inventory[_num].Get_Equipment_Grade];

        // 아이템 이미지 교체
        Item_Image.sprite = UserInfo.Equip_Inventory[_num].Get_Item_Image;

        // UI Text 효과 텍스트 출력
        Base_Option(UserInfo.Equip_Inventory[_num].Get_Item_Atk, UserInfo.Equip_Inventory[_num].Get_Item_DEF,
            UserInfo.Equip_Inventory[_num].Get_Item_HP, UserInfo.Equip_Inventory[_num].Get_Item_CRI_RATE,
            UserInfo.Equip_Inventory[_num].Get_Item_CRI_DMG);
    }
    #endregion

    #region EquipItemInfo
    public void Open_Equip_Info(Item _item, bool _isChange = false)
    {
        Equipment_Item_Root.SetActive(true);
        EquipBtn_Obj.SetActive(true);
        Spend_Item_Root.SetActive(false);
        Chanage_Equipment(_isChange);

        Refresh_Equipment(_item);
    }

    void Chanage_Equipment(bool _isChange)
    {
        Change_Btn.SetActive(_isChange);
        UnEquip_Btn.SetActive(_isChange);
        Equip_Btn.SetActive(!_isChange);
    }

    void Refresh_Equipment(Item _item)
    {
        Equipment_Item_Name.text = _item.Get_Item_Name;

        Item_Back.gameObject.SetActive(true);
        // 등급에 따른 UI 효과 차별
        Item_Back.color = Inventory_UI_Ref.Get_Colors[(int)_item.Get_Equipment_Grade];
        Item_Grade.sprite = Grade_Sprites[(int)_item.Get_Equipment_Grade];

        // 아이템 이미지 교체
        Item_Image.sprite = _item.Get_Item_Image;

        // UI Text 효과 텍스트 출력
        Base_Option(_item.Get_Item_Atk, _item.Get_Item_DEF,
            _item.Get_Item_HP, _item.Get_Item_CRI_RATE,
            _item.Get_Item_CRI_DMG);
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
            Equipment_Base_Option.text += $"크리티컬 확률 +{(_criR * 100.0f).ToString("N1")}%\n";
        }

        if (_criD != 0)
        {
            Equipment_Base_Option.text += $"크리티컬 데미지 +{(_criD * 100.0f).ToString("N1")}%\n";
        }
    }
    #endregion

    #region Item_Equip_UnEquip
    public void On_Click_EquipBtn()
    {
        if (CurrentItem == null)
            return;

        Inventory_UI_Ref.EquipSlots.SetActive(false);
        CurrentItem.Get_isEquip = true;

        if (CurrentItem.Get_OwnCharacter != null)
        {
            CurrentItem.Get_OwnCharacter.Refresh_Char_Equipment_State(false, CurrentItem.Get_EquipType);
            // SlotItemInfo.Get_OwnCharacter.Get_EquipItems[(int)SlotItemInfo.Get_EquipType] = null;
        }

        // 선택된 캐릭터가 있을 시
        if (GameManager.Instance.Get_SelectChar != null)
        {
            // 선택된 캐릭터의 현재 선택 아이템타입의 장비를 착용하고 있다면
            if (GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType] != null)
            {
                // 장착해제
                GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType].Get_isEquip = false;
                GameManager.Instance.Get_SelectChar.Refresh_Char_Equipment_State(false, CurrentItem.Get_EquipType);
                // 선택된 캐릭터에서 해제
                // GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)SlotItemInfo.Get_EquipType].Get_OwnCharacter = null;
            }

            // 장착 캐릭터 등록
            GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType] = CurrentItem;
            CurrentItem.Get_OwnCharacter = GameManager.Instance.Get_SelectChar;

            // 능력치 전달
            GameManager.Instance.Get_SelectChar.Refresh_Char_Equipment_State(true, CurrentItem.Get_EquipType);
        }

        GameManager.Instance.Get_SelectChar.TestState();

        // 장비타입에 맞는 장비가 하나도 없을 때 다른 장비타입의 정보가 남아있는걸 방지
        for (int i = 0; i < Inventory_UI_Ref.Get_EquipSlot_List.Count; i++)
        {
            Inventory_UI_Ref.Get_EquipSlot_List[i].Get_SlotItemInfo = null;
        }

        if (CharacterListUI_Ref != null)
            CharacterListUI_Ref.Refresh_EquipItem_Image();


        On_Click_Close_ItemInfo();
        CurrentItem = null;
    }

    public void On_Click_UnEquipBtn()
    {
        if (CurrentItem.Get_OwnCharacter == null)
            return;

        // GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType] = null;
        GameManager.Instance.Get_SelectChar.Refresh_Char_Equipment_State(false, CurrentItem.Get_EquipType);
        On_Click_Close_ItemInfo();
        CharacterListUI_Ref.Refresh_EquipItem_Image();

        // 해제라는 행동을 했으니 선택된 장비를 없애준다
        CurrentItem = null;

        GameManager.Instance.Get_SelectChar.TestState();
    }
    #endregion
}

