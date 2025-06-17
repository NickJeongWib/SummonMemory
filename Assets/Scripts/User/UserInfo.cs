using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UserInfo
{
    #region Character
    // string 캐릭터 이름 / Character 캐릭터 데이터
    public static Dictionary<string, Character> UserCharDict = new Dictionary<string, Character>();
    // 캐릭터 장착 시 인벤토리에서 해제하기 위한 복사체
    public static List<KeyValuePair<string, Character>> UserCharDict_Copy;
    // 캐릭터 정보창에 캐릭터 정보를 나열하기 위한 원본 복사체
    public static List<KeyValuePair<string, Character>> UserCharDict_Copy_2;
    public static List<KeyValuePair<string, Character>> Old_UserCharDict_Copy;
    // 캐릭터 장착 시 인벤토리에서 제거한 캐릭터 값을 받기위한 리스트
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

    public static List<Inventory_Item> Spend_Inventory = new List<Inventory_Item>(); // 소모품
    public static List<Inventory_Item> Upgrade_Inventory = new List<Inventory_Item>();  // 요리 아이템
    #endregion

    #region Quest
    public static List<Quest_Slot> QuestSlot_List = new List<Quest_Slot>();
    #endregion


    [Header("---Currency---")]
    public static int Money = 10000000;
    public static int Dia;
    public static int EnterHealth;

    public static int SummonTicket;
    public static int EquipmentTicket;

    [Header("---Gacha---")]
    public static int SR_Set_Count;
    public static int SSR_Set_Count;

    #region R등급과 SR,SSR등급의 이미지 차별 적용
    public static void Get_Square_Image(Image[] _image, int _equipIndex)
    {
        // R등급 캐릭터는 따로 Lobby이미지가 없기 때문에 
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
        // R등급과 SR, SSR은 정사각형으로 된 이미지가 달라서 이용
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
        // R등급과 SR, SSR은 정사각형으로 된 이미지가 달라서 이용
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
        // 아이템이 Dictionary에 없다면 추가
        if (InventoryDict.ContainsKey(_item.Get_Item_Name) == false)
        {
            InventoryDict.Add(_item.Get_Item_Name, _item);
            _item.Get_Amount += _add;
        }
        else // 이미 존재한다면
        {
            InventoryDict[_item.Get_Item_Name].Get_Amount += _add;
        }

        // Debug.Log(_item.Get_Amount);

        #region Inventory_List_Add
        // 유저가 리스트에 들고 있기 하기 위한 코드
        Spend_Inventory.Clear();
        Upgrade_Inventory.Clear();

        // Dictionary 탐색
        foreach (Inventory_Item item in InventoryDict.Values)
        {
            // 소모 아이템
            if (item.Get_InventoryType == INVENTORY_TYPE.SPEND)
            {
                // 리스트 추가
                for (int i = 0; i < Spend_Inventory.Count; i++)
                {
                    if (Spend_Inventory[i].Get_Item_Name == item.Get_Item_Name)
                        return;         
                }
                Spend_Inventory.Add(item);
            }
            // 강화 아이템
            else if (item.Get_InventoryType == INVENTORY_TYPE.UPGRADE)
            {
                // 리스트 추가
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
        // 유저가 리스트에 들고 있기 하기 위한 코드
        Spend_Inventory.Clear();
        Upgrade_Inventory.Clear();

        // Dictionary 탐색
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
            // 소모 아이템
            if (item.Get_InventoryType == INVENTORY_TYPE.SPEND)
            {
                Spend_Inventory.Add(item);
            }
            // 강화 아이템
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
