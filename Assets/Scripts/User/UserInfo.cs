using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UserInfo
{
    #region Character
    // string ĳ���� �̸� / Character ĳ���� ������
    public static Dictionary<string, Character> UserCharDict = new Dictionary<string, Character>();
    // ĳ���� ���� �� �κ��丮���� �����ϱ� ���� ����ü
    public static List<KeyValuePair<string, Character>> UserCharDict_Copy;
    // ĳ���� ����â�� ĳ���� ������ �����ϱ� ���� ���� ����ü
    public static List<KeyValuePair<string, Character>> UserCharDict_Copy_2;
    public static List<KeyValuePair<string, Character>> Old_UserCharDict_Copy;
    // ĳ���� ���� �� �κ��丮���� ������ ĳ���� ���� �ޱ����� ����Ʈ
    public static List<Character> Equip_Characters = new List<Character>();
    #endregion

    #region Equip_Inventory
    public static List<Item> Equip_Inventory = new List<Item>();
    public static List<Item> Weapon_Equipment = new List<Item>();
    public static List<Item> Helmet_Equipment = new List<Item>();
    public static List<Item> Upper_Equipment = new List<Item>();
    public static List<Item> Accessory_Equipment = new List<Item>();
    public static List<Item> Glove_Equipment = new List<Item>();
    #endregion

    #region Inventory
    public static Dictionary<string, Inventory_Item> InventoryDict = new Dictionary<string, Inventory_Item>();

    public static List<Inventory_Item> Spend_Inventory = new List<Inventory_Item>(); // �Ҹ�ǰ
    public static List<Inventory_Item> Upgrade_Inventory = new List<Inventory_Item>();  // �丮 ������
    #endregion

    [Header("---Currency---")]
    public static int Money = 10000000;
    public static int Dia;
    public static int EnterHealth;

    public static int SummonTicket;
    public static int EquipmentTicket;

    #region R��ް� SR,SSR����� �̹��� ���� ����
    public static void Get_Square_Image(Image[] _image, int _equipIndex)
    {
        // R��� ĳ���ʹ� ���� Lobby�̹����� ���� ������ 
        if (Equip_Characters[_equipIndex].Get_CharGrade != Define.CHAR_GRADE.R)
        {
            _image[_equipIndex].sprite = Equip_Characters[_equipIndex].Get_SquareIllust_Img;
        }
        else
        {
            _image[_equipIndex].sprite = Equip_Characters[_equipIndex].Get_Illust_Img;
        }
    }

    public static void Get_Square_Image(Image _image, int _equipIndex)
    {
        // R��ް� SR, SSR�� ���簢������ �� �̹����� �޶� �̿�
        if (UserInfo.Equip_Characters[_equipIndex].Get_CharGrade != Define.CHAR_GRADE.R)
        {
            _image.sprite = UserInfo.Equip_Characters[_equipIndex].Get_SquareIllust_Img;
        }
        else
        {
            _image.sprite = UserInfo.Equip_Characters[_equipIndex].Get_Illust_Img;
        }
    }

    public static void Get_Square_Image(Image _image, Character _character)
    {
        // R��ް� SR, SSR�� ���簢������ �� �̹����� �޶� �̿�
        if (_character.Get_CharGrade != Define.CHAR_GRADE.R)
        {
            _image.sprite = _character.Get_SquareIllust_Img;
        }
        else
        {
            _image.sprite = _character.Get_Illust_Img;
        }
    }
    #endregion

    public static void Add_Inventory_Item(Inventory_Item _item, int _add = 1)
    {
        // �������� Dictionary�� ���ٸ� �߰�
        if (InventoryDict.ContainsKey(_item.Get_Item_Name) == false)
        {
            InventoryDict.Add(_item.Get_Item_Name, _item);
            _item.Get_Amount = 1;
        }
        else // �̹� �����Ѵٸ�
        {
            InventoryDict[_item.Get_Item_Name].Get_Amount += _add;
        }

        // Debug.Log(_item.Get_Amount);

        #region Inventory_List_Add
        // ������ ����Ʈ�� ��� �ֱ� �ϱ� ���� �ڵ�
        Spend_Inventory.Clear();
        Upgrade_Inventory.Clear();

        foreach (Inventory_Item item in InventoryDict.Values)
        {
            if (item.Get_InventoryType == INVENTORY_TYPE.SPEND)
            {
                for (int i = 0; i < Spend_Inventory.Count; i++)
                {
                    if (Spend_Inventory[i].Get_Item_Name == item.Get_Item_Name)
                        return;         
                }
                Spend_Inventory.Add(item);
            }
            else if (item.Get_InventoryType == INVENTORY_TYPE.UPGRADE)
            {
                for (int i = 0; i < Upgrade_Inventory.Count; i++)
                {
                    if (Upgrade_Inventory[i].Get_Item_Name == item.Get_Item_Name)
                        return;                     
                }

                Upgrade_Inventory.Add(item);
            }
        }

        #region Test
        for (int i = 0; i < Spend_Inventory.Count; i++)
        {
            Debug.Log($"{i}/{Spend_Inventory[i].Get_Item_Name}/{Spend_Inventory[i].Get_Amount}");
        }

        for (int i = 0; i < Upgrade_Inventory.Count; i++)
        {
            Debug.Log($"{i}/{Upgrade_Inventory[i].Get_Item_Name}/{Upgrade_Inventory[i].Get_Amount}");
        }
        #endregion
        #endregion
    }


    #region Test
    public static void Test()
    {
        for (int i = 0; i < Equip_Characters.Count; i++)
        {
            Debug.Log(Equip_Characters[i].Get_CharName + " : " + Equip_Characters[i].Get_CharGrade);
        }
    }

    #endregion
}
