using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby_Manager : MonoBehaviour
{
    [Header("---Transition---")]
    [SerializeField] GameObject CircleTransition;
    [SerializeField] GameObject ShaderTransition;
    [SerializeField] GameObject NotTouch_RayCast;

    [Header("---GachaMovie---")]
    [SerializeField] GachaVideo gachaVideo;

    [Header("---Gacha_CharacterList---")]
    [SerializeField] GameObject Gacha_10;
    [SerializeField] GameObject Gacha_1;
    [SerializeField] GameObject[] Book_Images;
    [SerializeField] GameObject Book_Image;

    [Header("---CharacterList---")]
    [SerializeField] CharImg_Anim CharImg_Anim_Ref;
    [SerializeField] CharacterList_UI CharacterList_UI_Ref;
    [SerializeField] Image Transition_Element_BG;
    [SerializeField] Image Transition_ElementCol;
    [SerializeField] Image Transition_White_Char;
    [SerializeField] Text Transition_Char_Name;
    [SerializeField] Image Transition_Grade;
    [SerializeField] Image Transition_Grade_Deco;
    [SerializeField] GameObject CharacterInfo_Panel;
    [SerializeField] GameObject CharacterInfo_Transition;
    [SerializeField] Color[] colors;
    [SerializeField] Color[] Transition_colors;

    [Header("---CharacterList_Equip---")]
    [SerializeField] int MaxEquip_Count;
    [SerializeField] GameObject Change_Char_Btn;    // 캐릭터 교체 창 버튼
    [SerializeField] GameObject Equip_Info_Btn;     // 캐릭터 교체 취소, 확인 버튼 모음
    [SerializeField] Image[] Equip_Char_Img;
    [SerializeField] GameObject[] UnEquip_Btn;

    [Header("---CharacterList_Info---")]
    public string[] Type_Kor_Str;                   // 한글로 타입 표기 위한 string타입 배열
    public string[] Element_Kor_Str;                // 한글로 속성 표기 위한 string타입 배열
    // 캐릭터 정보 관련
    [SerializeField] Image CharInfo_Img;            // 캐릭터 정보 창 캐릭터 이미지 텍스트
    [SerializeField] Text CharInfo_Name_Txt;        // 캐릭터 정보 창 캐릭터 이름 텍스트
    [SerializeField] Text CharInfo_Lv_Txt;          // 캐릭터 정보 창 캐릭터 레벨 텍스트 
    [SerializeField] Text CharInfo_Star_Txt;        // 캐릭터 정보 창 캐릭터 성급 텍스트
    [SerializeField] Text CharInfo_Type_Txt;        // 캐릭터 정보 창 캐릭터 타입 텍스트
    [SerializeField] Text CharInfo_Element_Txt;     // 캐릭터 정보 창 캐릭터 속성 텍스트
    [SerializeField] Text CharInfo_MaxHP_Txt;       // 캐릭터 정보 창 캐릭터 최대체력 텍스트
    [SerializeField] Text CharInfo_Atk_Txt;         // 캐릭터 정보 창 캐릭터 공격력 텍스트
    [SerializeField] Text CharInfo_Def_Txt;         // 캐릭터 정보 창 캐릭터 방어력 텍스트
    [SerializeField] Text CharInfo_CrtDmg_Txt;      // 캐릭터 정보 창 캐릭터 치피 텍스트
    [SerializeField] Text CharInfo_CrtRate_Txt;     // 캐릭터 정보 창 캐릭터 치확 텍스트

    private void Start()
    {
        // TODO ## 초기 테스트 값
        Equip_Image_Refresh(false);
    }

    #region Shader_Graph_Transition
    // 상점, 도감, 업적 등 여러 창으로 이동
    public void On_Click_OnPanel(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // 이동 중 버튼 클릭 방지
        ShaderTransition.SetActive(true);
        _obj.SetActive(true);
    }

    // 현재 열려 있는 창을 닫고 로비로 이동
    public void On_Click_OffPanel(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // 화면전환 중 버튼 클릭 방지
        ShaderTransition.SetActive(true);
        _obj.SetActive(false);
    }
    #endregion

    #region Circle_Transition
    public void On_Click_OnPanel_Circle(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // 이동 중 버튼 클릭 방지
        CircleTransition.SetActive(true);
        _obj.SetActive(true);
    }

    // 현재 열려 있는 창을 닫고 로비로 이동
    public void On_Click_OffPanel_Circle(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // 화면전환 중 버튼 클릭 방지
        CircleTransition.SetActive(true);
        _obj.SetActive(false);
    }
    #endregion

    public void On_Click_Back(GameObject _obj)
    {
        _obj.SetActive(false);
    }

    public void On_Click_On(GameObject _obj)
    {
        _obj.SetActive(true);
    }


    #region Gacha_UI_Active

    public void On_Click_Skip_GachaMovie(GameObject _obj)
    {
        // 가챠 연출 스킵
        _obj.SetActive(false);
        _obj.transform.parent.gameObject.SetActive(false);

        gachaVideo.Get_VideoPlayer.Stop();
    }

    public void On_Click_Back_GachaList(GameObject _obj)
    {
        // 가챠 뽑기 목록 닫기
        Gacha_10.SetActive(false);
        Gacha_1.SetActive(false);

        // 책 재료 팝업 닫기
        for (int i = 0; i < Book_Images.Length; i++)
        {
            Book_Images[i].SetActive(false);
        }
        Book_Image.SetActive(false);

        _obj.SetActive(false);
    }
    #endregion

    #region CharacterList_UI
    public void On_Click_CharInfo(CharacterSlot _slot)
    {
        #region Character_Transition_Set
        // TODO ## Lobby_Manager 캐릭터 정보 창 이동 트랜지션 이미지 초기화 구문
        // 화면전환 중 버튼 클릭 방지
        NotTouch_RayCast.SetActive(true); 
        CharacterInfo_Panel.SetActive(true);
        // MeshRenderer의 머티리얼에 접근해 Color변수의 레퍼타입에 접근해서 색상 변경
        CharacterInfo_Transition.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[(int)_slot.character.Get_CharElement]);

        // 화면 전환 화면의 UI 선택한 캐릭터 속성 색으로 변경
        Transition_Char_Name.color = colors[(int)_slot.character.Get_CharElement];
        Transition_Grade.color = colors[(int)_slot.character.Get_CharElement];
        Transition_Grade_Deco.color = colors[(int)_slot.character.Get_CharElement];

        Transition_White_Char.color = Transition_colors[(int)_slot.character.Get_CharElement];

        Transition_ElementCol.sprite = CharacterList_UI_Ref.ElementColors[(int)_slot.character.Get_CharElement];
        Transition_Element_BG.sprite = CharacterList_UI_Ref.Elements_BG[(int)_slot.character.Get_CharElement];
        Transition_White_Char.sprite = _slot.character.Get_WhiteIllust_Img;

        // 캐릭터의 영문 이름 표시
        Transition_Char_Name.text = _slot.character.Get_CharEngName;
        CharInfo_Img.sprite = _slot.character.Get_Normal_Img;
        #endregion

        #region Character_Info_Text_Refresh
        // TODO ## Lobby_Manager 캐릭터 정보 창 캐릭터 정보 초기화
        CharInfo_Name_Txt.text = $"이름 : {_slot.character.Get_CharName}";
        CharInfo_Lv_Txt.text = $"레벨 : {_slot.character.Get_Character_Lv}";
        CharInfo_Star_Txt.text = $"성급 : {_slot.character.Get_CharStar}성";
        CharInfo_Type_Txt.text = $"타입 : {Type_Kor_Str[(int)_slot.character.Get_CharType]}";
        CharInfo_Element_Txt.text = $"속성 : {Element_Kor_Str[(int)_slot.character.Get_CharElement]}";
        CharInfo_MaxHP_Txt.text = $"체력 : {_slot.character.Get_CharHP}";
        CharInfo_Atk_Txt.text = $"공격력 : {_slot.character.Get_CharATK}";
        CharInfo_Def_Txt.text = $"방어력 : {_slot.character.Get_CharDEF}";
        CharInfo_CrtDmg_Txt.text = $"치명타피해 : {(_slot.character.Get_Char_CRT_Damage * 100.0f).ToString("N1")}%";
        CharInfo_CrtRate_Txt.text = $"치명타확률 : {(_slot.character.Get_Char_CRT_Rate * 100.0f).ToString("N1")}%";
        #endregion

        // 등급에 따른 다이아 이미지 차별화
        if (_slot.character.Get_CharGrade == Define.CHAR_GRADE.R)
        {
            Transition_Grade.rectTransform.sizeDelta = new Vector2(96.0f, 30.0f);
        }
        else if (_slot.character.Get_CharGrade == Define.CHAR_GRADE.SR)
        {
            Transition_Grade.rectTransform.sizeDelta = new Vector2(128.0f, 30.0f);
        }
        else
        {
            Transition_Grade.rectTransform.sizeDelta = new Vector2(160.0f, 30.0f);
        }
    }
    #endregion

    #region CharacterList_Equip_Btn
    // TODO ## Lobby_Manager 캐릭터 교체를 위한 버튼 작동
    public void On_Click_Change()
    {
        Change_Char_Btn.SetActive(false);
        Equip_Info_Btn.SetActive(true);

        Equip_Char_Btn(true);
    }

    public void On_Click_ChangeCancel()
    {
        // 장착해제 버튼 비활성화
        for (int i = 0; i < UnEquip_Btn.Length; i++)
        {
            UnEquip_Btn[i].SetActive(false);
        }
        // 캐릭터 교체버튼 활성화, 교체 자동배치 버튼 비활성화

        Equip_Char_Btn(false);
        Change_Char_Btn.SetActive(true);
        Equip_Info_Btn.SetActive(false);
    }

    [SerializeField] int Equip_Count;
    // 캐릭터 장착 버튼 활성화
    void Equip_Char_Btn(bool _bool)
    {
        if (UserInfo.UserCharDict.Count <= 0)
            return;

        for (int i = 0; i < UserInfo.UserCharDict_Copy.Count; i++)
        {
            CharacterList_UI_Ref.Slots[i].Equip_Btn.gameObject.SetActive(_bool);
            CharacterList_UI_Ref.Slots[i].Select_Btn.interactable = !_bool;
        }

        // 장착 시 장착 캐릭터칸 위에 캐릭터를 뺄 수 있게 
        Equip_Count = 0;
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            Equip_Count = i;
            UnEquip_Btn[i].SetActive(_bool);
        }
    }

    // TODO ## Lobby_Manager 캐릭터 장착 해제 시 UI 초기화
    void Equip_Image_Refresh(bool _bool)
    {
        int EquipNum = 0;

        // 캐릭터가 장착된 리스트의 크기만 만큼 반복
        for (int on = 0; on < UserInfo.Equip_Characters.Count; on++)
        {
            Equip_Char_Img[on].color = Color.white;

            // R등급 캐릭터는 따로 Lobby이미지가 없기 때문에 
            UserInfo.Get_Square_Image(Equip_Char_Img, on);

            if (_bool)
            {
                UnEquip_Btn[on].SetActive(true);
            }

            EquipNum = on + 1;
        }

        // 리스트를 돌고 남은 값부터 시작해서 최대 장착크기인 5까지 반복
        for (int off = EquipNum; off < MaxEquip_Count; off++)
        {
            // 이미지를 꺼준다
            Equip_Char_Img[off].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Equip_Char_Img[off].sprite = null;
        }
    }
    #endregion

    #region Character Equip_UnEquip
    // TODO ## Lobby_Manager 캐릭터 해제 작동 부분
    public void On_Click_UnEquip_Char(int _equipNum)
    {
        if (UserInfo.Equip_Characters.Count <= 1)
        {
            Debug.Log("캐릭터 하나는 유지");
            return;
        }

        UserInfo.UserCharDict_Copy.Add(
            new KeyValuePair<string, Character>(UserInfo.Equip_Characters[_equipNum].Get_CharName, UserInfo.Equip_Characters[_equipNum]));

        UserInfo.Equip_Characters.RemoveAt(_equipNum);

        // 장착해제 버튼 비활성화
        for (int i = 0; i < MaxEquip_Count; i++)
        {
            if (i < UserInfo.Equip_Characters.Count)
            {
                UnEquip_Btn[i].SetActive(true);
            }
            else if (UserInfo.Equip_Characters.Count <= i)
            {
                UnEquip_Btn[i].SetActive(false);
            }
        }

        CharImg_Anim_Ref.Get_ImageIndex = 0;
        CharImg_Anim_Ref.R_SR_Image_Change(CharImg_Anim_Ref.Get_ImageIndex);
        // 장착한 캐릭터에 따라 이미지 교체 함수
        CharImg_Anim_Ref.CharImage_ChangeAnimF();
        CharacterList_UI_Ref.Refresh_CharacterList();
        CharacterList_UI_Ref.Refresh_Equip_Btn();
        Equip_Image_Refresh(true);
    }

    // TODO ## Lobby_Manager 캐릭터 장착 작동 부분
    public void On_Click_EquipChar(CharacterSlot _slot)
    {   
        if (5 <= UserInfo.Equip_Characters.Count)
        {
            Debug.Log("장착 슬롯 꽉");
            return;
        }

        // 캐릭터 장착 리스트에 추가
        UserInfo.Equip_Characters.Add(_slot.character);
        UserInfo.UserCharDict_Copy.RemoveAt(_slot.Slot_Num);

        CharImg_Anim_Ref.Get_ImageIndex = 0;
        CharImg_Anim_Ref.R_SR_Image_Change(CharImg_Anim_Ref.Get_ImageIndex);;

        // 장착한 캐릭터에 따라 이미지 교체 함수
        CharImg_Anim_Ref.CharImage_ChangeAnimF();
        // 장착 캐릭터 슬롯 초기화
        CharacterList_UI_Ref.Refresh_CharacterList();
        Equip_Image_Refresh(true);
    }
    #endregion
}
