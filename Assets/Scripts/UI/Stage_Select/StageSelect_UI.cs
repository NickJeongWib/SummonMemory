using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageSelect_UI : MonoBehaviour
{
    public static StageSelect_UI Inst = null;

    [Header("Copy_Equip")]
    // 현재 장착 캐릭터를 담기 위한 List
    public List<Character> Copy_EquipChar = new List<Character>();

    [Header("StandCharacters")]
    GameObject[] StandCharacters = new GameObject[5]; 

    [Header("Transition")]
    public bool isChange = false;
    [SerializeField] Pop_UpDown Stage_Transition;
    [SerializeField] GameObject Char_Panel;
    [SerializeField] GameObject Change_Back_Btn;

    [Header("Drag")]
    [SerializeField] Transform[] CharPos;

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
    }

    #region Transition
    // 캐릭터 교체 창으로 가기
    public void On_Click_ChangeChar()
    {
        isChange = true;

        Stage_Transition.On_Click_SelectStage_Close();
        Char_Panel.SetActive(true);
        // 캐릭터 교체화면에 돌아오기 버튼 활성화
        Change_Back_Btn.SetActive(true);
    }

    // 캐릭터 교체 후 다시 돌아오기
    public void On_Click_ReturnStage()
    {
        isChange = false;

        Stage_Transition.On_Click_SelectStage();
        // 캐릭터 교체화면에 돌아오기 버튼 비활성화
        Change_Back_Btn.SetActive(false);
    }

    public void ActiveF_CharPanel()
    {
        Char_Panel.SetActive(false);
    }
    #endregion

    #region Spawn_Stand_Character
    public void Spawn_Stand_Char()
    {
        // 현재 장착 캐릭터를 담기 위한 List
        Copy_EquipChar = new List<Character>();
        // 장착 캐릭터 순서를 조종하기 위해 복사
        for(int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            Copy_EquipChar.Add(UserInfo.Equip_Characters[i]);
        }

        // 기존 장착한 캐릭터 프리펩 전체 삭제
        for(int i = 0; i < StandCharacters.Length; i++)
        {
            // 아무거도 없으면 그냥 끝내기
            if (StandCharacters[i] == null)
                break;
            Destroy(StandCharacters[i]);
        }

        // UI 생성
        for(int i = 0; i < Copy_EquipChar.Count; i++)
        {
            GameObject char_UI = Resources.Load(Copy_EquipChar[i].Get_UI_Prefab_Path) as GameObject;
            GameObject spawn_UI = Instantiate(char_UI);

            StandCharacters[i] = spawn_UI;
            spawn_UI.transform.SetParent(CharPos[i]);
            spawn_UI.transform.localScale = new Vector2(90.0f, 90.0f);
            spawn_UI.transform.localPosition = Vector2.zero;
        }
    }

    #endregion
}
