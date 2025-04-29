using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_Fade : MonoBehaviour
{
    [Header("---All---")]
    [SerializeField] GameObject NotTouch_Raycast;

    [Header("---Character_Transition---")]
    [SerializeField] GameObject Character_Transition_Panel;
    [SerializeField] GameObject NotTouch_RayCast;

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
    public void ActiveF_CharTrans()
    {
        Character_Transition_Panel.SetActive(false);
    }

    public void ActiveT_CharTrans()
    {
        Character_Transition_Panel.SetActive(true);
    }

    // 캐릭터 정보창 이동 트래지션의 효과가 끝났을 때 작동
    public void On_Click_OffPanel_CircleEnd()
    {
        NotTouch_RayCast.SetActive(false); // 화면전환 중 버튼 클릭 방지
    }
    #endregion
}
