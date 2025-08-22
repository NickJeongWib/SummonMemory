using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_F : MonoBehaviour
{
    public void Active_Off()
    {
        this.gameObject.SetActive(false);
    }

    public void Active_Face_UI_Off()
    {
        this.gameObject.SetActive(false);
    }

    public void On_Skill_Effect()
    {
        InGame_Mgr.Inst.UseSkill_ON();

        InGame_Mgr.Inst.UseSkill_ON = null;
    }

    public void On_MonSkill_Effect()
    {
        InGame_Mgr.Inst.UseMonSkill_ON();

        InGame_Mgr.Inst.UseMonSkill_ON = null;
    }

    public void TurnStart()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.TURN_START;
        this.gameObject.SetActive(false);
    }

    public void Prefab_ActiveF()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
}
