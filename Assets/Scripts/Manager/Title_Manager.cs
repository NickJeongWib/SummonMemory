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

    bool invalidEmailType = false;       // �̸��� ������ �ùٸ��� üũ
    bool isValidFormat = false;          // �ùٸ� �������� �ƴ��� üũ

    public void OnClick_GameStart()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    #region Login
    public void Login()
    {
        // ��ǲ�ʵ尪 Ȯ��
        string strID = Login_ID_IF.text;
        string strPass = Login_Pass_IF.text;

        // ���� ����
        strID = strID.Trim();
        strPass = strPass.Trim();

        if (string.IsNullOrEmpty(strID) == true ||
            string.IsNullOrEmpty(strPass) == true)
        {
            MessageOnOff("���� ���� �Է��� �ּ���.");
        }

        if (!(6 <= strID.Length && strID.Length <= 20))  // 6 ~ 20
        {
            MessageOnOff("�̸����� 6���ں��� 20���ڱ���\n�ۼ��� �ּ���.");
            return;
        }

        if (!(6 <= strPass.Length && strPass.Length <= 20))
        {
            MessageOnOff("��й�ȣ�� 6���ں��� 20���ڱ���\n�ۼ��� �ּ���.");
            return;
        }

        if (!CheckEmailAddress(strID))
        {
            MessageOnOff("�̸��� ������ ���� �ʽ��ϴ�.");
            return;
        }

        //�α��� ������ ���� ������ ���������� �����ϴ� �ɼ� ��ü ����
        var option = new GetPlayerCombinedInfoRequestParams()
        {
            GetPlayerProfile = true,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                //DisplayName(�г���) �������� ���� ��û �ɼ�
                ShowDisplayName = true, 
            },
            GetUserData = true
        };

        //�α��� ���̵� ����
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
        // TODO ## TitleManager "�ݵ�� ����" �α��� �� ���� ���� �Ѱ�����ϴ� ��
        MessageOnOff("�α��� ����");
        UserInfo.UID = _result.PlayFabId;

        // ���� �̸� ��������
        if (_result.InfoResultPayload != null)
        {
            UserInfo.UserName = _result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        // ���̵� ���� ��� ����
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
        // �α��� ���� �� �ȳ�����
        if (_error.GenerateErrorReport().Contains("User not found"))
        {
            MessageOnOff("�ش� �̸����� �������� �ʽ��ϴ�.");
        }
        else if (_error.GenerateErrorReport().Contains("Invalid email address or password"))
        {
            MessageOnOff("�н����尡 ��ġ���� �ʽ��ϴ�.");
        }
        else
        {
            MessageOnOff("�α��� ����");
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
            MessageOnOff("��ĭ ���� �Է� ���ּ���");
            return;
        }

        if (!(6 <= idStr.Length && idStr.Length <= 20))
        {
            MessageOnOff("���̵�� 6~20�� ���̷�\n�Է����ּ���.");
            return;
        }

        if (!(6 <= passStr.Length && passStr.Length <= 20))
        {
            MessageOnOff("��й�ȣ�� 6~20�� ���̷�\n�Է����ּ���.");
            return;
        }

        if (!(3 <= nickStr.Length && nickStr.Length <= 20))
        {
            MessageOnOff("�̸��� 3~20�� ���̷�\n�Է����ּ���.");
            return;
        }

        if (!CheckEmailAddress(idStr))
        {
            MessageOnOff("�̸��� ������ ���� �ʽ��ϴ�.");
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

        MessageOnOff("ȸ������ ��...\n��ø� ��ٷ� �ּ���");

        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFail);
    }

    private void RegisterSuccess(RegisterPlayFabUserResult _result)
    {
        MessageOnOff("���Լ���");

        // ������ ���̵� ��й�ȣ�� �Է��� �� �ְ� ����
        Create_ID_IF.text = Save_ID;

        // �ǳ� ��ü
        CreateAccountPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    private void RegisterFail(PlayFabError _error)
    {
        if (_error.GenerateErrorReport().Contains("Email address already exists"))
        {
            MessageOnOff("�̹��� �����ϴ�\n�̸����Դϴ�");
        }
        else if (_error.GenerateErrorReport().Contains("The display name entered is not available"))
        {
            MessageOnOff("�̹� �����ϴ� �г����Դϴ�.");
        }
        else
        {
            MessageOnOff("���� ����");
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
        // ��й�ȣ �������� ����
        if (Hide_Pass_Toggle.isOn)
        {
            Login_Pass_IF.contentType = InputField.ContentType.Standard;
        }
        else
        {
            Login_Pass_IF.contentType = InputField.ContentType.Password;
        }

        // ������ ���̺� ������Ʈ
        Login_Pass_IF.ForceLabelUpdate();
    }

    public void Info_BG_RayCast_OnOff(bool _isOn)
    {
        Info_BG.raycastTarget = _isOn;
    }
    #endregion

    #region Email_Check
    private bool CheckEmailAddress(string EmailStr)
    {
        if (string.IsNullOrEmpty(EmailStr)) isValidFormat = false;

        EmailStr = Regex.Replace(EmailStr, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);
        if (invalidEmailType) isValidFormat = false;

        // true �� ��ȯ�� ��, �ùٸ� �̸��� ������.
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
