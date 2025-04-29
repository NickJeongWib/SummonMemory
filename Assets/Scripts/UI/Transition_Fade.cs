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
    // TODO ## Transition_Fade �⺻ Fade Out
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
    // TODO ## Transition_Fade ĳ���� ���� Transition
    public void ActiveF_CharTrans()
    {
        Character_Transition_Panel.SetActive(false);
    }

    public void ActiveT_CharTrans()
    {
        Character_Transition_Panel.SetActive(true);
    }

    // ĳ���� ����â �̵� Ʈ�������� ȿ���� ������ �� �۵�
    public void On_Click_OffPanel_CircleEnd()
    {
        NotTouch_RayCast.SetActive(false); // ȭ����ȯ �� ��ư Ŭ�� ����
    }
    #endregion
}
