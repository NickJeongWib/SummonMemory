using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


public class EquipmentOption
{
    public EQUIPMENT_OPTION EquipmentRandomOption;
    public EQUIPMENT_OPTION_GRADE EquipmentOptionGrade;
    public int RandIntMin = 0;
    public int RandIntMax = 0;
    public float RandFloatMin = 0.0f;
    public float RandFloatMax = 0.0f;

    public void Set_MinMax(object _numMin, object _numMax, EQUIPMENT_OPTION _optionType)
    {
        EquipmentRandomOption = _optionType;

        RandFloatMin = (float)_numMin;
        RandFloatMax = (float)_numMax;

        // Debug.Log($"{RandIntMin} / {RandIntMax} / {RandFloatMin} / {RandFloatMax}");
    }

    public float Get_OptionValue(ref EQUIPMENT_OPTION_GRADE _optionGrade)
    {
        float value = 0;

        value = Random.Range(RandFloatMin, RandFloatMax);

        float optionGradeMax = RandFloatMax - RandFloatMin;
        float optionGrade = (float)value - RandFloatMin;

        if (0 < optionGrade && optionGrade <= optionGradeMax * 0.25f)
        {
            _optionGrade = EQUIPMENT_OPTION_GRADE.C;
            // Debug.Log("C등급");
        }
        else if (optionGradeMax * 0.25f < optionGrade && optionGrade <= optionGradeMax * 0.5f)
        {
            _optionGrade = EQUIPMENT_OPTION_GRADE.B;
            // Debug.Log("B등급");
        }
        else if (optionGradeMax * 0.5f < optionGrade && optionGrade <= optionGradeMax * 0.75f)
        {
            _optionGrade = EQUIPMENT_OPTION_GRADE.A;
            // Debug.Log("A등급");
        }
        else if (optionGradeMax * 0.75f < optionGrade && optionGrade <= optionGradeMax)
        {
            _optionGrade = EQUIPMENT_OPTION_GRADE.S;
            // Debug.Log("S등급");
        }

        return value;
    }
}
