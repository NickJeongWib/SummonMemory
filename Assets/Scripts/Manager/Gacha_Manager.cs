using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Gacha_Manager : MonoBehaviour
{
    [SerializeField] CharacterList_UI CharListRef;

    [SerializeField] GameObject GachaEnter_Transition;
    [SerializeField] GameObject GachaCharacterList;

    [SerializeField] float R_Summon_Rate; // 0.8
    [SerializeField] float SR_Summon_Rate; // 0.16
    [SerializeField] float SSR_Summon_Rate; // 0.04

    [SerializeField] static int SR_Set_Count;
    [SerializeField] static int SSR_Set_Count;

    [Header("----Gacha_Video----")]
    [SerializeField] GameObject Gacha_Video;
    [SerializeField] VideoClip[] Gacha_Scenes;
    [SerializeField] VideoPlayer Videoplayer;

    bool isR_Summon;
    bool isSR_Summon;
    bool isSSR_Summon;

    float Summon_Rate()
    {
        float RandomRate = Random.Range(0.01f, 1.0f);
        return RandomRate;
    }

    // TODO ## Gacha_Manager ���� �ý���
    public void Summon(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            float RandomRate = Summon_Rate();

            SSR_Set_Count++;
            SR_Set_Count++;

            // SSR Ȯ�� �̱� ����
            if (SSR_Set_Count >= 80)
            {
                SSR_Summon();
                SSR_Set_Count = 0;
                isSSR_Summon = true;
                continue;
            }

            // SR Ȯ�� �̱�
            if (SR_Set_Count >= 10)
            {
                SR_Summon();
                SR_Set_Count = 0;
                isSR_Summon = true;
                continue;
            }

            // �Ϲ� SSR Ȯ��
            if (SSR_Summon_Rate >= RandomRate)
            {
                SSR_Summon();
                SSR_Set_Count = 0;
                isSSR_Summon = true;
            }
            // 0.04 < @ && 0.16 <= @@ (SR_Summon_Rate - SSR_Summon_Rate)%
            else if (SSR_Summon_Rate < RandomRate && RandomRate <= SR_Summon_Rate)
            {
                SR_Summon();
                SR_Set_Count = 0;
                isSR_Summon = true;
            }
            else if (SR_Summon_Rate + SSR_Summon_Rate < RandomRate)
            {
                R_Summon();
                isR_Summon = true;
            }
        }

        // ĳ���� �κ��丮 Refresh
        CharListRef.Refresh_CharacterList();

        Gacha_Video_Play();

    }

    void SSR_Summon()
    {
        int RandomSSR = Random.Range(0, Character_List.SSR_Char.Count);

        #region SSR ��� �˻�
        if (Character_List.SSR_Char == null)
        {
            Debug.Log("SSR����Ʈ ã���� ����");
            return;
        }
        #endregion

        Character character = Character_List.SSR_Char[RandomSSR];

        // SSR ĳ���Ͱ� ���ٸ�
        if (!UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName))
        {
            UserInfo.UserCharDict.Add(Character_List.SSR_Char[RandomSSR].Get_CharName, character);
        }
        // ����� ���� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar < 5) 
        {
            UserInfo.UserCharDict[character.Get_CharName].Get_CharStar++;
            Debug.Log(UserInfo.UserCharDict[character.Get_CharName].Get_CharName + " : " + UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
            return;
        }
        // ����� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SSR_Char[RandomSSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar == 5)
        {
            Debug.Log("�ݻ� ���� ȹ��");
        }
        
    }

    void SR_Summon()
    {
        int RandomSR = Random.Range(0, Character_List.SR_Char.Count);

        #region SR ��� �˻�
        if (Character_List.SR_Char == null)
        {
            Debug.Log("SR����Ʈ ã���� ����");
            return;
        }
        #endregion

        Character character = Character_List.SR_Char[RandomSR];

        // SSR ĳ���Ͱ� ���ٸ�
        if (!UserInfo.UserCharDict.ContainsKey(Character_List.SR_Char[RandomSR].Get_CharName))
        {
            UserInfo.UserCharDict.Add(Character_List.SR_Char[RandomSR].Get_CharName, character);
        }
        // ����� ���� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SR_Char[RandomSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar < 5)
        {
            UserInfo.UserCharDict[character.Get_CharName].Get_CharStar++;
            Debug.Log(UserInfo.UserCharDict[character.Get_CharName].Get_CharName + " : " + UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
            return;
        }
        // ����� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.SR_Char[RandomSR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar == 5)
        {
            Debug.Log("���� ���� ȹ��");
        }
    }

    void R_Summon()
    {
        int RandomR = Random.Range(0, Character_List.R_Char.Count);

        #region R ��� �˻�
        if (Character_List.R_Char == null)
        {
            Debug.Log("SR����Ʈ ã���� ����");
            return;
        }
        #endregion

        Character character = Character_List.R_Char[RandomR];
        // SSR ĳ���Ͱ� ���ٸ�
        if (!UserInfo.UserCharDict.ContainsKey(Character_List.R_Char[RandomR].Get_CharName))
        {
            UserInfo.UserCharDict.Add(Character_List.R_Char[RandomR].Get_CharName, character);
        }
        // ����� ���� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.R_Char[RandomR].Get_CharName) &&  UserInfo.UserCharDict[character.Get_CharName].Get_CharStar < 5)
        {
            UserInfo.UserCharDict[character.Get_CharName].Get_CharStar++;
            Debug.Log(UserInfo.UserCharDict[character.Get_CharName].Get_CharName + " : " + UserInfo.UserCharDict[character.Get_CharName].Get_CharStar);
            return;
        }
        // ����� �� �ö��ٸ�
        else if (UserInfo.UserCharDict.ContainsKey(Character_List.R_Char[RandomR].Get_CharName) && UserInfo.UserCharDict[character.Get_CharName].Get_CharStar == 5)
        {
            Debug.Log("���� ���� ȹ��");
        }
    }

    // TODO ## Gacha_Manager �������� ����
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
}
