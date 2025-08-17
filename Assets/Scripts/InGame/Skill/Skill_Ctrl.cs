using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Ctrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TurnEnd()
    {
        InGame_Mgr.Inst.InGameState = INGAME_STATE.TURN_END;
    }
}
