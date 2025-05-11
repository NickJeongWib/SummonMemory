using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

// TODO ## Character ������ ���� Ŭ����
public class Character
{
    ///------- Var
    #region Character_State
    int Character_Lv;
    public int Get_Character_Lv { get => Character_Lv; set => Character_Lv = value; }

    // google_Sheet ���� ������
    int CharacterID;
    public int Get_CharacterID { get => CharacterID; }

    string CharName;
    public string Get_CharName { get => CharName; }

    string CharEngName;
    public string Get_CharEngName { get => CharEngName; }

    CHAR_GRADE CharGrade;
    public CHAR_GRADE Get_CharGrade { get => CharGrade; }

    CHAR_TYPE CharType;
    public CHAR_TYPE Get_CharType { get => CharType; }

    CHAR_ELE CharElement;
    public CHAR_ELE Get_CharElement { get => CharElement; }

    int CharStar;
    public int Get_CharStar { get => CharStar; set => CharStar = value; }

    float BaseHP;
    public float Get_BaseHP { get => BaseHP; }
    float CharHP;
    public float Get_CharHP { get => CharHP; }

    float BaseAtk;
    public float Get_BaseAtk { get => BaseAtk; }
    float CharATK;
    public float Get_CharATK { get => CharATK; }

    float BaseDef;
    public float Get_BaseDef { get => BaseDef; }
    float CharDEF;
    public float Get_CharDEF { get => CharDEF; }

    float BaseCRID;
    public float Get_BaseCRID { get => BaseCRID; }
    float Char_CRT_DAMAGE;
    public float Get_Char_CRT_Damage { get => Char_CRT_DAMAGE; }

    float BaseCRIR;
    public float Get_BaseCRIR { get => BaseCRIR; }
    float Char_CRT_RATE;
    public float Get_Char_CRT_Rate { get => Char_CRT_RATE; }

    float CombatPower;
    public float Get_CombatPower { get => CombatPower; }
    #endregion

    #region Character_Growing_State
    // ���� ���� �Ķ����
    float linearFactor;     // ���� ���� ���
    public float Get_linearFactor { get => linearFactor; }

    float expFactor;        // ���� ���� ���
    public float Get_expFactor { get => expFactor; }

    float expMultiplier;    // ���� ���� ����ġ
    public float Get_expMultiplier { get => expMultiplier; }

    int transitionLevel;  // ���� ��� ��ȯ ����
    public int Get_transitionLevel { get => transitionLevel; }
    #endregion

    #region Character_EquipItem
    Item[] EquipItems = new Item[5];
    public Item[] Get_EquipItems { get => EquipItems; set => EquipItems = value; }
    #endregion
    ///------- Func

    #region Image_Resource
    // -----------------------Image Resources Variable----------------------

    string Illust_Address;
    public string Get_Illust_Address { get => Illust_Address; }
    
    string Normal_Image_Address;
    public string Get_Normal_Image_Address { get => Normal_Image_Address; }

    string Grade_Up_Image_Address;
    public string Get_Grade_Up_Image_Address { get => Grade_Up_Image_Address; }

    string Profile_Address;
    public string Get_Profile_Address { get => Profile_Address; }

    string White_Illust_Address;
    public string Get_White_Illust_Address { get => White_Illust_Address; }

    string Square_Illust_Address;
    public string Get_Square_Illust_Address { get => Square_Illust_Address; }


    // �̹����� 
    Sprite Illust_Img;
    public Sprite Get_Illust_Img { get => Illust_Img; }

    Sprite Normal_Img;
    public Sprite Get_Normal_Img { get => Normal_Img; }

    Sprite Grade_Up_Img;
    public Sprite Get_Grade_Up_Img { get => Grade_Up_Img; }

    Sprite Profile_Img;
    public Sprite Get_Profile_Img { get => Profile_Img; }

    Sprite WhiteIllust_Img;
    public Sprite Get_WhiteIllust_Img { get => WhiteIllust_Img; }

    Sprite SquareIllust_Img;
    public Sprite Get_SquareIllust_Img { get => SquareIllust_Img; }
    #endregion

    #region Constructor
    public Character(int _id, string _name, string _engName, CHAR_GRADE _grade, CHAR_TYPE _type, CHAR_ELE _ele, int _star,
       float _hp, float _atk, float _def, float _crtDamage, float _crtRate, string _illustAdd = "", string _normalImageAdd = "", string _gradeUpAdd = "", string _profileAdd = "", int _lv = 1)
    {
        Character_Lv = _lv;

        // ĳ���� �ɷ�ġ
        CharacterID = _id;
        CharName = _name;
        CharEngName = _engName;
        CharGrade = _grade;
        CharType = _type;
        CharElement = _ele;
        CharStar = _star;
        CharHP = _hp;
        CharATK = _atk;
        CharDEF = _def;
        Char_CRT_DAMAGE = _crtDamage;
        Char_CRT_RATE = _crtRate;

        BaseSet(CharATK, CharDEF, CharHP, Char_CRT_RATE, Char_CRT_DAMAGE);

        // �̹��� �ּ�
        Illust_Address = _illustAdd;
        Normal_Image_Address = _normalImageAdd;
        Grade_Up_Image_Address = _gradeUpAdd;
        Profile_Address = _profileAdd;

        Illust_Img = Resources.Load<Sprite>(Illust_Address);
        Normal_Img = Resources.Load<Sprite>(Normal_Image_Address);
        Grade_Up_Img = Resources.Load<Sprite>(Grade_Up_Image_Address);
        Profile_Img = Resources.Load<Sprite>(Profile_Address);
    }

    void BaseSet(float _atk, float _def, float _hp, float criR, float criD)
    {
        BaseAtk = _atk;
        BaseDef = _def;
        BaseHP = _hp;
        BaseCRIR = criR;
        BaseCRID = criD;
    }
    #endregion

    #region Load_Resources
    public void Load_Resources(string _illustAdd, string _normalImageAdd, string _gradeUpAdd, string _profileAdd, string _whiteIllustAdd, string _squareIllustAdd = null)
    {
        // �̹��� �ּ�
        Illust_Address = _illustAdd;
        Normal_Image_Address = _normalImageAdd;
        Grade_Up_Image_Address = _gradeUpAdd;
        Profile_Address = _profileAdd;
        White_Illust_Address = _whiteIllustAdd;
        Square_Illust_Address = _squareIllustAdd;

        // �̹��� �ε�
        Illust_Img = Resources.Load<Sprite>(Illust_Address);
        Normal_Img = Resources.Load<Sprite>(Normal_Image_Address);
        Grade_Up_Img = Resources.Load<Sprite>(Grade_Up_Image_Address);
        Profile_Img = Resources.Load<Sprite>(Profile_Address);
        WhiteIllust_Img = Resources.Load<Sprite>(White_Illust_Address);
        SquareIllust_Img = Resources.Load<Sprite>(Square_Illust_Address);
    }
    #endregion

    #region Load_Growing_State
    public void Load_Growing_State(float _linearFactor, float _expFactor, float _expMultiplier, int _transitionLevel)
    {
        linearFactor = (_linearFactor * 100);
        expFactor = (_expFactor * 100);
        expMultiplier = (_expMultiplier * 100);
        transitionLevel = _transitionLevel;
    }
    #endregion

    #region Character_Growing
    // ȥ���� ���� ����
    public float Calculate_State(int level)
    {
        if (level > 50) level = 50; // �ִ� ���� ����

        float attack;
        if (level < transitionLevel)
        {
            // �ʹ�: ���� ����
            attack = CharATK + (level * linearFactor);
        }
        else
        {
            // �Ĺ�: ���� ���� �߰�
            attack = CharATK + (transitionLevel * linearFactor) + ((level - transitionLevel) * linearFactor * 0.5f) + (Mathf.Pow(level, expFactor) * expMultiplier);
        }

        return attack;
    }

    // ������ �Լ�
    public void LevelUp()
    {
        if (Character_Lv < 50)
        {
            Character_Lv++;
            Calculate_State(Character_Lv);
        }
        else
        {
            
        }
    }
    #endregion

    #region Character_Equipment_State_Refresh
    // TODO ## Character ��� ������ ���� �� ĳ���� �ɷ�ġ ���
    public void Refresh_Char_Equipment_State(bool _isEquip, EQUIP_TYPE _equipType = EQUIP_TYPE.NONE)
    {
        // �������� ���� �ߴٸ�
        if (_isEquip)
        {
            // ����� �ɷ�ġ��ŭ ĳ���� �ɷ�ġ ����
            CharATK += EquipItems[(int)_equipType].Get_Item_Atk;
            CharDEF += EquipItems[(int)_equipType].Get_Item_DEF;
            CharHP += EquipItems[(int)_equipType].Get_Item_HP;
            Char_CRT_DAMAGE += EquipItems[(int)_equipType].Get_Item_CRI_DMG;
            Char_CRT_RATE += EquipItems[(int)_equipType].Get_Item_CRI_RATE;

            if (Char_CRT_RATE >= 1.0f)
                Char_CRT_RATE = 1.0f;
        }
        else // ������ ���� ��
        {
            // ������ �ɷ�ġ ����
            CharATK -= EquipItems[(int)_equipType].Get_Item_Atk;
            CharDEF -= EquipItems[(int)_equipType].Get_Item_DEF;
            CharHP -= EquipItems[(int)_equipType].Get_Item_HP;
            Char_CRT_DAMAGE -= EquipItems[(int)_equipType].Get_Item_CRI_DMG;
            Char_CRT_RATE -= EquipItems[(int)_equipType].Get_Item_CRI_RATE;

            // ������ ����
            EquipItems[(int)_equipType].Get_OwnCharacter = null;
            EquipItems[(int)_equipType].Get_isEquip = false;
            EquipItems[(int)_equipType] = null;
        }
    }

    // �������� ��� ��ȭ ���� �� ĳ���� �ɷ�ġ ����
    public void EquipmentUpgrade_State_Refresh(EQUIP_TYPE _equipType, bool isAfter)
    {
        if (isAfter == false)
        {
            // ������ �ɷ�ġ ����
            CharATK -= EquipItems[(int)_equipType].Get_Item_Atk;
            CharDEF -= EquipItems[(int)_equipType].Get_Item_DEF;
            CharHP -= EquipItems[(int)_equipType].Get_Item_HP;
            Char_CRT_DAMAGE -= EquipItems[(int)_equipType].Get_Item_CRI_DMG;
            Char_CRT_RATE -= EquipItems[(int)_equipType].Get_Item_CRI_RATE;
        }
        else
        {
            // ����� �ɷ�ġ��ŭ ĳ���� �ɷ�ġ ����
            CharATK += EquipItems[(int)_equipType].Get_Item_Atk;
            CharDEF += EquipItems[(int)_equipType].Get_Item_DEF;
            CharHP += EquipItems[(int)_equipType].Get_Item_HP;
            Char_CRT_DAMAGE += EquipItems[(int)_equipType].Get_Item_CRI_DMG;
            Char_CRT_RATE += EquipItems[(int)_equipType].Get_Item_CRI_RATE;

            if (Char_CRT_RATE >= 1.0f)
                Char_CRT_RATE = 1.0f;
        }
    }

    public void EquipmentOption_State_Calc(EQUIPMENT_OPTION _equipOption, int _num, Item _item, bool _isEquip)
    {
        #region ATK_INT
        if (_equipOption == EQUIPMENT_OPTION.ATK_INT)
        {
            if (_isEquip)
            {
                CharATK += _item.Get_OptionValue[_num];
            }
            else
            {
                CharATK -= _item.Get_OptionValue[_num];
            }
        }
        #endregion
        #region ATK_Percent
        if (_equipOption == EQUIPMENT_OPTION.ATK_PERCENT)
        {
            float rate = BaseAtk * _item.Get_OptionValue[_num];
            if (_isEquip)
            {
                CharATK += rate;
            }
            else
            {
                CharATK -= rate;
            }
        }
        #endregion
        #region DEF_INT
        if (_equipOption == EQUIPMENT_OPTION.DEF_INT)
        {
            if (_isEquip)
            {
                CharDEF += _item.Get_OptionValue[_num];
            }
            else
            {
                CharDEF -= _item.Get_OptionValue[_num];
            }
        }
        #endregion
        #region DEF_Percent
        if (_equipOption == EQUIPMENT_OPTION.DEF_PERCENT)
        {
            float rate = BaseDef * _item.Get_OptionValue[_num];
            if (_isEquip)
            {
                CharDEF += rate;
            }
            else
            {
                CharDEF -= rate;
            }
        }
        #endregion
        #region HP_INT
        if (_equipOption == EQUIPMENT_OPTION.HP_INT)
        {
            if (_isEquip)
            {
                CharHP += _item.Get_OptionValue[_num];
            }
            else
            {
                CharHP -= _item.Get_OptionValue[_num];
            }
        }
        #endregion
        #region HP_Percent
        if (_equipOption == EQUIPMENT_OPTION.HP_PERCENT)
        {
            float rate = BaseHP * _item.Get_OptionValue[_num];
            if (_isEquip)
            {
                CharHP += rate;
            }
            else
            {
                CharHP -= rate;
            }
        }
        #endregion
        #region CRIR
        if (_equipOption == EQUIPMENT_OPTION.CRIR_PERCENT)
        {
            if (_isEquip)
            {
                Char_CRT_RATE += _item.Get_OptionValue[_num];
            }
            else
            {
                Char_CRT_RATE -= _item.Get_OptionValue[_num];

                if (BaseCRIR > Char_CRT_RATE)
                    Char_CRT_RATE = BaseCRIR;
            }
        }
        #endregion
        #region CRID
        if (_equipOption == EQUIPMENT_OPTION.CRID_PERCENT)
        {
            if (_isEquip)
            {
                Char_CRT_DAMAGE += _item.Get_OptionValue[_num];
            }
            else
            {
                Char_CRT_DAMAGE -= _item.Get_OptionValue[_num];

                if (BaseCRID > Char_CRT_DAMAGE)
                    Char_CRT_DAMAGE = BaseCRID;
            }
        }
        #endregion

        // TestState();
    }
    #endregion

    #region CombatPower
    public string Calc_CombatPower()
    {
        CombatPower = (CharATK * 2.0f) + (CharDEF * 0.5f) + (CharHP * 0.2f) +
            (Char_CRT_DAMAGE * 100 * 10.0f) + (Char_CRT_RATE * 100 * 3.0f);

        return CombatPower.ToString("N0");
    }
    #endregion
    public void TestState()
    {
        Debug.Log($"{CharName}: ���ݷ� : {CharATK}\n���� : {CharDEF}\nü�� : {CharHP}\nũ�� : {Char_CRT_DAMAGE}\nũȮ : {Char_CRT_RATE}");
    }
}
