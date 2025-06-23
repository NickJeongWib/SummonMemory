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
    [SerializeField] int Item_ID; // 아이템 ID
    public int Get_Item_ID { get => Item_ID; }
    [SerializeField] string Item_Name;
    public string Get_Item_Name { get => Item_Name; }
    [SerializeField] string EquipChar_Name; // 아이템 ID
    public string Set_EquipCharName { get => EquipChar_Name; set => EquipChar_Name = value; }
    // 장착 여부
    [SerializeField] bool isEquip;
    public bool Get_isEquip { get => isEquip; set => isEquip = value; }

    [SerializeField] ITEM_TYPE ItemType;
    public ITEM_TYPE Get_ItemType { get => ItemType; }
    // 아이템 타입
    [SerializeField] EQUIP_TYPE EquipType;
    public EQUIP_TYPE Get_EquipType { get => EquipType; set => EquipType = value; }
    [SerializeField] EQUIPMENT_GRADE Equipment_Grade;
    public EQUIPMENT_GRADE Get_Equipment_Grade { get => Equipment_Grade; set => Equipment_Grade = value; }

    [SerializeField] int Item_Lv;
    public int Get_Item_Lv { get => Item_Lv; set => Item_Lv = value; }

    [SerializeField] float Item_ATK; // 아이템 공격력
    public float Get_Item_Atk { get => Item_ATK; }

    [SerializeField] float Item_DEF; // 아이템 방어력
    public float Get_Item_DEF { get => Item_DEF; }

    [SerializeField] float Item_CRI_RATE; // 아이템 크리티컬 확률
    public float Get_Item_CRI_RATE { get => Item_CRI_RATE; }

    [SerializeField] float Item_CRI_DMG; // 아이템 크리티컬 데미지
    public float Get_Item_CRI_DMG { get => Item_CRI_DMG; }

    [SerializeField] float Item_HP;  //아이템 체력
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
    // 아이템 이미지 
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
    // 생성 시 아이템 등급 결정 여부
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

    // 아이템 랜덤 능력 설정
    public void Set_UpgradeOption()
    {
        int count = (int)(Item_Lv / 3);

        for (int i = 0; i < count; i++)
        {
            // 옵션이 이미 설정이 되어 있다면
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

    // 아이템 옵션 수치 변경
    public void Set_ResetOptionValue(bool[] _bool)
    {
        // 장착 캐릭터가 존재한다면
        if (OwnCharacter != null)
        {
            // 옵션 만큼 반복
            for(int i = 0; i < EquipmentOption.Length; i++)
            {
                // 옵션이 설정안되어있으면 for문 빠져나감
                if (EquipmentOption[i] == EQUIPMENT_OPTION.NONE)
                    break;

                // 장착중인 캐릭터 능력치 계산
                OwnCharacter.EquipmentOption_State_Calc(EquipmentOption[i], i, this, false);
            }
        }

        // 아이템에 부여된 옵션만큼 
        for (int i = 0; i < EquipmentOption.Length; i++)
        {
            if (_bool[i] == true)
                continue;

            // 설정가능한 옵션만큼 반복
            for (int ii = 0; ii < UserInfo.OptionList.Count; ii++)
            {
                // 만약 현재 옵션이 OptionList에 저장된옵션과 같다면
                if (EquipmentOption[i] == UserInfo.OptionList[ii].EquipmentRandomOption)
                {
                    // 옵션 수치 재설정
                    OptionValue[i] = UserInfo.OptionList[ii].Get_OptionValue(ref EquipmentOptionGrade[i]);
                    break;
                }
            }
        }

        // 장착캐릭터가 존재한다면
        if (OwnCharacter != null)
        {
            for (int i = 0; i < EquipmentOption.Length; i++)
            {
                // 장착 캐릭터에 능력치를 다시 더해줌
                OwnCharacter.EquipmentOption_State_Calc(EquipmentOption[i], i, this, true);
            }
        }
    }

    // 아이템 옵션 변경
    public void Set_ResetOption(bool[] _bool)
    {
        // 장착 캐릭터가 존재한다면
        if (OwnCharacter != null)
        {
            // 옵션 만큼 반복
            for (int i = 0; i < EquipmentOption.Length; i++)
            {
                // 장착중인 캐릭터 능력치 계산
                OwnCharacter.EquipmentOption_State_Calc(EquipmentOption[i], i, this, false);
            }
        }

        // 아이템에 부여된 옵션만큼 
        for (int i = 0; i < EquipmentOption.Length; i++)
        {
            // 잠금이 되었다면 패스
            if (_bool[i] == true)
                continue;

            int RandOption = UserInfo.RandomValue(0, UserInfo.OptionList.Count); //  Random.Range();
            EquipmentOption[i] = UserInfo.OptionList[RandOption].EquipmentRandomOption;
            OptionValue[i] = UserInfo.OptionList[RandOption].Get_OptionValue(ref EquipmentOptionGrade[i]);
        }

        // 장착캐릭터가 존재한다면
        if (OwnCharacter != null)
        {
            for (int i = 0; i < EquipmentOption.Length; i++)
            {
                // 장착 캐릭터에 능력치를 다시 더해줌
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
            return "공격력";
        else if (_option == EQUIPMENT_OPTION.DEF_INT || _option == EQUIPMENT_OPTION.DEF_PERCENT)
            return "방어력";
        else if (_option == EQUIPMENT_OPTION.HP_INT || _option == EQUIPMENT_OPTION.HP_PERCENT)
            return "체력";
        else if (_option == EQUIPMENT_OPTION.CRIR_PERCENT)
            return "치명확률";
        else
            return "치명피해";
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
