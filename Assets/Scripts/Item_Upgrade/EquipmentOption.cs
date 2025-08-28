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

    // 객체를 생성할 때 아이템이 추가될 수 있는 능력치의 범위를 가져오고 옵션이 무슨 옵션인지 가져온다
    public void Set_MinMax(object _numMin, object _numMax, EQUIPMENT_OPTION _optionType)
    {
        EquipmentRandomOption = _optionType;

        RandFloatMin = (float)_numMin;
        RandFloatMax = (float)_numMax;
    }

    // 매개변수는 바로 설정한 등급으로 나갈 수 있게 ref타입 추가
    public float Get_OptionValue(ref EQUIPMENT_OPTION_GRADE _optionGrade)
    {
        float value = 0;

        // 객체를 생성하면서 가져온 아이템의 정보 중 랜덤으로 추가 능력이 붙이기 위해
        value = Random.Range(RandFloatMin, RandFloatMax);

        // 상승할 수 있는 최대 값
        float optionGradeMax = RandFloatMax - RandFloatMin;
        // 최소값으로부터 얼마나 상승을 했는지 알기위해
        float optionGrade = (float)value - RandFloatMin;

        // ex. 10~30% 중 랜덤으로 값을 받음
        // 15%가 떴다면 최소값으로부터 5% 상승, 최대값 30%의 25%는 7.5%니까 5퍼는 C등급에 속함  

        // 어아탬 등급 결정을 위한 필터
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
