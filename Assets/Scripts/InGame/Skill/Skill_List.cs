using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill_List : MonoBehaviour
{
    [SerializeField] GoogleSheetSO GoogleSheetSORef;

    public List<Skill> SkillData_List = new List<Skill>();

    void Awake()
    {
       
    }
}
