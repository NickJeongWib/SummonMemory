using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby_Manager : MonoBehaviour
{
    [Header("---Transition---")]
    [SerializeField] GameObject ShaderTransition;
    [SerializeField] GameObject NotTouch_RayCast;

    [Header("---GachaMovie---")]
    [SerializeField] GachaVideo gachaVideo;

    [Header("---Gacha_CharacterList---")]
    [SerializeField] GameObject Gacha_10;
    [SerializeField] GameObject Gacha_1;
    [SerializeField] GameObject[] Book_Images;
    [SerializeField] GameObject Book_Image;

    [Header("---CharacterList---")]
    [SerializeField] CharacterList_UI CharacterList_UI_Ref;
    [SerializeField] Image Transition_Element_BG;
    [SerializeField] GameObject CharacterInfo_Panel;
    [SerializeField] Text Transition_Char_Name;
    [SerializeField] Image Transition_Grade;
    [SerializeField] Color[] colors;

    // 상점, 도감, 업적 등 여러 창으로 이동
    public void On_Click_OnPanel(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // 이동 중 버튼 클릭 방지
        ShaderTransition.SetActive(true);
        _obj.SetActive(true);
    }

    // 현재 열려 있는 창을 닫고 로비로 이동
    public void On_Click_OffPanel(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // 화면전환 중 버튼 클릭 방지
        ShaderTransition.SetActive(true);
        _obj.SetActive(false);
    }

    public void On_Click_Back(GameObject _obj)
    {
        _obj.SetActive(false);
    }

    public void On_Click_On(GameObject _obj)
    {
        _obj.SetActive(true);
    }


    #region Gacha_UI_Active

    public void On_Click_Skip_GachaMovie(GameObject _obj)
    {
        // 가챠 연출 스킵
        _obj.SetActive(false);
        _obj.transform.parent.gameObject.SetActive(false);

        gachaVideo.Get_VideoPlayer.Stop();
    }

    public void On_Click_Back_GachaList(GameObject _obj)
    {
        // 가챠 뽑기 목록 닫기
        Gacha_10.SetActive(false);
        Gacha_1.SetActive(false);

        // 책 재료 팝업 닫기
        for (int i = 0; i < Book_Images.Length; i++)
        {
            Book_Images[i].SetActive(false);
        }
        Book_Image.SetActive(false);

        _obj.SetActive(false);
    }
    #endregion

    #region CharacterList_UI
    public void On_Click_CharInfo(CharacterSlot _slot)
    {
        CharacterInfo_Panel.SetActive(true);

        // 화면 전환 화면의 UI 선택한 캐릭터 속성 색으로 변경
        Transition_Char_Name.color = colors[(int)_slot.character.Get_CharElement];
        Transition_Grade.color = colors[(int)_slot.character.Get_CharElement];
        Transition_Element_BG.sprite = CharacterList_UI_Ref.Elements_BG[(int)_slot.character.Get_CharElement];

        // 캐릭터의 영문 이름 표시
        Transition_Char_Name.text = _slot.character.Get_CharEngName;

        // 등급에 따른 이미지 차별화
        if (_slot.character.Get_CharGrade == Define.CHAR_GRADE.R)
        {
            Transition_Grade.rectTransform.sizeDelta = new Vector2(96.0f, 30.0f);
        }
        else if (_slot.character.Get_CharGrade == Define.CHAR_GRADE.SR)
        {
            Transition_Grade.rectTransform.sizeDelta = new Vector2(128.0f, 30.0f);
        }
        else
        {
            Transition_Grade.rectTransform.sizeDelta = new Vector2(160.0f, 30.0f);
        }
    }
    #endregion
}
