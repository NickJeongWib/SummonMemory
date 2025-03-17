using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{
    // string 캐릭터 이름 / Character 캐릭터 데이터
    public static Dictionary<string, Character> UserCharDict = new Dictionary<string, Character>();
    // public static Dictionary<string, Character> UserCharDict_Copy = new Dictionary<string, Character>();
    public static List<KeyValuePair<string, Character>> UserCharDict_Copy;

    public static List<Character> Equip_Characters = new List<Character>();

    [Header("---Currency---")]
    public static int Money;
    public static int Dia;
    public static int EnterHealth;

    public static int R_Book;
    public static int SR_Book;
    public static int SSR_Book;


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
