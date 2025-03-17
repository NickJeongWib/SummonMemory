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
    [SerializeField] GameObject Change_Char_Btn;    // ĳ���� ��ü â ��ư
    [SerializeField] GameObject Equip_Info_Btn;     // ĳ���� ��ü ���, Ȯ�� ��ư ����
    [SerializeField] Image[] Equip_Char_Img;
    [SerializeField] GameObject[] UnEquip_Btn;

    [Header("---CharacterList_Info---")]
    public string[] Type_Kor_Str;                   // �ѱ۷� Ÿ�� ǥ�� ���� stringŸ�� �迭
    public string[] Element_Kor_Str;                // �ѱ۷� �Ӽ� ǥ�� ���� stringŸ�� �迭
    // ĳ���� ���� ����
    [SerializeField] Image CharInfo_Img;            // ĳ���� ���� â ĳ���� �̹��� �ؽ�Ʈ
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

    private void Start()
    {
        // TODO ## �ʱ� �׽�Ʈ ��
        Equip_Image_Refresh(false);
    }

    #region Shader_Graph_Transition
    // ����, ����, ���� �� ���� â���� �̵�
    public void On_Click_OnPanel(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // �̵� �� ��ư Ŭ�� ����
        ShaderTransition.SetActive(true);
        _obj.SetActive(true);
    }

    // ���� ���� �ִ� â�� �ݰ� �κ�� �̵�
    public void On_Click_OffPanel(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // ȭ����ȯ �� ��ư Ŭ�� ����
        ShaderTransition.SetActive(true);
        _obj.SetActive(false);
    }
    #endregion

    #region Circle_Transition
    public void On_Click_OnPanel_Circle(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // �̵� �� ��ư Ŭ�� ����
        CircleTransition.SetActive(true);
        _obj.SetActive(true);
    }

    // ���� ���� �ִ� â�� �ݰ� �κ�� �̵�
    public void On_Click_OffPanel_Circle(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // ȭ����ȯ �� ��ư Ŭ�� ����
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
        // ��í ���� ��ŵ
        _obj.SetActive(false);
        _obj.transform.parent.gameObject.SetActive(false);

        gachaVideo.Get_VideoPlayer.Stop();
    }

    public void On_Click_Back_GachaList(GameObject _obj)
    {
        // ��í �̱� ��� �ݱ�
        Gacha_10.SetActive(false);
        Gacha_1.SetActive(false);

        // å ��� �˾� �ݱ�
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
        // TODO ## Lobby_Manager ĳ���� ���� â �̵� Ʈ������ �̹��� �ʱ�ȭ ����
        // ȭ����ȯ �� ��ư Ŭ�� ����
        NotTouch_RayCast.SetActive(true); 
        CharacterInfo_Panel.SetActive(true);
        // MeshRenderer�� ��Ƽ���� ������ Color������ ����Ÿ�Կ� �����ؼ� ���� ����
        CharacterInfo_Transition.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[(int)_slot.character.Get_CharElement]);

        // ȭ�� ��ȯ ȭ���� UI ������ ĳ���� �Ӽ� ������ ����
        Transition_Char_Name.color = colors[(int)_slot.character.Get_CharElement];
        Transition_Grade.color = colors[(int)_slot.character.Get_CharElement];
        Transition_Grade_Deco.color = colors[(int)_slot.character.Get_CharElement];

        Transition_White_Char.color = Transition_colors[(int)_slot.character.Get_CharElement];

        Transition_ElementCol.sprite = CharacterList_UI_Ref.ElementColors[(int)_slot.character.Get_CharElement];
        Transition_Element_BG.sprite = CharacterList_UI_Ref.Elements_BG[(int)_slot.character.Get_CharElement];
        Transition_White_Char.sprite = _slot.character.Get_WhiteIllust_Img;

        // ĳ������ ���� �̸� ǥ��
        Transition_Char_Name.text = _slot.character.Get_CharEngName;
        CharInfo_Img.sprite = _slot.character.Get_Normal_Img;
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

        // ��޿� ���� ���̾� �̹��� ����ȭ
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
        if (UserInfo.UserCharDict.Count <= 0)
            return;

        for (int i = 0; i < UserInfo.UserCharDict_Copy.Count; i++)
        {
            CharacterList_UI_Ref.Slots[i].Equip_Btn.gameObject.SetActive(_bool);
            CharacterList_UI_Ref.Slots[i].Select_Btn.interactable = !_bool;
        }

        // ���� �� ���� ĳ����ĭ ���� ĳ���͸� �� �� �ְ� 
        Equip_Count = 0;
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            Equip_Count = i;
            UnEquip_Btn[i].SetActive(_bool);
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
        CharacterList_UI_Ref.Refresh_CharacterList();
        CharacterList_UI_Ref.Refresh_Equip_Btn();
        Equip_Image_Refresh(true);
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
        UserInfo.UserCharDict_Copy.RemoveAt(_slot.Slot_Num);

        CharImg_Anim_Ref.Get_ImageIndex = 0;
        CharImg_Anim_Ref.R_SR_Image_Change(CharImg_Anim_Ref.Get_ImageIndex);;

        // ������ ĳ���Ϳ� ���� �̹��� ��ü �Լ�
        CharImg_Anim_Ref.CharImage_ChangeAnimF();
        // ���� ĳ���� ���� �ʱ�ȭ
        CharacterList_UI_Ref.Refresh_CharacterList();
        Equip_Image_Refresh(true);
    }
    #endregion
}
