using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_Fade : MonoBehaviour
{
    [SerializeField] GameObject NotTouch_Raycast;

    #region Base
    // TODO ## Transition_Fade 기본 Fade Out
    public void Transition_ActiveF()
    {
        this.gameObject.SetActive(false);
        NotTouch_Raycast.SetActive(false);
    }

    public void Transition_Off()
    {
        this.gameObject.SetActive(false);
    }
    #endregion

    #region Character_Transition
    // TODO ## Transition_Fade 캐릭터 정보 Transition

    #endregion
}
