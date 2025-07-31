using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Ctrl : MonoBehaviour
{
    [Header("HP")]
    float MaxHP;
    [SerializeField] Image HP_Image;
    [SerializeField] float CurHP;

    protected virtual void Update()
    {
        
    }


    #region HP_Set
    // ü�� �ʱ�ȭ
    public virtual void Set_HP(float _hp)
    {
        MaxHP = _hp;
        CurHP = MaxHP;
    }

    #endregion
}
