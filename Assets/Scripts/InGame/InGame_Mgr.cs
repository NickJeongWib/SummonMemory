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
    public Skill_Use_Face Skill_On_CharFace;
    // 현재 게임진행 상태
    public INGAME_STATE InGameState = INGAME_STATE.READY;
    [SerializeField] InGame_ObjPool ObjPool;

    // 스킬 이펙트 나오게 하기 위한 델리게이트
    public delegate void UseSkill();
    public UseSkill UseSkill_ON;
    public Animator SP_animator;

    [Header("Game_Info")]
    public int CurTurnCharIndex = 0;

    [Header("UI")]
    [SerializeField] Text TurnText;
    public int CurrentTurn = 1;

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
                    InGame_Mgr.Inst.CurTurnCharIndex = 0;
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

            default:
                break;
        }
    }

    public void Test_Back()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
