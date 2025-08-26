using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot_Panel : MonoBehaviour
{
    [SerializeField] Animator animator;
    

    public void On_Click_Close()
    {
        SoundManager.Inst.PlayUISound();
        animator.Play("EquipSlot_Close");
    }

    public void ActiveF()
    {
        this.gameObject.SetActive(false);
    }
}
