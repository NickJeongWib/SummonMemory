using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.UI;
using System.Linq;

public class Character_List : MonoBehaviour
{
    GoogleSheetSO GoogleSheetSORef;

    public static List<Character> R_Char = new List<Character>();
    public static List<Character> SR_Char = new List<Character>();
    public static List<Character> SSR_Char = new List<Character>();

    public static List<int> Level = new List<int>();
    public static List<int> Require_Exp = new List<int>();
    public static List<int> Cumulative_Exp = new List<int>();

    // 스킬 데이터
    public List<Skill> SkillData_List = new List<Skill>();
    public List<Voice> VoiceList = new List<Voice>();

    private void Awake()
    {
        GoogleSheetSORef = GoogleSheetManager.SO<GoogleSheetSO>();

        #region Character_Level
        for (int i = 0; i < GoogleSheetSORef.Level_DBList.Count; i++)
        {
            Level.Add(GoogleSheetSORef.Level_DBList[i].LEVEL);
            Require_Exp.Add(GoogleSheetSORef.Level_DBList[i].REQUIRE_EXP);
            Cumulative_Exp.Add(GoogleSheetSORef.Level_DBList[i].CUMULATIVE_EXP);
        }
        #endregion

        #region Character_Skill
        for (int i = 0; i < GoogleSheetSORef.SKILL_DATAList.Count; i++)
        {
            BUFF_TYPE.TryParse(GoogleSheetSORef.SKILL_DATAList[i].BUFF_TYPE, out BUFF_TYPE BuffType);
            DEBUFF_TYPE.TryParse(GoogleSheetSORef.SKILL_DATAList[i].DEBUFF_TYPE, out DEBUFF_TYPE DeBuffType);
            SKILL_TYPE.TryParse(GoogleSheetSORef.SKILL_DATAList[i].SKILL_TYPE, out SKILL_TYPE SkillType);

            Skill node = new Skill(GoogleSheetSORef.SKILL_DATAList[i].SKILL_NAME, int.Parse(GoogleSheetSORef.SKILL_DATAList[i].SKILL_LV), SkillType, GoogleSheetSORef.SKILL_DATAList[i].SKILL_POINT,
                GoogleSheetSORef.SKILL_DATAList[i].TARGET_NUM, GoogleSheetSORef.SKILL_DATAList[i].DAMAGE_RATIO, GoogleSheetSORef.SKILL_DATAList[i].DEBUFF_RATIO, DeBuffType,
                GoogleSheetSORef.SKILL_DATAList[i].BUFF_RATIO, BuffType, GoogleSheetSORef.SKILL_DATAList[i].SP_HILL_COUNT, GoogleSheetSORef.SKILL_DATAList[i].BUFF_TIME,
                GoogleSheetSORef.SKILL_DATAList[i].SKILL_DESC, GoogleSheetSORef.SKILL_DATAList[i].SKILL_ICON, GoogleSheetSORef.SKILL_DATAList[i].SKILL_PREFAB,
                GoogleSheetSORef.SKILL_DATAList[i].NORMAL_ATK_RATIO, GoogleSheetSORef.SKILL_DATAList[i].NORMAL_ATK_DESC, GoogleSheetSORef.SKILL_DATAList[i].SKILL_SFX_PATH);

            SkillData_List.Add(node);
        }
        #endregion

        #region CharVoice
        for (int i = 0; i < GoogleSheetSORef.CHAR_VOICEList.Count; i++)
        {
            Voice voice = new Voice(GoogleSheetSORef.CHAR_VOICEList[i].SELECT_VOICE, GoogleSheetSORef.CHAR_VOICEList[i].USESKILL_VOICE);
            VoiceList.Add(voice);
        }
        #endregion

        #region Character_Data
        for (int i = 0; i < GoogleSheetSORef.Character_DBList.Count; i++)
        {
            CHAR_GRADE.TryParse(GoogleSheetSORef.Character_DBList[i].CHAR_GRADE, out CHAR_GRADE charGrade);
            CHAR_TYPE.TryParse(GoogleSheetSORef.Character_DBList[i].CHAR_TYPE, out CHAR_TYPE charType);
            CHAR_ELE.TryParse(GoogleSheetSORef.Character_DBList[i].CHAR_ELEMENT, out CHAR_ELE charEle);

            // TODO ## Character_List 캐릭터 데이터 저장
            Character Node = new Character(GoogleSheetSORef.Character_DBList[i].CHAR_ID, GoogleSheetSORef.Character_DBList[i].CHAR_NAME, GoogleSheetSORef.Character_DBList[i].CHAR_ENG_NAME,
               charGrade, charType, charEle, GoogleSheetSORef.Character_DBList[i].CHAR_STAR, GoogleSheetSORef.Character_DBList[i].CHAR_HP, GoogleSheetSORef.Character_DBList[i].CHAR_ATK, GoogleSheetSORef.Character_DBList[i].CHAR_DEF,
               GoogleSheetSORef.Character_DBList[i].CHAR_CRI_DAMAGE, GoogleSheetSORef.Character_DBList[i].CHAR_CRI_RATE);

            // TODO ## Character_List 이미지 리소스 저장
            Node.Load_Resources(GoogleSheetSORef.Character_Image_AddressList[i].CHAR_UI_PATH, GoogleSheetSORef.Character_Image_AddressList[i].CHAR_PREFAB_PATH, GoogleSheetSORef.Character_Image_AddressList[i].CHAR_ILLUST,
               GoogleSheetSORef.Character_Image_AddressList[i].CHAR_NORMAL_IMAGE, GoogleSheetSORef.Character_Image_AddressList[i].CHAR_GRADE_UP_IMAGE,
               GoogleSheetSORef.Character_Image_AddressList[i].CHAR_PROFILE_IMAGE, GoogleSheetSORef.Character_Image_AddressList[i].CHAR_WHITE_IMAGE, GoogleSheetSORef.Character_Image_AddressList[i].CHAR_PIXEL_IMAGE,
               GoogleSheetSORef.Character_Image_AddressList[i].CHAR_ICON_IMAGE, GoogleSheetSORef.Character_Image_AddressList[i].CHAR_BG_IMAGE, GoogleSheetSORef.Character_Image_AddressList[i].CHAR_ILLUST_SQUARE);

            Node.Load_Growing_State(GoogleSheetSORef.Character_Growing_StateList[i].LINEAR_FACTOR, GoogleSheetSORef.Character_Growing_StateList[i].TRANSITION_LEVEL);

            // 캐릭터에 스킬 데이터 넘겨주기
            Node.SkillData = SkillData_List[i];
            Node.VoicePath = VoiceList[i];

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
        #endregion

        GameManager.Inst.Get_CharMaxCount = GoogleSheetSORef.Character_DBList.Count;
    }
}
