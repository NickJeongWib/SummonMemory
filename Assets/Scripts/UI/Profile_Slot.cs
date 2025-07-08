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

    // �����ϸ鼭 ������ ���� ���� �ʱ�ȭ
    public void Set_Change_Char_Profile(string _name, Sprite _userLobby, Sprite _userInfo_BG, Sprite _charIcon)
    {
        Profile_Char_Name = _name;
        Character_Icon.sprite = _charIcon;
        User_Lobby_Sprite = _userLobby;
        UserInfo_Panel_BG = _userInfo_BG;

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
        LobbyManager_Ref.Select_Char_Icon(Character_Icon.sprite, UserInfo_Panel_BG, User_Lobby_Sprite);

        Info_Close();
    }

    // ������ ���� �� �ٷ� â�� �ݱ� ���� �Լ�
    public void Info_Close()
    {
        LobbyManager_Ref.Get_ProfileChar_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Down();
    }
}
