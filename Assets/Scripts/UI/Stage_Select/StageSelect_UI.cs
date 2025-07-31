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
        // ���� ĳ���� ������ �����ϱ� ���� ����
        for(int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            Copy_EquipChar.Add(UserInfo.Equip_Characters[i]);
        }

        // ���� ������ ĳ���� ������ ��ü ����
        for(int i = 0; i < StandCharacters.Length; i++)
        {
            // �ƹ��ŵ� ������ �׳� ������
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
}
