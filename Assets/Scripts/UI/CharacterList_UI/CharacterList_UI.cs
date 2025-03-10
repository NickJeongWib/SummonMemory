using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class CharacterList_UI : MonoBehaviour
{
    public List<CharacterSlot> Slots = new List<CharacterSlot>();
    public Sprite[] Elements;
    public Sprite[] Elements_BG;
    public Sprite[] Grades;

    // TODO ## CharacterList_UI 캐릭터창 Refresh
    public void Refresh_CharacterList()
    {
        int SlotNum = 0;

        foreach(KeyValuePair<string, Character> Dict in UserInfo.UserCharDict)
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

            Slots[SlotNum].character = Dict.Value;
            Slots[SlotNum].Grade_Image.sprite = Grades[(int)Dict.Value.Get_CharGrade];
            Slots[SlotNum].Element_BG.sprite = Elements_BG[(int)Dict.Value.Get_CharElement];
            Slots[SlotNum].Element_Image.sprite = Elements[(int)Dict.Value.Get_CharElement];

            Slots[SlotNum].Char_Porfile.sprite = Dict.Value.Get_Profile_Img;
            Slots[SlotNum].Star_Image.rectTransform.sizeDelta = new Vector2(20 * Dict.Value.Get_CharStar, 20.0f);
            // Debug.Log(SlotNum + " " + Dict.Value.Get_CharName + " " + Dict.Value.Get_CharStar);

           SlotNum++;
        }
    }
}
