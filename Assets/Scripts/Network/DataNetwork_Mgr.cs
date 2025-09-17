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
            // 캐릭터 목록 업데이트
            UpdateCharListCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.EQUIP_CHAR_LIST)
        {
            // 장착 캐릭터 업데이트
            UpdateEquipCharCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.CLEAR_EQUIP_CHAR)
        {
            // 장착 캐릭터 초기화 업데이트
            UpdateClearEquipCharCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.ITEM_INVENTORY)
        {
            // 소모, 강화 아이템 인벤토리 업데이트
            UpdateItemInvenCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.EQUIP_ITEM_INVENTORY)
        {
            // 장비 아이템 인벤토리 업데이트
            UpdateEquipItemInvenCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.DIA)
        {
            // 다이아 업데이트
            UpdateDiaCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.MONEY)
        {
            // 골드 업데이트
            UpdateMoneyCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.PROFILE_IMG)
        {
            // 유저 프로필 업데이트
            UpdateProfileCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.QUEST)
        {
            // 퀘스트 데이터 업데이트
            UpdateQuestCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.STAGE)
        {
            // 스테이지 진행도 업데이트
            UpdateStageCo();
        }
        else if (PacketBuff[0] == PACKETTYPE.GACHA_COUNT)
        {
            // 뽑기 카운트 업데이트
            UpdateGachaCountCo();
        }
        PacketBuff.RemoveAt(0);
    }

    #region UserGachaCount
    private void UpdateGachaCountCo()
    {
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        // SSR, SR 카운트를 저장
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
                Debug.Log("가차 카운트 저장 실패 : " + _error.GenerateErrorReport());
            });
    }
    #endregion

    #region StageClear
    private void UpdateStageCo()
    {
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        StageClearListWrapper wrapper = new StageClearListWrapper();
        wrapper.StageClear = UserInfo.StageClear;

        // 리스트 전체 직렬화
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
                // Debug.Log("퀘스트 리스트 저장 성공!");
            },
            (_error) =>
            {
                Debug.LogError("퀘스트 저장 실패: " + _error.GenerateErrorReport());
            });
    }
    #endregion

    #region QuestData
    private void UpdateQuestCo()
    {
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        QuestDataListWrapper wrapper = new QuestDataListWrapper();
        wrapper.QuestData_List = UserInfo.QuestData_List;

        // 리스트 전체 직렬화
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
                // Debug.Log("퀘스트 리스트 저장 성공!");
            },
            (_error) =>
            {
                Debug.LogError("퀘스트 저장 실패: " + _error.GenerateErrorReport());
            });
    }
    #endregion

    #region Profile_Img
    private void UpdateProfileCo()
    {
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        string Sprites_Path = "";

        // 이미지 주소 나열
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
               Debug.Log("프로필 저장 성공");
           },
           (_error) =>
           {
               Debug.Log(_error.GenerateErrorReport());
               Debug.Log("프로필 저장 실패");
           });
    }
    #endregion

    #region Dia_Data
    private void UpdateDiaCo()
    {
        // UID가 없다면 return;
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
               Debug.Log("다이아 저장 성공");
           },
           (_error) =>
           {
               Debug.Log(_error.GenerateErrorReport());
               Debug.Log("다이아 저장 실패");
           });
    }
    #endregion

    #region Money_Data
    private void UpdateMoneyCo()
    {
        // UID가 없다면 return;
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
               Debug.Log("보유골드 저장 성공");
           },
           (_error) =>
           {
               Debug.Log(_error.GenerateErrorReport());
               Debug.Log("보유골드 저장 실패");
           });
    }
    #endregion

    #region Equip_Item_Inven_Data
    private void UpdateEquipItemInvenCo()
    {
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        // 장비 아이템 목록
        var allEntries = UserInfo.Equip_Inventory;
        // 덩어리 사이즈
        int chunkSize = 10;

        // 저장 시킬 덩어리 최대 반복 값
        int totalChunks = Mathf.CeilToInt(allEntries.Count / (float)chunkSize);

        // totalChunks만큼 반복
        for (int i = 0; i < totalChunks; i++)
        {
            // allEntries의 장비목록 리스트에서 10개씩 가지고 오기
            var chunk = allEntries.Skip(i * chunkSize).Take(chunkSize).ToList();

            // chunk의 값을 json으로 변환 시키기 위해 EquipItemListWrapper사용
            var wrapper = new EquipItemListWrapper(chunk);
            // json 변환
            string json = JsonUtility.ToJson(wrapper, true);
            // 장비 아이템 PlayFab의 Key값
            string keyName = $"Equip_Inven_Part_{i + 1}";
            // Debug.Log(json);

            // 요청 데이터 작성
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {
                { 
                    keyName,
                    json 
                }
            }};

            NetWaitTime = 0.5f;

            // request의 데이터 API를 호출하여 데이터 저장 
            PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
                // Debug.Log("장착 아이템 리스트 저장 성공");
                if(LoadingPanel != null && LoadingPanel.gameObject.activeSelf)
                {
                    LoadingPanel.StartCoroutine(LoadingPanel.LoadImage());
                }
            },
            (_error) =>
            {
                Debug.Log("장착 아이템 리스트 저장 실패 : " + _error.GenerateErrorReport());
            });
        }
    }
    #endregion

    #region Item_InvenUpdate
    private void UpdateItemInvenCo()
    {
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        var allEntries = UserInfo.InventoryDict.ToList(); // Dictionary → List<KeyValuePair>
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
                // Debug.Log("아이템 리스트 저장 성공");
            },
            (_error) =>
            {
                Debug.Log("아이템 리스트 저장 실패 : " +_error.GenerateErrorReport());
            });
        }
    }
    #endregion

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
                    Debug.Log("장착 캐릭터 : " + error.GenerateErrorReport());
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
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        var Get_request = new GetUserDataRequest();
        // 키값 삭제하기위한 리스트
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

            // 리스트에 저장된 값들 삭제
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
                    Debug.Log("장착 캐릭터 초기화: " + error.GenerateErrorReport());
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

        // PlayFab에 저장시킬 데이터 <EquipChar_i, 캐릭터 이름>형식
        Dictionary<string, string> CharData = new Dictionary<string, string>();
        // 장착 캐릭터 리스트
        List<Character> EquipChar_List;
        // 장착 캐릭터 리스트는 UserInfo.Equip_Characters
        EquipChar_List = UserInfo.Equip_Characters;

        // 장착 캐릭터만큼 반복
        for (int i = 0; i < EquipChar_List.Count; i++)
        {
            string Data = "";
            // 데이터 값은 캐릭터 이름
            Data = $"{EquipChar_List[i].Get_CharName}";
            // 키 값은 EquipChar_i
            CharData.Add($"EquipChar_{i}", Data);
        }

        // 요청할 데이터
        var request = new UpdateUserDataRequest()
        {
            // UserData에 CharData 업로드
            Data = CharData
        };

        NetWaitTime = 1.0f;

        // 위의 request를 UserData에 업데이트 시킬 API 호출 
        PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
                // Debug.Log("장착 캐릭터 리스트 데이터 저장 성공");
            },
            (_error) =>
            {
                Debug.Log("장착 캐릭터 리스트 데이터 저장 실패 : " +_error.GenerateErrorReport());
                // Debug.Log("장착 캐릭터 리스트 데이터 저장 실패");
            });
    }
    #endregion

    #region Original_CharacterListUpdate_Network
    private void UpdateCharListCo()
    {
        // UID가 없다면 return;
        if (UserInfo.UID == "")
            return;

        // 로딩창 켜주기
        DataNetwork_Mgr.Inst.LoadingPanel.gameObject.SetActive(true);

        // 캐릭터 보유 Dictionary List<KeyValuePair>형태로 원본 복사
        var allEntries = UserInfo.UserCharDict.ToList();
        // 한번에 저장할 수 있는 Json의 캐릭터 수 10개 
        int chunkSize = 10;
        // 현재 보유 캐릭터 10캐릭터씩 쪼개서 데이터를 저장시키기 위해 몇번 반복할지 반복값 설정
        // ex) 보유캐릭터 17캐릭 2번 반복, 21캐릭 3번 반복
        int totalChunks = Mathf.CeilToInt(allEntries.Count / (float)chunkSize);

        // totalChunks만큼 반복
        for (int i = 0; i < totalChunks; i++)
        {
            // allEntries크기 중 chunkSize사이즈 만큼 잘라서 가져오기
            // chunkSize보다 allEntries작으면 그냥 allEntries값 만큼만 가져옴 
            var chunk = allEntries.Skip(i * chunkSize).Take(chunkSize);

            // Json으로 바꾸기 위한 Wrapper 객체 생성
            CharacterListWrapper wrapper = new CharacterListWrapper();
            // CharacterListWrapper의 Characters에 추가
            foreach (var pair in chunk)
            {
                wrapper.Characters.Add(new CharacterListPair { key = pair.Key, value = pair.Value });
            }

            // 저장된 캐릭터 값 wrapper Json으로 변환
            string json = JsonUtility.ToJson(wrapper);
            // 키 값은 CharData_Parti번째
            string keyName = $"CharData_Part{i + 1}";

            // 요청 데이터 작성
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {
                { keyName, json }
            }
            };

            NetWaitTime = 0.5f;

            // request의 데이터 API를 호출하여 데이터 저장 
            PlayFabClientAPI.UpdateUserData(request,
            (_result) =>
            {
                // 성공적으로 완료 되었다면 로딩창 꺼주기
                if (LoadingPanel != null && LoadingPanel.gameObject.activeSelf)
                {
                    LoadingPanel.StartCoroutine(LoadingPanel.LoadImage());
                }
            },
            (_error) =>
            {
                Debug.Log("캐릭터 리스트 원본 저장 실패 : " + _error.GenerateErrorReport());
                // Debug.Log("캐릭터 리스트 원본 저장 실패");
            });
        }
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

