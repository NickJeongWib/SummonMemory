using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using System;

[Serializable]
public class Item
{
    [NonSerialized] Character OwnCharacter;
    public Character Get_OwnCharacter { get => OwnCharacter; set => OwnCharacter = value; }
    [SerializeField] int Item_ID; // ������ ID
    public int Get_Item_ID { get => Item_ID; }
    [SerializeField] string Item_Name;
    public string Get_Item_Name { get => Item_Name; }
    [SerializeField] string EquipChar_Name; // ������ ID
    public string Set_EquipCharName { get => EquipChar_Name; set => EquipChar_Name = value; }
    // ���� ����
    [SerializeField] bool isEquip;
    public bool Get_isEquip { get => isEquip; set => isEquip = value; }

    [SerializeField] ITEM_TYPE ItemType;
    public ITEM_TYPE Get_ItemType { get => ItemType; }
    // ������ Ÿ��
    [SerializeField] EQUIP_TYPE EquipType;
    public EQUIP_TYPE Get_EquipType { get => EquipType; set => EquipType = value; }
    [SerializeField] EQUIPMENT_GRADE Equipment_Grade;
    public EQUIPMENT_GRADE Get_Equipment_Grade { get => Equipment_Grade; set => Equipment_Grade = value; }

    [SerializeField] int Item_Lv;
    public int Get_Item_Lv { get => Item_Lv; set => Item_Lv = value; }

    [SerializeField] float Item_ATK; // ������ ���ݷ�
    public float Get_Item_Atk { get => Item_ATK; }

    [SerializeField] float Item_DEF; // ������ ����
    public float Get_Item_DEF { get => Item_DEF; }

    [SerializeField] float Item_CRI_RATE; // ������ ũ��Ƽ�� Ȯ��
    public float Get_Item_CRI_RATE { get => Item_CRI_RATE; }

    [SerializeField] float Item_CRI_DMG; // ������ ũ��Ƽ�� ������
    public float Get_Item_CRI_DMG { get => Item_CRI_DMG; }

    [SerializeField] float Item_HP;  //������ ü��
    public float Get_Item_HP { get => Item_HP; }

    [SerializeField] float ValueMinRange;
    public float Get_ValueMinRange { get => ValueMinRange; }
    [SerializeField] float ValueMaxRange;
    public float Get_ValueMaxRange { get => ValueMaxRange; }

    [SerializeField] string ItemImage_Path;
    public string Get_ItemImage_Path { get => ItemImage_Path; }

    [SerializeField] EQUIPMENT_OPTION[] EquipmentOption = new EQUIPMENT_OPTION[3];
    public EQUIPMENT_OPTION[] Get_EquipmentOption { get => EquipmentOption; set => EquipmentOption = value; }

    [SerializeField] EQUIPMENT_OPTION_GRADE[] EquipmentOptionGrade = new EQUIPMENT_OPTION_GRADE[3];
    public EQUIPMENT_OPTION_GRADE[] Get_EquipmentOptionGrade { get => EquipmentOptionGrade; set => EquipmentOptionGrade = value; }

    [SerializeField] float[] OptionValue = new float[3];
    public float[] Get_OptionValue { get => OptionValue; set => OptionValue = value; }

    // public List<EquipmentOption> UserInfo.OptionList = new List<EquipmentOption>();
    // ������ �̹��� 
    Sprite Item_Image;
    public Sprite Get_Item_Image { get => Item_Image; set => Item_Image = value; }


    #region Constructor
    public Item(int _itemID, string _itemName, float _itemAtk, float _itemDef, float _itemCriRate, float _itemCriDMG, float _itemHp, float _itemValueMin, float _itemValueMax,
        ITEM_TYPE _itemType = ITEM_TYPE.NONE, EQUIP_TYPE _equipType = EQUIP_TYPE.NONE)
    {
        Item_ID = _itemID; 
        Item_Name = _itemName;
        Item_ATK = _itemAtk;
        Item_DEF = _itemDef;
        Item_CRI_RATE = _itemCriRate;
        Item_CRI_DMG = _itemCriDMG;
        Item_HP = _itemHp;
        ValueMinRange = _itemValueMin;
        ValueMaxRange = _itemValueMax;

        ItemType = _itemType;
        EquipType = _equipType;

        Item_Lv = 0;

        for(int i = 0; i < EquipmentOption.Length; i++)
        {
            EquipmentOption[i] = EQUIPMENT_OPTION.NONE;
            EquipmentOptionGrade[i] = EQUIPMENT_OPTION_GRADE.NONE;
        }
    }
    #endregion

    #region Image_Load
    public void Load_Item_Icon(string _path)
    {
        ItemImage_Path = _path;
        Item_Image = Resources.Load<Sprite>(_path);
    }

    public void Image_Set(Sprite _sprite, string _path)
    {
        ItemImage_Path = _path;
        Item_Image = _sprite;
    }
    #endregion

    #region Grading
    // ���� �� ������ ��� ���� ����
    public void Spawn_Grading()
    {
        float randValue = UserInfo.RandomValue(ValueMinRange, ValueMaxRange);// Random.Range(ValueMinRange, ValueMaxRange);

        if (ValueMinRange <= randValue && randValue < ValueMaxRange * 0.25f)
        {
            this.Equipment_Grade = EQUIPMENT_GRADE.C;
        }
        else if (ValueMaxRange * 0.25f <= randValue && randValue < ValueMaxRange * 0.5f)
        {
            this.Equipment_Grade = EQUIPMENT_GRADE.B;
        }
        else if (ValueMaxRange * 0.5f <= randValue && randValue < ValueMaxRange * 0.75f)
        {
            this.Equipment_Grade = EQUIPMENT_GRADE.A;
        }
        else if (ValueMaxRange * 0.75f <= randValue && randValue < ValueMaxRange)
        {
            this.Equipment_Grade = EQUIPMENT_GRADE.S;
        }

        Item_ATK += (Item_ATK * randValue);
        Item_DEF += (Item_DEF * randValue);
        Item_CRI_RATE += (Item_CRI_RATE * randValue);
        Item_CRI_DMG += (Item_CRI_DMG * randValue);
        Item_HP += (Item_HP * randValue);

        #region DebugTest
        //Debug.Log($"{Item_Name} : ATK:{Item_ATK.ToString("N1")} DEF:{Item_DEF.ToString("N1")}" +
        //    $" CRIR:{Item_CRI_RATE.ToString("N1")} CRID:{Item_CRI_DMG.ToString("N1")} HP:{Item_HP.ToString("N1")} GRADE:{Equipment_Grade}");
        #endregion
    }
    #endregion

    #region Option
    public void Set_OptionList(List<EquipmentOption> _OptionList)
    {
        UserInfo.OptionList = _OptionList;
    }

    // ������ ���� �ɷ� ����
    public void Set_UpgradeOption()
    {
        int count = (int)(Item_Lv / 3);

        for (int i = 0; i < count; i++)
        {
            // �ɼ��� �̹� ������ �Ǿ� �ִٸ�
            if (EquipmentOption[i] != EQUIPMENT_OPTION.NONE)
            {
                continue;
            }

            int RandOption = UserInfo.RandomValue(0, UserInfo.OptionList.Count); // Random.Range(0, OptionList.Count);
            EquipmentOption[i] = UserInfo.OptionList[RandOption].EquipmentRandomOption;
            OptionValue[i] = UserInfo.OptionList[RandOption].Get_OptionValue(ref EquipmentOptionGrade[i]);

            if (OwnCharacter != null) 
                OwnCharacter.EquipmentOption_State_Calc(EquipmentOption[i], i, this, true);
        }

        #region Test_Debug
        for (int i = 0; i < EquipmentOption.Length; i++)
        {
            // Debug.Log(EquipmentOption[i]);
        }

        //if (OwnCharacter != null)
        //    OwnCharacter.TestState();
        #endregion
    }

    // ������ �ɼ� ��ġ ����
    public void Set_ResetOptionValue(bool[] _bool)
    {
        // ���� ĳ���Ͱ� �����Ѵٸ�
        if (OwnCharacter != null)
        {
            // �ɼ� ��ŭ �ݺ�
            for(int i = 0; i < EquipmentOption.Length; i++)
            {
                // �ɼ��� �����ȵǾ������� for�� ��������
                if (EquipmentOption[i] == EQUIPMENT_OPTION.NONE)
                    break;

                // �������� ĳ���� �ɷ�ġ ���
                OwnCharacter.EquipmentOption_State_Calc(EquipmentOption[i], i, this, false);
            }
        }

        // �����ۿ� �ο��� �ɼǸ�ŭ 
        for (int i = 0; i < EquipmentOption.Length; i++)
        {
            if (_bool[i] == true)
                continue;

            // ���������� �ɼǸ�ŭ �ݺ�
            for (int ii = 0; ii < UserInfo.OptionList.Count; ii++)
            {
                // ���� ���� �ɼ��� OptionList�� ����ȿɼǰ� ���ٸ�
                if (EquipmentOption[i] == UserInfo.OptionList[ii].EquipmentRandomOption)
                {
                    // �ɼ� ��ġ �缳��
                    OptionValue[i] = UserInfo.OptionList[ii].Get_OptionValue(ref EquipmentOptionGrade[i]);
                    break;
                }
            }
        }

        // ����ĳ���Ͱ� �����Ѵٸ�
        if (OwnCharacter != null)
        {
            for (int i = 0; i < EquipmentOption.Length; i++)
            {
                // ���� ĳ���Ϳ� �ɷ�ġ�� �ٽ� ������
                OwnCharacter.EquipmentOption_State_Calc(EquipmentOption[i], i, this, true);
            }
        }
    }

    // ������ �ɼ� ����
    public void Set_ResetOption(bool[] _bool)
    {
        // ���� ĳ���Ͱ� �����Ѵٸ�
        if (OwnCharacter != null)
        {
            // �ɼ� ��ŭ �ݺ�
            for (int i = 0; i < EquipmentOption.Length; i++)
            {
                // �������� ĳ���� �ɷ�ġ ���
                OwnCharacter.EquipmentOption_State_Calc(EquipmentOption[i], i, this, false);
            }
        }

        // �����ۿ� �ο��� �ɼǸ�ŭ 
        for (int i = 0; i < EquipmentOption.Length; i++)
        {
            // ����� �Ǿ��ٸ� �н�
            if (_bool[i] == true)
                continue;

            int RandOption = UserInfo.RandomValue(0, UserInfo.OptionList.Count); //  Random.Range();
            EquipmentOption[i] = UserInfo.OptionList[RandOption].EquipmentRandomOption;
            OptionValue[i] = UserInfo.OptionList[RandOption].Get_OptionValue(ref EquipmentOptionGrade[i]);
        }

        // ����ĳ���Ͱ� �����Ѵٸ�
        if (OwnCharacter != null)
        {
            for (int i = 0; i < EquipmentOption.Length; i++)
            {
                // ���� ĳ���Ϳ� �ɷ�ġ�� �ٽ� ������
                OwnCharacter.EquipmentOption_State_Calc(EquipmentOption[i], i, this, true);
            }
        }
    }

    public void EquipOption_Stat_Calc(bool _isEquip)
    {
        if (OwnCharacter == null)
            return;

        //Debug.Log(OwnCharacter.Get_CharName);
        int count = (int)(Item_Lv / 3);

        for (int i = 0; i < count; i++)
        {
            //Debug.Log($"for{i}");
            OwnCharacter.EquipmentOption_State_Calc(EquipmentOption[i], i, this, _isEquip);
        }
    }

    public string Get_Option_KorString(EQUIPMENT_OPTION _option)
    {
        if (_option == EQUIPMENT_OPTION.ATK_INT || _option == EQUIPMENT_OPTION.ATK_PERCENT)
            return "���ݷ�";
        else if (_option == EQUIPMENT_OPTION.DEF_INT || _option == EQUIPMENT_OPTION.DEF_PERCENT)
            return "����";
        else if (_option == EQUIPMENT_OPTION.HP_INT || _option == EQUIPMENT_OPTION.HP_PERCENT)
            return "ü��";
        else if (_option == EQUIPMENT_OPTION.CRIR_PERCENT)
            return "ġ��Ȯ��";
        else
            return "ġ������";
    }
    #endregion

    #region Item_State_Up
    public void UpGrade_Success(float _rate)
    {
        if (Item_ATK != 0)
        {
            Item_ATK += Item_ATK * _rate;
        }

        if (Item_DEF != 0)
        {
            Item_DEF += Item_DEF * _rate;
        }

        if (Item_HP != 0)
        {
            Item_HP += Item_HP * _rate;
        }

        if (Item_CRI_RATE != 0)
        {
            Item_CRI_RATE += Item_CRI_RATE * _rate;
        }

        if (Item_CRI_DMG != 0)
        {
            Item_CRI_DMG += Item_CRI_DMG * _rate;
        }

        Item_Lv++;
    }
    #endregion

}
