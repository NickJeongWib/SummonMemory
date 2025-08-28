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

    // ��ü�� ������ �� �������� �߰��� �� �ִ� �ɷ�ġ�� ������ �������� �ɼ��� ���� �ɼ����� �����´�
    public void Set_MinMax(object _numMin, object _numMax, EQUIPMENT_OPTION _optionType)
    {
        EquipmentRandomOption = _optionType;

        RandFloatMin = (float)_numMin;
        RandFloatMax = (float)_numMax;
    }

    // �Ű������� �ٷ� ������ ������� ���� �� �ְ� refŸ�� �߰�
    public float Get_OptionValue(ref EQUIPMENT_OPTION_GRADE _optionGrade)
    {
        float value = 0;

        // ��ü�� �����ϸ鼭 ������ �������� ���� �� �������� �߰� �ɷ��� ���̱� ����
        value = Random.Range(RandFloatMin, RandFloatMax);

        // ����� �� �ִ� �ִ� ��
        float optionGradeMax = RandFloatMax - RandFloatMin;
        // �ּҰ����κ��� �󸶳� ����� �ߴ��� �˱�����
        float optionGrade = (float)value - RandFloatMin;

        // ex. 10~30% �� �������� ���� ����
        // 15%�� ���ٸ� �ּҰ����κ��� 5% ���, �ִ밪 30%�� 25%�� 7.5%�ϱ� 5�۴� C��޿� ����  

        // ����� ��� ������ ���� ����
        if (0 < optionGrade && optionGrade <= optionGradeMax * 0.25f)
        {
            _optionGrade = EQUIPMENT_OPTION_GRADE.C;
            // Debug.Log("C���");
        }
        else if (optionGradeMax * 0.25f < optionGrade && optionGrade <= optionGradeMax * 0.5f)
        {
            _optionGrade = EQUIPMENT_OPTION_GRADE.B;
            // Debug.Log("B���");
        }
        else if (optionGradeMax * 0.5f < optionGrade && optionGrade <= optionGradeMax * 0.75f)
        {
            _optionGrade = EQUIPMENT_OPTION_GRADE.A;
            // Debug.Log("A���");
        }
        else if (optionGradeMax * 0.75f < optionGrade && optionGrade <= optionGradeMax)
        {
            _optionGrade = EQUIPMENT_OPTION_GRADE.S;
            // Debug.Log("S���");
        }

        return value;
    }
}
