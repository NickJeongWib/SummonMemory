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

    [SerializeField] Skill SkillData;

    protected virtual void Update()
    {
        
    }


    #region HP_Set
    // 체력 초기화
    public virtual void Set_Init(float _hp, Skill _skillData)
    {
        MaxHP = _hp;
        CurHP = MaxHP;

        SkillData = _skillData;
    }

    #endregion
}
