using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Character
{
    int CharacterID;
    public int Get_CharacterID { get => CharacterID; }

    string CharName;
    public string Get_CharName { get => CharName; }

    CHAR_GRADE CharGrade;
    public CHAR_GRADE Get_CharGrade { get => CharGrade; }

    CHAR_TYPE CharType;
    public CHAR_TYPE Get_CharType { get => CharType; }

    CHAR_ELE CharElement;
    public CHAR_ELE Get_CharElement { get => CharElement; }

    float CharHP;
    public float Get_CharHP { get => CharHP; }

    float CharATK;
    public float Get_CharATK { get => CharATK; }

    float CharDEF;
    public float Get_CharDEF { get => CharDEF; }

    float Char_CRT_DAMAGE;
    public float Get_Char_CRT_Damage { get => Char_CRT_DAMAGE; }

    float Char_CRT_RATE;
    public float Get_Char_CRT_Rate { get => Char_CRT_RATE; }

    string Illust_Address;
    public string Get_Illust_Address { get => Illust_Address; }
    
    string Normal_Image_Address;
    public string Get_Normal_Image_Address { get => Normal_Image_Address; }

    string Grade_Up_Image_Address;
    public string Get_Grade_Up_Image_Address { get => Grade_Up_Image_Address; }

    string Profile_Address;
    public string Get_Profile_Address { get => Profile_Address; }

    Sprite Illust_Img;
    public Sprite Get_Illust_Img { get => Illust_Img; }

    Sprite Normal_Img;
    public Sprite Get_Normal_Img { get => Normal_Img; }

    Sprite Grade_Up_Img;
    public Sprite Get_Grade_Up_Img { get => Grade_Up_Img; }

    Sprite Profile_Img;
    public Sprite Get_Profile_Img { get => Profile_Img; }

    public Character(int _id, string _name, CHAR_GRADE _grade, CHAR_TYPE _type, CHAR_ELE _ele,
       float _hp, float _atk, float _def, float _crtDamage, float _crtRate, string _illustAdd, string _normalImageAdd, string _gradeUpAdd, string _profileAdd)
    {
        // 캐릭터 능력치
        CharacterID = _id;
        CharName = _name;
        CharGrade = _grade;
        CharType = _type;
        CharElement = _ele;
        CharHP = _hp;
        CharATK = _atk;
        CharDEF = _def;
        Char_CRT_DAMAGE = _crtDamage;
        Char_CRT_RATE = _crtRate;

        // 이미지 주소
        Illust_Address = _illustAdd;
        Normal_Image_Address = _normalImageAdd;
        Grade_Up_Image_Address = _gradeUpAdd;
        Profile_Address = _profileAdd;

        Illust_Img = Resources.Load<Sprite>(Illust_Address);
        Normal_Img = Resources.Load<Sprite>(Normal_Image_Address);
        Grade_Up_Img = Resources.Load<Sprite>(Grade_Up_Image_Address);
        Profile_Img = Resources.Load<Sprite>(Profile_Address);

        //if (Illust_Img == null)
        //{
        //    Debug.Log("빔");
        //}
        //else
        //{
        //    Debug.Log("성공");    
        //}
    }
}
