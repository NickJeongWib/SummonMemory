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
    public int LastIndex;
    public int CurTurnCharIndex = 0;
    public bool isAuto;

    [Header("UI")]
    public Skill_Use_Face Skill_On_CharFace;
    public MonSkill_Use_Face Skill_On_MonFace;
    [SerializeField] GameObject[] CharStat_Pop_Images;
    bool isCharStat_Open = false;
    [SerializeField] GameObject[] MonStat_Pop_Images;
    bool isMonStat_Open = false;
    [SerializeField] Text TurnText;
    [SerializeField] Text StageNum;
    public int CurrentTurn = 1;
    public Color[] TextColor;
    [SerializeField] Skill_Desc Skill_Desc_UI;
    [SerializeField] Toggle AutoToggle;
    [SerializeField] GameObject Toggle_BG;
    [SerializeField] Info_Panel Info_Panel_Ref;
    public Info_Panel Get_Info_Panel_Ref { get => Info_Panel_Ref; }


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

    [Header("SFX")]
    [SerializeField] AudioClip InGame_BGM;

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
        SoundManager.Inst.PlayBGM(InGame_BGM);

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

        // 전에 자동전투를 켜놓았다면 다시 클릭하지 않기 위해
        AutoToggle.isOn = GameManager.Inst.isAutoBattle;
        StageNum.text = $"스토리 {GoogleSheetManager.SO<GoogleSheetSO>().STAGE_DBList[GameManager.Inst.StageIndex].STAGE_NUM}";
    }
    #endregion

    private void Update()
    {
        GameStateUpdate();
    }

    #region FSM_InGameState
    void GameStateUpdate()
    {
        switch(InGameState)
        {
            case INGAME_STATE.READY:

                break;

            case INGAME_STATE.TURN_START:
                // UI 팝업
                SP_animator.Play("SP_UI_PopUp");

                // 턴의 마지막 캐릭터 인덱스 설정
                LastIndex = CharCtrl_List.Count - 1;
                while(0 <= LastIndex)
                {
                    if (0 < CharCtrl_List[LastIndex].Get_CurHP)
                    {
                        break;
                    }

                    LastIndex--;
                }

                // 캐릭터가 죽어있으면
                if(CharCtrl_List[CurTurnCharIndex].Get_CurHP <= 0)
                {
                    for(int i = CurTurnCharIndex + 1; i < CharCtrl_List.Count; i++)
                    {
                        // 다음 캐릭터 순 반복으로 죽었는지 체크
                        if (0 < CharCtrl_List[i].Get_CurHP)
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
                        ObjPool.SkillIcon_List[i].Skill_Use_Frame();
                    }
                    else
                    {
                        ObjPool.SkillIcon_List[i].gameObject.SetActive(false);
                    }
                }

                InGameState = INGAME_STATE.STANDBY;
                break;

            case INGAME_STATE.STANDBY:

                break;

            case INGAME_STATE.TURN_END:

                // 다음 캐릭터 UI 표시해주기 위해 인덱스 증가
                CurTurnCharIndex++;

                // 마지막 캐릭터까지 돌았으면 다시 반복
                if (LastIndex < CurTurnCharIndex)
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

                // 시작 Index잡아주기
                for (int i = MonTurnIndex; i < CurMonsters.Count; i++)
                {
                    if (0 < CurMonsters[i].Get_CurHP)
                    {
                        MonTurnIndex = i;
                        break;
                    }
                }

                for (int i = MonTurnIndex; i < CurMonsters.Count; i++)
                {
                    if (CurMonsters[i].Get_CurHP <= 0)
                    {
                        continue;
                    }
                        
                    // CC상태면 공격하지 못하게 Break
                    if (CurMonsters[i].Get_isCC)
                    {
                        Skill_On_MonFace.Set_UI_Init(CurMonsters[i].Get_Icon, CurMonsters[i].MonName, "행동 불가 상태", "MonSkill_Skip");
                        break;
                    }

                    // 만약 몬스터가 죽지 않고 살아있는 몬스터를 찾았다면 스킬 사용
                    MonTurnIndex = i;

                    UseMonSkill_ON += CurMonsters[i].Skill_Use;
                    
                    // 적을 공격하는 애니메이션이 있다면
                    if (CurMonsters[i].EnemyAnimClip.Attack_Clip != null)
                    {
                        CurMonsters[i].Get_animator.Play(CurMonsters[i].EnemyAnimClip.Attack_Clip.name);
                    }
                    
                    Skill_On_MonFace.Set_UI_Init(CurMonsters[i].Get_Icon, CurMonsters[i].MonName, "아군 다수 공격", "MonSkill_Play");
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
    #endregion

    #region BuffTurn_Ctrl
    void Buff_Decreased()
    {
        // 버프들 턴 감소
        for(int i = 0; i < ObjPool.BuffIcon_List.Count; i++)
        {
            // 꺼져있으면 건너뛰기
            if (ObjPool.BuffIcon_List[i].gameObject.activeSelf == false)
                continue;

            // 유지 턴 감소
            ObjPool.BuffIcon_List[i].Turn_Decreased();
        }
    }
    #endregion

    #region UI
    public void Show_Info_Panel(string _text)
    {
        Info_Panel_Ref.Set_Text(_text);
    }

    // 자동전투 토글 누를 시
    public void On_Click_AutoBattle()
    {
        // 토글에 따라 자동 전투
        isAuto = AutoToggle.isOn;
        GameManager.Inst.isAutoBattle = AutoToggle.isOn;
        // 토글 뒷 배경 감추기
        Toggle_BG.SetActive(!AutoToggle.isOn);

        // 자동 전투가 아니라면 return
        if (Inst.isAuto && InGameState == INGAME_STATE.STANDBY)
        {
            // 스킬 사용 가능 상태이고 현재 보유 SP가 스킬에 사용할 SP이상 지니고 있을 때
            if (ObjPool.SkillIcon_List[CurTurnCharIndex].Get_InGame_Char.CanSkill &&
               ObjPool.SkillIcon_List[CurTurnCharIndex].Get_InGame_Char.Get_SkillData.Get_SP <= CurSP)
            {
                ObjPool.SkillIcon_List[CurTurnCharIndex].On_Click_Skill_Btn();
            }

            // 현재 캐릭터가 스킬을 사용할 수 없는 상태이거나 스킬 보유 SP가 사용해야할 SP보다 적다면 기본공격
            if (ObjPool.SkillIcon_List[CurTurnCharIndex].Get_InGame_Char.CanSkill == false ||
               CurSP < ObjPool.SkillIcon_List[CurTurnCharIndex].Get_InGame_Char.Get_SkillData.Get_SP)
            {
                ObjPool.SkillIcon_List[CurTurnCharIndex].On_Click_NoramlAtk();
            }
        }
    }

    public void Show_Skill_Desc( bool _isOn, BuffIcon_UI _buffIcon = null, Skill _skill = null, Transform _tr = null, Sprite _icon = null, bool _isUseSkill = false, bool _isSkillBtn = true)
    {
        // 아군 적용이나 몬스터 적용인지에 따라 보여주는 ui위치 바꿔주기 위한
        float setPos = 0.0f;
        if (_buffIcon != null && _buffIcon.Get_Character_Ctrl != null)
        {
            // 캐릭터 적용이라면 오른쪽으로 200 옮겨줌
            setPos = 200.0f;
        }

        // 디버프 아이콘이라면
        if(_buffIcon != null && _buffIcon.Get_EnemyCtrl != null)
        {
            // 몬스터 적용이라면 왼쪽으로 200 옮겨줌
            setPos = -200.0f;
        }

        // 만약 스킬 사용 버튼이라면
        if(_isUseSkill)
        {
            setPos = 100.0f;
        }

        Skill_Desc_UI.Show_Skill_Desc(_skill, _isOn, _tr, _icon, setPos, _isUseSkill, _isSkillBtn);
    }

    public void On_Click_NextStage()
    {
        if (GameManager.Inst.StageIndex == Stage_List.StageList.Count - 1)
        {
            Debug.Log("현재 스테이지가 마지막입니다.");
            return;
        }

        SoundManager.Inst.StopEffSound();
        GameManager.Inst.StageIndex++;
        SceneManager.LoadScene("InGameScene");
    }


    // 캐릭터 상태창 열기 닫기
    public void On_Click_OpenCharStat(Animator _animator)
    {
        SoundManager.Inst.PlayUISound();

        if (isCharStat_Open)
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

    // 몬스터 상태창 열기 닫기
    public void On_Click_OpenMonStat(Animator _animator)
    {
        SoundManager.Inst.PlayUISound();

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
