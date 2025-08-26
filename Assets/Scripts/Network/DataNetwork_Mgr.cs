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

    public Active_F LoadingPanel;

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
        // ��Ʈ��ũ�� ���ӽð��ϰ� ������� ȣ��Ǿ��Ѵ�.
        NetWaitTime -= Time.unscaledDeltaTime;

        if (NetWaitTime < 0)
            NetWaitTime = 0.0f;

        // ��Ŷ ó�� ���� ���°� �ƴϸ�
        if (NetWaitTime <= 0.0F)
        {
            //��� ��Ŷ�� �����Ѵٸ�
            if (0 < PacketBuff.Count)
            {
                Req_NetWork();
            }
            else // ó���� ��Ŷ�� �ϳ��� ���ٸ�
            {
                // �Ź� ó���� ��Ŷ�� �ϳ��� ���� ���� ����ó�� �ؾ����� Ȯ��
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
        else if (PacketBuff[0] == PACKETTYPE.EQUIP_CHAR_LIST)
        {
            UpdateEquipCharCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.CLEAR_EQUIP_CHAR)
        {
            UpdateClearEquipCharCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.ITEM_INVENTORY)
        {
            UpdateItemInvenCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.EQUIP_ITEM_INVENTORY)
        {
            UpdateEquipItemInvenCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.DIA)
        {
            UpdateDiaCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.MONEY)
        {
            UpdateMoneyCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.PROFILE_IMG)
        {
            UpdateProfileCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.QUEST)
        {
            UpdateQuestCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.STAGE)
        {
            UpdateStageCo();
        }
        PacketBuff.RemoveAt(0);
    }

    #region StageClear
    private void UpdateStageCo()
    {
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

        StageClearListWrapper wrapper = new StageClearListWrapper();
        wrapper.StageClear = UserInfo.StageClear;

        // ����Ʈ ��ü ����ȭ
        string json = JsonUtility.ToJson(wrapper, true);

        var request = new PlayFab.ClientModels.UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            { "StageClear", json }
        }
        };

        PlayFab.PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
                // Debug.Log("����Ʈ ����Ʈ ���� ����!");
            },
            (_error) =>
            {
                Debug.LogError("����Ʈ ���� ����: " + _error.GenerateErrorReport());
            });
    }
    #endregion

    #region QuestData
    private void UpdateQuestCo()
    {
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

        QuestDataListWrapper wrapper = new QuestDataListWrapper();
        wrapper.QuestData_List = UserInfo.QuestData_List;

        // ����Ʈ ��ü ����ȭ
        string json = JsonUtility.ToJson(wrapper, true); 

        var request = new PlayFab.ClientModels.UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            { "QuestDataList", json }
        }};

        PlayFab.PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
                // Debug.Log("����Ʈ ����Ʈ ���� ����!");
            },
            (_error) =>
            {
                Debug.LogError("����Ʈ ���� ����: " + _error.GenerateErrorReport());
            });
    }
    #endregion

    #region Profile_Img
    private void UpdateProfileCo()
    {
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

        string Sprites_Path = "";

        // �̹��� �ּ� ����
        for(int i = 0; i < UserInfo.Profile_Setting.Profile_Sprite_Path.Count; i++)
        {
            Sprites_Path += $"{UserInfo.Profile_Setting.Profile_Sprite_Path[i]},";
        }

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {
                    "UserProfile",
                    Sprites_Path
                }
            }
        };

        // NetWaitTime = 0.5f;

        PlayFabClientAPI.UpdateUserData(request,
           (_result) =>
           {
               Debug.Log("������ ���� ����");
           },
           (_error) =>
           {
               Debug.Log(_error.GenerateErrorReport());
               Debug.Log("������ ���� ����");
           });
    }
    #endregion

    #region Dia_Data
    private void UpdateDiaCo()
    {
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {
                    "UserDia",
                    UserInfo.Dia.ToString()
                }
            }
        };

        // NetWaitTime = 0.5f;

        PlayFabClientAPI.UpdateUserData(request,
           (_result) =>
           {
               Debug.Log("���̾� ���� ����");
           },
           (_error) =>
           {
               Debug.Log(_error.GenerateErrorReport());
               Debug.Log("���̾� ���� ����");
           });
    }
    #endregion

    #region Money_Data
    private void UpdateMoneyCo()
    {
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {
                    "UserMoney",
                    UserInfo.Money.ToString()
                }
            }
        };

        // NetWaitTime = 0.5f;

        PlayFabClientAPI.UpdateUserData(request,
           (_result) =>
           {
               Debug.Log("������� ���� ����");
           },
           (_error) =>
           {
               Debug.Log(_error.GenerateErrorReport());
               Debug.Log("������� ���� ����");
           });
    }
    #endregion

    #region Equip_Item_Inven_Data
    private void UpdateEquipItemInvenCo()
    {
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

        var allEntries = UserInfo.Equip_Inventory;
        int chunkSize = 10;

        int totalChunks = Mathf.CeilToInt(allEntries.Count / (float)chunkSize);

        for (int i = 0; i < totalChunks; i++)
        {
            var chunk = allEntries.Skip(i * chunkSize).Take(chunkSize).ToList();

            var wrapper = new EquipItemListWrapper(chunk);
            string json = JsonUtility.ToJson(wrapper, true);
            string keyName = $"Equip_Inven_Part_{i + 1}";
            // Debug.Log(json);

            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {
                { 
                    keyName,
                    json 
                }
            }};

            // NetWaitTime = 0.5f;

            PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
                // Debug.Log("���� ������ ����Ʈ ���� ����");
                if(LoadingPanel != null)
                {
                    LoadingPanel.StartCoroutine(LoadingPanel.LoadImage());
                }
            },
            (_error) =>
            {
                Debug.Log("���� ������ ����Ʈ ���� ���� : " + _error.GenerateErrorReport());
            });
        }
    }
    #endregion

    #region Item_InvenUpdate
    private void UpdateItemInvenCo()
    {
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

        var allEntries = UserInfo.InventoryDict.ToList(); // Dictionary �� List<KeyValuePair>
        int chunkSize = 10;

        int totalChunks = Mathf.CeilToInt(allEntries.Count / (float)chunkSize);

        for (int i = 0; i < totalChunks; i++)
        {
            var chunk = allEntries.Skip(i * chunkSize).Take(chunkSize);

            InventoryDictWrapper wrapper = new InventoryDictWrapper();
            foreach (var pair in chunk)
            {
                wrapper.items.Add(new InventoryItemPair { key = pair.Key, value = pair.Value });
            }

            string json = JsonUtility.ToJson(wrapper);
            string keyName = $"Item_Inven_Part_{i + 1}";

            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {
                { keyName, json }
            }
            };

            // NetWaitTime = 0.5f;

            PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
                // Debug.Log("������ ����Ʈ ���� ����");
            },
            (_error) =>
            {
                Debug.Log("������ ����Ʈ ���� ���� : " +_error.GenerateErrorReport());
            });
        }
    }
    #endregion

    #region Clear_Equip_Char_Data
    private void UpdateClearEquipCharCo()
    {
        var Get_request = new GetUserDataRequest();
        // Ű�� �����ϱ����� ����Ʈ
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

            // ����Ʈ�� ����� ���� ����
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
                    Debug.Log("���� ĳ���� : " + error.GenerateErrorReport());
                });
            }
        },
        error =>
        {

        });
    }
    #endregion

    #region Clear_Char_Data
    private void UpdateClearCharCo()
    {
        var Get_request = new GetUserDataRequest();
        // Ű�� �����ϱ����� ����Ʈ
        List<string> RemoveKey = new List<string>();

        PlayFabClientAPI.GetUserData(Get_request,
        result =>
        {
            foreach (var eachData in result.Data)
            {
                if (eachData.Key.Contains("Character_"))
                {
                    RemoveKey.Add(eachData.Key);
                    // Debug.Log(RemoveKey.Count);
                }
            }

            // ����Ʈ�� ����� ���� ����
            if (0 < RemoveKey.Count)
            {
                var updateRequest = new UpdateUserDataRequest()
                {
                    KeysToRemove = RemoveKey
                };

                NetWaitTime = 1.0f;

                PlayFabClientAPI.UpdateUserData(updateRequest,
                updateResult =>
                {
                    PushPacket(PACKETTYPE.CHARLIST);
                },
                error =>
                {
                    Debug.Log("���� ĳ���� �ʱ�ȭ: " + error.GenerateErrorReport());
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
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

        Dictionary<string, string> CharData = new Dictionary<string, string>();
        List<Character> EquipChar_List;
        EquipChar_List = UserInfo.Equip_Characters;

        for (int i = 0; i < EquipChar_List.Count; i++)
        {
            string Data = "";
            Data = $"{EquipChar_List[i].Get_CharName}";
            #region Data_Save
            // Info
            //Data += $"{EquipChar_List[i].Get_CharacterID}|";          // 0
            // 1
            //Data += $"{EquipChar_List[i].Get_CharEngName}|";          // 2
            //Data += $"{EquipChar_List[i].Get_CharGrade}|";            // 3
            //Data += $"{EquipChar_List[i].Get_CharType}|";             // 4
            //Data += $"{EquipChar_List[i].Get_CharElement}|";          // 5
            //Data += $"{EquipChar_List[i].Get_CharStar}|";             // 6

            //// Stat
            //Data += $"{EquipChar_List[i].Get_BaseHP}|";               // 7
            //Data += $"{EquipChar_List[i].Get_CharHP}|";               // 8
            //Data += $"{EquipChar_List[i].Get_BaseAtk}|";              // 9
            //Data += $"{EquipChar_List[i].Get_CharATK}|";              // 10
            //Data += $"{EquipChar_List[i].Get_BaseDef}|";              // 11
            //Data += $"{EquipChar_List[i].Get_CharDEF}|";              // 12
            //Data += $"{EquipChar_List[i].Get_BaseCRID}|";             // 13
            //Data += $"{EquipChar_List[i].Get_Char_CRT_Damage}|";      // 14
            //Data += $"{EquipChar_List[i].Get_BaseCRIR}|";             // 15
            //Data += $"{EquipChar_List[i].Get_Char_CRT_Rate}|";        // 16
            //Data += $"{EquipChar_List[i].Get_CombatPower}|";          // 17

            //// Growing
            //Data += $"{EquipChar_List[i].Get_linearFactor}|";         // 18
            //Data += $"{EquipChar_List[i].Get_expFactor}|";            // 19
            //Data += $"{EquipChar_List[i].Get_expMultiplier}|";        // 20
            //Data += $"{EquipChar_List[i].Get_transitionLevel}|";      // 21

            //// Lv
            //Data += $"{EquipChar_List[i].Get_Character_Lv}|";         // 22
            //Data += $"{EquipChar_List[i].Get_Max_Lv}|";               // 23
            //Data += $"{EquipChar_List[i].Get_CurrentExp}|";           // 24
            //Data += $"{EquipChar_List[i].Get_Cumulative_Exp}|";       // 25

            //// Path
            //Data += $"{EquipChar_List[i].Get_Illust_Address}|";       // 26
            //Data += $"{EquipChar_List[i].Get_Normal_Image_Address}|"; // 27
            //Data += $"{EquipChar_List[i].Get_Grade_Up_Image_Address}|";// 28
            //Data += $"{EquipChar_List[i].Get_Profile_Address}|";      // 29
            //Data += $"{EquipChar_List[i].Get_White_Illust_Address}|"; // 30
            //Data += $"{EquipChar_List[i].Get_Pixel_Illust_Address}|"; // 31

            //if (EquipChar_List[i].Get_Square_Illust_Address == "null") // 32
            //{
            //    Data += $"NULL";
            //}
            //else
            //{
            //    Data += $"{EquipChar_List[i].Get_Square_Illust_Address}";
            //}
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
                // Debug.Log("���� ĳ���� ����Ʈ ������ ���� ����");
            },
            (_error) =>
            {
                Debug.Log("���� ĳ���� ����Ʈ ������ ���� ���� : " +_error.GenerateErrorReport());
                // Debug.Log("���� ĳ���� ����Ʈ ������ ���� ����");
            });
    }
    #endregion

    #region Original_CharacterListUpdate_Network
    private void UpdateCharListCo()
    {
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

        var allEntries = UserInfo.UserCharDict.ToList(); // Dictionary �� List<KeyValuePair>
        int chunkSize = 10;

        int totalChunks = Mathf.CeilToInt(allEntries.Count / (float)chunkSize);

        for (int i = 0; i < totalChunks; i++)
        {
            var chunk = allEntries.Skip(i * chunkSize).Take(chunkSize);

            CharacterListWrapper wrapper = new CharacterListWrapper();
            foreach (var pair in chunk)
            {
                wrapper.Characters.Add(new CharacterListPair { key = pair.Key, value = pair.Value });
            }

            string json = JsonUtility.ToJson(wrapper);
            string keyName = $"CharData_Part{i + 1}";

            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {
                { keyName, json }
            }
            };

            NetWaitTime = 0.5f;

            PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
                // Debug.Log("ĳ���� ����Ʈ ���� ���� ����");
            },
            (_error) =>
            {
                Debug.Log("ĳ���� ����Ʈ ���� ���� ���� : " + _error.GenerateErrorReport());
                // Debug.Log("ĳ���� ����Ʈ ���� ���� ����");
            });
        }
    }
    #endregion

    #region PacketCheck
    // ��Ŷ �ߺ� ���� �����ϰ� �߰��ϱ� ���� �Լ�
    public void PushPacket(PACKETTYPE _packetType)
    {
        // ������ false
        bool isExist = false;

        // �̹� ��Ŷ�� �����Ѵٸ� true
        for (int i = 0; i < PacketBuff.Count; i++)
        {
            if (PacketBuff[i] == _packetType)
            {
                isExist = true;
            }
        }

        // �̸� ���� ��Ŷ�� ���ٸ� �߰�
        if (isExist == false)
            PacketBuff.Add(_packetType);
    }
    #endregion
}

