using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Define;

public enum INGAME_STATE
{
    READY,       // ���� ����
    TURN_START,  // �� ����
    STANDBY,     // ���
    TURN_END,    // �� ����
    STAGE_END,   // �������� ����
    BATTLE,      // ����
    ENEMY_TURN,
    ENEMY_TURN_END,
}

public class InGame_Mgr : MonoBehaviour
{
    // ���� �������� ����
    public INGAME_STATE InGameState = INGAME_STATE.STANDBY;
    [SerializeField] InGame_ObjPool ObjPool;
    public InGame_ObjPool Get_ObjPool { get => ObjPool; }

    [Header("Character")]
    public List<Character_Ctrl> CharCtrl_List = new List<Character_Ctrl>();

    [Header("Skill")]
    public int CurSP;
    public Animator[] SP_ChargeAnimator;
    public Animator SP_animator;

    [Header("Game_Info")]
    public int CurTurnCharIndex = 0;

    [Header("UI")]
    public Skill_Use_Face Skill_On_CharFace;
    public MonSkill_Use_Face Skill_On_MonFace;
    [SerializeField] GameObject[] CharStat_Pop_Images;
    bool isCharStat_Open = false;
    [SerializeField] GameObject[] MonStat_Pop_Images;
    bool isMonStat_Open = false;
    [SerializeField] Text TurnText;
    public int CurrentTurn = 1;
    public Color[] TextColor;

    // ���� Ŭ���� ���ο� ���� ������Ʈ Ȱ��ȭ�ϱ� ���� ����
    public GameObject[] UI_Canvas;
    public GameObject StageClear_Root;
    public GameObject StageFail_Root;

    [Header("Monster")]
    [SerializeField] int MonTurnIndex = 0;
    public int Get_MonTurnIndex { get => MonTurnIndex; }
    public List<Enemy_Ctrl> CurMonsters = new List<Enemy_Ctrl>();

    // delegate
    // ĳ���� ��ų ����Ʈ ������ �ϱ� ���� ��������Ʈ
    public delegate void UseSkill();
    public UseSkill UseSkill_ON;
    // ���� ��ų ����Ʈ ������ �ϱ� ���� ��������Ʈ
    public delegate void UseMonSkill();
    public UseMonSkill UseMonSkill_ON;

    #region Init
    // �̱���
    public static InGame_Mgr Inst = null;
    private void Awake()
    {
        #region Singleton
        if (Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
    }

    private void Start()
    {
        if(GameManager.Inst.TestMode == false)
        {
            CurSP = 6;
        }
        else
        {
            CurSP = 15;
        }

        for (int i = 0; i < CurSP; i++)
        {
            SP_ChargeAnimator[i].Play("SP_UP");
        }
    }
    #endregion

    private void Update()
    {
        GameStateUpdate();
    }

    void GameStateUpdate()
    {
        switch(InGameState)
        {
            case INGAME_STATE.READY:

                break;

            case INGAME_STATE.TURN_START:
                // UI �˾�
                SP_animator.Play("SP_UI_PopUp");
                
                if(CharCtrl_List[CurTurnCharIndex].Get_CurHP <= 0)
                {
                    for(int i = CurTurnCharIndex + 1; i < CharCtrl_List.Count; i++)
                    {
                        if(0 < CharCtrl_List[i].Get_CurHP)
                        {
                            CurTurnCharIndex = i;
                            break;
                        }
                    }
                }

                for (int i = 0; i < ObjPool.SkillIcon_List.Count; i++)
                {
                    // ���� ���� ĳ���� ��ų UI�� ���ֱ�
                    if(i == CurTurnCharIndex)
                    {
                        ObjPool.SkillIcon_List[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        ObjPool.SkillIcon_List[i].gameObject.SetActive(false);
                    }
                }
                break;

            case INGAME_STATE.STANDBY:

                break;

            case INGAME_STATE.TURN_END:
                // ���� ĳ���� UI ǥ�����ֱ� ���� �ε��� ����
                CurTurnCharIndex++;
                // ������ ĳ���ͱ��� �������� �ٽ� �ݺ�
                if (UserInfo.Equip_Characters.Count <= CurTurnCharIndex)
                {
                    Buff_Decreased();
                    CurTurnCharIndex = 0;
                    CurrentTurn++;
                    TurnText.text = $"TURN.{CurrentTurn}";
                    InGameState = INGAME_STATE.ENEMY_TURN;
                    break;
                }

                InGameState = INGAME_STATE.TURN_START;
                break;

            case INGAME_STATE.STAGE_END:

                break;

            case INGAME_STATE.BATTLE:
                break;

            case INGAME_STATE.ENEMY_TURN:
                for(int i = MonTurnIndex; i < CurMonsters.Count; i++)
                {
                    if (CurMonsters[i].Get_CurHP <= 0)
                        continue;

                    // ���� ���Ͱ� ���� �ʰ� ����ִ� ���͸� ã�Ҵٸ� ��ų ���
                    MonTurnIndex = i;
                    UseMonSkill_ON += CurMonsters[i].Skill_Use;
                    Skill_On_MonFace.Set_UI_Init(CurMonsters[i].Get_Icon, CurMonsters[i].MonName);
                    break;
                }
                
                InGameState = INGAME_STATE.BATTLE;
                break;

            case INGAME_STATE.ENEMY_TURN_END:
                MonTurnIndex++;
                // ������ ĳ���ͱ��� �������� �ٽ� �ݺ�
                if (CurMonsters.Count <= MonTurnIndex)
                {
                    Buff_Decreased();
                    MonTurnIndex = 0;
                    CurrentTurn++;
                    TurnText.text = $"TURN.{CurrentTurn}";
                    InGameState = INGAME_STATE.TURN_START;
                    break;
                }
                InGameState = INGAME_STATE.ENEMY_TURN;
                break;
            default:
                break;
        }
    }

    void Buff_Decreased()
    {
        for(int i = 0; i < ObjPool.BuffIcon_List.Count; i++)
        {
            // ���������� �ǳʶٱ�
            if (ObjPool.BuffIcon_List[i].gameObject.activeSelf == false)
                continue;

            // ���� �� ����
            ObjPool.BuffIcon_List[i].Turn_Decreased();
        }
    }

    public void On_Click_GoLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void On_Click_Retry()
    {
        SceneManager.LoadScene("InGameScene");
    }

    public void On_Click_NextStage()
    {
        if (GameManager.Inst.StageIndex == Stage_List.StageList.Count - 1)
        {
            Debug.Log("���� ���������� �������Դϴ�.");
            return;
        }

        GameManager.Inst.StageIndex++;
        SceneManager.LoadScene("InGameScene");
    }

    #region UI
    // ĳ���� ����â ���� �ݱ�
    public void On_Click_OpenCharStat(Animator _animator)
    {
        if(isCharStat_Open)
        {
            _animator.Play("Pop_Down");
            CharStat_Pop_Images[0].SetActive(true);
            CharStat_Pop_Images[1].SetActive(false);
        }
        else
        {
            _animator.Play("Pop_Up");
            CharStat_Pop_Images[0].SetActive(false);
            CharStat_Pop_Images[1].SetActive(true);
        }

        isCharStat_Open = !isCharStat_Open;
    }

    public void On_Click_OpenMonStat(Animator _animator)
    {
        if (isMonStat_Open)
        {
            _animator.Play("Pop_Down");
            MonStat_Pop_Images[0].SetActive(true);
            MonStat_Pop_Images[1].SetActive(false);
        }
        else
        {
            _animator.Play("Pop_Up");
            MonStat_Pop_Images[0].SetActive(false);
            MonStat_Pop_Images[1].SetActive(true);
        }

        isMonStat_Open = !isMonStat_Open;
    }
    #endregion
}
