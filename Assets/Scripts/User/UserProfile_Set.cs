using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserProfile_Set
{
    //[SerializeField] string Char_BG_Path;
    //[SerializeField] string Char_Lobby_Illust_Path;
    //[SerializeField] string Char_Icon_Path;
    [SerializeField] public List<string> Profile_Sprite_Path = new List<string>();
}
