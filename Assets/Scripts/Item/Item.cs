using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Item
{
    Character OwnCharacter;
    public Character Get_OwnCharacter { get => OwnCharacter; set => OwnCharacter = value; }

    // 장착 여부
    bool isEquip;
    public bool Get_isEquip { get => isEquip; set => isEquip = value; }

    ITEM_TYPE ItemType;
    public ITEM_TYPE Get_ItemType { get => ItemType; }
    // 아이템 타입
    EQUIP_TYPE EquipType;
    public EQUIP_TYPE Get_EquipType { get => EquipType; set => EquipType = value; }
    EQUIPMENT_GRADE Equipment_Grade;
    public EQUIPMENT_GRADE Get_Equipment_Grade { get => Equipment_Grade; set => Equipment_Grade = value; }

    // 아이템 이미지 
    Sprite Item_Image;
    public Sprite Get_Item_Image { get => Item_Image; set => Item_Image = value; }

    int Item_Lv;
    public int Get_Item_Lv { get => Item_Lv; set => Item_Lv = value; }

    int Item_ID; // 아이템 ID
    public int Get_Item_ID { get => Item_ID; }

    string Item_Name;
    public string Get_Item_Name { get => Item_Name; }

    float Item_ATK; // 아이템 공격력
    public float Get_Item_Atk { get => Item_ATK; }

    float Item_DEF; // 아이템 방어력
    public float Get_Item_DEF { get => Item_DEF; }

    float Item_CRI_RATE; // 아이템 크리티컬 확률
    public float Get_Item_CRI_RATE { get => Item_CRI_RATE; }

    float Item_CRI_DMG; // 아이템 크리티컬 데미지
    public float Get_Item_CRI_DMG { get => Item_CRI_DMG; }

    float Item_HP;  //아이템 체력
    public float Get_Item_HP { get => Item_HP; }

    float ValueMinRange;
    public float Get_ValueMinRange { get => ValueMinRange; }
    float ValueMaxRange;
    public float Get_ValueMaxRange { get => ValueMaxRange; }

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
    }
    #endregion

    #region Image_Load
    public void Load_Item_Icon(string _add)
    {
        Item_Image = Resources.Load<Sprite>(_add);
    }

    public void Image_Set(Sprite _sprite)
    {
        Item_Image = _sprite;
    }
    #endregion

    #region Grading
    // 생성 시 아이템 등급 결정 여부
    public void Spawn_Grading()
    {
        float randValue = Random.Range(ValueMinRange, ValueMaxRange);

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

        Debug.Log($"{Item_Name} : ATK:{Item_ATK.ToString("N1")} DEF:{Item_DEF.ToString("N1")}" +
            $" CRIR:{Item_CRI_RATE.ToString("N1")} CRID:{Item_CRI_DMG.ToString("N1")} HP:{Item_HP.ToString("N1")} GRADE:{Equipment_Grade}");
    }
    #endregion

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
}
