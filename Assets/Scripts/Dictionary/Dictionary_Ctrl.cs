using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dictionary_Ctrl : MonoBehaviour
{
    #region
    [SerializeField] Character SelectChar;
    [SerializeField] GameObject CharacterPanel;
    [SerializeField] CharacterList_UI CharacterUI_Ref;
    #endregion

    #region UI
    [Header("UI_Var")]
    [SerializeField] Color[] FrameColors;
    [SerializeField] Color[] colors;
    [SerializeField] Color[] Transition_colors;
    public Sprite[] Elements;
    public Sprite[] ElementColors;
    public Sprite[] Elements_BG;
    public Sprite[] Grades;

    [Header("Slot")]
    [SerializeField] GameObject R_Char_Slot;
    [SerializeField] GameObject SR_Char_Slot;
    [SerializeField] GameObject SSR_Char_Slot;
    [SerializeField] Text Char_Count;
    public List<Dict_Slot> R_Slot = new List<Dict_Slot>();
    public List<Dict_Slot> SR_Slot = new List<Dict_Slot>();
    public List<Dict_Slot> SSR_Slot = new List<Dict_Slot>();

    [Header("UI")]
    [SerializeField] GameObject CharInfo_Panel;
    [SerializeField] Image Frame;
    [SerializeField] Image Character_Image;
    [SerializeField] Image Element_Back;
    [SerializeField] Image Char_Ele;
    [SerializeField] Image Char_Grade;
    [SerializeField] Text CharName_Lv;

    [Header("Transition")]
    [SerializeField] GameObject Circle_Transition;
    [SerializeField] MeshRenderer Transition_BG;
    [SerializeField] Text TransChar_Name;
    [SerializeField] Image TransChar_Image;
    [SerializeField] Image TransChar_Grade;
    [SerializeField] Image TransChar_Element;
    [SerializeField] Image TransChar_EleBG;
    [SerializeField] Image Trans_Deco;
    #endregion

    /// ------Func

    #region Click_Character_Grade
    public void On_Click_CharGrade_Btn(int _num)
    {
        // 3성 클릭
        if (_num == 0)
        {
            R_Char_Slot.SetActive(true);
            SR_Char_Slot.SetActive(false);
            SSR_Char_Slot.SetActive(false);
            Char_Count.text = $"{R_Count} / {Character_List.R_Char.Count}  ({(R_Count + SR_Count + SSR_Count)} / " +
              $"{(Character_List.R_Char.Count + Character_List.SR_Char.Count + Character_List.SSR_Char.Count)})";
        }
        // 4성 클릭
        else if (_num == 1)
        {
            R_Char_Slot.SetActive(false);
            SR_Char_Slot.SetActive(true);
            SSR_Char_Slot.SetActive(false);
            Char_Count.text = $"{SR_Count} / {Character_List.SR_Char.Count}  ({(R_Count + SR_Count + SSR_Count)} / " +
              $"{(Character_List.R_Char.Count + Character_List.SR_Char.Count + Character_List.SSR_Char.Count)})";
        }
        // 5성 클릭
        else
        {
            R_Char_Slot.SetActive(false);
            SR_Char_Slot.SetActive(false);
            SSR_Char_Slot.SetActive(true);
            Char_Count.text = $"{SSR_Count} / {Character_List.SSR_Char.Count}  ({(R_Count + SR_Count + SSR_Count)} / " +
                $"{(Character_List.R_Char.Count + Character_List.SR_Char.Count + Character_List.SSR_Char.Count)})";
        }
    }
    #endregion

    #region Hold_Character_Count
    int R_Count;
    int SR_Count;
    int SSR_Count;
    public void User_Char_Count()
    {
        R_Count = 0;
        SR_Count = 0;
        SSR_Count = 0;

        // 보유 등급 캐릭터 개수 확인
        foreach(var character in UserInfo.UserCharDict)
        {
            if (character.Value.Get_CharGrade == Define.CHAR_GRADE.R)
            {
                R_Count++;
            }
            else if (character.Value.Get_CharGrade == Define.CHAR_GRADE.SR)
            {
                SR_Count++;
            }
            else
            {
                SSR_Count++;
            }
        }

        // 텍스트 초기화
        Char_Count.text = $"{R_Count} / {Character_List.R_Char.Count}  ({(R_Count + SR_Count + SSR_Count)} / " +
             $"{(Character_List.R_Char.Count + Character_List.SR_Char.Count + Character_List.SSR_Char.Count)})";
    }
    #endregion

    #region Click_Char_Info
    public void Show_Char_Info(Character _char)
    {
        CharInfo_Panel.SetActive(true);
        SelectChar = _char;

        #region Char_Info_UI
        UserInfo.Get_Square_Image(Character_Image, _char);
        CharStar_Refresh(_char, Char_Grade);


        Char_Ele.sprite = Elements[(int)_char.Get_CharElement];
        Element_Back.sprite = Elements_BG[(int)_char.Get_CharElement];

        Frame.material.color = FrameColors[(int)_char.Get_CharElement];
        Char_Grade.color = colors[(int)_char.Get_CharElement];

        CharName_Lv.text = $"{_char.Get_CharName} Lv.1";
        // Char_Grade.rectTransform.sizeDelta = new Vector3(20 * _char.Get_CharStar, 20.0f, 0.0f);
        #endregion

        #region Transition
        CharStar_Refresh(_char, TransChar_Grade);

        Transition_BG.material.SetColor("_Color", colors[(int)_char.Get_CharElement]);
        TransChar_Name.text = $"{_char.Get_CharEngName}";

        TransChar_EleBG.sprite = Elements_BG[(int)_char.Get_CharElement];
        TransChar_Element.sprite = ElementColors[(int)_char.Get_CharElement];
        TransChar_Image.sprite = _char.Get_WhiteIllust_Img;

        TransChar_Name.color = Transition_colors[(int)_char.Get_CharElement];
        TransChar_Image.color = Transition_colors[(int)_char.Get_CharElement];
        TransChar_Grade.color = Transition_colors[(int)_char.Get_CharElement];
        Trans_Deco.color = Transition_colors[(int)_char.Get_CharElement];
        #endregion
    }
    #endregion

    #region Change_Star_UI
    void CharStar_Refresh(Character _char, Image _image)
    {
        if (_char.Get_CharGrade == Define.CHAR_GRADE.R)
        {
            _image.rectTransform.sizeDelta = new Vector2(96.0f, 30.0f);
        }
        else if (_char.Get_CharGrade == Define.CHAR_GRADE.SR)
        {
            _image.rectTransform.sizeDelta = new Vector2(128.0f, 30.0f);
        }
        else
        {
            _image.rectTransform.sizeDelta = new Vector2(160.0f, 30.0f);
        }
    }
    #endregion

    #region Clear_Select_CharS
    public void Clear_SelectChar()
    {
        if (SelectChar != null)
            SelectChar = null;
    }
    #endregion

    public void On_Click_Character_Info()
    {
        #region Off_this_Panel
        On_Click_CharGrade_Btn(0);
        CharInfo_Panel.SetActive(false);
        this.gameObject.SetActive(false);
        #endregion

        Circle_Transition.SetActive(true);
        CharacterUI_Ref.CharInfo_From_Dict(true);
        CharacterUI_Ref.On_Click_CharInfo(SelectChar);
        CharacterPanel.SetActive(true);
    }
}
