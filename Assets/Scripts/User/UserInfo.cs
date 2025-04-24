using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public static List<Item> Spend_Inventory = new List<Item>();
    public static List<Item> Cook_Inventory = new List<Item>();
    #endregion

    [Header("---Currency---")]
    public static int Money = 10000000;
    public static int Dia;
    public static int EnterHealth;

    public static int R_Book;
    public static int SR_Book;
    public static int SSR_Book;

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
