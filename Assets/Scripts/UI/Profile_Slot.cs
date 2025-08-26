using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile_Slot : MonoBehaviour
{
    Lobby_Manager LobbyManager_Ref;

    string Profile_Char_Name;
    public string Get_Profile_Char_Name { get => Profile_Char_Name; }

    string Icon_Path;

    [SerializeField] Image Character_Icon;
    [SerializeField] GameObject Lock_Image;
    Sprite User_Lobby_Sprite;
    Sprite UserInfo_Panel_BG;

    public List<string> Profile_Path = new List<string>();

    // �����ϸ鼭 ������ ���� ���� �ʱ�ȭ
    public void Set_Change_Char_Profile(string _name, Sprite _userLobby, Sprite _userInfo_BG, Sprite _charIcon, string _userLobbyPath, string _userInfo_BG_Path, string _CharIcon_Path)
    {
        Profile_Char_Name = _name;
        Character_Icon.sprite = _charIcon;
        User_Lobby_Sprite = _userLobby;
        UserInfo_Panel_BG = _userInfo_BG;

        Profile_Path.Add(_userLobbyPath);
        Profile_Path.Add(_userInfo_BG_Path);
        Profile_Path.Add(_CharIcon_Path);

        // ĳ���͸� ���ϰ� �ִٸ� ��ư Ȱ��ȭ �ƴϸ� ��Ȱ��ȭ
        Lock_Image.SetActive(UserInfo.UserCharDict.ContainsKey(Profile_Char_Name) ? false : true);
    }

    // LobbyManger ������
    public void Set_LobbyMgr(Lobby_Manager _lobbyMgr)
    {
        LobbyManager_Ref = _lobbyMgr;
    }

    // �������
    public void Set_UnLock(bool _isOwn)
    {
        Lock_Image.SetActive(!_isOwn);
    }

    public void On_Click_Icon()
    {
        SoundManager.Inst.PlayUISound();

        LobbyManager_Ref.Select_Char_Icon(Character_Icon.sprite, UserInfo_Panel_BG, User_Lobby_Sprite);

        // �ʱ�ȭ �����ְ� �� ��ư�� ��� �ִ� �̹��� �ּҵ� �Ѱ��ֱ�
        UserInfo.Profile_Setting.Profile_Sprite_Path.Clear();
        UserInfo.Profile_Setting.Profile_Sprite_Path = this.Profile_Path;

        DataNetwork_Mgr.Inst.PushPacket(Define.PACKETTYPE.PROFILE_IMG);

        Info_Close();
    }

    // ������ ���� �� �ٷ� â�� �ݱ� ���� �Լ�
    public void Info_Close()
    {
        LobbyManager_Ref.Get_ProfileChar_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Down();
    }
}
