using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_StateChange : MonoBehaviour
{
    public void StandBy()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.STANDBY;
    }

    public void TurnEnd()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.TURN_END;
    }
}
