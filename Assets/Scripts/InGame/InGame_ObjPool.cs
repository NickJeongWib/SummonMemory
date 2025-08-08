using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_ObjPool : MonoBehaviour
{
    // �����ܿ� �Ѱ��� ĳ���� �Ӽ� ������
    [SerializeField] Material[] Skill_ON_Frames;
    // ��ų ������ ������
    [SerializeField] GameObject Skill_Icon;
    // ��ų ������ �θ� ������Ʈ
    [SerializeField] Transform SkillIcon_Tr;

    [SerializeField] Transform[] SpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        // ��ų������ ���� ����
        for(int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            GameObject spawnCharPath = Resources.Load<GameObject>(UserInfo.Equip_Characters[i].Get_Prefab_Path);
            GameObject spawnChar = Instantiate(spawnCharPath, SpawnPos[UserInfo.Pos_Index[i]].position, Quaternion.identity);

            // ������ ĳ���� ü�� �Ѱ��ֱ�
            spawnChar.GetComponent<Character_Ctrl>().Set_HP(UserInfo.Equip_Characters[i].Get_CharHP);

            GameObject skill = Instantiate(Skill_Icon);
            skill.transform.SetParent(SkillIcon_Tr, false);
            // ��ų ������ UI �Ѱ��ֱ�
            skill.GetComponent<Skill_Icon>().Set_Character_UI(UserInfo.Equip_Characters[i].Get_Icon_Img,
                Skill_ON_Frames[(int)UserInfo.Equip_Characters[i].Get_CharElement], spawnChar.GetComponent<Character_Ctrl>());
        }
    }
}
