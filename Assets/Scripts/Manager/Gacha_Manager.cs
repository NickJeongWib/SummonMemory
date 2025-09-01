using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using static Define;

public class Gacha_Manager : MonoBehaviour
{
    [SerializeField] Lobby_Manager LobbyManger_Ref;
    [SerializeField] CharacterList_UI CharListRef;
    [SerializeField] Dictionary_Ctrl DictionaryCtrl_Ref;

    [SerializeField] GameObject GachaEnter_Transition;
    [SerializeField] GameObject GachaCharacterList;
    [SerializeField] GameObject GachaInfo_Panel;
    [SerializeField] GameObject GachaFail_Info_Panel;

    // 연속 뽑기 횟수
    [SerializeField] Text Gacha_Count_Text;
    [SerializeField] int Gacha_Count;
    // 뽑기를 했을 떄 올라가는 카운트
    [SerializeField] int Gacha_Num;

    public List<Character> Gacha_Characters = new List<Character>();

    [Header("----Gacha_Frame_Shader----")]
    [SerializeField] Material SR_Frame;
    [SerializeField] Material SSR_Frame;

    [Header("----Gacha_Image----")]
    [SerializeField] Image[] Gacha_10_CharImages;
    [SerializeField] GameObject[] Gacha_Frame;
    [SerializeField] GameObject SingleGacha_Frame;
    [SerializeField] Image Gacha_CharImage;
    [SerializeField] GameObject Gacha_10;
    [SerializeField] GameObject Gacha_1;
    [SerializeField] GameObject[] Gacha_New_Images;
    [SerializeField] GameObject Gacha_New_Image;

    [SerializeField] GameObject[] Book_Images;
    [SerializeField] GameObject Book_Image;
    [SerializeField] Sprite[] Books;

    [Header("----SummonRate----")]
    [SerializeField] float R_Summon_Rate; // 0.86
    [SerializeField] float SR_Summon_Rate; // 0.12
    [SerializeField] float SSR_Summon_Rate; // 0.02

    [Header("----Gacha_Video----")]
    [SerializeField] GameObject Gacha_Video;
    [SerializeField] VideoClip[] Gacha_Scenes;
    [SerializeField] AudioClip[] Gacha_SFX_Path;
    [SerializeField] VideoPlayer Videoplayer;

    [Header("Inventory_UI")]
    [SerializeField] Inventory_UI InventoryUI_Ref;

    [Header("Gacha_Info")]
    [SerializeField] TextMeshProUGUI UserTicket;
    [SerializeField] Text FailInfoCount;
    [SerializeField] Text User_Ticket_Amount;

    bool isR_Summon;
    bool isSR_Summon;
    bool isSSR_Summon;

    private void Start()
    {
        // 천장 수치 출력
        Gacha_Count_Text.text = $"{UserInfo.SSR_Set_Count} / 80";
    }

    float Summon_Rate()
    {
        float RandomRate = Random.Range(0.00f, 1.0f);
        return RandomRate;
    }

    public void GachaInfo_Open(int _num)
    {
        SoundManager.Inst.PlayUISound();

        if (!GameManager.Inst.TestMode)
        {
            if (UserInfo.InventoryDict.ContainsKey("캐릭터 티켓") == false)
            {
                GachaFail_Info_Panel.SetActive(true);
                FailInfoCount.text = $"<color=red>{Mathf.Abs(_num)}</color>개 부족합니다.";
                return;
            }


            if (UserInfo.InventoryDict["캐릭터 티켓"].Get_Amount < _num)
            {
                GachaFail_Info_Panel.SetActive(true);
                FailInfoCount.text = $"<color=red>{Mathf.Abs(_num - UserInfo.InventoryDict["캐릭터 티켓"].Get_Amount)}</color>개 부족합니다.";
                return;
            }

            UserTicket.text = $"<color=orange>{UserInfo.InventoryDict["캐릭터 티켓"].Get_Amount}</color> <sprite=0> " +
                $"<color=red>{UserInfo.InventoryDict["캐릭터 티켓"].Get_Amount - _num}</color>";
        }

        Gacha_Count = _num;
        GachaInfo_Panel.SetActive(true);
        // GachaInfo_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Up();
    }

    #region Info_Close
    public void GachaInfo_Close()
    {
        SoundManager.Inst.PlayUISound();

        Gacha_Count = 0;
        GachaInfo_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Down();
        // GachaInfo_Panel.SetActive(false);
    }

    public void GachaFailInfo_Close()
    {
        SoundManager.Inst.PlayUISound();

        Gacha_Count = 0;
        GachaFail_Info_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Down();
        // GachaInfo_Panel.SetActive(false);
    }
    #endregion

    #region Summon_System
    // TODO ## Gacha_Manager 가차 시스템
    public void Summon()
    {
        // 아이템 뽑기 연출은 1.0f라서 0.7f로 바꿔주기
        Videoplayer.playbackSpeed = 0.7f;

        // TODO ## Gacha_Manager : TestMode
        if (!GameManager.Inst.TestMode)
        {
            // 티켓 감소, 슬롯 초기화
            UserInfo.InventoryDict["캐릭터 티켓"].Get_Amount -= Gacha_Count;
            UserInfo.Remove_Inventory_Item();
            InventoryUI_Ref.Reset_Spend_Inventory();
            InventoryUI_Ref.Spend_Slot_Refresh();
        }


        // 새로 뽑는 친구들을 채우기 위해 리스트 초기화
        Gacha_Characters.Clear();

        GachaInfo_Panel.SetActive(false);

        Gacha_Num = 0;

        // 정해진 가차만큼 반복 단일뽑기, 10연 뽑기인지
        while (Gacha_Num < Gacha_Count)
        {
            // 소환될 확률 받아오기
            float RandomRate = Summon_Rate();
            // 카운트 증가
            UserInfo.SSR_Set_Count++;
            UserInfo.SR_Set_Count++;

            // 만약 SSR_Set_Count가 80이상이 되면 SSR 획득
            if (UserInfo.SSR_Set_Count >= 80)
            {
                // SSR 소환
                SSR_Summon();
                // SSR 소환 카운트 초기화
                UserInfo.SSR_Set_Count = 0;
                // SSR의 뽑기 연출을 위해 true
                isSSR_Summon = true;
            }
            else if (UserInfo.SR_Set_Count >= 10)
            {
                // SR 소환
                SR_Summon();
                // SR 소환 카운트 초기화
                UserInfo.SR_Set_Count = 0;
                // SR의 뽑기 연출을 위해 true
                isSR_Summon = true;
            }
            // 확정 뽑기가 아니라면
            else
            {
                // 받은 랜덤 숫자가 SSR 확률보다 작으면 SSR소환 0.00~0.02 사이 수가 뜨면 SSR(2%)
                if (SSR_Summon_Rate >= RandomRate)
                {
                    SSR_Summon();
                    UserInfo.SSR_Set_Count = 0;
                    isSSR_Summon = true;
                }
                // 0.02% 초과이거나 0.12보다 작으면 0.03 ~ 0.12가 사이 수가 뜨면 SR(10%)
                else if (SSR_Summon_Rate < RandomRate && RandomRate <= SR_Summon_Rate)
                {
                    SR_Summon();
                    UserInfo.SR_Set_Count = 0;
                    isSR_Summon = true;
                }
                // SSR, SR범위를 모두 빗겨나간다면 R등급 소환
                else
                {
                    R_Summon();
                    isR_Summon = true;
                }
            }

            // 10연속 뽑기일 경우 다시 뽑기 위해 뽑기카운트 증가
            Gacha_Num++;
        }

        // 만약 10연속 뽑기일 경우
        if (Gacha_Count == 10)
        {
            // 뽑기가 종료 후 10연속으로 뽑은 캐릭터 목록 UI화면을 활성화 시킨다.
            Gacha_10.SetActive(true);

            for (int i = 0; i < Gacha_Characters.Count; i++)
            {
                Gacha_10_CharImages[i].sprite = Gacha_Characters[i].Get_Normal_Img;

                // 가차 프레임 머티리얼 변경
                #region Gacha_Frame
                if (Gacha_Characters[i].Get_CharGrade == Define.CHAR_GRADE.SSR)
                {
                    Gacha_Frame[i].GetComponent<Image>().enabled = true;
                    Gacha_Frame[i].GetComponent<Image>().material = SSR_Frame;
                }
                else if (Gacha_Characters[i].Get_CharGrade == Define.CHAR_GRADE.SR)
                {
                    Gacha_Frame[i].GetComponent<Image>().enabled = true;
                    Gacha_Frame[i].GetComponent<Image>().material = SR_Frame;
                }
                else
                {
                    Gacha_Frame[i].GetComponent<Image>().enabled = false;
                }
                #endregion
            }
        }
        // 단일 뽑기 였다면
        else
        {
            // 뽑기가 종료 후 단일 뽑기로 뽑은 캐릭터 목록 UI화면을 활성화 시킨다.
            Gacha_1.SetActive(true);

            Gacha_CharImage.sprite = Gacha_Characters[0].Get_Normal_Img;
            SingleGacha_Frame.GetComponent<Image>().enabled = true;

            // 가차 프레임 머티리얼 변경
            #region Gacha_Frame
            if (Gacha_Characters[0].Get_CharGrade == Define.CHAR_GRADE.SSR)
            {
                SingleGacha_Frame.GetComponent<Image>().material = SSR_Frame;
            }
            else if (Gacha_Characters[0].Get_CharGrade == Define.CHAR_GRADE.SR)
            {
                SingleGacha_Frame.GetComponent<Image>().material = SR_Frame;
            }
            else
            {
                SingleGacha_Frame.GetComponent<Image>().enabled = false;
            }
            #endregion
        }

        Gacha_Count_Text.text = $"{UserInfo.SSR_Set_Count} / 80";

        // 원본 Dictionary 복사
        foreach(var character in UserInfo.UserCharDict)
        {
            UserInfo.UserCharDict_Copy.Add(new KeyValuePair<string, Character>(character.Key, character.Value));
            UserInfo.UserCharDict_Copy_2.Add(new KeyValuePair<string, Character>(character.Key, character.Value));
        }
      
        // 장착된 캐릭터는 캐릭터 리스트에 추가하지 않기 위해 제거
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            if (UserInfo.UserCharDict_Copy.Contains(new KeyValuePair<string, Character>(UserInfo.Equip_Characters[i].Get_CharName, UserInfo.Equip_Characters[i])))
            {
                UserInfo.UserCharDict_Copy.Remove(new KeyValuePair<string, Character>(UserInfo.Equip_Characters[i].Get_CharName, UserInfo.Equip_Characters[i]));
            }

            // 뽑은 캐릭터나 중복으로 뽑은 캐릭터가 장착 중일 시 장착 중인 캐릭터의 데이터를 바꿔준다.
            if(UserInfo.UserCharDict.Contains(new KeyValuePair<string, Character>(UserInfo.Equip_Characters[i].Get_CharName, UserInfo.Equip_Characters[i])))
            {
                UserInfo.Equip_Characters[i] = UserInfo.UserCharDict[$"{UserInfo.Equip_Characters[i].Get_CharName}"];
            }
        }

        // 캐릭터 인벤토리 Refresh
        CharListRef.Refresh_CharacterList();
        Refresh_SummonTicket();
        // 바로 위 장착 캐릭터 예외 처리 후 저장이 필요함
        UserInfo.Old_UserCharDict_Copy = UserInfo.UserCharDict_Copy.ToList();

        // 데이터 저장
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.GACHA_COUNT);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CLEAR_EQUIP_CHAR);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CHARLIST);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.ITEM_INVENTORY);

        // UI초기화 함수들 호출
        LobbyManger_Ref.Refresh_User_CharAmount();
        LobbyManger_Ref.Refresh_User_CombatPower();
        LobbyManger_Ref.Refresh_OwnChar_Profile();

        // 플레이펩에 저장될 때까지 기다렸다가 영상이 재생되게 대기
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        DataNetwork_Mgr.Inst.LoadingPanel.gameObject.SetActive(true);
        // 패널이 꺼질 때까지 대기
        yield return new WaitUntil(() => DataNetwork_Mgr.Inst.LoadingPanel.gameObject.activeInHierarchy == false);

        Gacha_Video_Play();
    }

    #endregion

    #region SummonRate
    void SSR_Summon()
    {
        int RandomSSR = Random.Range(0, Character_List.SSR_Char.Count);

        #region SSR 목록 검사
        //if (Character_List.SSR_Char == null)
        //{
        //    Debug.Log("SSR리스트 찾을수 없음");
        //    return;
        //}
        #endregion

        Character character = Character_List.SSR_Char[RandomSSR];

        Gacha_Characters.Add(character);

        // SSR 캐릭터가 없다면
        if (!UserInfo.UserCharDict.ContainsKey(character.Get_CharName))
        {
            // 새로운 캐릭터를 뽑았기 때문에 해당 Index의 New이미지를 켜준다.
            Gacha_New_Images[Gacha_Num].SetActive(true);
            // UserInfo의 캐릭터 리스트 원본에 저장
            UserInfo.UserCharDict.Add(character.Get_CharName, character);
            // UserInfo.UserCharDict[character.Get_CharName]의 최대 성장 레벨을 20으로 만든다.
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20;

            // 도감에서 SSR 슬롯 항목의 크기만큼 반복
            for (int i = 0; i < DictionaryCtrl_Ref.SSR_Slot.Count; i++)
            {
                // 만약 i번째 슬롯의 캐릭터 이름이 뽑은 캐릭터와 이름이 동일하다면
                if (DictionaryCtrl_Ref.SSR_Slot[i].Get_Slot_Char.Get_CharName == character.Get_CharName)
                {
                    // UI에서 Lock된 이미지들을 비활성화시키기 위한 함수
                    DictionaryCtrl_Ref.SSR_Slot[i].Set_UI_Refresh(true);
                    break;
                }
            }
        }
        // 등급이 아직 덜 올랐다면
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar < 5) 
        {
            New_PopUp_Active();
            // 한계돌파 증가
            UserInfo.UserCharDict[character.Get_CharName].Get_CharStar++;
            // 한계돌파 1단계 당 10레벨 추가 성장 가능 최대 70레벨까지 성장 가능
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20 + (10 * UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
        }
        // 등급이 다 올랐다면
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar >= 5)
        {
            New_PopUp_Active();
            // 소환서 이미지 활성화
            Book_PopUp_Active(Books[2]);

            // Item_List.Spend_Item_List의 카운트만큼 반복
            for (int i = 0; i < Item_List.Spend_Item_List.Count; i++)
            {
                // 스프레드 시트에서 받아올 때 만든 Item_List의 Spend_Item_List에서 i번째에 "고급 소환서"가 있다면
                if (Item_List.Spend_Item_List[i].Get_Item_Name == "고급 소환서")
                {
                    // Item_List.Spend_Item_List[i](고급 소환서) 아이템을 인벤토리에 추가한다.
                    UserInfo.Add_Inventory_Item(Item_List.Spend_Item_List[i]);
                    break;
                }
            }
            // 고급 소환서를 획득했으니 인벤토리 Refresh
            InventoryUI_Ref.Spend_Slot_Refresh();
        }  
    }

    void SR_Summon()
    {
        int RandomSR = Random.Range(0, Character_List.SR_Char.Count);

        #region SR 목록 검사
        //if (Character_List.SR_Char == null)
        //{
        //    Debug.Log("SR리스트 찾을수 없음");
        //    return;
        //}
        #endregion

        Character character = Character_List.SR_Char[RandomSR];

        Gacha_Characters.Add(character);
        // Debug.Log(character.Get_CharName);

        // SR 캐릭터가 없다면
        // if (!UserInfo.UserCharDict.ContainsKey(Character_List.SR_Char[RandomSR].Get_CharName))
        if (!UserInfo.UserCharDict.ContainsKey(character.Get_CharName))
        {
            Gacha_New_Images[Gacha_Num].SetActive(true);
            //UserInfo.UserCharDict.Add(Character_List.SR_Char[RandomSR].Get_CharName, character);
            UserInfo.UserCharDict.Add(character.Get_CharName, character);
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20;

            for (int i = 0; i < DictionaryCtrl_Ref.SR_Slot.Count; i++)
            {
                if (DictionaryCtrl_Ref.SR_Slot[i].Get_Slot_Char.Get_CharName == character.Get_CharName)
                {
                    DictionaryCtrl_Ref.SR_Slot[i].Set_UI_Refresh(true);
                    break;
                }
            }
        }
        // 등급이 아직 덜 올랐다면
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SR_Char[RandomSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar < 5)
        {
            New_PopUp_Active();
            UserInfo.UserCharDict[character.Get_CharName].Get_CharStar++;
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20 + (10 * UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
            // Debug.Log(UserInfo.UserCharDict[character.Get_CharName].Get_CharName + " : " + UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
        }
        // 등급이 다 올랐다면
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SR_Char[RandomSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar >= 5)
        {
            New_PopUp_Active();
            Book_PopUp_Active(Books[1]);

            for (int i = 0; i < Item_List.Spend_Item_List.Count; i++)
            {
                if (Item_List.Spend_Item_List[i].Get_Item_Name == "중급 소환서")
                {
                    UserInfo.Add_Inventory_Item(Item_List.Spend_Item_List[i]);
                    break;
                }
            }

            InventoryUI_Ref.Spend_Slot_Refresh();
        }
    }

    void R_Summon()
    {
        int RandomR = Random.Range(0, Character_List.R_Char.Count);

        #region R 목록 검사
        //if (Character_List.R_Char == null)
        //{
        //    Debug.Log("SR리스트 찾을수 없음");
        //    return;
        //}
        #endregion

        Character character = Character_List.R_Char[RandomR];

        Gacha_Characters.Add(character);
        // Debug.Log(character.Get_CharName);

        // R 캐릭터가 없다면
        //if (!UserInfo.UserCharDict.ContainsKey(Character_List.R_Char[RandomR].Get_CharName))
        if (!UserInfo.UserCharDict.ContainsKey(character.Get_CharName))
        {
            Gacha_New_Images[Gacha_Num].SetActive(true);
            // UserInfo.UserCharDict.Add(Character_List.R_Char[RandomR].Get_CharName, character);
            UserInfo.UserCharDict.Add(character.Get_CharName, character);
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20;

            for (int i = 0; i < DictionaryCtrl_Ref.R_Slot.Count; i++)
            {
                if (DictionaryCtrl_Ref.R_Slot[i].Get_Slot_Char.Get_CharName == character.Get_CharName)
                {
                    DictionaryCtrl_Ref.R_Slot[i].Set_UI_Refresh(true);
                    break;
                }
            }
        }
        // 등급이 아직 덜 올랐다면
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.R_Char[RandomR].Get_CharName) &&  UserInfo.UserCharDict[character.Get_CharName].Get_CharStar < 5)
        {
            New_PopUp_Active();
            UserInfo.UserCharDict[character.Get_CharName].Get_CharStar++;
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20 + (10 * UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
            // Debug.Log(UserInfo.UserCharDict[character.Get_CharName].Get_CharName + " : " + UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
        }
        // 등급이 다 올랐다면
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.R_Char[RandomR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar >= 5)
        {
            New_PopUp_Active();
            Book_PopUp_Active(Books[0]);

            for (int i = 0; i < Item_List.Spend_Item_List.Count; i++)
            {
                if (Item_List.Spend_Item_List[i].Get_Item_Name == "하급 소환서")
                {
                    UserInfo.Add_Inventory_Item(Item_List.Spend_Item_List[i]);
                    break;
                }
            }
            InventoryUI_Ref.Spend_Slot_Refresh();
        }
    }

    // 새로운 캐릭터 뽑았을 때 New 팝업 표시
    void New_PopUp_Active()
    {
        if (Gacha_Count == 10)
        {
            Gacha_New_Images[Gacha_Num].SetActive(false);
        }
        else
        {
            Gacha_New_Image.SetActive(false);
        }
    }

    void Book_PopUp_Active(Sprite _sprite)
    {
        if (Gacha_Count == 10)
        {
            Book_Images[Gacha_Num].SetActive(true);
            Book_Images[Gacha_Num].transform.GetChild(0).GetComponent<Image>().sprite = _sprite;

        }
        else
        {
            Book_Image.SetActive(true);
            Book_Image.transform.GetChild(0).GetComponent<Image>().sprite = _sprite;
        }
    }
    #endregion

    // TODO ## Gacha_Manager 가차연출 구현
    #region Gacha_Movie
    void Gacha_Video_Play()
    {
        // 영상 실행 초기 자연스러운 전환을 위한 UI들 활성화
        GachaEnter_Transition.SetActive(true);
        Gacha_Video.SetActive(true);
        // 영상 시작과 동시에 나온 캐릭터들 목록 활성화
        GachaCharacterList.SetActive(true);

        // R등급 캐릭터만 소환됬다면
        if (isR_Summon && !isSR_Summon && !isSSR_Summon)
        {
            Gacha_VideoPlay(0);

            return;
        }
        // SR이상 캐릭터가 소환 되었다면
        else if ((isR_Summon && isSR_Summon && !isSSR_Summon) || (!isR_Summon && isSR_Summon && !isSSR_Summon))
        {
            Gacha_VideoPlay(1);

            return;
        }
        // SSR이상 캐릭터가  소환 되 었다면
        else if ((isR_Summon && isSR_Summon && isSSR_Summon) || (!isR_Summon && isSR_Summon && isSSR_Summon) ||
            (isR_Summon && !isSR_Summon && isSSR_Summon) || (!isR_Summon && !isSR_Summon && isSSR_Summon))
        {
            Gacha_VideoPlay(2);

            return;
        } 
    }

    // 등급에 따른 다른 영상 출력
    void Gacha_VideoPlay(int _index)
    {
        Videoplayer.clip = Gacha_Scenes[_index];
        SoundManager.Inst.PlayGachaSound(Gacha_SFX_Path[_index]);
        Videoplayer.Play();
        // 무슨 등급 뽑았는지 초기화
        isR_Summon = false;
        isSR_Summon = false;
        isSSR_Summon = false;
    }
    #endregion

    #region UI
    public void Refresh_SummonTicket()
    {
        if (UserInfo.InventoryDict.ContainsKey("캐릭터 티켓"))
        {
            User_Ticket_Amount.text = $"{UserInfo.InventoryDict["캐릭터 티켓"].Get_Amount}";
        }
        else
        {
            User_Ticket_Amount.text = "0";
        }
    }
    #endregion
}
