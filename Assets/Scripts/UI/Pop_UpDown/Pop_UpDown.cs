using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop_UpDown : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject PopUp_Panel;

    public void Pop_Up()
    {
        
    }

    public void Pop_Down()
    {
        animator.SetTrigger("PopDown");
    }

    public void PopUp_Panel_ActiveF()
    {
        PopUp_Panel.SetActive(false);
    }
}
