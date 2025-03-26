using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class CharacterList_UI : MonoBehaviour
{
    [SerializeField] Lobby_Manager LobbyMgr_Ref;

    public List<CharacterSlot> Slots = new List<CharacterSlot>();
    public Sprite[] Elements;
    public Sprite[] ElementColors;
    public Sprite[] Elements_BG;
    public Sprite[] Grades;

    [Header("---CharacterList---")]
    [SerializeField] CharImg_Anim CharImg_Anim_Ref;
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
    [SerializeField] List<Equip_Slot> EquipSlot_List;
    [SerializeField] Button[] EquipChar_Select_Btn;
    [SerializeField] int MaxEquip_Count;
    [SerializeField] GameObject Change_Char_Btn;    // 캐릭터 교체 창 버튼
    [SerializeField] GameObject Equip_Info_Btn;     // 캐릭터 교체 취소, 확인 버튼 모음
    [SerializeField] Image[] Equip_Char_Img;
    [SerializeField] GameObject[] UnEquip_Btn;

    [Header("---CharacterList_Info---")]
    [SerializeField] Scrollbar Select_Scroll;
    public string[] Type_Kor_Str;                   // 한글로 타입 표기 위한 string타입 배열
    public string[] Element_Kor_Str;                // 한글로 속성 표기 위한 string타입 배열

    #region 정보 관련
    // 캐릭터 정보 관련
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
    #endregion

    [Header("---Character_Info---")]
    [SerializeField] Color[] FrameColors;
    [SerializeField] List<CharInfo_CharSelect_Btn> Info_Char_Slot;
    [SerializeField] Image CharInfo_Frame;          // 캐릭터 창 프레임 이미지
    [SerializeField] Image CharInfo_Ele_BG;         // 캐릭터 창 속성 배경
    [SerializeField] Image CharElement_Img;         // 캐릭터 창 속성
    [SerializeField] Image CharInfo_Img;            // 캐릭터 창 캐릭터 이미지
    [SerializeField] Image CharStar_Img;            // 캐릭터 창 성급
    [SerializeField] Text CharInfo_name;            // 캐릭터 창 이름


    [SerializeField] ScrollRect scrollRect; // ScrollRect 참조
    [SerializeField] RectTransform content; // 캐릭터 리스트(Content)
    [SerializeField] GridLayoutGroup gridLayoutGroup; // GridLayoutGroup 참조

    private void Start()
    {
        // TODO ## 초기 테스트 값
        Equip_Image_Refresh(false);
    }

    #region 보유 Character창 Refresh
    /// <summary>
    /// 캐릭터의 정보가 레벨이나 수치 변경 시 한번 호출 해 줄 필요가 있음
    /// </summary> 
    // TODO ## CharacterList_UI 캐릭터창 Refresh
    public void Refresh_CharacterList()
    {
        int SlotNum = 0;

        foreach(KeyValuePair<string, Character> Dict in UserInfo.UserCharDict_Copy)
        {
            // 이미지들 꺼져 있다면 켜주기
            if (!Slots[SlotNum].Element_BG.IsActive())
            {
                Slots[SlotNum].Element_BG.gameObject.SetActive(true);
            }
            if (!Slots[SlotNum].Element_Image.IsActive())
            {
                Slots[SlotNum].Element_Image.gameObject.SetActive(true);
            }
            if (!Slots[SlotNum].Char_Porfile.IsActive())
            {
                Slots[SlotNum].Char_Porfile.gameObject.SetActive(true);
            }
            if (!Slots[SlotNum].Star_Image.IsActive())
            {
                Slots[SlotNum].Star_Image.gameObject.SetActive(true);
            }
            if (!Slots[SlotNum].Grade_Image.IsActive())
            {
                Slots[SlotNum].Grade_Image.gameObject.SetActive(true);
            }
            if(!Slots[SlotNum].Select_Btn.IsActive())
            {
                Slots[SlotNum].Select_Btn.gameObject.SetActive(true);
            }

            Slots[SlotNum].Slot_Num = SlotNum;
            Slots[SlotNum].character = Dict.Value;
            Slots[SlotNum].Grade_Image.sprite = Grades[(int)Dict.Value.Get_CharGrade];
            Slots[SlotNum].Element_BG.sprite = Elements_BG[(int)Dict.Value.Get_CharElement];
            Slots[SlotNum].Element_Image.sprite = Elements[(int)Dict.Value.Get_CharElement];

            Slots[SlotNum].Char_Porfile.sprite = Dict.Value.Get_Profile_Img;
            Slots[SlotNum].Star_Image.rectTransform.sizeDelta = new Vector2(20 * Dict.Value.Get_CharStar, 20.0f);
            // Debug.Log(SlotNum + " " + Dict.Value.Get_CharName + " " + Dict.Value.Get_CharStar);

           SlotNum++;
        }

        for (int i = SlotNum; i < Slots.Count; i++)
        {
            Slots[SlotNum].character = null;

            // 이미지들 켜져 있다면 꺼주기
            if (Slots[SlotNum].Element_BG.IsActive())
            {
                Slots[SlotNum].Element_BG.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Element_Image.IsActive())
            {
                Slots[SlotNum].Element_Image.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Char_Porfile.IsActive())
            {
                Slots[SlotNum].Char_Porfile.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Star_Image.IsActive())
            {
                Slots[SlotNum].Star_Image.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Grade_Image.IsActive())
            {
                Slots[SlotNum].Grade_Image.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Select_Btn.IsActive())
            {
                Slots[SlotNum].Select_Btn.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Equip_Btn.IsActive())
            {
                Slots[SlotNum].Equip_Btn.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region 캐릭터 장착 시 복사된 리스트에서 삭제 후 EquipCharacter리스트로 이동
    // 캐릭터 장착 시 보유 캐릭터 창에서 장착창으로 이동
    public void Remove_List_Char()
    {
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            if (UserInfo.UserCharDict_Copy.Contains(new KeyValuePair<string, Character>(UserInfo.Equip_Characters[i].Get_CharName, UserInfo.Equip_Characters[i])))
            {
                UserInfo.UserCharDict_Copy.Remove(new KeyValuePair<string, Character>(UserInfo.Equip_Characters[i].Get_CharName, UserInfo.Equip_Characters[i]));
            }
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
        Refresh_CharacterList();
        Refresh_Equip_Btn();
        Equip_Image_Refresh(true);
        Interact_EquipSlot_Btn();
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
        
        // 장착 캐릭터 슬롯 초기화
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            EquipSlot_List[i].EquipCharacter = UserInfo.Equip_Characters[i];
        }

        UserInfo.UserCharDict_Copy.RemoveAt(_slot.Slot_Num);

        CharImg_Anim_Ref.Get_ImageIndex = 0;
        CharImg_Anim_Ref.R_SR_Image_Change(CharImg_Anim_Ref.Get_ImageIndex); ;

        // 장착한 캐릭터에 따라 이미지 교체 함수
        CharImg_Anim_Ref.CharImage_ChangeAnimF();
        // 장착 캐릭터 슬롯 초기화
        Refresh_CharacterList();
        Equip_Image_Refresh(true);
        Interact_EquipSlot_Btn();
    }
    #endregion

    #region CharacterList_Equip_Btn
    // 캐릭터 교체 버튼 활성화
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
        // 캐릭터가 없다면 return
        if (UserInfo.UserCharDict.Count <= 0)
            return;

        // 캐릭터 장착 버튼 활성 비활성 전환
        for (int i = 0; i < UserInfo.UserCharDict_Copy.Count; i++)
        {
            Slots[i].Equip_Btn.gameObject.SetActive(_bool);
            Slots[i].Select_Btn.interactable = !_bool;
        }

        // 장착 시 장착 캐릭터 칸 위에 캐릭터를 뺄 수 있게 
        Equip_Count = 0;
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            Equip_Count = i;
            // 캐릭터 해제버튼 활성화
            UnEquip_Btn[i].SetActive(_bool);
            // 캐릭터 선택버튼 비활성화
            EquipChar_Select_Btn[i].interactable = !_bool;
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

    // 캐릭터교체 버튼 클릭 시 장착버튼 활성화
    public void Refresh_Equip_Btn()
    {
        int SlotNum = 0;

        foreach (KeyValuePair<string, Character> Dict in UserInfo.UserCharDict_Copy)
        {
            if (!Slots[SlotNum].Equip_Btn.IsActive())
            {
                Slots[SlotNum].Equip_Btn.gameObject.SetActive(true);
            }

            SlotNum++;
        }
    }
    #endregion

    #region CharacterList_UI
    // 캐릭터 장착 슬롯 버튼 활성화
    void Interact_EquipSlot_Btn()
    {
        int count = 0;

        // 만약 캐릭터 교체 버튼을 활성화 시 캐릭터정보창으로 이동 할 수 있는 버튼을 활성화 시키고
        // 클릭이 안되게 interactable를 비활성화 시킨다.
        for (int on = 0; on < UserInfo.Equip_Characters.Count; on++)
        {
            EquipSlot_List[on].SelectBtn.gameObject.SetActive(true);
            EquipSlot_List[on].SelectBtn.interactable = false;
            count = on;
        }

        // 캐릭터 장착 수 외 나머지 칸은 버튼을 비활성화 시킨다.
        for (int off = count + 1; off < EquipSlot_List.Count; off++)
        {
            EquipSlot_List[off].SelectBtn.gameObject.SetActive(false);
        }
    }

    // 캐릭터 슬롯에서 클릭 시 캐릭터 정보 창으로 이동
    public void On_Click_CharInfo(CharacterSlot _slot)
    {
        #region Character_Transition_Set
        // TODO ## Lobby_Manager 캐릭터 정보 창 이동 트랜지션 이미지 초기화 구문
        // 화면전환 중 버튼 클릭 방지
        LobbyMgr_Ref.Get_NotTouch_RayCast.SetActive(true);
        CharacterInfo_Panel.SetActive(true);
        // MeshRenderer의 머티리얼에 접근해 Color변수의 레퍼타입에 접근해서 색상 변경
        CharacterInfo_Transition.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[(int)_slot.character.Get_CharElement]);

        // 화면 전환 화면의 UI 선택한 캐릭터 속성 색으로 변경
        Transition_Char_Name.color = colors[(int)_slot.character.Get_CharElement];
        Transition_Grade.color = colors[(int)_slot.character.Get_CharElement];
        Transition_Grade_Deco.color = colors[(int)_slot.character.Get_CharElement];

        Transition_White_Char.color = Transition_colors[(int)_slot.character.Get_CharElement];

        Transition_ElementCol.sprite = ElementColors[(int)_slot.character.Get_CharElement];
        Transition_Element_BG.sprite = Elements_BG[(int)_slot.character.Get_CharElement];
        Transition_White_Char.sprite = _slot.character.Get_WhiteIllust_Img;

        // 캐릭터의 영문 이름 표시
        Transition_Char_Name.text = _slot.character.Get_CharEngName;
        CharStar_Refresh(_slot.character, Transition_Grade);

        CharInfo_Select_Btn(_slot.character);
        #endregion

        #region Character_Info_Change
        // 캐릭터 정보창 이미지 변경
        UserInfo.Get_Square_Image(CharInfo_Img, _slot.character);
        CharInfo_Ele_BG.sprite = Elements_BG[(int)_slot.character.Get_CharElement];
        CharInfo_Frame.material.color = FrameColors[(int)_slot.character.Get_CharElement];
        CharElement_Img.sprite = ElementColors[(int)_slot.character.Get_CharElement];
        CharInfo_name.text = $"{_slot.character.Get_CharName}  Lv.{_slot.character.Get_Character_Lv}";
        // 등급에 따른 다이아 이미지 차별화
        CharStar_Refresh(_slot.character, CharStar_Img);
        CharStar_Img.color = colors[(int)_slot.character.Get_CharElement];
        #endregion

        Refresh_Select_Img(_slot.character, false);

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
    }

    // 장착 캐릭터 클릭 시 이동 오버로딩
    public void On_Click_CharInfo(Equip_Slot _slot)
    {
        #region Character_Transition_Set
        // TODO ## Lobby_Manager 캐릭터 정보 창 이동 트랜지션 이미지 초기화 구문
        // 화면전환 중 버튼 클릭 방지
        LobbyMgr_Ref.Get_NotTouch_RayCast.SetActive(true);
        CharacterInfo_Panel.SetActive(true);
        // MeshRenderer의 머티리얼에 접근해 Color변수의 레퍼타입에 접근해서 색상 변경
        CharacterInfo_Transition.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[(int)_slot.EquipCharacter.Get_CharElement]);

        // 화면 전환 화면의 UI 선택한 캐릭터 속성 색으로 변경
        Transition_Char_Name.color = colors[(int)_slot.EquipCharacter.Get_CharElement];
        Transition_Grade.color = colors[(int)_slot.EquipCharacter.Get_CharElement];
        Transition_Grade_Deco.color = colors[(int)_slot.EquipCharacter.Get_CharElement];
        Transition_White_Char.color = Transition_colors[(int)_slot.EquipCharacter.Get_CharElement];

        Transition_ElementCol.sprite = ElementColors[(int)_slot.EquipCharacter.Get_CharElement];
        Transition_Element_BG.sprite = Elements_BG[(int)_slot.EquipCharacter.Get_CharElement];
        Transition_White_Char.sprite = _slot.EquipCharacter.Get_WhiteIllust_Img;

        // 캐릭터의 영문 이름 표시
        Transition_Char_Name.text = _slot.EquipCharacter.Get_CharEngName;
        CharStar_Refresh(_slot.EquipCharacter, Transition_Grade);

        CharInfo_Select_Btn(_slot.EquipCharacter);
        #endregion

        #region Character_Info_Change
        // 캐릭터 정보창 이미지 변경
        UserInfo.Get_Square_Image(CharInfo_Img, _slot.EquipCharacter);
        CharInfo_Ele_BG.sprite = Elements_BG[(int)_slot.EquipCharacter.Get_CharElement];
        CharInfo_Frame.material.color = FrameColors[(int)_slot.EquipCharacter.Get_CharElement];
        CharElement_Img.sprite = ElementColors[(int)_slot.EquipCharacter.Get_CharElement];
        CharInfo_name.text = $"{_slot.EquipCharacter.Get_CharName}  Lv.{_slot.EquipCharacter.Get_Character_Lv}";
        // 등급에 따른 다이아 이미지 차별화
        CharStar_Refresh(_slot.EquipCharacter, CharStar_Img);
        CharStar_Img.color = colors[(int)_slot.EquipCharacter.Get_CharElement];
        #endregion

        Refresh_Select_Img(_slot.EquipCharacter, false);

        #region Character_Info_Text_Refresh
        // TODO ## Lobby_Manager 캐릭터 정보 창 캐릭터 정보 초기화
        CharInfo_Name_Txt.text = $"이름 : {_slot.EquipCharacter.Get_CharName}";
        CharInfo_Lv_Txt.text = $"레벨 : {_slot.EquipCharacter.Get_Character_Lv}";
        CharInfo_Star_Txt.text = $"성급 : {_slot.EquipCharacter.Get_CharStar}성";
        CharInfo_Type_Txt.text = $"타입 : {Type_Kor_Str[(int)_slot.EquipCharacter.Get_CharType]}";
        CharInfo_Element_Txt.text = $"속성 : {Element_Kor_Str[(int)_slot.EquipCharacter.Get_CharElement]}";
        CharInfo_MaxHP_Txt.text = $"체력 : {_slot.EquipCharacter.Get_CharHP}";
        CharInfo_Atk_Txt.text = $"공격력 : {_slot.EquipCharacter.Get_CharATK}";
        CharInfo_Def_Txt.text = $"방어력 : {_slot.EquipCharacter.Get_CharDEF}";
        CharInfo_CrtDmg_Txt.text = $"치명타피해 : {(_slot.EquipCharacter.Get_Char_CRT_Damage * 100.0f).ToString("N1")}%";
        CharInfo_CrtRate_Txt.text = $"치명타확률 : {(_slot.EquipCharacter.Get_Char_CRT_Rate * 100.0f).ToString("N1")}%";
        #endregion
    }

    // 장착 캐릭터 클릭 시 이동 오버로딩
    public void On_Click_CharInfo(CharInfo_CharSelect_Btn _slot)
    {

        Refresh_Select_Img(_slot.character);

        #region Character_Info_Change
        // 캐릭터 정보창 이미지 변경
        UserInfo.Get_Square_Image(CharInfo_Img, _slot.character);
        CharInfo_Ele_BG.sprite = Elements_BG[(int)_slot.character.Get_CharElement];
        CharInfo_Frame.material.color = FrameColors[(int)_slot.character.Get_CharElement];
        CharElement_Img.sprite = ElementColors[(int)_slot.character.Get_CharElement];
        CharInfo_name.text = $"{_slot.character.Get_CharName}  Lv.{_slot.character.Get_Character_Lv}";
        // 등급에 따른 다이아 이미지 차별화
        CharStar_Refresh(_slot.character, CharStar_Img);
        CharStar_Img.color = colors[(int)_slot.character.Get_CharElement];
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
    }

    void Refresh_Select_Img(Character _char, bool _isInfoScene = true)
    {
        int CharNum = 0;

        // 캐릭터 모아둔 리스트만큼 반복
        for (int i = 0; i < UserInfo.UserCharDict_Copy_2.Count; i++)
        {
            // _slot의 character의 id가 UserInfo.UserCharDict_Copy_2[i].Value.Get_CharacterID와 같다면 선택된 이미지 활성화
            if (_char.Get_CharacterID == Info_Char_Slot[i].character.Get_CharacterID)
            {
                Info_Char_Slot[i].Select_Img.gameObject.SetActive(true);
                CharNum = i;
            }
            else // _slot의 character의 id가 UserInfo.UserCharDict_Copy_2[i].Value.Get_CharacterID와 아니면 선택된 이미지 비활성화
            {
                Info_Char_Slot[i].Select_Img.gameObject.SetActive(false);
            }
        }

        if (!_isInfoScene)
        {
            // Grid Layout의 정보 가져오기
            float cellHeight = gridLayoutGroup.cellSize.y; // 한 칸의 높이
            float spacing = gridLayoutGroup.spacing.y; // 간격
            int totalRows = content.childCount; // 총 캐릭터 개수 (한 줄에 하나씩 있다고 가정)

            // 선택된 캐릭터가 위에서 몇 번째 줄인지 계산
            float targetY = (cellHeight + spacing) * CharNum;

            // 전체 컨텐츠 높이
            float contentHeight = content.rect.height;

            // normalizedPosition 계산 (0 = 최하단, 1 = 최상단)
            float normalizedPos = 1 - (targetY / contentHeight);

            // 스크롤 이동
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(normalizedPos);
        }
    }

    // 캐릭터 정보창에 스크롤뷰로 되어 있는 캐릭터선택 버튼 활성화
    void CharInfo_Select_Btn(Character _char)
    {

        for (int i = 0; i < UserInfo.UserCharDict_Copy_2.Count; i++)
        {
            if (Info_Char_Slot[i].gameObject.activeSelf == false)
            {
                Info_Char_Slot[i].gameObject.SetActive(true);
            }

            Info_Char_Slot[i].character = UserInfo.UserCharDict_Copy_2[i].Value;
            Info_Char_Slot[i].Character_Face.sprite = UserInfo.UserCharDict_Copy_2[i].Value.Get_Normal_Img;
        }
    }
    #endregion

    #region 성급 숫자 변경
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
}
