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
            // ĳ���� ��� ������Ʈ
            UpdateCharListCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.EQUIP_CHAR_LIST)
        {
            // ���� ĳ���� ������Ʈ
            UpdateEquipCharCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.CLEAR_EQUIP_CHAR)
        {
            // ���� ĳ���� �ʱ�ȭ ������Ʈ
            UpdateClearEquipCharCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.ITEM_INVENTORY)
        {
            // �Ҹ�, ��ȭ ������ �κ��丮 ������Ʈ
            UpdateItemInvenCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.EQUIP_ITEM_INVENTORY)
        {
            // ��� ������ �κ��丮 ������Ʈ
            UpdateEquipItemInvenCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.DIA)
        {
            // ���̾� ������Ʈ
            UpdateDiaCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.MONEY)
        {
            // ��� ������Ʈ
            UpdateMoneyCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.PROFILE_IMG)
        {
            // ���� ������ ������Ʈ
            UpdateProfileCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.QUEST)
        {
            // ����Ʈ ������ ������Ʈ
            UpdateQuestCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.STAGE)
        {
            // �������� ���൵ ������Ʈ
            UpdateStageCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.GACHA_COUNT)
        {
            // �̱� ī��Ʈ ������Ʈ
            UpdateGachaCountCo();
        }
        PacketBuff.RemoveAt(0);
    }

    #region UserGachaCount
    private void UpdateGachaCountCo()
    {
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

        // SSR, SR ī��Ʈ�� ����
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                { "SSR_Count", UserInfo.SSR_Set_Count.ToString() },
                { "SR_Count", UserInfo.SR_Set_Count.ToString() }
            }
        };

        NetWaitTime = 1.0f;

        PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
            },
            (_error) =>
            {
                Debug.Log("���� ī��Ʈ ���� ���� : " + _error.GenerateErrorReport());
            });
    }
    #endregion

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

        // ��� ������ ���
        var allEntries = UserInfo.Equip_Inventory;
        // ��� ������
        int chunkSize = 10;

        // ���� ��ų ��� �ִ� �ݺ� ��
        int totalChunks = Mathf.CeilToInt(allEntries.Count / (float)chunkSize);

        // totalChunks��ŭ �ݺ�
        for (int i = 0; i < totalChunks; i++)
        {
            // allEntries�� ����� ����Ʈ���� 10���� ������ ����
            var chunk = allEntries.Skip(i * chunkSize).Take(chunkSize).ToList();

            // chunk�� ���� json���� ��ȯ ��Ű�� ���� EquipItemListWrapper���
            var wrapper = new EquipItemListWrapper(chunk);
            // json ��ȯ
            string json = JsonUtility.ToJson(wrapper, true);
            // ��� ������ PlayFab�� Key��
            string keyName = $"Equip_Inven_Part_{i + 1}";
            // Debug.Log(json);

            // ��û ������ �ۼ�
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {
                { 
                    keyName,
                    json 
                }
            }};

            NetWaitTime = 0.5f;

            // request�� ������ API�� ȣ���Ͽ� ������ ���� 
            PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
                // Debug.Log("���� ������ ����Ʈ ���� ����");
                if(LoadingPanel != null && LoadingPanel.gameObject.activeSelf)
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
        // UID�� ���ٸ� return;
        if (UserInfo.UID == "")
            return;

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

        // PlayFab�� �����ų ������ <EquipChar_i, ĳ���� �̸�>����
        Dictionary<string, string> CharData = new Dictionary<string, string>();
        // ���� ĳ���� ����Ʈ
        List<Character> EquipChar_List;
        // ���� ĳ���� ����Ʈ�� UserInfo.Equip_Characters
        EquipChar_List = UserInfo.Equip_Characters;

        // ���� ĳ���͸�ŭ �ݺ�
        for (int i = 0; i < EquipChar_List.Count; i++)
        {
            string Data = "";
            // ������ ���� ĳ���� �̸�
            Data = $"{EquipChar_List[i].Get_CharName}";
            // Ű ���� EquipChar_i
            CharData.Add($"EquipChar_{i}", Data);
        }

        // ��û�� ������
        var request = new UpdateUserDataRequest()
        {
            // UserData�� CharData ���ε�
            Data = CharData
        };

        NetWaitTime = 1.0f;

        // ���� request�� UserData�� ������Ʈ ��ų API ȣ�� 
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

        // �ε�â ���ֱ�
        DataNetwork_Mgr.Inst.LoadingPanel.gameObject.SetActive(true);

        // ĳ���� ���� Dictionary List<KeyValuePair>���·� ���� ����
        var allEntries = UserInfo.UserCharDict.ToList();
        // �ѹ��� ������ �� �ִ� Json�� ĳ���� �� 10�� 
        int chunkSize = 10;
        // ���� ���� ĳ���� 10ĳ���;� �ɰ��� �����͸� �����Ű�� ���� ��� �ݺ����� �ݺ��� ����
        // ex) ����ĳ���� 17ĳ�� 2�� �ݺ�, 21ĳ�� 3�� �ݺ�
        int totalChunks = Mathf.CeilToInt(allEntries.Count / (float)chunkSize);

        // totalChunks��ŭ �ݺ�
        for (int i = 0; i < totalChunks; i++)
        {
            // allEntriesũ�� �� chunkSize������ ��ŭ �߶� ��������
            // chunkSize���� allEntries������ �׳� allEntries�� ��ŭ�� ������ 
            var chunk = allEntries.Skip(i * chunkSize).Take(chunkSize);

            // Json���� �ٲٱ� ���� Wrapper ��ü ����
            CharacterListWrapper wrapper = new CharacterListWrapper();
            // CharacterListWrapper�� Characters�� �߰�
            foreach (var pair in chunk)
            {
                wrapper.Characters.Add(new CharacterListPair { key = pair.Key, value = pair.Value });
            }

            // ����� ĳ���� �� wrapper Json���� ��ȯ
            string json = JsonUtility.ToJson(wrapper);
            // Ű ���� CharData_Parti��°
            string keyName = $"CharData_Part{i + 1}";

            // ��û ������ �ۼ�
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {
                { keyName, json }
            }
            };

            NetWaitTime = 0.5f;

            // request�� ������ API�� ȣ���Ͽ� ������ ���� 
            PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
                // ���������� �Ϸ� �Ǿ��ٸ� �ε�â ���ֱ�
                if (LoadingPanel != null && LoadingPanel.gameObject.activeSelf)
                {
                    LoadingPanel.StartCoroutine(LoadingPanel.LoadImage());
                }
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

