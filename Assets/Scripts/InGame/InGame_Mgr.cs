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

    [Header("SFX")]
    [SerializeField] AudioClip InGame_BGM;

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

        // ���� �ڵ������� �ѳ��Ҵٸ� �ٽ� Ŭ������ �ʱ� ����
        AutoToggle.isOn = GameManager.Inst.isAutoBattle;
        StageNum.text = $"���丮 {GoogleSheetManager.SO<GoogleSheetSO>().STAGE_DBList[GameManager.Inst.StageIndex].STAGE_NUM}";
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
                // UI �˾�
                SP_animator.Play("SP_UI_PopUp");

                // ���� ������ ĳ���� �ε��� ����
                LastIndex = CharCtrl_List.Count - 1;
                while(0 <= LastIndex)
                {
                    if (0 < CharCtrl_List[LastIndex].Get_CurHP)
                    {
                        break;
                    }

                    LastIndex--;
                }

                // ĳ���Ͱ� �׾�������
                if(CharCtrl_List[CurTurnCharIndex].Get_CurHP <= 0)
                {
                    for(int i = CurTurnCharIndex + 1; i < CharCtrl_List.Count; i++)
                    {
                        // ���� ĳ���� �� �ݺ����� �׾����� üũ
                        if (0 < CharCtrl_List[i].Get_CurHP)
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

                // ���� ĳ���� UI ǥ�����ֱ� ���� �ε��� ����
                CurTurnCharIndex++;

                // ������ ĳ���ͱ��� �������� �ٽ� �ݺ�
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

                // ���� Index����ֱ�
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
                        
                    // CC���¸� �������� ���ϰ� Break
                    if (CurMonsters[i].Get_isCC)
                    {
                        Skill_On_MonFace.Set_UI_Init(CurMonsters[i].Get_Icon, CurMonsters[i].MonName, "�ൿ �Ұ� ����", "MonSkill_Skip");
                        break;
                    }

                    // ���� ���Ͱ� ���� �ʰ� ����ִ� ���͸� ã�Ҵٸ� ��ų ���
                    MonTurnIndex = i;

                    UseMonSkill_ON += CurMonsters[i].Skill_Use;
                    
                    // ���� �����ϴ� �ִϸ��̼��� �ִٸ�
                    if (CurMonsters[i].EnemyAnimClip.Attack_Clip != null)
                    {
                        CurMonsters[i].Get_animator.Play(CurMonsters[i].EnemyAnimClip.Attack_Clip.name);
                    }
                    
                    Skill_On_MonFace.Set_UI_Init(CurMonsters[i].Get_Icon, CurMonsters[i].MonName, "�Ʊ� �ټ� ����", "MonSkill_Play");
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
    #endregion

    #region BuffTurn_Ctrl
    void Buff_Decreased()
    {
        // ������ �� ����
        for(int i = 0; i < ObjPool.BuffIcon_List.Count; i++)
        {
            // ���������� �ǳʶٱ�
            if (ObjPool.BuffIcon_List[i].gameObject.activeSelf == false)
                continue;

            // ���� �� ����
            ObjPool.BuffIcon_List[i].Turn_Decreased();
        }
    }
    #endregion

    #region UI
    public void Show_Info_Panel(string _text)
    {
        Info_Panel_Ref.Set_Text(_text);
    }

    // �ڵ����� ��� ���� ��
    public void On_Click_AutoBattle()
    {
        // ��ۿ� ���� �ڵ� ����
        isAuto = AutoToggle.isOn;
        GameManager.Inst.isAutoBattle = AutoToggle.isOn;
        // ��� �� ��� ���߱�
        Toggle_BG.SetActive(!AutoToggle.isOn);

        // �ڵ� ������ �ƴ϶�� return
        if (Inst.isAuto && InGameState == INGAME_STATE.STANDBY)
        {
            // ��ų ��� ���� �����̰� ���� ���� SP�� ��ų�� ����� SP�̻� ���ϰ� ���� ��
            if (ObjPool.SkillIcon_List[CurTurnCharIndex].Get_InGame_Char.CanSkill &&
               ObjPool.SkillIcon_List[CurTurnCharIndex].Get_InGame_Char.Get_SkillData.Get_SP <= CurSP)
            {
                ObjPool.SkillIcon_List[CurTurnCharIndex].On_Click_Skill_Btn();
            }

            // ���� ĳ���Ͱ� ��ų�� ����� �� ���� �����̰ų� ��ų ���� SP�� ����ؾ��� SP���� ���ٸ� �⺻����
            if (ObjPool.SkillIcon_List[CurTurnCharIndex].Get_InGame_Char.CanSkill == false ||
               CurSP < ObjPool.SkillIcon_List[CurTurnCharIndex].Get_InGame_Char.Get_SkillData.Get_SP)
            {
                ObjPool.SkillIcon_List[CurTurnCharIndex].On_Click_NoramlAtk();
            }
        }
    }

    public void Show_Skill_Desc( bool _isOn, BuffIcon_UI _buffIcon = null, Skill _skill = null, Transform _tr = null, Sprite _icon = null, bool _isUseSkill = false, bool _isSkillBtn = true)
    {
        // �Ʊ� �����̳� ���� ���������� ���� �����ִ� ui��ġ �ٲ��ֱ� ����
        float setPos = 0.0f;
        if (_buffIcon != null && _buffIcon.Get_Character_Ctrl != null)
        {
            // ĳ���� �����̶�� ���������� 200 �Ű���
            setPos = 200.0f;
        }

        // ����� �������̶��
        if(_buffIcon != null && _buffIcon.Get_EnemyCtrl != null)
        {
            // ���� �����̶�� �������� 200 �Ű���
            setPos = -200.0f;
        }

        // ���� ��ų ��� ��ư�̶��
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
            Debug.Log("���� ���������� �������Դϴ�.");
            return;
        }

        SoundManager.Inst.StopEffSound();
        GameManager.Inst.StageIndex++;
        SceneManager.LoadScene("InGameScene");
    }


    // ĳ���� ����â ���� �ݱ�
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

    // ���� ����â ���� �ݱ�
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
