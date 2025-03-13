using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.UI;

public class Character_List : MonoBehaviour
{
    [SerializeField] GoogleSheetSO GoogleSheetSORef;

    public static List<Character> R_Char = new List<Character>();
    public static List<Character> SR_Char = new List<Character>();
    public static List<Character> SSR_Char = new List<Character>();

    private void Start()
    {
        for (int i = 0; i < GoogleSheetSORef.Character_DBList.Count; i++)
        {
            CHAR_GRADE.TryParse(GoogleSheetSORef.Character_DBList[i].CHAR_GRADE, out CHAR_GRADE charGrade);
            CHAR_TYPE.TryParse(GoogleSheetSORef.Character_DBList[i].CHAR_TYPE, out CHAR_TYPE charType);
            CHAR_ELE.TryParse(GoogleSheetSORef.Character_DBList[i].CHAR_ELEMENT, out CHAR_ELE charEle);

            //TODO ## Character_List 캐릭터 데이터 저장
            Character Node = new Character(GoogleSheetSORef.Character_DBList[i].CHAR_ID, GoogleSheetSORef.Character_DBList[i].CHAR_NAME, GoogleSheetSORef.Character_DBList[i].CHAR_ENG_NAME,
               charGrade, charType, charEle, GoogleSheetSORef.Character_DBList[i].CHAR_STAR, GoogleSheetSORef.Character_DBList[i].CHAR_HP, GoogleSheetSORef.Character_DBList[i].CHAR_ATK, GoogleSheetSORef.Character_DBList[i].CHAR_DEF,
               GoogleSheetSORef.Character_DBList[i].CHAR_CRI_DAMAGE, GoogleSheetSORef.Character_DBList[i].CHAR_CRI_RATE);

            // TODO ## Character_List 이미지 리소스 저장
            Node.Load_Resources(GoogleSheetSORef.Character_Image_AddressList[i].CHAR_ILLUST,
               GoogleSheetSORef.Character_Image_AddressList[i].CHAR_NORMAL_IMAGE, GoogleSheetSORef.Character_Image_AddressList[i].CHAR_GRADE_UP_IMAGE,
               GoogleSheetSORef.Character_Image_AddressList[i].CHAR_PROFILE_IMAGE, GoogleSheetSORef.Character_Image_AddressList[i].CHAR_WHITE_IMAGE);

            #region 이미지 누락 경고
            if (Node.Get_Grade_Up_Img == null)
            {
                Debug.Log($"{GoogleSheetSORef.Character_DBList[i].CHAR_NAME}의 업그레이드 이미지가 없습니다");
            }
            else if (Node.Get_Illust_Img == null)
            {
                Debug.Log($"{GoogleSheetSORef.Character_DBList[i].CHAR_NAME}의 일러스트 이미지가 없습니다");
            }
            else if (Node.Get_Normal_Img == null)
            {
                Debug.Log($"{GoogleSheetSORef.Character_DBList[i].CHAR_NAME}의 노말 이미지가 없습니다");
            }
            else if (Node.Get_Profile_Img == null)
            {
                Debug.Log($"{GoogleSheetSORef.Character_DBList[i].CHAR_NAME}의 프로필 이미지가 없습니다");
            }
            #endregion

            if (Node.Get_CharGrade == CHAR_GRADE.R)
            {
                R_Char.Add(Node);
            }
            else if (Node.Get_CharGrade == CHAR_GRADE.SR)
            {
                SR_Char.Add(Node);
            }
            else if (Node.Get_CharGrade == CHAR_GRADE.SSR)
            {
                SSR_Char.Add(Node);
            }
        }
    }

}
