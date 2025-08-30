using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop_UpDown : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject PopUp_Panel;

    [SerializeField] GameObject SelectStage_Transition; // �������� ���� ȭ�� ��ȯ
    [SerializeField] GameObject SelectStage_BG;

    public void Pop_Up()
    {
        
    }

    public void Pop_Down()
    {
        SoundManager.Inst.PlayUISound();
        animator.SetTrigger("PopDown");
    }

    public void PopUp_Panel_ActiveF()
    {
        PopUp_Panel.SetActive(false);
    }

    #region StageSelect_On/Off
    // ���� ���� �� �ڿ� ���ȭ���� �������� ������ �˱� ���� ����
    bool isStageOn = false;
    // �������� ���� Ŭ��
    public void On_Click_SelectStage()
    {
        SoundManager.Inst.PlayUISound();

        if(SelectStage_Transition.activeSelf == false)
        {
            SelectStage_Transition.SetActive(true);
        }
        else
        {
            // ���� ȭ����ȯ ����
            animator.Play("Stage_Transition");
        }
        

        isStageOn = true;
    }
    // �������� ���� �ݱ�
    public void On_Click_SelectStage_Close()
    {
        SoundManager.Inst.PlayUISound();

        // �ݴ� ȭ����ȯ ����
        animator.Play("Stage_Transition_Close");
        isStageOn = false;
    }

    // �����ϴ� ȭ����ȯ�̸� ���ȭ���� ������ �ݴ� ȭ����ȯ�̸� ��� �������
    public void ActiveF_SelectStage()
    {
        SelectStage_BG.SetActive(isStageOn);

        // ���� ĳ���� ��üȭ�� ���� �� �ٽ� �������� �������� ���ƿ� ��
        if (StageSelect_UI.Inst.isChange == false)
        {
            StageSelect_UI.Inst.ActiveF_CharPanel();
        }
    }
    #endregion
}
