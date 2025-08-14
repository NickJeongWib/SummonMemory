using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_ObjPool : MonoBehaviour
{
    // 아이콘에 넘겨줄 캐릭터 속성 프레임
    [SerializeField] Material[] Skill_ON_Frames;
    // 스킬 아이콘 프레임
    [SerializeField] GameObject Skill_Icon;
    [SerializeField] Vector3 Skill_Icon_SpawnPos;

    // 스킬 아이콘 부모 오브젝트
    [SerializeField] Transform SkillIcon_Tr;
    [SerializeField] Transform[] SpawnPos;

    public List<Skill_Icon> SkillIcon_List = new List<Skill_Icon>();

    // Start is called before the first frame update
    void Start()
    {
        // 스킬아이콘 동적 생성
        for(int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            GameObject spawnCharPath = Resources.Load<GameObject>(UserInfo.Equip_Characters[i].Get_Prefab_Path);
            GameObject spawnChar = Instantiate(spawnCharPath, SpawnPos[UserInfo.Pos_Index[i]].position, Quaternion.identity);

            // 생성된 캐릭터 체력 넘겨주기
            spawnChar.GetComponent<Character_Ctrl>().Set_Init(UserInfo.Equip_Characters[i].Get_CharHP, UserInfo.Equip_Characters[i].SkillData, UserInfo.Equip_Characters[i]);

            GameObject skill = Instantiate(Skill_Icon, Skill_Icon_SpawnPos, Quaternion.identity);
            skill.transform.SetParent(SkillIcon_Tr, false);

            // 스킬 아이콘 UI 넘겨주기
            skill.GetComponent<Skill_Icon>().Set_Character_UI(UserInfo.Equip_Characters[i].Get_Icon_Img, UserInfo.Equip_Characters[i].SkillData.Get_Skill_Icon,
                Skill_ON_Frames[(int)UserInfo.Equip_Characters[i].Get_CharElement], spawnChar.GetComponent<Character_Ctrl>());

            skill.SetActive(false);

            SkillIcon_List.Add(skill.GetComponent<Skill_Icon>());
        }
    }
}
