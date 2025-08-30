using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop_UpDown : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject PopUp_Panel;

    [SerializeField] GameObject SelectStage_Transition; // 스테이지 선택 화면 전환
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
    // 열고 닫을 때 뒤에 배경화면을 유지할지 안할지 알기 위한 변수
    bool isStageOn = false;
    // 스테이지 선택 클릭
    public void On_Click_SelectStage()
    {
        SoundManager.Inst.PlayUISound();

        if(SelectStage_Transition.activeSelf == false)
        {
            SelectStage_Transition.SetActive(true);
        }
        else
        {
            // 여는 화면전환 실행
            animator.Play("Stage_Transition");
        }
        

        isStageOn = true;
    }
    // 스테이지 선택 닫기
    public void On_Click_SelectStage_Close()
    {
        SoundManager.Inst.PlayUISound();

        // 닫는 화면전환 실행
        animator.Play("Stage_Transition_Close");
        isStageOn = false;
    }

    // 오픈하는 화면전환이면 배경화면이 열리고 닫는 화면전환이면 배경 사라지게
    public void ActiveF_SelectStage()
    {
        SelectStage_BG.SetActive(isStageOn);

        // 만약 캐릭터 교체화면 진입 후 다시 스테이지 선택으로 돌아올 때
        if (StageSelect_UI.Inst.isChange == false)
        {
            StageSelect_UI.Inst.ActiveF_CharPanel();
        }
    }
    #endregion
}
