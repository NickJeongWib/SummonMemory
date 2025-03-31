using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Lobby_Manager : MonoBehaviour
{
    [Header("---UI_Scripts_Ref")]
    [SerializeField] Inventory_UI Inventory_UI_Ref;

    [Header("---Transition---")]
    [SerializeField] GameObject CircleTransition;
    [SerializeField] GameObject ShaderTransition;
    [SerializeField] GameObject NotTouch_RayCast;
    public GameObject Get_NotTouch_RayCast { get => NotTouch_RayCast; }

    [SerializeField] Equip_Slot EquipSlot_List;

    [Header("---GachaMovie---")]
    [SerializeField] GachaVideo gachaVideo;

    [Header("---Gacha_CharacterList---")]
    [SerializeField] GameObject Gacha_10;
    [SerializeField] GameObject Gacha_1;
    [SerializeField] GameObject[] Book_Images;
    [SerializeField] GameObject Book_Image;

    private void Awake()
    {
        // TODO ## 초기 테스트 값
        if (UserInfo.UserCharDict.Count <= 0)
        {
            // TODO ## 시작 캐릭터 설정 Character_List "레제" 초반 스타트 캐릭
            UserInfo.UserCharDict.Add($"{Character_List.SR_Char[0].Get_CharName}", Character_List.SR_Char[0]);
            UserInfo.UserCharDict_Copy = UserInfo.UserCharDict.ToList();
            UserInfo.UserCharDict_Copy_2 = UserInfo.UserCharDict.ToList();

            UserInfo.Equip_Characters.Add(UserInfo.UserCharDict_Copy[0].Value);
            EquipSlot_List.EquipCharacter = UserInfo.UserCharDict_Copy[0].Value;

            UserInfo.UserCharDict_Copy.RemoveAt(0);
        }
    }

    #region Shader_Graph_Transition
    // 상점, 도감, 업적 등 여러 창으로 이동
    public void On_Click_OnPanel(GameObject _obj)
    {
        if (_obj.name == "MyBag_Panel")
        {
            Inventory_UI_Ref.On_Click_Spend_Item_Btn();
        }

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
    #endregion

    #region Circle_Transition
    public void On_Click_OnPanel_Circle(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // 이동 중 버튼 클릭 방지
        CircleTransition.SetActive(true);
        _obj.SetActive(true);
    }

    // 현재 열려 있는 창을 닫고 로비로 이동
    public void On_Click_OffPanel_Circle(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // 화면전환 중 버튼 클릭 방지
        CircleTransition.SetActive(true);
        _obj.SetActive(false);
    }
    #endregion

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

    public void Test()
    {
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            Debug.Log("Equip " + UserInfo.Equip_Characters[i].Get_CharName);
        }

        for (int i = 0; i < UserInfo.UserCharDict_Copy_2.Count; i++)
        {
            Debug.Log("Dict2 " + UserInfo.UserCharDict_Copy_2[i].Value.Get_CharName);
        }

        Debug.Log("5" + EquipSlot_List.EquipCharacter.Get_CharName);
    }
}
