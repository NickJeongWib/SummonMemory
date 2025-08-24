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
    [SerializeField] GameObject CircleTransition;       // 원형으로 페이드 인 앤 아웃 되는 화면전환
    [SerializeField] GameObject ShaderTransition;       // 쉐이더그래프를 이용한 패턴이 있는 화면전환 
    [SerializeField] GameObject NotTouch_RayCast;       // 화면 전환시 클릭 방지

    public GameObject Get_NotTouch_RayCast { get => NotTouch_RayCast; }

    [SerializeField] Equip_Slot EquipSlot_List;         // 게임 로그인 처음인 계정 시작 캐릭터를 장착하기 위한 
    [SerializeField] GameObject SelectStage_BG;
    [Header("---GachaMovie---")]
    [SerializeField] GachaVideo gachaVideo;             // 가차 비디오를 출력 시켜 값을 다루기 위한

    [Header("---Gacha_CharacterList---")]
    [SerializeField] GameObject Gacha_10;               // 10연 가차일 때 나오는
    [SerializeField] GameObject Gacha_1;                // 단일 가차일 때 나오는
    [SerializeField] GameObject[] Book_Images;          // 10연 가차에서 나온 캐릭터가 최고 등급에 도달했지만 중복으로 나올 경우 획득한 소환서를 알기 위한
    [SerializeField] GameObject Book_Image;             // 단일 가차에서 나온 캐릭터가 최고 등급에 도달했지만 중복으로 나올 경우 획득한 소환서를 알기 위한

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
        // TODO ## 초기 테스트 값
        if (UserInfo.UserCharDict.Count <= 0)
        {
            // TODO ## 시작 캐릭터 설정 Character_List "레제" 초반 스타트 캐릭
            UserInfo.UserCharDict.Add($"{Character_List.SR_Char[0].Get_CharName}", Character_List.SR_Char[0]);
            UserInfo.UserCharDict[Character_List.SR_Char[0].Get_CharName].Get_Max_Lv = 20;
            UserInfo.UserCharDict_Copy = UserInfo.UserCharDict.ToList();
            UserInfo.UserCharDict_Copy_2 = UserInfo.UserCharDict.ToList();

            // 프로필 스프라이트 주소 저장
            UserInfo.Profile_Setting.Profile_Sprite_Path.Add(UserInfo.UserCharDict[Character_List.SR_Char[0].Get_CharName].Get_Square_Illust_Address);
            UserInfo.Profile_Setting.Profile_Sprite_Path.Add(UserInfo.UserCharDict[Character_List.SR_Char[0].Get_CharName].Get_BG_Address);
            UserInfo.Profile_Setting.Profile_Sprite_Path.Add(UserInfo.UserCharDict[Character_List.SR_Char[0].Get_CharName].Get_Icon_Address);

            UserInfo.Equip_Characters.Add(UserInfo.UserCharDict_Copy[0].Value);
            CharacterList_UI_Ref.Get_Old_Equip_Characters = UserInfo.Equip_Characters.ToList();

            EquipSlot_List.EquipCharacter = UserInfo.UserCharDict_Copy[0].Value;
            UserInfo.UserCharDict_Copy.RemoveAt(0);
            UserInfo.Old_UserCharDict_Copy = UserInfo.UserCharDict_Copy.ToList();

            // 전투배치 캐릭터 저장
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

        // 불러온 초기 데이터 표시하기 위한 작업
        Refresh_UI_Money();
        Refresh_UI_Dia();
        Refresh_UserName();
        Refresh_User_CombatPower();
        Refresh_User_CharAmount();
        // 대기 캐릭터 UI 생성
        StageSelect_UI.Inst.Spawn_Stand_Char();
    }
    #endregion

    #region Shader_Graph_Transition
    // 상점, 도감, 업적 등 여러 창으로 이동
    public void On_Click_OnPanel(GameObject _obj)
    {
        // 로비 이동 후 다시 재입장, 처음 입장 시 첫번째 목록으로 초기화
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
        _obj.SetActive(false);
        NotTouch_RayCast.SetActive(true); // 화면전환 중 버튼 클릭 방지
        CircleTransition.SetActive(true);
    }
    #endregion

    #region Pop_ONOFF
    // TODO ## Lobby_Manager 로비화면에서 팝업 온 오프
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
        // 가챠 연출 스킵
        _obj.SetActive(false);
        _obj.transform.parent.gameObject.SetActive(false);

        gachaVideo.Get_VideoPlayer.Stop();
        // gachaVideo.Get_VideoPlayer.time = 0.0f;
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

    #region Lobby_Currency_Text_Refresh
    public void Refresh_UI_Money()
    {
        // 골드 값 초기화
        for (int i = 0; i < GoldCount_Texts.Length; i++)
        {
            GoldCount_Texts[i].text = $"{UserInfo.Money.ToString("N0")}";
        }
    }

    public void Refresh_UI_Dia()
    {
        // 다이아 값 초기화
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
            $"캐릭터 보유 {UserInfo.UserCharDict.Count}/{GameManager.Inst.Get_CharMaxCount}  <color=orange>{amountPercent.ToString("N0")}%</color>";
    }

    public void Refresh_User_CombatPower()
    {
        float TotalCombat = 0;

        foreach (Character character in UserInfo.UserCharDict.Values)
        {
            character.Calc_CombatPower();
            TotalCombat += character.Get_CombatPower;
        }

        UserCombatPower_Text.text = $"총 전투력 {TotalCombat.ToString("N0")}";
    }

    #endregion

    #region UserProfile_Refresh
    // 캐릭터 추가 시 프로필 잠금 해제
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

    // 캐릭터 프로필 선택 창에서 캐릭터 선택 시 이미지 교체
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
        // 입력받은 InputField
        string nickStr = NameChange_IF.text;
        // 빈칸 제거
        nickStr.Trim();
        // 공란 체크
        if (string.IsNullOrEmpty(nickStr))
        {
            ErorrPanel_Text("빈칸 없이 입력 해주세요");
            return;
        }

        // 글자 수 체크
        if (!(3 <= nickStr.Length && nickStr.Length <= 10))
        {
            ErorrPanel_Text("이름은 3~10자 사이로\n입력해주세요.");
            return;
        }

        // 이름 변경 판넬 꺼주기
        NameChange_Panel.SetActive(false);

        // 닉네임 설정
        var request = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = nickStr,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            _result =>
            {
                // 성공
                ErorrPanel_Text("닉네임 변경 성공.");
                // 유저 이름 변경
                UserInfo.UserName = nickStr;
                //UI 이름 바꿔주기
                Refresh_UserName();
            },
            _error => 
            {
                // 실패(이미 중복된 닉네임이라면)
                if (_error.GenerateErrorReport().Contains("The display name entered is not available"))
                {
                    ErorrPanel_Text("이미 존재하는 닉네임입니다.");
                }
            });
    }

    // 닉네임 관련 에러 문구 표기 Panel
    void ErorrPanel_Text(string _text)
    {
        Erorr_Panel.SetActive(true);
        ErorrMessage.text = _text;
    }

    public void Init_NameIF()
    {
        // UI 오픈 시 공란 유지
        NameChange_IF.text = "";
    }
    #endregion

    #region Stage_UI_Init
    public void Set_Stage_UI_Image()
    {
        int index = 0;
        // 장착 가능 이미지 표시
        for(int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            index = i;
            CharDragUI_List[i].Set_UI(false);
        }

        // 장착 불가 이미지 표시
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

    // 이미지 초기 값 세팅
    void Init_Profile_Img()
    {
        // 불러와진 저장된 이미지 리소스를 로비화면에 출력
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
