using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class CharacterList_UI : MonoBehaviour
{
    public List<CharacterSlot> Slots = new List<CharacterSlot>();
    public Sprite[] Elements;
    public Sprite[] ElementColors;
    public Sprite[] Elements_BG;
    public Sprite[] Grades;

    private void Start()
    {

        // Refresh_CharacterList();
    }

    /// <summary>
    /// 캐릭터의 정보가 레벨이나 수치 변경 시 한번 호출 해 줄 필요가 있음
    /// </summary> 
    // TODO ## CharacterList_UI 캐릭터창 Refresh
    public void Refresh_CharacterList()
    {
        int SlotNum = 0;

        foreach(KeyValuePair<string, Character> Dict in UserInfo.UserCharDict_Copy)
        {
            // 이미지들 꺼져 있다면 켜주기
            if (!Slots[SlotNum].Element_BG.IsActive())
            {
                Slots[SlotNum].Element_BG.gameObject.SetActive(true);
            }
            if (!Slots[SlotNum].Element_Image.IsActive())
            {
                Slots[SlotNum].Element_Image.gameObject.SetActive(true);
            }
            if (!Slots[SlotNum].Char_Porfile.IsActive())
            {
                Slots[SlotNum].Char_Porfile.gameObject.SetActive(true);
            }
            if (!Slots[SlotNum].Star_Image.IsActive())
            {
                Slots[SlotNum].Star_Image.gameObject.SetActive(true);
            }
            if (!Slots[SlotNum].Grade_Image.IsActive())
            {
                Slots[SlotNum].Grade_Image.gameObject.SetActive(true);
            }
            if(!Slots[SlotNum].Select_Btn.IsActive())
            {
                Slots[SlotNum].Select_Btn.gameObject.SetActive(true);
            }

            Slots[SlotNum].Slot_Num = SlotNum;
            Slots[SlotNum].character = Dict.Value;
            Slots[SlotNum].Grade_Image.sprite = Grades[(int)Dict.Value.Get_CharGrade];
            Slots[SlotNum].Element_BG.sprite = Elements_BG[(int)Dict.Value.Get_CharElement];
            Slots[SlotNum].Element_Image.sprite = Elements[(int)Dict.Value.Get_CharElement];

            Slots[SlotNum].Char_Porfile.sprite = Dict.Value.Get_Profile_Img;
            Slots[SlotNum].Star_Image.rectTransform.sizeDelta = new Vector2(20 * Dict.Value.Get_CharStar, 20.0f);
            // Debug.Log(SlotNum + " " + Dict.Value.Get_CharName + " " + Dict.Value.Get_CharStar);

           SlotNum++;
        }

        for (int i = SlotNum; i < Slots.Count; i++)
        {
            Slots[SlotNum].character = null;

            // 이미지들 켜져 있다면 꺼주기
            if (Slots[SlotNum].Element_BG.IsActive())
            {
                Slots[SlotNum].Element_BG.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Element_Image.IsActive())
            {
                Slots[SlotNum].Element_Image.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Char_Porfile.IsActive())
            {
                Slots[SlotNum].Char_Porfile.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Star_Image.IsActive())
            {
                Slots[SlotNum].Star_Image.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Grade_Image.IsActive())
            {
                Slots[SlotNum].Grade_Image.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Select_Btn.IsActive())
            {
                Slots[SlotNum].Select_Btn.gameObject.SetActive(false);
            }
            if (Slots[SlotNum].Equip_Btn.IsActive())
            {
                Slots[SlotNum].Equip_Btn.gameObject.SetActive(false);
            }
        }
    }

    public void Remove_List_Char()
    {
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            if (UserInfo.UserCharDict_Copy.Contains(new KeyValuePair<string, Character>(UserInfo.Equip_Characters[i].Get_CharName, UserInfo.Equip_Characters[i])))
            {
                UserInfo.UserCharDict_Copy.Remove(new KeyValuePair<string, Character>(UserInfo.Equip_Characters[i].Get_CharName, UserInfo.Equip_Characters[i]));
            }
        }
    }

    // 캐릭터 교체 버튼 활성화
    public void Refresh_Equip_Btn()
    {
        int SlotNum = 0;

        foreach (KeyValuePair<string, Character> Dict in UserInfo.UserCharDict_Copy)
        {
            if (!Slots[SlotNum].Equip_Btn.IsActive())
            {
                Slots[SlotNum].Equip_Btn.gameObject.SetActive(true);
            }

            SlotNum++;
        }
    }
}
