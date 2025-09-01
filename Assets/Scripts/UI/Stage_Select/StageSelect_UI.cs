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

    public GameObject[] MovePanel_Parents;

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
        int index = 0;
        // 장착 캐릭터 순서를 조종하기 위해 복사
        for(int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            Copy_EquipChar.Add(UserInfo.Equip_Characters[i]);
            UserInfo.Pos_Index[i] = i;
            index = i;
        }
        // 장착안 한 칸 확인 하기 위해
        for(int i = index + 1; i < UserInfo.Pos_Index.Length; i++)
        {
            UserInfo.Pos_Index[i] = -1;
        }

        // 기존 장착한 캐릭터 프리펩 전체 삭제
        for(int i = 0; i < StandCharacters.Length; i++)
        {
            // 아무것도 없으면 그냥 끝내기
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

    public void Check_Pos()
    {
        for(int i = 0; i < CharPos.Length; i++)
        {
            // CharPos[i].localPosition = Vector3.zero;
            CharPos[i].GetComponent<EquipChar_Move>().canvasRectTransform = CharPos[i].parent.GetComponent<RectTransform>();
            // Debug.Log(CharPos[i].parent.name);

            // UI프리펩이 달려 있지 않으면 계속
            if (CharPos[i].childCount <= 0)
            {
                continue;
            }
            // CharPos의 장착 캐릭터 순번 들고 오기
            string[] name = CharPos[i].name.Split("_");
            int index = int.Parse(name[2]) - 1;

            // 오브젝트의 부모 이름으로 어디 소환할지 알기
            string[] spawnPos = CharPos[i].parent.name.Split("_");
            int spawnindex = int.Parse(spawnPos[1]) - 1;
            UserInfo.Pos_Index[index] = spawnindex;
        }
    }
}
