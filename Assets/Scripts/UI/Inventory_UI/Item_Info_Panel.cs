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
    // ĳ���� ���� �ɼ�
    [SerializeField] Text[] Equipment_Random_Options;

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
    [SerializeField] ItemUpGrade ItemUpGrade_Ref;
    [SerializeField] GameObject InventoryListRoot;
    [SerializeField] GameObject UpgradePanel;
    [SerializeField] Transition_Fade Transition;

#endregion
    // -------------

    public void On_Click_Close_ItemInfo()
    {
        if (CurrentItem != null)
            CurrentItem = null;
        animator.Play("Pop_Down_Item_Info");
    }

    // ������Ʈ ��Ȱ��ȭ
    public void ActiveF_Panel()
    {
        this.gameObject.SetActive(false);
    }

    #region Upgrade_PopUP
    public void On_Click_UpgradePanel()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }

        if (InventoryListRoot.activeSelf)
        {
            InventoryListRoot.SetActive(false);
        }

        ItemUpGrade_Ref.Get_SelectItem = CurrentItem;

        Transition.gameObject.SetActive(true);
        UpgradePanel.SetActive(true);
    }

    #endregion

    #region Iventory_PopUP
    // ������ ���� UI �ʱ�ȭ
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
        Equipment_Item_Name.text = $"{UserInfo.Equip_Inventory[_num].Get_Item_Name} +{UserInfo.Equip_Inventory[_num].Get_Item_Lv}";

        Item_Back.gameObject.SetActive(true);
        // ��޿� ���� UI ȿ�� ����
        Item_Back.color = Inventory_UI_Ref.Get_Colors[(int)UserInfo.Equip_Inventory[_num].Get_Equipment_Grade];
        Item_Grade.sprite = Grade_Sprites[(int)UserInfo.Equip_Inventory[_num].Get_Equipment_Grade];

        // ������ �̹��� ��ü
        Item_Image.sprite = UserInfo.Equip_Inventory[_num].Get_Item_Image;

        // �������� ������ ĳ���Ͱ� �����Ѵٸ�
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

        // UI Text ȿ�� �ؽ�Ʈ ���
        Base_Option(UserInfo.Equip_Inventory[_num].Get_Item_Atk, UserInfo.Equip_Inventory[_num].Get_Item_DEF,
            UserInfo.Equip_Inventory[_num].Get_Item_HP, UserInfo.Equip_Inventory[_num].Get_Item_CRI_RATE,
            UserInfo.Equip_Inventory[_num].Get_Item_CRI_DMG);
    }
    #endregion

    #region EquipItemInfo
    // ��� ���� ����
    public void Open_Equip_Info(Item _item, bool _isChange = false)
    {
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
        // ���� ĳ���Ͱ� ���ٸ�
        if (_item.Get_OwnCharacter == null)
        {
            Change_Btn.SetActive(false);
            UnEquip_Btn.SetActive(false);
            Equip_Btn.SetActive(true);
        }
        else // ������ ĳ���Ͱ� �ִٸ�
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
        // ��޿� ���� UI ȿ�� ����
        Item_Back.color = Inventory_UI_Ref.Get_Colors[(int)_item.Get_Equipment_Grade];
        Item_Grade.sprite = Grade_Sprites[(int)_item.Get_Equipment_Grade];

        // ������ �̹��� ��ü
        Item_Image.sprite = _item.Get_Item_Image;

        // �������� ������ ĳ���Ͱ� �����Ѵٸ�
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

        // UI Text ȿ�� �ؽ�Ʈ ���
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
                Equipment_Random_Options[i].text = $"��ȭ {OptionLv} �޼� �� ������ �ɷ�ġ �߰�";
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
        Change_Btn.SetActive(_isOn);
    }

    public void Set_EquipBtn(bool _isOn)
    {
        Equip_Btn.SetActive(_isOn);
    }
    #endregion

    #region Text_Refresh
    void Base_Option(float _atk, float _def, float _hp, float _criR, float _criD)
    {
        // �ؽ�Ʈ �ʱ�ȭ
        Equipment_Base_Option.text = "";

        if (_atk != 0)
        {
            Equipment_Base_Option.text = $"���ݷ� +{_atk.ToString("N0")}\n";
        }
        
        if (_def != 0)
        {
            Equipment_Base_Option.text += $"���� +{_def.ToString("N0")}\n";
        }

        if (_hp != 0)
        {
            Equipment_Base_Option.text += $"ü�� +{_hp.ToString("N0")}\n";
        }

        if (_criR != 0)
        {
            Equipment_Base_Option.text += $"ġ��Ȯ�� +{(_criR * 100.0f).ToString("N1")}%\n";
        }

        if (_criD != 0)
        {
            Equipment_Base_Option.text += $"ġ������ +{(_criD * 100.0f).ToString("N1")}%\n";
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
            // ���õ� �������� �����ϰ� �ִ� ĳ������ �ɷ�ġ�� ����
            CurrentItem.EquipOption_Stat_Calc(false);
            CurrentItem.Get_OwnCharacter.Refresh_Char_Equipment_State(false, CurrentItem.Get_EquipType);
            // SlotItemInfo.Get_OwnCharacter.Get_EquipItems[(int)SlotItemInfo.Get_EquipType] = null;
        }

        // ���õ� ĳ���Ͱ� ���� ��
        if (GameManager.Instance.Get_SelectChar != null)
        {
            // ���õ� ĳ������ ���� ���� ������Ÿ���� ��� �����ϰ� �ִٸ�
            if (GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType] != null)
            {
                // ��������
                GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType].Get_isEquip = false;
                GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType].EquipOption_Stat_Calc(false);
                GameManager.Instance.Get_SelectChar.Refresh_Char_Equipment_State(false, CurrentItem.Get_EquipType);
                // ���õ� ĳ���Ϳ��� ����
                // GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)SlotItemInfo.Get_EquipType].Get_OwnCharacter = null;
            }

            // ���� ĳ���� ���
            GameManager.Instance.Get_SelectChar.Get_EquipItems[(int)CurrentItem.Get_EquipType] = CurrentItem;
            CurrentItem.Get_OwnCharacter = GameManager.Instance.Get_SelectChar;

            // ��� �ɼǰ� ���� 
            CurrentItem.EquipOption_Stat_Calc(true);
            // �ɷ�ġ ����
            GameManager.Instance.Get_SelectChar.Refresh_Char_Equipment_State(true, CurrentItem.Get_EquipType);
        }

        GameManager.Instance.Get_SelectChar.TestState();

        // ���Ÿ�Կ� �´� ��� �ϳ��� ���� �� �ٸ� ���Ÿ���� ������ �����ִ°� ����
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
        CurrentItem.EquipOption_Stat_Calc(false);

        if (CurrentItem.Get_OwnCharacter.Get_EquipItems[(int)CurrentItem.Get_EquipType] != null)
        {
            CurrentItem.Get_OwnCharacter.Refresh_Char_Equipment_State(false, CurrentItem.Get_EquipType);
        }

        Character_Equipment_Ref.Refresh_List_UI((int)CurrentItem.Get_EquipType);
        On_Click_Close_ItemInfo();
        CharacterListUI_Ref.Refresh_EquipItem_Image();

        // ������� �ൿ�� ������ ���õ� ��� �����ش�
        CurrentItem = null;

        GameManager.Instance.Get_SelectChar.TestState();
    }
    #endregion
}

