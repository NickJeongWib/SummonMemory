using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

#region Json
[System.Serializable]
public class InventoryItemPair
{
    public string key;
    public Inventory_Item value;
}

[System.Serializable]
public class InventoryDictWrapper
{
    public List<InventoryItemPair> items = new List<InventoryItemPair>();
}

[System.Serializable]
public class CharacterListPair
{
    public string key;
    public Character value;
}

[System.Serializable]
public class CharacterListWrapper
{
    public List<CharacterListPair> Characters = new List<CharacterListPair>();
}

[System.Serializable]
public class EquipItemListWrapper
{
    public List<Item> Items;

    public EquipItemListWrapper(List<Item> items)
    {
        Items = items;
    }
}
#endregion

public class UserInfo
{
    #region User
    public static string UID;
    public static string UserName;

    public static UserProfile_Set Profile_Setting = new UserProfile_Set();
    #endregion

    #region Character
    // string ĳ���� �̸� / Character ĳ���� ����Ʈ ���� ������
    public static Dictionary<string, Character> UserCharDict = new Dictionary<string, Character>();
    // ĳ���� ���� �� �κ��丮���� �����ϱ� ���� ����ü
    public static List<KeyValuePair<string, Character>> UserCharDict_Copy = new List<KeyValuePair<string, Character>>();
    // ĳ���� ����â���� ĳ���͸� �������� ���� ĳ���� ����, ����, ��ų��ȭ â���� ���ʿ� ǥ�õǴ� ��ũ�ѿ� ǥ���ϱ� ���� ����ü
    public static List<KeyValuePair<string, Character>> UserCharDict_Copy_2 = new List<KeyValuePair<string, Character>>();
    // ������ ĳ���͸� ���� �� �������� �ʰ� ��� �� ���� ���� ĳ���� ����Ʈ�� ���ư��� ���� ����Ʈ
    public static List<KeyValuePair<string, Character>> Old_UserCharDict_Copy = new List<KeyValuePair<string, Character>>();
    // ĳ���� ���� �� ĳ���� ����Ʈ���� ������ ĳ���� ���� �ޱ����� ����Ʈ
    public static List<Character> Equip_Characters = new List<Character>();
    #endregion

    #region Equip_Inventory
    public static List<EquipmentOption> OptionList = new List<EquipmentOption>();

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

    #region Quest & Stage
    public static List<bool> StageClear = new List<bool>();
    public static List<QuestData> QuestData_List = new List<QuestData>();
    public static List<Quest_Slot> QuestSlot_List = new List<Quest_Slot>();
    #endregion

    #region Data
    public static string JsonData;
    #endregion

    #region StagePos_Index
    public static int[] Pos_Index = {-1, -1, -1, -1, -1};
    #endregion

    [Header("---Currency---")]
    public static int Money = 10000000;
    public static int Dia = 10000;
    public static int EnterHealth;

    public static int SummonTicket;
    public static int EquipmentTicket;

    [Header("---Gacha---")]
    public static int SR_Set_Count;
    public static int SSR_Set_Count;

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

    #region Inventory_Refresh
    public static void Add_Inventory_Item(Inventory_Item _item, int _add = 1)
    {
        // �������� Dictionary�� ���ٸ� �߰�
        if (InventoryDict.ContainsKey(_item.Get_Item_Name) == false)
        {
            InventoryDict.Add(_item.Get_Item_Name, _item);
            _item.Get_Amount += _add;
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

        // Dictionary Ž��
        foreach (Inventory_Item item in InventoryDict.Values)
        {
            // �Ҹ� ������
            if (item.Get_InventoryType == INVENTORY_TYPE.SPEND)
            {
                // ����Ʈ �߰�
                for (int i = 0; i < Spend_Inventory.Count; i++)
                {
                    if (Spend_Inventory[i].Get_Item_Name == item.Get_Item_Name)
                        return;         
                }
                Spend_Inventory.Add(item);
            }
            // ��ȭ ������
            else if (item.Get_InventoryType == INVENTORY_TYPE.UPGRADE)
            {
                // ����Ʈ �߰�
                for (int i = 0; i < Upgrade_Inventory.Count; i++)
                {
                    if (Upgrade_Inventory[i].Get_Item_Name == item.Get_Item_Name)
                        return;                     
                }

                Upgrade_Inventory.Add(item);
            }
        }

        #region Test
        //for (int i = 0; i < Spend_Inventory.Count; i++)
        //{
        //    Debug.Log($"{i}/{Spend_Inventory[i].Get_Item_Name}/{Spend_Inventory[i].Get_Amount}");
        //}

        //for (int i = 0; i < Upgrade_Inventory.Count; i++)
        //{
        //    Debug.Log($"{i}/{Upgrade_Inventory[i].Get_Item_Name}/{Upgrade_Inventory[i].Get_Amount}");
        //}
        #endregion
        #endregion
    }

    public static void Remove_Inventory_Item()
    {
        List<string> removeList = new List<string>();

        #region Inventory_List_Add
        // ������ ����Ʈ�� ��� �ֱ� �ϱ� ���� �ڵ�
        Spend_Inventory.Clear();
        Upgrade_Inventory.Clear();

        // Dictionary Ž��
        foreach (Inventory_Item item in InventoryDict.Values)
        {
            if (item.Get_Amount <= 0)
            {
                removeList.Add(item.Get_Item_Name);
            }
        }

        for (int i = 0; i < removeList.Count; i++)
        {
            InventoryDict.Remove(removeList[i]);
        }

        removeList.Clear();

        foreach (Inventory_Item item in InventoryDict.Values)
        {
            // �Ҹ� ������
            if (item.Get_InventoryType == INVENTORY_TYPE.SPEND)
            {
                Spend_Inventory.Add(item);
            }
            // ��ȭ ������
            else if (item.Get_InventoryType == INVENTORY_TYPE.UPGRADE)
            {
                Upgrade_Inventory.Add(item);
            }
        }

        #region Test
        //foreach(Inventory_Item item in InventoryDict.Values)
        //{
        //    Debug.Log($"Dict : {item.Get_Item_Name}");
        //}

        //for (int i = 0; i < Spend_Inventory.Count; i++)
        //{
        //    Debug.Log($"{i}/{Spend_Inventory[i].Get_Item_Name}/{Spend_Inventory[i].Get_Amount}");
        //}

        //for (int i = 0; i < Upgrade_Inventory.Count; i++)
        //{
        //    Debug.Log($"{i}/{Upgrade_Inventory[i].Get_Item_Name}/{Upgrade_Inventory[i].Get_Amount}");
        //}
        #endregion
        #endregion
    }
    #endregion

    #region Random
    public static int RandomValue(int _min, int _max)
    {
        int rand = Random.Range(_min, _max);
        return rand;
    }

    public static float RandomValue(float _min, float _max)
    {
        float rand = Random.Range(_min, _max);
        return rand;
    }
    #endregion

    #region Json
    public static void Inventory_ToJson()
    {
        InventoryDictWrapper wrapper = new InventoryDictWrapper();
        foreach (var invenItem in InventoryDict)
        {
            wrapper.items.Add(new InventoryItemPair { key = invenItem.Key, value = invenItem.Value });
        }

        string json = JsonUtility.ToJson(wrapper, true);
        Debug.Log(json);
    }

    public static void CharacterList_ToJson()
    {
        CharacterListWrapper wrapper = new CharacterListWrapper();
        foreach (var character in UserCharDict)
        {
            wrapper.Characters.Add(new CharacterListPair { key = character.Key, value = character.Value });
        }

        string json = JsonUtility.ToJson(wrapper, true);
        Debug.Log(json);
    }
    #endregion

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


