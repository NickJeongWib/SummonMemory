using PlayFab;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class DataNetwork_Mgr : MonoBehaviour
{
    List<PACKETTYPE> PacketBuff = new List<PACKETTYPE>();
    Dictionary<string, string> Data;
    float NetWaitTime;

    #region Singleton
    public static DataNetwork_Mgr Inst = null;
    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
    }
    #endregion

    #region Update
    void Update()
    {
        // 네트워크는 게임시간하고 관계없이 호출되야한다.
        NetWaitTime -= Time.unscaledDeltaTime;

        if (NetWaitTime < 0)
            NetWaitTime = 0.0f;

        // 패킷 처리 중인 상태가 아니면
        if (NetWaitTime <= 0.0F)
        {
            //대기 패킷이 존재한다면
            if (0 < PacketBuff.Count)
            {
                Req_NetWork();
            }
            else // 처리할 패킷이 하나도 없다면
            {
                // 매법 처리할 패킷이 하나도 없을 때만 종료처리 해야할지 확인
                MoveScene();
            }
        }
    }
    #endregion

    private void MoveScene()
    {

    }

    private void Req_NetWork()
    {
        if (PacketBuff[0] == PACKETTYPE.CHARLIST)
        {
            UpdateCharListCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.CHAR_INVENTORY)
        {
            UpdateCharInvenCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.EQUIP_CHAR_LIST)
        {
            UpdateEquipCharCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.CLEAR_CHAR_INVEN)
        {
            UpdateClearCharInvenCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.CLEAR_EQUIP_CHAR)
        {
            UpdateClearEquipCharCo();
        }

        PacketBuff.RemoveAt(0);
    }

    #region Clear_Equip_Char_Data
    private void UpdateClearEquipCharCo()
    {
        var Get_request = new GetUserDataRequest();
        // 키값 삭제하기위한 리스트
        List<string> RemoveKey = new List<string>();
        PlayFabClientAPI.GetUserData(Get_request,
        result =>
        {
            foreach (var eachData in result.Data)
            {
                if (eachData.Key.Contains("EquipChar_"))
                {
                    RemoveKey.Add(eachData.Key);
                }
            }

            // 리스트에 저장된 값들 삭제
            if (0 < RemoveKey.Count)
            {
                var updateRequest = new UpdateUserDataRequest()
                {
                    KeysToRemove = RemoveKey
                };

                NetWaitTime = 2.0f;

                PlayFabClientAPI.UpdateUserData(updateRequest,
                updateResult =>
                {
                    UpdateEquipCharCo();
                },
                error =>
                {

                });
            }
        },
        error =>
        {

        });
    }
    #endregion

    #region Clear_Char_Inven_Data
    private void UpdateClearCharInvenCo()
    {
        Debug.Log("ClearCharInven");
        var Get_request = new GetUserDataRequest();
        // 키값 삭제하기위한 리스트
        List<string> RemoveKey = new List<string>();

        PlayFabClientAPI.GetUserData(Get_request,
        result =>
        {
            foreach (var eachData in result.Data)
            {
                if (eachData.Key.Contains("CharInven_"))
                {
                    RemoveKey.Add(eachData.Key);
                    Debug.Log(RemoveKey.Count);
                }
            }

            // 리스트에 저장된 값들 삭제
            if (0 < RemoveKey.Count)
            {
                var updateRequest = new UpdateUserDataRequest()
                {
                    KeysToRemove = RemoveKey
                };

                NetWaitTime = 2.0f;

                PlayFabClientAPI.UpdateUserData(updateRequest,
                updateResult =>
                {
                    UpdateCharInvenCo();
                },
                error =>
                {

                });
            }
        },
        error =>
        {

        });
    }
    #endregion

    #region EquipChar_List_Network
    private void UpdateEquipCharCo()
    {
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        Dictionary<string, string> CharData = new Dictionary<string, string>();
        List<Character> EquipChar_List;
        EquipChar_List = UserInfo.Equip_Characters;

        for (int i = 0; i < EquipChar_List.Count; i++)
        {
            string Data = "";
            #region Data_Save
            // Info
            Data += $"{EquipChar_List[i].Get_CharacterID}|";          // 0
            Data += $"{EquipChar_List[i].Get_CharName}|";             // 1
            Data += $"{EquipChar_List[i].Get_CharEngName}|";          // 2
            Data += $"{EquipChar_List[i].Get_CharGrade}|";            // 3
            Data += $"{EquipChar_List[i].Get_CharType}|";             // 4
            Data += $"{EquipChar_List[i].Get_CharElement}|";          // 5
            Data += $"{EquipChar_List[i].Get_CharStar}|";             // 6

            // Stat
            Data += $"{EquipChar_List[i].Get_BaseHP}|";               // 7
            Data += $"{EquipChar_List[i].Get_CharHP}|";               // 8
            Data += $"{EquipChar_List[i].Get_BaseAtk}|";              // 9
            Data += $"{EquipChar_List[i].Get_CharATK}|";              // 10
            Data += $"{EquipChar_List[i].Get_BaseDef}|";              // 11
            Data += $"{EquipChar_List[i].Get_CharDEF}|";              // 12
            Data += $"{EquipChar_List[i].Get_BaseCRID}|";             // 13
            Data += $"{EquipChar_List[i].Get_Char_CRT_Damage}|";      // 14
            Data += $"{EquipChar_List[i].Get_BaseCRIR}|";             // 15
            Data += $"{EquipChar_List[i].Get_Char_CRT_Rate}|";        // 16
            Data += $"{EquipChar_List[i].Get_CombatPower}|";          // 17

            // Growing
            Data += $"{EquipChar_List[i].Get_linearFactor}|";         // 18
            Data += $"{EquipChar_List[i].Get_expFactor}|";            // 19
            Data += $"{EquipChar_List[i].Get_expMultiplier}|";        // 20
            Data += $"{EquipChar_List[i].Get_transitionLevel}|";      // 21

            // Lv
            Data += $"{EquipChar_List[i].Get_Character_Lv}|";         // 22
            Data += $"{EquipChar_List[i].Get_Max_Lv}|";               // 23
            Data += $"{EquipChar_List[i].Get_CurrentExp}|";           // 24
            Data += $"{EquipChar_List[i].Get_Cumulative_Exp}|";       // 25

            // Path
            Data += $"{EquipChar_List[i].Get_Illust_Address}|";       // 26
            Data += $"{EquipChar_List[i].Get_Normal_Image_Address}|"; // 27
            Data += $"{EquipChar_List[i].Get_Grade_Up_Image_Address}|";// 28
            Data += $"{EquipChar_List[i].Get_Profile_Address}|";      // 29
            Data += $"{EquipChar_List[i].Get_White_Illust_Address}|"; // 30
            Data += $"{EquipChar_List[i].Get_Pixel_Illust_Address}|"; // 31

            if (EquipChar_List[i].Get_Square_Illust_Address == "null") // 32
            {
                Data += $"NULL";
            }
            else
            {
                Data += $"{EquipChar_List[i].Get_Square_Illust_Address}";
            }
            #endregion
            CharData.Add($"EquipChar_{i}", Data);
        }

        var request = new UpdateUserDataRequest()
        {
            Data = CharData
        };

        NetWaitTime = 1.0f;

        PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {

            },
            (_error) =>
            {

            });
    }
    #endregion

    #region Character_InventoryList_Network
    private void UpdateCharInvenCo()
    {
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        Dictionary<string, string> InvenCharData = new Dictionary<string, string>();
        List<KeyValuePair<string, Character>> CharDict_Copy;
        CharDict_Copy = UserInfo.UserCharDict_Copy;

        for (int i = 0; i < CharDict_Copy.Count; i++)
        {
            string Data = "";
            #region Data_Save
            // Info
            Data += $"{CharDict_Copy[i].Value.Get_CharacterID}|";          // 0
            Data += $"{CharDict_Copy[i].Value.Get_CharName}|";             // 1
            Data += $"{CharDict_Copy[i].Value.Get_CharEngName}|";          // 2
            Data += $"{CharDict_Copy[i].Value.Get_CharGrade}|";            // 3
            Data += $"{CharDict_Copy[i].Value.Get_CharType}|";             // 4
            Data += $"{CharDict_Copy[i].Value.Get_CharElement}|";          // 5
            Data += $"{CharDict_Copy[i].Value.Get_CharStar}|";             // 6

            // Stat
            Data += $"{CharDict_Copy[i].Value.Get_BaseHP}|";               // 7
            Data += $"{CharDict_Copy[i].Value.Get_CharHP}|";               // 8
            Data += $"{CharDict_Copy[i].Value.Get_BaseAtk}|";              // 9
            Data += $"{CharDict_Copy[i].Value.Get_CharATK}|";              // 10
            Data += $"{CharDict_Copy[i].Value.Get_BaseDef}|";              // 11
            Data += $"{CharDict_Copy[i].Value.Get_CharDEF}|";              // 12
            Data += $"{CharDict_Copy[i].Value.Get_BaseCRID}|";             // 13
            Data += $"{CharDict_Copy[i].Value.Get_Char_CRT_Damage}|";      // 14
            Data += $"{CharDict_Copy[i].Value.Get_BaseCRIR}|";             // 15
            Data += $"{CharDict_Copy[i].Value.Get_Char_CRT_Rate}|";        // 16
            Data += $"{CharDict_Copy[i].Value.Get_CombatPower}|";          // 17

            // Growing
            Data += $"{CharDict_Copy[i].Value.Get_linearFactor}|";         // 18
            Data += $"{CharDict_Copy[i].Value.Get_expFactor}|";            // 19
            Data += $"{CharDict_Copy[i].Value.Get_expMultiplier}|";        // 20
            Data += $"{CharDict_Copy[i].Value.Get_transitionLevel}|";      // 21

            // Lv
            Data += $"{CharDict_Copy[i].Value.Get_Character_Lv}|";         // 22
            Data += $"{CharDict_Copy[i].Value.Get_Max_Lv}|";               // 23
            Data += $"{CharDict_Copy[i].Value.Get_CurrentExp}|";           // 24
            Data += $"{CharDict_Copy[i].Value.Get_Cumulative_Exp}|";       // 25

            // Path
            Data += $"{CharDict_Copy[i].Value.Get_Illust_Address}|";       // 26
            Data += $"{CharDict_Copy[i].Value.Get_Normal_Image_Address}|"; // 27
            Data += $"{CharDict_Copy[i].Value.Get_Grade_Up_Image_Address}|";// 28
            Data += $"{CharDict_Copy[i].Value.Get_Profile_Address}|";      // 29
            Data += $"{CharDict_Copy[i].Value.Get_White_Illust_Address}|"; // 30
            Data += $"{CharDict_Copy[i].Value.Get_Pixel_Illust_Address}|"; // 31

            if (CharDict_Copy[i].Value.Get_Square_Illust_Address == "null") // 32
            {
                Data += $"NULL";
            }
            else
            {
                Data += $"{CharDict_Copy[i].Value.Get_Square_Illust_Address}";
            }
            #endregion
            InvenCharData.Add($"CharInven_{i}", Data);
        }

        var request = new UpdateUserDataRequest()
        {
            Data = InvenCharData
        };

        NetWaitTime = 1.0f;

        PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {

            },
            (_error) =>
            {

            });
    }
    #endregion

    #region Original_CharacterListUpdate_Network
    private void UpdateCharListCo()
    {
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        Dictionary<string, string> CharData = new Dictionary<string, string>();
        List<KeyValuePair<string, Character>> CharDict_Copy;
        CharDict_Copy = UserInfo.UserCharDict.ToList();

        for (int i = 0; i < CharDict_Copy.Count; i++)
        {
            string Data = "";
            #region Data_Save
            // Info
            Data += $"{CharDict_Copy[i].Value.Get_CharacterID}|";          // 0
            Data += $"{CharDict_Copy[i].Value.Get_CharName}|";             // 1
            Data += $"{CharDict_Copy[i].Value.Get_CharEngName}|";          // 2
            Data += $"{CharDict_Copy[i].Value.Get_CharGrade}|";            // 3
            Data += $"{CharDict_Copy[i].Value.Get_CharType}|";             // 4
            Data += $"{CharDict_Copy[i].Value.Get_CharElement}|";          // 5
            Data += $"{CharDict_Copy[i].Value.Get_CharStar}|";             // 6

            // Stat
            Data += $"{CharDict_Copy[i].Value.Get_BaseHP}|";               // 7
            Data += $"{CharDict_Copy[i].Value.Get_CharHP}|";               // 8
            Data += $"{CharDict_Copy[i].Value.Get_BaseAtk}|";              // 9
            Data += $"{CharDict_Copy[i].Value.Get_CharATK}|";              // 10
            Data += $"{CharDict_Copy[i].Value.Get_BaseDef}|";              // 11
            Data += $"{CharDict_Copy[i].Value.Get_CharDEF}|";              // 12
            Data += $"{CharDict_Copy[i].Value.Get_BaseCRID}|";             // 13
            Data += $"{CharDict_Copy[i].Value.Get_Char_CRT_Damage}|";      // 14
            Data += $"{CharDict_Copy[i].Value.Get_BaseCRIR}|";             // 15
            Data += $"{CharDict_Copy[i].Value.Get_Char_CRT_Rate}|";        // 16
            Data += $"{CharDict_Copy[i].Value.Get_CombatPower}|";          // 17

            // Growing
            Data += $"{CharDict_Copy[i].Value.Get_linearFactor}|";         // 18
            Data += $"{CharDict_Copy[i].Value.Get_expFactor}|";            // 19
            Data += $"{CharDict_Copy[i].Value.Get_expMultiplier}|";        // 20
            Data += $"{CharDict_Copy[i].Value.Get_transitionLevel}|";      // 21

            // Lv
            Data += $"{CharDict_Copy[i].Value.Get_Character_Lv}|";         // 22
            Data += $"{CharDict_Copy[i].Value.Get_Max_Lv}|";               // 23
            Data += $"{CharDict_Copy[i].Value.Get_CurrentExp}|";           // 24
            Data += $"{CharDict_Copy[i].Value.Get_Cumulative_Exp}|";       // 25

            // Path
            Data += $"{CharDict_Copy[i].Value.Get_Illust_Address}|";       // 26
            Data += $"{CharDict_Copy[i].Value.Get_Normal_Image_Address}|"; // 27
            Data += $"{CharDict_Copy[i].Value.Get_Grade_Up_Image_Address}|";// 28
            Data += $"{CharDict_Copy[i].Value.Get_Profile_Address}|";      // 29
            Data += $"{CharDict_Copy[i].Value.Get_White_Illust_Address}|"; // 30
            Data += $"{CharDict_Copy[i].Value.Get_Pixel_Illust_Address}|"; // 31

            if (CharDict_Copy[i].Value.Get_Square_Illust_Address == "null") // 32
            {
                Data += $"NULL";
            }
            else
            {
                Data += $"{CharDict_Copy[i].Value.Get_Square_Illust_Address}";
            }
            #endregion
            CharData.Add($"Character_{i}", Data);
        }

        var request = new UpdateUserDataRequest()
        {
            Data = CharData
        };

        NetWaitTime = 2.0f;

        PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {

            },
            (_error) =>
            {

            });
    }
    #endregion

    #region PacketCheck
    // 패킷 중복 적용 방지하고 추가하기 위한 함수
    public void PushPacket(PACKETTYPE _packetType)
    {
        // 시작은 false
        bool isExist = false;

        // 이미 패킷이 존재한다면 true
        for (int i = 0; i < PacketBuff.Count; i++)
        {
            if (PacketBuff[i] == _packetType)
            {
                isExist = true;
            }
        }

        // 미리 받은 패킷이 없다면 추가
        if (isExist == false)
            PacketBuff.Add(_packetType);
    }
    #endregion
}

