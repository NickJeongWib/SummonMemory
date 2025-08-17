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
    public Skill_Use_Face Skill_On_CharFace;
    // ���� �������� ����
    public INGAME_STATE InGameState = INGAME_STATE.READY;
    [SerializeField] InGame_ObjPool ObjPool;

    // ��ų ����Ʈ ������ �ϱ� ���� ��������Ʈ
    public delegate void UseSkill();
    public UseSkill UseSkill_ON;
    public Animator SP_animator;

    [Header("Game_Info")]
    public int CurTurnCharIndex = 0;

    [Header("UI")]
    [SerializeField] Text TurnText;
    public int CurrentTurn = 1;

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
