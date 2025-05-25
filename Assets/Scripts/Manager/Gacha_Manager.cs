using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Linq;

public class Gacha_Manager : MonoBehaviour
{
    [SerializeField] CharacterList_UI CharListRef;

    [SerializeField] GameObject GachaEnter_Transition;
    [SerializeField] GameObject GachaCharacterList;
    [SerializeField] GameObject GachaInfo_Panel;

    // ���� �̱� Ƚ�� 
    [SerializeField] int Gacha_Count;
    // �̱⸦ ���� �� �ö󰡴� ī��Ʈ
    [SerializeField] int Gacha_Num;

    public List<Character> Gacha_Characters = new List<Character>();

    [Header("----Gacha_Image----")]
    [SerializeField] Image[] Gacha_10_CharImages;
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

    // TODO ## Build Fix Static ���� 
    [SerializeField] public int SR_Set_Count;
    [SerializeField] public int SSR_Set_Count;

    [Header("----Gacha_Video----")]
    [SerializeField] GameObject Gacha_Video;
    [SerializeField] VideoClip[] Gacha_Scenes;
    [SerializeField] VideoPlayer Videoplayer;

    [Header("Inventory_UI")]
    [SerializeField] Inventory_UI InventoryUI_Ref;

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
        Gacha_Count = _num;
        GachaInfo_Panel.SetActive(true);
        // GachaInfo_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Up();
    }

    public void GachaInfo_Close()
    {
        Gacha_Count = 0;
        GachaInfo_Panel.transform.GetChild(0).GetComponent<Pop_UpDown>().Pop_Down();
        // GachaInfo_Panel.SetActive(false);
    }

    // TODO ## Gacha_Manager ���� �ý���
    public void Summon()
    {
        // ���� �̴� ģ������ ä��� ���� ����Ʈ �ʱ�ȭ
        Gacha_Characters.Clear();

        GachaInfo_Panel.SetActive(false);

        Gacha_Num = 0;


        while (Gacha_Num < Gacha_Count)
        {
            float RandomRate = Summon_Rate();

            SSR_Set_Count++;
            SR_Set_Count++;

            if (SSR_Set_Count >= 80)
            {
                // Debug.Log(i + " ssr Ȯ��");
                SSR_Summon();
                SSR_Set_Count = 0;
                isSSR_Summon = true;
            }
            else if (SR_Set_Count >= 10)
            {
                // Debug.Log(i + " sr Ȯ��");
                SR_Summon();
                SR_Set_Count = 0;
                isSR_Summon = true;
            }
            else
            {
                if (SSR_Summon_Rate >= RandomRate)
                {
                    // Debug.Log(i + " ssr");
                    SSR_Summon();
                    SSR_Set_Count = 0;
                    isSSR_Summon = true;
                }
                else if (SSR_Summon_Rate < RandomRate && RandomRate <= SR_Summon_Rate)
                {
                    // Debug.Log(i + " sr");
                    SR_Summon();
                    SR_Set_Count = 0;
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
            }
        }
        else
        {
            Gacha_1.SetActive(true);

            Gacha_CharImage.sprite = Gacha_Characters[0].Get_Normal_Img;
        }

        // UserInfo.UserCharDict_Copy = UserInfo.UserCharDict.ToDictionary(entry => entry.Key, entry => entry.Value); // ������ ���� ĳ���� Ǯ ����
        UserInfo.UserCharDict_Copy = UserInfo.UserCharDict.ToList();
        UserInfo.UserCharDict_Copy_2 = UserInfo.UserCharDict.ToList();
      
        // ������ ĳ���ʹ� ĳ���� ����Ʈ�� �߰����� �ʱ� ���� ����
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            if (UserInfo.UserCharDict_Copy.Contains(new KeyValuePair<string, Character>(UserInfo.Equip_Characters[i].Get_CharName, UserInfo.Equip_Characters[i])))
            {
                UserInfo.UserCharDict_Copy.Remove(new KeyValuePair<string, Character>(UserInfo.Equip_Characters[i].Get_CharName, UserInfo.Equip_Characters[i]));
            }

            // ���� ĳ���ͳ� �ߺ����� ���� ĳ���Ͱ� ���� ���� �� ���� ���� ĳ������ �����͸� �ٲ��ش�.
            if(UserInfo.UserCharDict.Contains(new KeyValuePair<string, Character>(UserInfo.Equip_Characters[i].Get_CharName, UserInfo.Equip_Characters[i])))
            {
                UserInfo.Equip_Characters[i] = UserInfo.UserCharDict[$"{UserInfo.Equip_Characters[i].Get_CharName}"];
            }
        }

        // ĳ���� �κ��丮 Refresh
        CharListRef.Refresh_CharacterList();

        // �ٷ� �� ���� ĳ���� ���� ó�� �� ������ �ʿ���
        UserInfo.Old_UserCharDict_Copy = UserInfo.UserCharDict_Copy.ToList();

        Gacha_Video_Play();
    }

    #region SummonRate
    void SSR_Summon()
    {
        int RandomSSR = Random.Range(0, Character_List.SSR_Char.Count);

        #region SSR ��� �˻�
        //if (Character_List.SSR_Char == null)
        //{
        //    Debug.Log("SSR����Ʈ ã���� ����");
        //    return;
        //}
        #endregion

        Character character = Character_List.SSR_Char[RandomSSR];

        Gacha_Characters.Add(character);
        // Debug.Log(character.Get_CharName);

        // SSR ĳ���Ͱ� ���ٸ�
        if (!UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName))
        {
            Gacha_New_Images[Gacha_Num].SetActive(true);
            UserInfo.UserCharDict.Add(Character_List.SSR_Char[RandomSSR].Get_CharName, character);
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20;
        }
        // ����� ���� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar < 5) 
        {
            New_PopUp_Active();
            UserInfo.UserCharDict[character.Get_CharName].Get_CharStar++;
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20 + (10 * UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
            // Debug.Log(UserInfo.UserCharDict[character.Get_CharName].Get_CharName + " : " + UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
        }
        // ����� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar >= 5)
        {
            New_PopUp_Active();
            Book_PopUp_Active(Books[2]);

            for (int i = 0; i < Item_List.Spend_Item_List.Count; i++)
            {
                if(Item_List.Spend_Item_List[i].Get_Item_Name == "��� ��ȯ��")
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

        #region SR ��� �˻�
        //if (Character_List.SR_Char == null)
        //{
        //    Debug.Log("SR����Ʈ ã���� ����");
        //    return;
        //}
        #endregion

        Character character = Character_List.SR_Char[RandomSR];

        Gacha_Characters.Add(character);
        // Debug.Log(character.Get_CharName);

        // SR ĳ���Ͱ� ���ٸ�
        if (!UserInfo.UserCharDict.ContainsKey(Character_List.SR_Char[RandomSR].Get_CharName))
        {
            Gacha_New_Images[Gacha_Num].SetActive(true);
            UserInfo.UserCharDict.Add(Character_List.SR_Char[RandomSR].Get_CharName, character);
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20;
        }
        // ����� ���� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SR_Char[RandomSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar < 5)
        {
            New_PopUp_Active();
            UserInfo.UserCharDict[character.Get_CharName].Get_CharStar++;
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20 + (10 * UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
            // Debug.Log(UserInfo.UserCharDict[character.Get_CharName].Get_CharName + " : " + UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
        }
        // ����� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SR_Char[RandomSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar >= 5)
        {
            New_PopUp_Active();
            Book_PopUp_Active(Books[1]);

            for (int i = 0; i < Item_List.Spend_Item_List.Count; i++)
            {
                if (Item_List.Spend_Item_List[i].Get_Item_Name == "�߱� ��ȯ��")
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

        #region R ��� �˻�
        //if (Character_List.R_Char == null)
        //{
        //    Debug.Log("SR����Ʈ ã���� ����");
        //    return;
        //}
        #endregion

        Character character = Character_List.R_Char[RandomR];

        Gacha_Characters.Add(character);
        // Debug.Log(character.Get_CharName);

        // R ĳ���Ͱ� ���ٸ�
        if (!UserInfo.UserCharDict.ContainsKey(Character_List.R_Char[RandomR].Get_CharName))
        {
            Gacha_New_Images[Gacha_Num].SetActive(true);
            UserInfo.UserCharDict.Add(Character_List.R_Char[RandomR].Get_CharName, character);
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20;
        }
        // ����� ���� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.R_Char[RandomR].Get_CharName) &&  UserInfo.UserCharDict[character.Get_CharName].Get_CharStar < 5)
        {
            New_PopUp_Active();
            UserInfo.UserCharDict[character.Get_CharName].Get_CharStar++;
            UserInfo.UserCharDict[character.Get_CharName].Get_Max_Lv = 20 + (10 * UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
            // Debug.Log(UserInfo.UserCharDict[character.Get_CharName].Get_CharName + " : " + UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
        }
        // ����� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.R_Char[RandomR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar >= 5)
        {
            New_PopUp_Active();
            Book_PopUp_Active(Books[0]);

            for (int i = 0; i < Item_List.Spend_Item_List.Count; i++)
            {
                if (Item_List.Spend_Item_List[i].Get_Item_Name == "�ϱ� ��ȯ��")
                {
                    UserInfo.Add_Inventory_Item(Item_List.Spend_Item_List[i]);
                    break;
                }
            }
            InventoryUI_Ref.Spend_Slot_Refresh();
        }
    }

    // ���ο� ĳ���� �̾��� �� New �˾� ǥ��
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

    // TODO ## Gacha_Manager �������� ����
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
}
