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
    [SerializeField] GameObject Change_Char_Btn;    // ĳ���� ��ü â ��ư
    [SerializeField] GameObject Equip_Info_Btn;     // ĳ���� ��ü ���, Ȯ�� ��ư ����
    [SerializeField] Image[] Equip_Char_Img;
    [SerializeField] GameObject[] UnEquip_Btn;

    [Header("---CharacterList_Info---")]
    [SerializeField] Scrollbar Select_Scroll;
    public string[] Type_Kor_Str;                   // �ѱ۷� Ÿ�� ǥ�� ���� stringŸ�� �迭
    public string[] Element_Kor_Str;                // �ѱ۷� �Ӽ� ǥ�� ���� stringŸ�� �迭

    #region ���� ����
    // ĳ���� ���� ����
    [SerializeField] Text CharInfo_Name_Txt;        // ĳ���� ���� â ĳ���� �̸� �ؽ�Ʈ
    [SerializeField] Text CharInfo_Lv_Txt;          // ĳ���� ���� â ĳ���� ���� �ؽ�Ʈ 
    [SerializeField] Text CharInfo_Star_Txt;        // ĳ���� ���� â ĳ���� ���� �ؽ�Ʈ
    [SerializeField] Text CharInfo_Type_Txt;        // ĳ���� ���� â ĳ���� Ÿ�� �ؽ�Ʈ
    [SerializeField] Text CharInfo_Element_Txt;     // ĳ���� ���� â ĳ���� �Ӽ� �ؽ�Ʈ
    [SerializeField] Text CharInfo_MaxHP_Txt;       // ĳ���� ���� â ĳ���� �ִ�ü�� �ؽ�Ʈ
    [SerializeField] Text CharInfo_Atk_Txt;         // ĳ���� ���� â ĳ���� ���ݷ� �ؽ�Ʈ
    [SerializeField] Text CharInfo_Def_Txt;         // ĳ���� ���� â ĳ���� ���� �ؽ�Ʈ
    [SerializeField] Text CharInfo_CrtDmg_Txt;      // ĳ���� ���� â ĳ���� ġ�� �ؽ�Ʈ
    [SerializeField] Text CharInfo_CrtRate_Txt;     // ĳ���� ���� â ĳ���� ġȮ �ؽ�Ʈ
    #endregion

    [Header("---Character_Info---")]
    [SerializeField] Color[] FrameColors;
    [SerializeField] List<CharInfo_CharSelect_Btn> Info_Char_Slot;
    [SerializeField] Image CharInfo_Frame;          // ĳ���� â ������ �̹���
    [SerializeField] Image CharInfo_Ele_BG;         // ĳ���� â �Ӽ� ���
    [SerializeField] Image CharElement_Img;         // ĳ���� â �Ӽ�
    [SerializeField] Image CharInfo_Img;            // ĳ���� â ĳ���� �̹���
    [SerializeField] Image CharStar_Img;            // ĳ���� â ����
    [SerializeField] Text CharInfo_name;            // ĳ���� â �̸�


    [SerializeField] ScrollRect scrollRect; // ScrollRect ����
    [SerializeField] RectTransform content; // ĳ���� ����Ʈ(Content)
    [SerializeField] GridLayoutGroup gridLayoutGroup; // GridLayoutGroup ����

    private void Start()
    {
        // TODO ## �ʱ� �׽�Ʈ ��
        Equip_Image_Refresh(false);
    }

    #region ���� Characterâ Refresh
    /// <summary>
    /// ĳ������ ������ �����̳� ��ġ ���� �� �ѹ� ȣ�� �� �� �ʿ䰡 ����
    /// </summary> 
    // TODO ## CharacterList_UI ĳ����â Refresh
    public void Refresh_CharacterList()
    {
        int SlotNum = 0;

        foreach(KeyValuePair<string, Character> Dict in UserInfo.UserCharDict_Copy)
        {
            // �̹����� ���� �ִٸ� ���ֱ�
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

            // �̹����� ���� �ִٸ� ���ֱ�
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

    #region ĳ���� ���� �� ����� ����Ʈ���� ���� �� EquipCharacter����Ʈ�� �̵�
    // ĳ���� ���� �� ���� ĳ���� â���� ����â���� �̵�
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
    // TODO ## Lobby_Manager ĳ���� ���� �۵� �κ�
    public void On_Click_UnEquip_Char(int _equipNum)
    {
        if (UserInfo.Equip_Characters.Count <= 1)
        {
            Debug.Log("ĳ���� �ϳ��� ����");
            return;
        }

        UserInfo.UserCharDict_Copy.Add(
            new KeyValuePair<string, Character>(UserInfo.Equip_Characters[_equipNum].Get_CharName, UserInfo.Equip_Characters[_equipNum]));

        UserInfo.Equip_Characters.RemoveAt(_equipNum);

        // �������� ��ư ��Ȱ��ȭ
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
        // ������ ĳ���Ϳ� ���� �̹��� ��ü �Լ�
        CharImg_Anim_Ref.CharImage_ChangeAnimF();
        Refresh_CharacterList();
        Refresh_Equip_Btn();
        Equip_Image_Refresh(true);
        Interact_EquipSlot_Btn();
    }

    // TODO ## Lobby_Manager ĳ���� ���� �۵� �κ�
    public void On_Click_EquipChar(CharacterSlot _slot)
    {
        if (5 <= UserInfo.Equip_Characters.Count)
        {
            Debug.Log("���� ���� ��");
            return;
        }

        // ĳ���� ���� ����Ʈ�� �߰�
        UserInfo.Equip_Characters.Add(_slot.character);
        
        // ���� ĳ���� ���� �ʱ�ȭ
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            EquipSlot_List[i].EquipCharacter = UserInfo.Equip_Characters[i];
        }

        UserInfo.UserCharDict_Copy.RemoveAt(_slot.Slot_Num);

        CharImg_Anim_Ref.Get_ImageIndex = 0;
        CharImg_Anim_Ref.R_SR_Image_Change(CharImg_Anim_Ref.Get_ImageIndex); ;

        // ������ ĳ���Ϳ� ���� �̹��� ��ü �Լ�
        CharImg_Anim_Ref.CharImage_ChangeAnimF();
        // ���� ĳ���� ���� �ʱ�ȭ
        Refresh_CharacterList();
        Equip_Image_Refresh(true);
        Interact_EquipSlot_Btn();
    }
    #endregion

    #region CharacterList_Equip_Btn
    // ĳ���� ��ü ��ư Ȱ��ȭ
    // TODO ## Lobby_Manager ĳ���� ��ü�� ���� ��ư �۵�
    public void On_Click_Change()
    {
        Change_Char_Btn.SetActive(false);
        Equip_Info_Btn.SetActive(true);

        Equip_Char_Btn(true);
    }

    public void On_Click_ChangeCancel()
    {
        // �������� ��ư ��Ȱ��ȭ
        for (int i = 0; i < UnEquip_Btn.Length; i++)
        {
            UnEquip_Btn[i].SetActive(false);
        }
        // ĳ���� ��ü��ư Ȱ��ȭ, ��ü �ڵ���ġ ��ư ��Ȱ��ȭ

        Equip_Char_Btn(false);
        Change_Char_Btn.SetActive(true);
        Equip_Info_Btn.SetActive(false);
    }

    [SerializeField] int Equip_Count;
    // ĳ���� ���� ��ư Ȱ��ȭ
    void Equip_Char_Btn(bool _bool)
    {
        // ĳ���Ͱ� ���ٸ� return
        if (UserInfo.UserCharDict.Count <= 0)
            return;

        // ĳ���� ���� ��ư Ȱ�� ��Ȱ�� ��ȯ
        for (int i = 0; i < UserInfo.UserCharDict_Copy.Count; i++)
        {
            Slots[i].Equip_Btn.gameObject.SetActive(_bool);
            Slots[i].Select_Btn.interactable = !_bool;
        }

        // ���� �� ���� ĳ���� ĭ ���� ĳ���͸� �� �� �ְ� 
        Equip_Count = 0;
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            Equip_Count = i;
            // ĳ���� ������ư Ȱ��ȭ
            UnEquip_Btn[i].SetActive(_bool);
            // ĳ���� ���ù�ư ��Ȱ��ȭ
            EquipChar_Select_Btn[i].interactable = !_bool;
        }
    }

    // TODO ## Lobby_Manager ĳ���� ���� ���� �� UI �ʱ�ȭ
    void Equip_Image_Refresh(bool _bool)
    {
        int EquipNum = 0;

        // ĳ���Ͱ� ������ ����Ʈ�� ũ�⸸ ��ŭ �ݺ�
        for (int on = 0; on < UserInfo.Equip_Characters.Count; on++)
        {
            Equip_Char_Img[on].color = Color.white;

            // R��� ĳ���ʹ� ���� Lobby�̹����� ���� ������ 
            UserInfo.Get_Square_Image(Equip_Char_Img, on);

            if (_bool)
            {
                UnEquip_Btn[on].SetActive(true);
            }

            EquipNum = on + 1;
        }

        // ����Ʈ�� ���� ���� ������ �����ؼ� �ִ� ����ũ���� 5���� �ݺ�
        for (int off = EquipNum; off < MaxEquip_Count; off++)
        {
            // �̹����� ���ش�
            Equip_Char_Img[off].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Equip_Char_Img[off].sprite = null;
        }
    }

    // ĳ���ͱ�ü ��ư Ŭ�� �� ������ư Ȱ��ȭ
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
    // ĳ���� ���� ���� ��ư Ȱ��ȭ
    void Interact_EquipSlot_Btn()
    {
        int count = 0;

        // ���� ĳ���� ��ü ��ư�� Ȱ��ȭ �� ĳ��������â���� �̵� �� �� �ִ� ��ư�� Ȱ��ȭ ��Ű��
        // Ŭ���� �ȵǰ� interactable�� ��Ȱ��ȭ ��Ų��.
        for (int on = 0; on < UserInfo.Equip_Characters.Count; on++)
        {
            EquipSlot_List[on].SelectBtn.gameObject.SetActive(true);
            EquipSlot_List[on].SelectBtn.interactable = false;
            count = on;
        }

        // ĳ���� ���� �� �� ������ ĭ�� ��ư�� ��Ȱ��ȭ ��Ų��.
        for (int off = count + 1; off < EquipSlot_List.Count; off++)
        {
            EquipSlot_List[off].SelectBtn.gameObject.SetActive(false);
        }
    }

    // ĳ���� ���Կ��� Ŭ�� �� ĳ���� ���� â���� �̵�
    public void On_Click_CharInfo(CharacterSlot _slot)
    {
        #region Character_Transition_Set
        // TODO ## Lobby_Manager ĳ���� ���� â �̵� Ʈ������ �̹��� �ʱ�ȭ ����
        // ȭ����ȯ �� ��ư Ŭ�� ����
        LobbyMgr_Ref.Get_NotTouch_RayCast.SetActive(true);
        CharacterInfo_Panel.SetActive(true);
        // MeshRenderer�� ��Ƽ���� ������ Color������ ����Ÿ�Կ� �����ؼ� ���� ����
        CharacterInfo_Transition.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[(int)_slot.character.Get_CharElement]);

        // ȭ�� ��ȯ ȭ���� UI ������ ĳ���� �Ӽ� ������ ����
        Transition_Char_Name.color = colors[(int)_slot.character.Get_CharElement];
        Transition_Grade.color = colors[(int)_slot.character.Get_CharElement];
        Transition_Grade_Deco.color = colors[(int)_slot.character.Get_CharElement];

        Transition_White_Char.color = Transition_colors[(int)_slot.character.Get_CharElement];

        Transition_ElementCol.sprite = ElementColors[(int)_slot.character.Get_CharElement];
        Transition_Element_BG.sprite = Elements_BG[(int)_slot.character.Get_CharElement];
        Transition_White_Char.sprite = _slot.character.Get_WhiteIllust_Img;

        // ĳ������ ���� �̸� ǥ��
        Transition_Char_Name.text = _slot.character.Get_CharEngName;
        CharStar_Refresh(_slot.character, Transition_Grade);

        CharInfo_Select_Btn(_slot.character);
        #endregion

        #region Character_Info_Change
        // ĳ���� ����â �̹��� ����
        UserInfo.Get_Square_Image(CharInfo_Img, _slot.character);
        CharInfo_Ele_BG.sprite = Elements_BG[(int)_slot.character.Get_CharElement];
        CharInfo_Frame.material.color = FrameColors[(int)_slot.character.Get_CharElement];
        CharElement_Img.sprite = ElementColors[(int)_slot.character.Get_CharElement];
        CharInfo_name.text = $"{_slot.character.Get_CharName}  Lv.{_slot.character.Get_Character_Lv}";
        // ��޿� ���� ���̾� �̹��� ����ȭ
        CharStar_Refresh(_slot.character, CharStar_Img);
        CharStar_Img.color = colors[(int)_slot.character.Get_CharElement];
        #endregion

        Refresh_Select_Img(_slot.character, false);

        #region Character_Info_Text_Refresh
        // TODO ## Lobby_Manager ĳ���� ���� â ĳ���� ���� �ʱ�ȭ
        CharInfo_Name_Txt.text = $"�̸� : {_slot.character.Get_CharName}";
        CharInfo_Lv_Txt.text = $"���� : {_slot.character.Get_Character_Lv}";
        CharInfo_Star_Txt.text = $"���� : {_slot.character.Get_CharStar}��";
        CharInfo_Type_Txt.text = $"Ÿ�� : {Type_Kor_Str[(int)_slot.character.Get_CharType]}";
        CharInfo_Element_Txt.text = $"�Ӽ� : {Element_Kor_Str[(int)_slot.character.Get_CharElement]}";
        CharInfo_MaxHP_Txt.text = $"ü�� : {_slot.character.Get_CharHP}";
        CharInfo_Atk_Txt.text = $"���ݷ� : {_slot.character.Get_CharATK}";
        CharInfo_Def_Txt.text = $"���� : {_slot.character.Get_CharDEF}";
        CharInfo_CrtDmg_Txt.text = $"ġ��Ÿ���� : {(_slot.character.Get_Char_CRT_Damage * 100.0f).ToString("N1")}%";
        CharInfo_CrtRate_Txt.text = $"ġ��ŸȮ�� : {(_slot.character.Get_Char_CRT_Rate * 100.0f).ToString("N1")}%";
        #endregion
    }

    // ���� ĳ���� Ŭ�� �� �̵� �����ε�
    public void On_Click_CharInfo(Equip_Slot _slot)
    {
        #region Character_Transition_Set
        // TODO ## Lobby_Manager ĳ���� ���� â �̵� Ʈ������ �̹��� �ʱ�ȭ ����
        // ȭ����ȯ �� ��ư Ŭ�� ����
        LobbyMgr_Ref.Get_NotTouch_RayCast.SetActive(true);
        CharacterInfo_Panel.SetActive(true);
        // MeshRenderer�� ��Ƽ���� ������ Color������ ����Ÿ�Կ� �����ؼ� ���� ����
        CharacterInfo_Transition.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[(int)_slot.EquipCharacter.Get_CharElement]);

        // ȭ�� ��ȯ ȭ���� UI ������ ĳ���� �Ӽ� ������ ����
        Transition_Char_Name.color = colors[(int)_slot.EquipCharacter.Get_CharElement];
        Transition_Grade.color = colors[(int)_slot.EquipCharacter.Get_CharElement];
        Transition_Grade_Deco.color = colors[(int)_slot.EquipCharacter.Get_CharElement];
        Transition_White_Char.color = Transition_colors[(int)_slot.EquipCharacter.Get_CharElement];

        Transition_ElementCol.sprite = ElementColors[(int)_slot.EquipCharacter.Get_CharElement];
        Transition_Element_BG.sprite = Elements_BG[(int)_slot.EquipCharacter.Get_CharElement];
        Transition_White_Char.sprite = _slot.EquipCharacter.Get_WhiteIllust_Img;

        // ĳ������ ���� �̸� ǥ��
        Transition_Char_Name.text = _slot.EquipCharacter.Get_CharEngName;
        CharStar_Refresh(_slot.EquipCharacter, Transition_Grade);

        CharInfo_Select_Btn(_slot.EquipCharacter);
        #endregion

        #region Character_Info_Change
        // ĳ���� ����â �̹��� ����
        UserInfo.Get_Square_Image(CharInfo_Img, _slot.EquipCharacter);
        CharInfo_Ele_BG.sprite = Elements_BG[(int)_slot.EquipCharacter.Get_CharElement];
        CharInfo_Frame.material.color = FrameColors[(int)_slot.EquipCharacter.Get_CharElement];
        CharElement_Img.sprite = ElementColors[(int)_slot.EquipCharacter.Get_CharElement];
        CharInfo_name.text = $"{_slot.EquipCharacter.Get_CharName}  Lv.{_slot.EquipCharacter.Get_Character_Lv}";
        // ��޿� ���� ���̾� �̹��� ����ȭ
        CharStar_Refresh(_slot.EquipCharacter, CharStar_Img);
        CharStar_Img.color = colors[(int)_slot.EquipCharacter.Get_CharElement];
        #endregion

        Refresh_Select_Img(_slot.EquipCharacter, false);

        #region Character_Info_Text_Refresh
        // TODO ## Lobby_Manager ĳ���� ���� â ĳ���� ���� �ʱ�ȭ
        CharInfo_Name_Txt.text = $"�̸� : {_slot.EquipCharacter.Get_CharName}";
        CharInfo_Lv_Txt.text = $"���� : {_slot.EquipCharacter.Get_Character_Lv}";
        CharInfo_Star_Txt.text = $"���� : {_slot.EquipCharacter.Get_CharStar}��";
        CharInfo_Type_Txt.text = $"Ÿ�� : {Type_Kor_Str[(int)_slot.EquipCharacter.Get_CharType]}";
        CharInfo_Element_Txt.text = $"�Ӽ� : {Element_Kor_Str[(int)_slot.EquipCharacter.Get_CharElement]}";
        CharInfo_MaxHP_Txt.text = $"ü�� : {_slot.EquipCharacter.Get_CharHP}";
        CharInfo_Atk_Txt.text = $"���ݷ� : {_slot.EquipCharacter.Get_CharATK}";
        CharInfo_Def_Txt.text = $"���� : {_slot.EquipCharacter.Get_CharDEF}";
        CharInfo_CrtDmg_Txt.text = $"ġ��Ÿ���� : {(_slot.EquipCharacter.Get_Char_CRT_Damage * 100.0f).ToString("N1")}%";
        CharInfo_CrtRate_Txt.text = $"ġ��ŸȮ�� : {(_slot.EquipCharacter.Get_Char_CRT_Rate * 100.0f).ToString("N1")}%";
        #endregion
    }

    // ���� ĳ���� Ŭ�� �� �̵� �����ε�
    public void On_Click_CharInfo(CharInfo_CharSelect_Btn _slot)
    {

        Refresh_Select_Img(_slot.character);

        #region Character_Info_Change
        // ĳ���� ����â �̹��� ����
        UserInfo.Get_Square_Image(CharInfo_Img, _slot.character);
        CharInfo_Ele_BG.sprite = Elements_BG[(int)_slot.character.Get_CharElement];
        CharInfo_Frame.material.color = FrameColors[(int)_slot.character.Get_CharElement];
        CharElement_Img.sprite = ElementColors[(int)_slot.character.Get_CharElement];
        CharInfo_name.text = $"{_slot.character.Get_CharName}  Lv.{_slot.character.Get_Character_Lv}";
        // ��޿� ���� ���̾� �̹��� ����ȭ
        CharStar_Refresh(_slot.character, CharStar_Img);
        CharStar_Img.color = colors[(int)_slot.character.Get_CharElement];
        #endregion

        #region Character_Info_Text_Refresh
        // TODO ## Lobby_Manager ĳ���� ���� â ĳ���� ���� �ʱ�ȭ
        CharInfo_Name_Txt.text = $"�̸� : {_slot.character.Get_CharName}";
        CharInfo_Lv_Txt.text = $"���� : {_slot.character.Get_Character_Lv}";
        CharInfo_Star_Txt.text = $"���� : {_slot.character.Get_CharStar}��";
        CharInfo_Type_Txt.text = $"Ÿ�� : {Type_Kor_Str[(int)_slot.character.Get_CharType]}";
        CharInfo_Element_Txt.text = $"�Ӽ� : {Element_Kor_Str[(int)_slot.character.Get_CharElement]}";
        CharInfo_MaxHP_Txt.text = $"ü�� : {_slot.character.Get_CharHP}";
        CharInfo_Atk_Txt.text = $"���ݷ� : {_slot.character.Get_CharATK}";
        CharInfo_Def_Txt.text = $"���� : {_slot.character.Get_CharDEF}";
        CharInfo_CrtDmg_Txt.text = $"ġ��Ÿ���� : {(_slot.character.Get_Char_CRT_Damage * 100.0f).ToString("N1")}%";
        CharInfo_CrtRate_Txt.text = $"ġ��ŸȮ�� : {(_slot.character.Get_Char_CRT_Rate * 100.0f).ToString("N1")}%";
        #endregion
    }

    void Refresh_Select_Img(Character _char, bool _isInfoScene = true)
    {
        int CharNum = 0;

        // ĳ���� ��Ƶ� ����Ʈ��ŭ �ݺ�
        for (int i = 0; i < UserInfo.UserCharDict_Copy_2.Count; i++)
        {
            // _slot�� character�� id�� UserInfo.UserCharDict_Copy_2[i].Value.Get_CharacterID�� ���ٸ� ���õ� �̹��� Ȱ��ȭ
            if (_char.Get_CharacterID == Info_Char_Slot[i].character.Get_CharacterID)
            {
                Info_Char_Slot[i].Select_Img.gameObject.SetActive(true);
                CharNum = i;
            }
            else // _slot�� character�� id�� UserInfo.UserCharDict_Copy_2[i].Value.Get_CharacterID�� �ƴϸ� ���õ� �̹��� ��Ȱ��ȭ
            {
                Info_Char_Slot[i].Select_Img.gameObject.SetActive(false);
            }
        }

        if (!_isInfoScene)
        {
            // Grid Layout�� ���� ��������
            float cellHeight = gridLayoutGroup.cellSize.y; // �� ĭ�� ����
            float spacing = gridLayoutGroup.spacing.y; // ����
            int totalRows = content.childCount; // �� ĳ���� ���� (�� �ٿ� �ϳ��� �ִٰ� ����)

            // ���õ� ĳ���Ͱ� ������ �� ��° ������ ���
            float targetY = (cellHeight + spacing) * CharNum;

            // ��ü ������ ����
            float contentHeight = content.rect.height;

            // normalizedPosition ��� (0 = ���ϴ�, 1 = �ֻ��)
            float normalizedPos = 1 - (targetY / contentHeight);

            // ��ũ�� �̵�
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(normalizedPos);
        }
    }

    // ĳ���� ����â�� ��ũ�Ѻ�� �Ǿ� �ִ� ĳ���ͼ��� ��ư Ȱ��ȭ
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

    #region ���� ���� ����
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
