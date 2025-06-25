using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

    bool invalidEmailType = false;       // 이메일 포맷이 올바른지 체크
    bool isValidFormat = false;          // 올바른 형식인지 아닌지 체크


    private void Start()
    {
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
    }


    #region Login
    public void Login()
    {
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

        // 유저 이름 가져오기
        if (_result.InfoResultPayload != null)
        {
            UserInfo.UserName = _result.InfoResultPayload.PlayerProfile.DisplayName;
            #region Character_Data_Load
            foreach (var eachData in _result.InfoResultPayload.UserData)
            {
                if (eachData.Key.Contains("Character_"))
                {
                    #region Data_Load
                    string Data = eachData.Value.Value;
                    string[] strArr = Data.Split('|');

                    int.TryParse(strArr[0], out int ID);
                    CHAR_GRADE.TryParse(strArr[3], out CHAR_GRADE CharGrade);
                    CHAR_TYPE.TryParse(strArr[4], out CHAR_TYPE CharType);
                    CHAR_ELE.TryParse(strArr[5], out CHAR_ELE CharEle);
                    int.TryParse(strArr[6], out int Star);
                    float.TryParse(strArr[7], out float BaseHP);
                    float.TryParse(strArr[8], out float CalHP);
                    float.TryParse(strArr[9], out float BaseAtk);
                    float.TryParse(strArr[10], out float CalAtk);
                    float.TryParse(strArr[11], out float BaseDef);
                    float.TryParse(strArr[12], out float CalDef);
                    float.TryParse(strArr[13], out float BaseCriD);
                    float.TryParse(strArr[14], out float CalCriD);
                    float.TryParse(strArr[15], out float BaseCriR);
                    float.TryParse(strArr[16], out float CalCriR);
                    float.TryParse(strArr[17], out float CombatPower);
                    float.TryParse(strArr[18], out float linearFactor);
                    float.TryParse(strArr[19], out float expFactor);
                    float.TryParse(strArr[20], out float expMultiplier);
                    int.TryParse(strArr[21], out int transitionLevel);
                    int.TryParse(strArr[22], out int Lv);
                    int.TryParse(strArr[23], out int MaxLv);
                    int.TryParse(strArr[24], out int CurrentExp);
                    int.TryParse(strArr[25], out int Cumulative_Exp);

                    Character node = new Character(ID, strArr[1], strArr[2], CharGrade, CharType, CharEle, Star, BaseHP, BaseAtk, BaseDef, BaseCriD, BaseCriR, Lv);
                    node.Load_Resources(strArr[26], strArr[27], strArr[28], strArr[29], strArr[30], strArr[31], strArr[32]);
                    node.Load_Data(linearFactor, expFactor, expMultiplier, transitionLevel, CalHP, CalAtk, CalDef, CalCriD, CalCriR, CombatPower, MaxLv, CurrentExp, Cumulative_Exp);

                    UserInfo.UserCharDict.Add(node.Get_CharName, node);
                    // 캐릭터 정보창 스크롤
                    UserInfo.UserCharDict_Copy_2 = UserInfo.UserCharDict.ToList();
                    #endregion
                }
                else if (eachData.Key.Contains("CharInven_"))
                {
                    #region Data_Load
                    string Data = eachData.Value.Value;
                    string[] strArr = Data.Split('|');

                    int.TryParse(strArr[0], out int ID);
                    CHAR_GRADE.TryParse(strArr[3], out CHAR_GRADE CharGrade);
                    CHAR_TYPE.TryParse(strArr[4], out CHAR_TYPE CharType);
                    CHAR_ELE.TryParse(strArr[5], out CHAR_ELE CharEle);
                    int.TryParse(strArr[6], out int Star);
                    float.TryParse(strArr[7], out float BaseHP);
                    float.TryParse(strArr[8], out float CalHP);
                    float.TryParse(strArr[9], out float BaseAtk);
                    float.TryParse(strArr[10], out float CalAtk);
                    float.TryParse(strArr[11], out float BaseDef);
                    float.TryParse(strArr[12], out float CalDef);
                    float.TryParse(strArr[13], out float BaseCriD);
                    float.TryParse(strArr[14], out float CalCriD);
                    float.TryParse(strArr[15], out float BaseCriR);
                    float.TryParse(strArr[16], out float CalCriR);
                    float.TryParse(strArr[17], out float CombatPower);
                    float.TryParse(strArr[18], out float linearFactor);
                    float.TryParse(strArr[19], out float expFactor);
                    float.TryParse(strArr[20], out float expMultiplier);
                    int.TryParse(strArr[21], out int transitionLevel);
                    int.TryParse(strArr[22], out int Lv);
                    int.TryParse(strArr[23], out int MaxLv);
                    int.TryParse(strArr[24], out int CurrentExp);
                    int.TryParse(strArr[25], out int Cumulative_Exp);

                    Character node = new Character(ID, strArr[1], strArr[2], CharGrade, CharType, CharEle, Star, BaseHP, BaseAtk, BaseDef, BaseCriD, BaseCriR, Lv);
                    node.Load_Resources(strArr[26], strArr[27], strArr[28], strArr[29], strArr[30], strArr[31], strArr[32]);
                    node.Load_Data(linearFactor, expFactor, expMultiplier, transitionLevel, CalHP, CalAtk, CalDef, CalCriD, CalCriR, CombatPower, MaxLv, CurrentExp, Cumulative_Exp);

                    UserInfo.UserCharDict_Copy.Add(new KeyValuePair<string, Character>(node.Get_CharName, node));
                    #endregion
                }
                else if (eachData.Key.Contains("EquipChar_"))
                {
                    #region Data_Load
                    string Data = eachData.Value.Value;
                    string[] strArr = Data.Split('|');

                    int.TryParse(strArr[0], out int ID);
                    CHAR_GRADE.TryParse(strArr[3], out CHAR_GRADE CharGrade);
                    CHAR_TYPE.TryParse(strArr[4], out CHAR_TYPE CharType);
                    CHAR_ELE.TryParse(strArr[5], out CHAR_ELE CharEle);
                    int.TryParse(strArr[6], out int Star);
                    float.TryParse(strArr[7], out float BaseHP);
                    float.TryParse(strArr[8], out float CalHP);
                    float.TryParse(strArr[9], out float BaseAtk);
                    float.TryParse(strArr[10], out float CalAtk);
                    float.TryParse(strArr[11], out float BaseDef);
                    float.TryParse(strArr[12], out float CalDef);
                    float.TryParse(strArr[13], out float BaseCriD);
                    float.TryParse(strArr[14], out float CalCriD);
                    float.TryParse(strArr[15], out float BaseCriR);
                    float.TryParse(strArr[16], out float CalCriR);
                    float.TryParse(strArr[17], out float CombatPower);
                    float.TryParse(strArr[18], out float linearFactor);
                    float.TryParse(strArr[19], out float expFactor);
                    float.TryParse(strArr[20], out float expMultiplier);
                    int.TryParse(strArr[21], out int transitionLevel);
                    int.TryParse(strArr[22], out int Lv);
                    int.TryParse(strArr[23], out int MaxLv);
                    int.TryParse(strArr[24], out int CurrentExp);
                    int.TryParse(strArr[25], out int Cumulative_Exp);

                    Character node = new Character(ID, strArr[1], strArr[2], CharGrade, CharType, CharEle, Star, BaseHP, BaseAtk, BaseDef, BaseCriD, BaseCriR, Lv);
                    node.Load_Resources(strArr[26], strArr[27], strArr[28], strArr[29], strArr[30], strArr[31], strArr[32]);
                    node.Load_Data(linearFactor, expFactor, expMultiplier, transitionLevel, CalHP, CalAtk, CalDef, CalCriD, CalCriR, CombatPower, MaxLv, CurrentExp, Cumulative_Exp);

                    UserInfo.Equip_Characters.Add(node);
                    #endregion
                }
            }
            #endregion

        }
        // 아이디 저장 토글 저장
        if (Save_ID_Toggle.isOn)
        {
            PlayerPrefs.SetString("MySave_ID", Save_ID);
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

        if (!(3 <= nickStr.Length && nickStr.Length <= 20))
        {
            MessageOnOff("이름은 3~20자 사이로\n입력해주세요.");
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
        LoginPanel.SetActive(false);
    }

    public void On_Click_CreateAccount_Panel()
    {
        LoginPanel.SetActive(false);
        CreateAccountPanel.SetActive(true);
    }

    public void On_Click_CreateAccount_Cancel()
    {
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
        SceneManager.LoadScene("LobbyScene");
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
}
