using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Define;

public enum INGAME_STATE
{
    READY,       // 게임 시작
    TURN_START,  // 턴 시작
    STANDBY,     // 대기
    TURN_END,    // 턴 종료
    STAGE_END,   // 스테이지 종료
    BATTLE,      // 전투
    ENEMY_TURN,
    ENEMY_TURN_END,
}

public class InGame_Mgr : MonoBehaviour
{
    // 현재 게임진행 상태
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

    // 게임 클리어 여부에 따른 오브젝트 활성화하기 위한 변수
    public GameObject[] UI_Canvas;
    public GameObject StageClear_Root;
    public GameObject StageFail_Root;

    [Header("Monster")]
    [SerializeField] int MonTurnIndex = 0;
    public int Get_MonTurnIndex { get => MonTurnIndex; }
    public List<Enemy_Ctrl> CurMonsters = new List<Enemy_Ctrl>();

    // delegate
    // 캐릭터 스킬 이펙트 나오게 하기 위한 델리게이트
    public delegate void UseSkill();
    public UseSkill UseSkill_ON;
    // 몬스터 스킬 이펙트 나오게 하기 위한 델리게이트
    public delegate void UseMonSkill();
    public UseMonSkill UseMonSkill_ON;

    #region Init
    // 싱글톤
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
                // UI 팝업
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
                    // 현재 차례 캐릭터 스킬 UI만 켜주기
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
                // 다음 캐릭터 UI 표시해주기 위해 인덱스 증가
                CurTurnCharIndex++;
                // 마지막 캐릭터까지 돌았으면 다시 반복
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

                    // 만약 몬스터가 죽지 않고 살아있는 몬스터를 찾았다면 스킬 사용
                    MonTurnIndex = i;
                    UseMonSkill_ON += CurMonsters[i].Skill_Use;
                    Skill_On_MonFace.Set_UI_Init(CurMonsters[i].Get_Icon, CurMonsters[i].MonName);
                    break;
                }
                
                InGameState = INGAME_STATE.BATTLE;
                break;

            case INGAME_STATE.ENEMY_TURN_END:
                MonTurnIndex++;
                // 마지막 캐릭터까지 돌았으면 다시 반복
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
            // 꺼져있으면 건너뛰기
            if (ObjPool.BuffIcon_List[i].gameObject.activeSelf == false)
                continue;

            // 유지 턴 감소
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
            Debug.Log("현재 스테이지가 마지막입니다.");
            return;
        }

        GameManager.Inst.StageIndex++;
        SceneManager.LoadScene("InGameScene");
    }

    #region UI
    // 캐릭터 상태창 열기 닫기
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
