using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageSelect_UI : MonoBehaviour
{
    public static StageSelect_UI Inst = null;

    [Header("Copy_Equip")]
    // ���� ���� ĳ���͸� ��� ���� List
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
    // ĳ���� ��ü â���� ����
    public void On_Click_ChangeChar()
    {
        isChange = true;

        Stage_Transition.On_Click_SelectStage_Close();
        Char_Panel.SetActive(true);
        // ĳ���� ��üȭ�鿡 ���ƿ��� ��ư Ȱ��ȭ
        Change_Back_Btn.SetActive(true);
    }

    // ĳ���� ��ü �� �ٽ� ���ƿ���
    public void On_Click_ReturnStage()
    {
        isChange = false;

        Stage_Transition.On_Click_SelectStage();
        // ĳ���� ��üȭ�鿡 ���ƿ��� ��ư ��Ȱ��ȭ
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
        // ���� ���� ĳ���͸� ��� ���� List
        Copy_EquipChar = new List<Character>();
        int index = 0;
        // ���� ĳ���� ������ �����ϱ� ���� ����
        for(int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            Copy_EquipChar.Add(UserInfo.Equip_Characters[i]);
            UserInfo.Pos_Index[i] = i;
            index = i;
        }
        // ������ �� ĭ Ȯ�� �ϱ� ����
        for(int i = index + 1; i < UserInfo.Pos_Index.Length; i++)
        {
            UserInfo.Pos_Index[i] = -1;
        }

        // ���� ������ ĳ���� ������ ��ü ����
        for(int i = 0; i < StandCharacters.Length; i++)
        {
            // �ƹ��͵� ������ �׳� ������
            if (StandCharacters[i] == null)
                break;
            Destroy(StandCharacters[i]);
        }

        // UI ����
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

            // UI�������� �޷� ���� ������ ���
            if (CharPos[i].childCount <= 0)
            {
                continue;
            }
            // CharPos�� ���� ĳ���� ���� ��� ����
            string[] name = CharPos[i].name.Split("_");
            int index = int.Parse(name[2]) - 1;

            // ������Ʈ�� �θ� �̸����� ��� ��ȯ���� �˱�
            string[] spawnPos = CharPos[i].parent.name.Split("_");
            int spawnindex = int.Parse(spawnPos[1]) - 1;
            UserInfo.Pos_Index[index] = spawnindex;
        }
    }
}
