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
    [SerializeField] float R_Summon_Rate; // 0.8
    [SerializeField] float SR_Summon_Rate; // 0.16
    [SerializeField] float SSR_Summon_Rate; // 0.04

    [Header("----Gacha_Video----")]
    [SerializeField] GameObject Gacha_Video;
    [SerializeField] VideoClip[] Gacha_Scenes;
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

    float Summon_Rate()
    {
        float RandomRate = Random.Range(0.01f, 1.0f);
        return RandomRate;
    }

    public void GachaInfo_Open(int _num)
    {
        if (!GameManager.Instance.TestMode)
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
        Gacha_Count = 0;
        GachaInfo_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Down();
        // GachaInfo_Panel.SetActive(false);
    }

    public void GachaFailInfo_Close()
    {
        Gacha_Count = 0;
        GachaFail_Info_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Down();
        // GachaInfo_Panel.SetActive(false);
    }
    #endregion

    #region Summon_System
    // TODO ## Gacha_Manager 가차 시스템
    public void Summon()
    {
        // TODO ## Gacha_Manager : TestMode
        if (!GameManager.Instance.TestMode)
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


        while (Gacha_Num < Gacha_Count)
        {
            float RandomRate = Summon_Rate();

            UserInfo.SSR_Set_Count++;
            UserInfo.SR_Set_Count++;

            if (UserInfo.SSR_Set_Count >= 80)
            {
                // Debug.Log(i + " ssr 확정");
                SSR_Summon();
                UserInfo.SSR_Set_Count = 0;
                isSSR_Summon = true;
            }
            else if (UserInfo.SR_Set_Count >= 10)
            {
                // Debug.Log(i + " sr 확정");
                SR_Summon();
                UserInfo.SR_Set_Count = 0;
                isSR_Summon = true;
            }
            else
            {
                if (SSR_Summon_Rate >= RandomRate)
                {
                    // Debug.Log(i + " ssr");
                    SSR_Summon();
                    UserInfo.SSR_Set_Count = 0;
                    isSSR_Summon = true;
                }
                else if (SSR_Summon_Rate < RandomRate && RandomRate <= SR_Summon_Rate)
                {
                    // Debug.Log(i + " sr");
                    SR_Summon();
                    UserInfo.SR_Set_Count = 0;
                    isSR_Summon = true;
                }
                else
                {
                    // Debug.Log(i + " r");
                    R_Summon();
                    isR_Summon = true;
                }
            }

            Gacha_Num++;
        }

        if (Gacha_Count == 10)
        {
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
        else
        {
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

        // UserInfo.UserCharDict_Copy = UserInfo.UserCharDict.ToDictionary(entry => entry.Key, entry => entry.Value); // 유저가 지닌 캐릭터 풀 복사
        UserInfo.UserCharDict_Copy = UserInfo.UserCharDict.ToList();
        UserInfo.UserCharDict_Copy_2 = UserInfo.UserCharDict.ToList();
      
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

        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CLEAR_EQUIP_CHAR);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.CHARLIST);
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
        // Debug.Log(character.Get_CharName);

        // SSR 캐릭터가 없다면
        //if (!UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName))
        if (!UserInfo.UserCharDict.ContainsKey(character.Get_CharName))
        {
            Gacha_New_Images[Gacha_Num].SetActive(true);
            //UserInfo.UserCharDict.Add(Character_List.SSR_Char[RandomSSR].Get_CharName, character);
            UserInfo.UserCharDict.Add(character.Get_CharName, character);
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20;

            for (int i = 0; i < DictionaryCtrl_Ref.SSR_Slot.Count; i++)
            {
                if (DictionaryCtrl_Ref.SSR_Slot[i].Get_Slot_Char.Get_CharName == character.Get_CharName)
                {
                    DictionaryCtrl_Ref.SSR_Slot[i].Set_UI_Refresh(true);
                    break;
                }
            }
        }
        // 등급이 아직 덜 올랐다면
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar < 5) 
        {
            New_PopUp_Active();
            UserInfo.UserCharDict[character.Get_CharName].Get_CharStar++;
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20 + (10 * UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
            // Debug.Log(UserInfo.UserCharDict[character.Get_CharName].Get_CharName + " : " + UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
        }
        // 등급이 다 올랐다면
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar >= 5)
        {
            New_PopUp_Active();
            Book_PopUp_Active(Books[2]);

            for (int i = 0; i < Item_List.Spend_Item_List.Count; i++)
            {
                if(Item_List.Spend_Item_List[i].Get_Item_Name == "고급 소환서")
                {
                    UserInfo.Add_Inventory_Item(Item_List.Spend_Item_List[i]);
                    break;
                }
            }

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
        GachaEnter_Transition.SetActive(true);
        Gacha_Video.SetActive(true);
        GachaCharacterList.SetActive(true);

        if (isR_Summon && !isSR_Summon && !isSSR_Summon)
        {
            Videoplayer.clip = Gacha_Scenes[0];
            Videoplayer.Play();

            isR_Summon = false;
            isSR_Summon = false;
            isSSR_Summon = false;

            return;
        }
        else if ((isR_Summon && isSR_Summon && !isSSR_Summon) || (!isR_Summon && isSR_Summon && !isSSR_Summon))
        {
            Videoplayer.clip = Gacha_Scenes[1];
            Videoplayer.Play();

            isR_Summon = false;
            isSR_Summon = false;
            isSSR_Summon = false;

            return;
        }
        else if ((isR_Summon && isSR_Summon && isSSR_Summon) || (!isR_Summon && isSR_Summon && isSSR_Summon) ||
            (isR_Summon && !isSR_Summon && isSSR_Summon) || (!isR_Summon && !isSR_Summon && isSSR_Summon))
        {
            Videoplayer.clip = Gacha_Scenes[2];
            Videoplayer.Play();

            isR_Summon = false;
            isSR_Summon = false;
            isSSR_Summon = false;

            return;
        } 
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
