using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using PlayFab.ClientModels;
using PlayFab;

public class Lobby_Manager : MonoBehaviour
{
    [Header("---UI_Scripts_Ref")]
    [SerializeField] Inventory_UI Inventory_UI_Ref;
    [SerializeField] Store_Manager StoreManager_Ref;
    [SerializeField] Gacha_UI Gacha_UI_Ref;
    [SerializeField] CharacterList_UI CharacterList_UI_Ref;

    [Header("---Transition---")]
    [SerializeField] GameObject CircleTransition;       // �������� ���̵� �� �� �ƿ� �Ǵ� ȭ����ȯ
    [SerializeField] GameObject ShaderTransition;       // ���̴��׷����� �̿��� ������ �ִ� ȭ����ȯ 
    [SerializeField] GameObject NotTouch_RayCast;       // ȭ�� ��ȯ�� Ŭ�� ����

    public GameObject Get_NotTouch_RayCast { get => NotTouch_RayCast; }

    [SerializeField] Equip_Slot EquipSlot_List;         // ���� �α��� ó���� ���� ���� ĳ���͸� �����ϱ� ���� 
    [SerializeField] GameObject SelectStage_BG;
    [Header("---GachaMovie---")]
    [SerializeField] GachaVideo gachaVideo;             // ���� ������ ��� ���� ���� �ٷ�� ����

    [Header("---Gacha_CharacterList---")]
    [SerializeField] GameObject Gacha_10;               // 10�� ������ �� ������
    [SerializeField] GameObject Gacha_1;                // ���� ������ �� ������
    [SerializeField] GameObject[] Book_Images;          // 10�� �������� ���� ĳ���Ͱ� �ְ� ��޿� ���������� �ߺ����� ���� ��� ȹ���� ��ȯ���� �˱� ����
    [SerializeField] GameObject Book_Image;             // ���� �������� ���� ĳ���Ͱ� �ְ� ��޿� ���������� �ߺ����� ���� ��� ȹ���� ��ȯ���� �˱� ����

    [Header("---UI---")]
    [SerializeField] GameObject CantTouchPanel;
    [SerializeField] Text[] DiaCount_Texts;
    [SerializeField] Text[] GoldCount_Texts;
    [SerializeField] GameObject NameChange_Panel;
    [SerializeField] InputField NameChange_IF;
    [SerializeField] GameObject Erorr_Panel;
    [SerializeField] Text ErorrMessage;
   

    public List<Profile_Slot> UserInfo_ProfileList = new List<Profile_Slot>();
    [SerializeField] Image[] UserProfile;
    [SerializeField] Image Profile_Panel_BG;
    [SerializeField] Image Lobby_Char_Illust;
    [SerializeField] GameObject ProfileChar_Panel;
    public GameObject Get_ProfileChar_Panel { get => ProfileChar_Panel; }

    [Header("---UserInfo---")]
    [SerializeField] Text LobbyUserName_Text;

    [Header("---UserInfo_Panel---")]
    [SerializeField] Text UID_Text;
    [SerializeField] Text UserInfoName_Text;
    [SerializeField] Text UserCombatPower_Text;
    [SerializeField] Text UserCharAmount_Text;

    [Header("---SelectStage_UI---")]
    [SerializeField] List<CharDrag_UI> CharDragUI_List = new List<CharDrag_UI>();

    #region Init
    private void Awake()
    {
        // TODO ## �ʱ� �׽�Ʈ ��
        if (UserInfo.UserCharDict.Count <= 0)
        {
            // TODO ## ���� ĳ���� ���� Character_List "����" �ʹ� ��ŸƮ ĳ��
            UserInfo.UserCharDict.Add($"{Character_List.SR_Char[0].Get_CharName}", Character_List.SR_Char[0]);
            UserInfo.UserCharDict[Character_List.SR_Char[0].Get_CharName].Get_Max_Lv = 20;
            UserInfo.UserCharDict_Copy = UserInfo.UserCharDict.ToList();
            UserInfo.UserCharDict_Copy_2 = UserInfo.UserCharDict.ToList();

            // ������ ��������Ʈ �ּ� ����
            UserInfo.Profile_Setting.Profile_Sprite_Path.Add(UserInfo.UserCharDict[Character_List.SR_Char[0].Get_CharName].Get_Square_Illust_Address);
            UserInfo.Profile_Setting.Profile_Sprite_Path.Add(UserInfo.UserCharDict[Character_List.SR_Char[0].Get_CharName].Get_BG_Address);
            UserInfo.Profile_Setting.Profile_Sprite_Path.Add(UserInfo.UserCharDict[Character_List.SR_Char[0].Get_CharName].Get_Icon_Address);

            UserInfo.Equip_Characters.Add(UserInfo.UserCharDict_Copy[0].Value);
            CharacterList_UI_Ref.Get_Old_Equip_Characters = UserInfo.Equip_Characters.ToList();

            EquipSlot_List.EquipCharacter = UserInfo.UserCharDict_Copy[0].Value;
            UserInfo.UserCharDict_Copy.RemoveAt(0);
            UserInfo.Old_UserCharDict_Copy = UserInfo.UserCharDict_Copy.ToList();

            // ������ġ ĳ���� ����
            DataNetwork_Mgr.Inst.PushPacket(Define.PACKETTYPE.EQUIP_CHAR_LIST);
            DataNetwork_Mgr.Inst.PushPacket(Define.PACKETTYPE.CHARLIST);

            UserInfo.Money = 10000000;
            UserInfo.Dia = 10000;

            DataNetwork_Mgr.Inst.PushPacket(Define.PACKETTYPE.DIA);
            DataNetwork_Mgr.Inst.PushPacket(Define.PACKETTYPE.MONEY);
            DataNetwork_Mgr.Inst.PushPacket(Define.PACKETTYPE.PROFILE_IMG); 
        }

        InitData();
    }

    void InitData()
    {
        UID_Text.text = $"UID : {UserInfo.UID}";

        Init_Profile_Img();

        // �ҷ��� �ʱ� ������ ǥ���ϱ� ���� �۾�
        Refresh_UI_Money();
        Refresh_UI_Dia();
        Refresh_UserName();
        Refresh_User_CombatPower();
        Refresh_User_CharAmount();
        // ��� ĳ���� UI ����
        StageSelect_UI.Inst.Spawn_Stand_Char();
    }
    #endregion

    #region Shader_Graph_Transition
    // ����, ����, ���� �� ���� â���� �̵�
    public void On_Click_OnPanel(GameObject _obj)
    {
        // �κ� �̵� �� �ٽ� ������, ó�� ���� �� ù��° ������� �ʱ�ȭ
        if (_obj.name == "MyBag_Panel")
        {
            Inventory_UI_Ref.On_Click_Spend_Item_Btn();
        }
        else if(_obj.name == "Store_Panel")
        {
            StoreManager_Ref.On_Click_Spend_Store_Btn();
        }
        else if (_obj.name == "Gacha_Panel")
        {
            Gacha_UI_Ref.On_Click_Summon_Btn();
        }
        else if (_obj.name == "Character_Panel")
        {
            CharacterList_UI_Ref.On_Click_EnterCharacter_Inven();
        }

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
        _obj.SetActive(false);
        NotTouch_RayCast.SetActive(true); // ȭ����ȯ �� ��ư Ŭ�� ����
        CircleTransition.SetActive(true);
    }
    #endregion

    #region Pop_ONOFF
    // TODO ## Lobby_Manager �κ�ȭ�鿡�� �˾� �� ����
    public void On_Click_Back(GameObject _obj)
    {
        _obj.SetActive(false);
    }

    public void On_Click_On(GameObject _obj)
    {
        _obj.SetActive(true);
    }
    #endregion

    #region Gacha_UI_Active

    public void On_Click_Skip_GachaMovie(GameObject _obj)
    {
        // ��í ���� ��ŵ
        _obj.SetActive(false);
        _obj.transform.parent.gameObject.SetActive(false);

        gachaVideo.Get_VideoPlayer.Stop();
        // gachaVideo.Get_VideoPlayer.time = 0.0f;
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

    #region Lobby_Currency_Text_Refresh
    public void Refresh_UI_Money()
    {
        // ��� �� �ʱ�ȭ
        for (int i = 0; i < GoldCount_Texts.Length; i++)
        {
            GoldCount_Texts[i].text = $"{UserInfo.Money.ToString("N0")}";
        }
    }

    public void Refresh_UI_Dia()
    {
        // ���̾� �� �ʱ�ȭ
        for (int i = 0; i < DiaCount_Texts.Length; i++)
        {
            DiaCount_Texts[i].text = $"{UserInfo.Dia.ToString("N0")}";
        }
    }
    #endregion

    #region UserData_Refresh
    void Refresh_UserName()
    {
        LobbyUserName_Text.text = UserInfo.UserName;
        UserInfoName_Text.text = UserInfo.UserName;
    }

    public void Refresh_User_CharAmount()
    {
        float amountPercent = ((float)UserInfo.UserCharDict.Count / (float)GameManager.Inst.Get_CharMaxCount) * 100;
        UserCharAmount_Text.text = 
            $"ĳ���� ���� {UserInfo.UserCharDict.Count}/{GameManager.Inst.Get_CharMaxCount}  <color=orange>{amountPercent.ToString("N0")}%</color>";
    }

    public void Refresh_User_CombatPower()
    {
        float TotalCombat = 0;

        foreach (Character character in UserInfo.UserCharDict.Values)
        {
            character.Calc_CombatPower();
            TotalCombat += character.Get_CombatPower;
        }

        UserCombatPower_Text.text = $"�� ������ {TotalCombat.ToString("N0")}";
    }

    #endregion

    #region UserProfile_Refresh
    // ĳ���� �߰� �� ������ ��� ����
    public void Refresh_OwnChar_Profile()
    {
        for (int i = 0; i < UserInfo_ProfileList.Count; i++)
        {
            if(UserInfo.UserCharDict.ContainsKey(UserInfo_ProfileList[i].Get_Profile_Char_Name))
            {
                UserInfo_ProfileList[i].Set_UnLock(true);
            }
            else
            {
                UserInfo_ProfileList[i].Set_UnLock(false);
            }
        }
    }

    // ĳ���� ������ ���� â���� ĳ���� ���� �� �̹��� ��ü
    public void Select_Char_Icon(Sprite _selectIcon, Sprite _panel_BG, Sprite _lobbyChar)
    {
        for(int i = 0; i < UserProfile.Length; i++)
        {
            UserProfile[i].sprite = _selectIcon;
        }

        Profile_Panel_BG.sprite = _panel_BG;
        Lobby_Char_Illust.sprite = _lobbyChar;
    }
    #endregion

    #region Name_Change
    public void NameChange()
    {
        // �Է¹��� InputField
        string nickStr = NameChange_IF.text;
        // ��ĭ ����
        nickStr.Trim();
        // ���� üũ
        if (string.IsNullOrEmpty(nickStr))
        {
            ErorrPanel_Text("��ĭ ���� �Է� ���ּ���");
            return;
        }

        // ���� �� üũ
        if (!(3 <= nickStr.Length && nickStr.Length <= 10))
        {
            ErorrPanel_Text("�̸��� 3~10�� ���̷�\n�Է����ּ���.");
            return;
        }

        // �̸� ���� �ǳ� ���ֱ�
        NameChange_Panel.SetActive(false);

        // �г��� ����
        var request = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = nickStr,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            _result =>
            {
                // ����
                ErorrPanel_Text("�г��� ���� ����.");
                // ���� �̸� ����
                UserInfo.UserName = nickStr;
                //UI �̸� �ٲ��ֱ�
                Refresh_UserName();
            },
            _error => 
            {
                // ����(�̹� �ߺ��� �г����̶��)
                if (_error.GenerateErrorReport().Contains("The display name entered is not available"))
                {
                    ErorrPanel_Text("�̹� �����ϴ� �г����Դϴ�.");
                }
            });
    }

    // �г��� ���� ���� ���� ǥ�� Panel
    void ErorrPanel_Text(string _text)
    {
        Erorr_Panel.SetActive(true);
        ErorrMessage.text = _text;
    }

    public void Init_NameIF()
    {
        // UI ���� �� ���� ����
        NameChange_IF.text = "";
    }
    #endregion

    #region Stage_UI_Init
    public void Set_Stage_UI_Image()
    {
        int index = 0;
        // ���� ���� �̹��� ǥ��
        for(int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            index = i;
            CharDragUI_List[i].Set_UI(false);
        }

        // ���� �Ұ� �̹��� ǥ��
        for(int i = index + 1; i < CharDragUI_List.Count; i++)
        {
            CharDragUI_List[i].Set_UI(true);
        }
    }
    #endregion

    public void Reset_SelectChar()
    {
        GameManager.Inst.Get_SelectChar = null;
    }

    // �̹��� �ʱ� �� ����
    void Init_Profile_Img()
    {
        // �ҷ����� ����� �̹��� ���ҽ��� �κ�ȭ�鿡 ���
        Lobby_Char_Illust.sprite = Resources.Load<Sprite>($"{UserInfo.Profile_Setting.Profile_Sprite_Path[0]}");
        Profile_Panel_BG.sprite = Resources.Load<Sprite>($"{UserInfo.Profile_Setting.Profile_Sprite_Path[1]}");

        for (int i = 0; i < UserProfile.Length; i++)
        {
            UserProfile[i].sprite = Resources.Load<Sprite>($"{UserInfo.Profile_Setting.Profile_Sprite_Path[2]}");
        }
    }

    public void On_Click_InGame(int _index)
    {
        CantTouchPanel.SetActive(true);
        GameManager.Inst.StageIndex = _index;
        SceneManager.LoadSceneAsync("InGameScene");
    }
}

[System.Serializable]
public class ItemListWrapper
{
    public List<Item> Equip_Inventory;
}

[System.Serializable]
public class StageClearListWrapper
{
    public List<bool> StageClear;
}

[System.Serializable]
public class QuestDataListWrapper
{
    public List<QuestData> QuestData_List;
}
