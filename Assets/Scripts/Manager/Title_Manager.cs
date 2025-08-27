using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Define;

public class Title_Manager : MonoBehaviour
{
    [Header("Login_Panel")]
    [SerializeField] GameObject LoginPanel;
    [SerializeField] InputField Login_ID_IF;
    [SerializeField] InputField Login_Pass_IF;
    [SerializeField] Toggle Hide_Pass_Toggle;
    bool isLoginSuccess;

    [Header("CreateAccount_Panel")]
    [SerializeField] GameObject CreateAccountPanel;
    [SerializeField] InputField Create_ID_IF;
    [SerializeField] InputField Create_Pass_IF;
    [SerializeField] InputField Create_Name_IF;

    [Header("Info_Panel")]
    [SerializeField] Image Info_BG;
    [SerializeField] GameObject Info_Panel;
    [SerializeField] Text Info_Text;

    [Header("Save_ID")]
    [SerializeField] Toggle Save_ID_Toggle;
    string Save_ID;

    [Header("UI")]
    [SerializeField] GameObject GameStart_Btn;
    [SerializeField] GameObject TouchPanel;

    bool invalidEmailType = false;       // 이메일 포맷이 올바른지 체크
    bool isValidFormat = false;          // 올바른 형식인지 아닌지 체크

    List<string> CharDataKey = new List<string>();
    List<string> InvenDataKet = new List<string>();
    List<string> EquipInvenDataKet = new List<string>();
    List<string> EquipCharNameData = new List<string>();

    [Header("BGM")]
    [SerializeField] AudioClip Title_BGM;

    [Header("Loading")]
    [SerializeField] GameObject LoadingPanel;
    bool isCharLoad;
    bool isInvenLoad;
    bool isQuestLoad;
    bool isStageLoad;
    bool isEquipItemLoad;

    private void Start()
    {
        SoundManager.Inst.PlayBGM(Title_BGM);

        // 아이지 저장 여부 불러오기
        string strId = PlayerPrefs.GetString("MySave_ID", "");
        if (!PlayerPrefs.HasKey("MySave_ID") || strId == "")
        {
            Save_ID_Toggle.isOn = false;
        }
        else
        {
            Save_ID_Toggle.isOn = true;
            Login_ID_IF.text = strId;
        }

        isLoginSuccess = false;
    }

    private void Update()
    {
        // 로그인 성공 시 로딩창이 데이터가 모두 로딩 완료되면 사라지게
        if(LoadingPanel.activeSelf == true && !isCharLoad && !isInvenLoad && !isQuestLoad && !isStageLoad && !isEquipItemLoad)
        {
            LoadingPanel.SetActive(false);
        }
    }

    #region Login
    public void Login()
    {
        SoundManager.Inst.PlayUISound();

        // 인풋필드값 확인
        string strID = Login_ID_IF.text;
        string strPass = Login_Pass_IF.text;

        // 공백 삭제
        strID = strID.Trim();
        strPass = strPass.Trim();

        if (string.IsNullOrEmpty(strID) == true ||
            string.IsNullOrEmpty(strPass) == true)
        {
            MessageOnOff("공백 없이 입력해 주세요.");
        }

        if (!(6 <= strID.Length && strID.Length <= 20))  // 6 ~ 20
        {
            MessageOnOff("이메일은 6글자부터 20글자까지\n작성해 주세요.");
            return;
        }

        if (!(6 <= strPass.Length && strPass.Length <= 20))
        {
            MessageOnOff("비밀번호는 6글자부터 20글자까지\n작성해 주세요.");
            return;
        }

        if (!CheckEmailAddress(strID))
        {
            MessageOnOff("이메일 형식이 맞지 않습니다.");
            return;
        }

        //로그인 성공시 유저 정보를 가져올지를 설정하는 옵션 객체 생성
        var option = new GetPlayerCombinedInfoRequestParams()
        {
            GetPlayerProfile = true,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                //DisplayName(닉네임) 가져오기 위한 요청 옵션
                ShowDisplayName = true, 
            },
            GetUserData = true
        };

        //로그인 아이디 저장
        Save_ID = strID;   

        var request = new LoginWithEmailAddressRequest()
        {
            Email = strID,
            Password = strPass,
            InfoRequestParameters = option
        };

        PlayFabClientAPI.LoginWithEmailAddress(request,
                            OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult _result)
    {
        // TODO ## TitleManager "반드시 구현" 로그인 시 유저 정보 넘겨줘야하는 곳
        MessageOnOff("로그인 성공");
        UserInfo.UID = _result.PlayFabId;
        LoadingPanel.SetActive(true);
        // 유저 이름 가져오기
        if (_result.InfoResultPayload != null && isLoginSuccess == false)
        {
            UserInfo.UserName = _result.InfoResultPayload.PlayerProfile.DisplayName;
            isLoginSuccess = true;

            int GetValue = 0;

            #region Character_Data_Load
            foreach (var eachData in _result.InfoResultPayload.UserData)
            {
                // Playfab 저장 데이터 불러오기
                if (eachData.Key.Contains("CharData_Part"))
                {
                    CharDataKey.Add(eachData.Key);
                }
                else if (eachData.Key.Contains("Item_Inven_Part_"))
                {
                    InvenDataKet.Add(eachData.Key);
                }
                else if (eachData.Key.Contains("Equip_Inven_Part_"))
                {
                    EquipInvenDataKet.Add(eachData.Key);
                }
                else if (eachData.Key.Contains("EquipChar_"))
                {
                    string Data = eachData.Value.Value;
                    //string[] strArr = Data.Split('|');
                    EquipCharNameData.Add(Data);
                }
                else if (eachData.Key.Contains("UserDia"))
                {
                    if(int.TryParse(eachData.Value.Value, out GetValue))
                    {
                        UserInfo.Dia = GetValue;
                    }
                }
                else if (eachData.Key.Contains("UserMoney"))
                {
                    if(int.TryParse(eachData.Value.Value, out GetValue))
                    {
                        UserInfo.Money = GetValue;
                    }
                }
                else if (eachData.Key.Contains("UserProfile"))
                {
                    string[] path = eachData.Value.Value.Split(",");

                    for(int i = 0; i < path.Length; i++)
                    {
                        UserInfo.Profile_Setting.Profile_Sprite_Path.Add(path[i]);
                    }
                }
            }

            // 데이터 로드
            // 캐릭터 정보 로드
            LoadUserCharactersFromChunks(CharDataKey);
            // 인벤토리 아이템 로드
            LoadUserInvenFromChunks(InvenDataKet);
            // 퀘스트 데이터 로드
            LoadQuestList();
            // 스테이지 클리어 정보 로드
            LoadStageClearList();
            #endregion

        }
        // 아이디 저장 토글 저장
        if (Save_ID_Toggle.isOn)
        {
            PlayerPrefs.SetString("MySave_ID", Save_ID);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.DeleteKey("MySave_ID");
        }

        LoginPanel.SetActive(false);
        GameStart_Btn.SetActive(true);
    }

    private void OnLoginFailure(PlayFabError _error)
    {
        // 로그인 실패 시 안내문구
        if (_error.GenerateErrorReport().Contains("User not found"))
        {
            MessageOnOff("해당 이메일이 존재하지 않습니다.");
        }
        else if (_error.GenerateErrorReport().Contains("Invalid email address or password"))
        {
            MessageOnOff("패스워드가 일치하지 않습니다.");
        }
        else
        {
            MessageOnOff("로그인 실패");
        }
    }
    #endregion

    #region Create_Account
    public void On_Click_CreateAccount()
    {
        SoundManager.Inst.PlayUISound();

        string idStr = Create_ID_IF.text;
        string passStr = Create_Pass_IF.text;
        string nickStr = Create_Name_IF.text;

        idStr.Trim();
        passStr.Trim();
        nickStr.Trim();

        if (string.IsNullOrEmpty(idStr) || string.IsNullOrEmpty(passStr) || string.IsNullOrEmpty(nickStr))
        {
            MessageOnOff("빈칸 없이 입력 해주세요");
            return;
        }

        if (!(6 <= idStr.Length && idStr.Length <= 20))
        {
            MessageOnOff("아이디는 6~20자 사이로\n입력해주세요.");
            return;
        }

        if (!(6 <= passStr.Length && passStr.Length <= 20))
        {
            MessageOnOff("비밀번호는 6~20자 사이로\n입력해주세요.");
            return;
        }

        if (!(3 <= nickStr.Length && nickStr.Length <= 10))
        {
            MessageOnOff("이름은 3~10자 사이로\n입력해주세요.");
            return;
        }

        if (!CheckEmailAddress(idStr))
        {
            MessageOnOff("이메일 형식이 맞지 않습니다.");
            return;
        }

        Save_ID = idStr;

        var request = new RegisterPlayFabUserRequest()
        {
            Email = idStr,
            Password = passStr,
            DisplayName = nickStr,

            RequireBothUsernameAndEmail = false
        };

        MessageOnOff("회원가입 중...\n잠시만 기다려 주세요");

        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFail);
    }

    private void RegisterSuccess(RegisterPlayFabUserResult _result)
    {
        MessageOnOff("가입성공");

        // 가입한 아이디 비밀번호만 입력할 수 있게 저장
        Create_ID_IF.text = Save_ID;

        // 판넬 교체
        CreateAccountPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    private void RegisterFail(PlayFabError _error)
    {
        if (_error.GenerateErrorReport().Contains("Email address already exists"))
        {
            MessageOnOff("이미지 존재하는\n이메일입니다");
        }
        else if (_error.GenerateErrorReport().Contains("The display name entered is not available"))
        {
            MessageOnOff("이미 존재하는 닉네임입니다.");
        }
        else
        {
            MessageOnOff("가입 실패");
        }
    }
    #endregion

    #region UI
    public void Click_Login()
    {
        SoundManager.Inst.PlayUISound();
        LoginPanel.SetActive(false);
    }

    public void On_Click_CreateAccount_Panel()
    {
        SoundManager.Inst.PlayUISound();
        LoginPanel.SetActive(false);
        CreateAccountPanel.SetActive(true);
    }

    public void On_Click_CreateAccount_Cancel()
    {
        SoundManager.Inst.PlayUISound();
        LoginPanel.SetActive(true);
        CreateAccountPanel.SetActive(false);
    }

    void MessageOnOff(string _message, bool _isOn = true)
    {
        if (_isOn)
        {
            Info_Text.text = _message;
            Info_BG_RayCast_OnOff(true);
            Info_Panel.SetActive(true);
        }
        else
        {
            Info_Text.text = "";
            Info_Panel.SetActive(false);
        }
    }

    public void On_Click_HidePass()
    {
        SoundManager.Inst.PlayUISound();

        // 비밀번호 보여줄지 말지
        if (Hide_Pass_Toggle.isOn)
        {
            Login_Pass_IF.contentType = InputField.ContentType.Standard;
        }
        else
        {
            Login_Pass_IF.contentType = InputField.ContentType.Password;
        }

        // 강제로 레이블 업데이트
        Login_Pass_IF.ForceLabelUpdate();
    }

    public void Info_BG_RayCast_OnOff(bool _isOn)
    {
        Info_BG.raycastTarget = _isOn;
    }

    public void OnClick_GameStart()
    {
        SoundManager.Inst.PlayUISound();

        TouchPanel.SetActive(true);
        SceneManager.LoadSceneAsync("LobbyScene");
    }
    #endregion

    #region Email_Check
    private bool CheckEmailAddress(string EmailStr)
    {
        if (string.IsNullOrEmpty(EmailStr)) isValidFormat = false;

        EmailStr = Regex.Replace(EmailStr, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);
        if (invalidEmailType) isValidFormat = false;

        // true 로 반환할 시, 올바른 이메일 포맷임.
        isValidFormat = Regex.IsMatch(EmailStr,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase);
        return isValidFormat;
    }

    private string DomainMapper(Match match)
    {
        // IdnMapping class with default property values.
        IdnMapping idn = new IdnMapping();

        string domainName = match.Groups[2].Value;
        try
        {
            domainName = idn.GetAscii(domainName);
        }
        catch (ArgumentException)
        {
            invalidEmailType = true;
        }
        return match.Groups[1].Value + domainName;
    }
    #endregion

    #region Data_Load
    void LoadUserCharactersFromChunks(List<string> _keys)
    {
        isCharLoad = true;

        // 캐릭터 데이터 요청
        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, result =>
        {
            // 성공했으면
            // Load할 캐릭터 Dictionary타입으로 받기 위해
            Dictionary<string, Character> loadedDict = new Dictionary<string, Character>();

            // 캐릭터 이름으로 비교하기 위해
            foreach (string key in _keys)
            {
                // Load 성공한 데이타에 _Key의 이름이 순서대로 포함되어있는지 확인
                if (result.Data.ContainsKey(key))
                {
                    // json 형식의 value값 받아오기
                    string json = result.Data[key].Value;
                    // Wrapper로 json형식 데이터 타입으로 받기 위해 쪼개기
                    CharacterListWrapper wrapper = JsonUtility.FromJson<CharacterListWrapper>(json);

                    // 플레이팹에 저장된 Json형식의 캐릭터 데이터 불러오기
                    foreach (var pair in wrapper.Characters)
                    {
                        loadedDict[pair.key] = pair.value;
                        loadedDict[pair.key].Load_Resources(pair.value.Get_UI_Prefab_Path, pair.value.Get_Prefab_Path, pair.value.Get_Illust_Address, pair.value.Get_Normal_Image_Address, pair.value.Get_Grade_Up_Image_Address,
                            pair.value.Get_Profile_Address, pair.value.Get_White_Illust_Address, pair.value.Get_Pixel_Illust_Address, pair.value.Get_Icon_Address, pair.value.Get_BG_Address,
                            pair.value.Get_Square_Illust_Address);
                        loadedDict[pair.key].Reset_Item();
                        loadedDict[pair.key].SkillData.Resource_Path_Init();
                    }
                }
            }

            // 유저가 지니고 있는 캐릭터를 위에서 받은 Dictionary로 초기화
            UserInfo.UserCharDict = loadedDict;

            // 장착한 캐릭터가 있는지 확인
            for (int i = 0; i < EquipCharNameData.Count; i++)
            {
                // 장착한 캐릭터 Equip_Characters리스트에 추가
                UserInfo.Equip_Characters.Add(UserInfo.UserCharDict[EquipCharNameData[i]]);
            }

            // 유저 전체 Dictionary만큼 반복
            foreach (var pair in UserInfo.UserCharDict)
            {
                //이름과 Character를 변수로 받아 장착했는지 비교
                string charName = pair.Key;
                Character character = pair.Value;

                bool isEquipped = UserInfo.Equip_Characters.Any(equip => equip.Get_CharName == charName);

                // UI로 보여주기 위한 Dictionary에 추가
                if (!isEquipped)
                {
                    UserInfo.UserCharDict_Copy.Add(new KeyValuePair<string, Character>(charName, character));
                }
            }
            // UserInfo.UserCharDict_Copy_2 = UserInfo.UserCharDict.ToList();

            // UserInfo.UserCharDict_Copy_2에 원본값 저장
            foreach (var UserCharDict in UserInfo.UserCharDict)
            {
                UserInfo.UserCharDict_Copy_2.Add(new KeyValuePair<string, Character>(UserCharDict.Key, UserCharDict.Value));
            }

            LoadUserEquipInvenFromChunks(EquipInvenDataKet);

            isCharLoad = false;

        },
        error => 
        Debug.LogError("불러오기 실패: " + error.GenerateErrorReport())
        );
    }

    void LoadUserInvenFromChunks(List<string> _keys)
    {
        isInvenLoad = true;

        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, 
        result =>
        {
            Dictionary<string, Inventory_Item> loadedDict = new Dictionary<string, Inventory_Item>();

            foreach (string key in _keys)
            {
                if (result.Data.ContainsKey(key))
                {

                    string json = result.Data[key].Value;
                    InventoryDictWrapper wrapper = JsonUtility.FromJson<InventoryDictWrapper>(json);

                    foreach (var pair in wrapper.items)
                    {
                        loadedDict[pair.key] = pair.value;
                        loadedDict[pair.key].Load_Item_Icon(pair.value.Get_ItemIcon_Address);
                    }
                }
            }

            UserInfo.InventoryDict = loadedDict;

            foreach (var pair in UserInfo.InventoryDict)
            {
                if (pair.Value.Get_InventoryType == INVENTORY_TYPE.SPEND)
                {
                    UserInfo.Spend_Inventory.Add(pair.Value);
                }
                else if (pair.Value.Get_InventoryType == INVENTORY_TYPE.UPGRADE)
                {
                    UserInfo.Upgrade_Inventory.Add(pair.Value);
                }
            }
            isInvenLoad = false;
        },
        error =>
        Debug.LogError("불러오기 실패: " + error.GenerateErrorReport())
        );
    }
    private void LoadUserEquipInvenFromChunks(List<string> _keys)
    {
        isEquipItemLoad = true;

        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, 
        result =>
        {
            List<Item> loadedList = new List<Item>();

            foreach (var key in _keys)
            {
                if (result.Data.ContainsKey(key))
                {
                    string json = result.Data[key].Value;
                    EquipItemListWrapper wrapper = JsonUtility.FromJson<EquipItemListWrapper>(json);

                    if (wrapper != null && wrapper.Items != null)
                        loadedList.AddRange(wrapper.Items);
                }
            }

            for(int i = 0; i < loadedList.Count; i++)
            {
                loadedList[i].Load_Item_Icon(loadedList[i].Get_ItemImage_Path);
            }
            // UserInfo.Equip_Inventory 로딩한 리스트 붙여넣기
            UserInfo.Equip_Inventory = loadedList;

            #region Type
            foreach (Item item in UserInfo.Equip_Inventory)
            {
                // 만약 아이템이 장착이고
                if (item.Get_isEquip)
                {
                    // 아이템 소유 중인 캐릭터가 있다면
                    if (UserInfo.UserCharDict.ContainsKey(item.Set_EquipCharName))
                    {
                        // 소유 주 등록
                        item.Get_OwnCharacter = UserInfo.UserCharDict[item.Set_EquipCharName];
                        // 찾아서 등록
                        UserInfo.UserCharDict[item.Set_EquipCharName].Get_EquipItems[(int)item.Get_EquipType] = item;
                    }

                    // 아이템을 장착한 캐릭터가 있고
                    if (item.Get_OwnCharacter != null)
                    {
                        // 전투 배치된 캐릭터만큼 돌려서
                        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
                        {
                            // 동일한 캐릭터라면
                            if (UserInfo.Equip_Characters[i].Get_CharName == item.Get_OwnCharacter.Get_CharName)
                            {
                                UserInfo.Equip_Characters[i].Get_EquipItems[(int)item.Get_EquipType] = item;
                                break;
                            }
                        }
                    }
                }

                if (item.Get_EquipType == EQUIP_TYPE.WEAPON)
                {
                    UserInfo.Weapon_Equipment.Add(item);
                }
                else if (item.Get_EquipType == EQUIP_TYPE.HELMET)
                {
                    UserInfo.Helmet_Equipment.Add(item);
                }
                else if (item.Get_EquipType == EQUIP_TYPE.UPPER)
                {
                    UserInfo.Upper_Equipment.Add(item);
                }
                else if (item.Get_EquipType == EQUIP_TYPE.ACCESSORY)
                {
                    UserInfo.Accessory_Equipment.Add(item);
                }
                else if (item.Get_EquipType == EQUIP_TYPE.GLOVE)
                {
                    UserInfo.Glove_Equipment.Add(item);
                }
            }
            #endregion
            isEquipItemLoad = false;
            // Debug.Log("장착 아이템 로드 완료");
        },
        error => 
        Debug.LogError("불러오기 실패: " + error.GenerateErrorReport())
        );
    }

    private void LoadQuestList()
    {
        isQuestLoad = true;

        PlayFab.PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest(),
            result =>
            {
                if (result.Data != null && result.Data.ContainsKey("QuestDataList"))
                {
                    string json = result.Data["QuestDataList"].Value;
                    QuestDataListWrapper wrapper = JsonUtility.FromJson<QuestDataListWrapper>(json);

                    if(wrapper.QuestData_List.Count != 0)
                    {
                        UserInfo.QuestData_List = wrapper.QuestData_List;
                    }
                }

                isQuestLoad = false;
            },
            error => 
            Debug.LogError("로드 실패: " + error.GenerateErrorReport())
        );
    }

    private void LoadStageClearList()
    {
        isStageLoad = true;

        PlayFab.PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest(),
           result =>
           {
               if (result.Data != null && result.Data.ContainsKey("StageClear"))
               {
                   string json = result.Data["StageClear"].Value;
                   StageClearListWrapper wrapper = JsonUtility.FromJson<StageClearListWrapper>(json);

                   if (wrapper.StageClear.Count != 0)
                   {
                       UserInfo.StageClear = wrapper.StageClear;
                   }
               }

               isStageLoad = false;
           },

           error => 
           Debug.LogError("로드 실패: " + error.GenerateErrorReport())
       );
    }
    #endregion

}
