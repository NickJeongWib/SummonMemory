using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class InGame_ObjPool : MonoBehaviour
{
    #region Character
    [Header("Char")]
    [SerializeField] Transform Char_Tr;
    // �����ܿ� �Ѱ��� ĳ���� �Ӽ� ������
    [SerializeField] Material[] Skill_ON_Frames;
    // ��ų ������ ������
    [SerializeField] GameObject Skill_Icon;
    [SerializeField] Vector3 Skill_Icon_SpawnPos;
    // ��ų ������ �θ� ������Ʈ
    [SerializeField] Transform SkillIcon_Tr;
    [SerializeField] Transform[] SpawnPos;
    public List<Skill_Icon> SkillIcon_List = new List<Skill_Icon>();
    [SerializeField] Sprite[] NormalAtk_Icon;

    [Header("Char_UI")]
    [SerializeField] Transform Char_UI_Tr;
    [SerializeField] GameObject CharStat_Prefab;
    public List<PrefabStat_UI> CharStatUI_List = new List<PrefabStat_UI>();

    [Header("NormalAtk")]
    [SerializeField] GameObject[] NormalAtk_Prefab;
    [SerializeField] Transform NormalAtk_Tr;

    public List<List<NormalAtk_Ctrl>> NormalAtk_List = new List<List<NormalAtk_Ctrl>>();
    #endregion

    #region Enemy
    [Header("Monster")]
    [SerializeField] Transform Enemy_Tr;
    [SerializeField] Transform[] EnemySpawnPos;
    [SerializeField] Transform EnemySkill_Tr;

    [Header("Monster_UI")]
    [SerializeField] Transform Mon_UI_Tr;
    [SerializeField] GameObject MonStat_Prefab;
    public List<PrefabStat_UI> MonStatUI_List = new List<PrefabStat_UI>();
    #endregion

    #region UI
    [SerializeField] GameObject Buff_Prefab;
    [SerializeField] Transform BuffIcon_Tr;
    public List<BuffIcon_UI> BuffIcon_List = new List<BuffIcon_UI>();

    [Header("---Config_Panel---")]
    [SerializeField] GameObject Config_Prefab;
    [SerializeField] Transform Config_Tr;
    [SerializeField] GameObject Config_Panel;
    public GameObject Get_Config_Panel { get => Config_Panel; }
    #endregion
    //----
    // Start is called before the first frame update
    void Start()
    {

        #region SpawnChar
        // �ش� �ε����� �ֱ� ���� �̸� ����Ʈ ���� �����α�
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            InGame_Mgr.Inst.CharCtrl_List.Add(null);
            SkillIcon_List.Add(null);
            CharStatUI_List.Add(null);
        }


        // ��ų������ ���� ����
        for (int i = 0; i < UserInfo.Equip_Characters.Count; i++)
        {
            int insertIndex = UserInfo.Pos_Index[i];
            // ĳ���� ����
            GameObject spawnCharPath = Resources.Load<GameObject>(UserInfo.Equip_Characters[i].Get_Prefab_Path);
            GameObject spawnChar = Instantiate(spawnCharPath, SpawnPos[UserInfo.Pos_Index[i]].position, Quaternion.identity);

            // ������ ĳ���� ü�� �Ѱ��ֱ�
            spawnChar.GetComponent<Character_Ctrl>().Set_Init(UserInfo.Equip_Characters[i].Get_CharHP, UserInfo.Equip_Characters[i].Get_CharATK,
                UserInfo.Equip_Characters[i].Get_CharDEF, UserInfo.Equip_Characters[i].Get_Char_CRT_Damage, UserInfo.Equip_Characters[i].Get_Char_CRT_Rate,
                UserInfo.Equip_Characters[i].SkillData, UserInfo.Equip_Characters[i], UserInfo.Equip_Characters[i].Get_CharElement);
            spawnChar.transform.SetParent(Char_Tr);

            // �ش� �ε����� �߰� �� �и� ĭ ����
            InGame_Mgr.Inst.CharCtrl_List.Insert(UserInfo.Pos_Index[i], spawnChar.GetComponent<Character_Ctrl>());
            InGame_Mgr.Inst.CharCtrl_List.RemoveAt(insertIndex + 1);

            // UI ���� -------------------------
            GameObject skill = Instantiate(Skill_Icon, Skill_Icon_SpawnPos, Quaternion.identity);
            skill.transform.SetParent(SkillIcon_Tr, false);

            skill.GetComponent<Skill_Icon>().Set_Character_UI(spawnChar.GetComponent<Character_Ctrl>().Get_character.Get_Icon_Img,
                spawnChar.GetComponent<Character_Ctrl>().Get_SkillData.Get_Skill_Icon, NormalAtk_Icon[(int)spawnChar.GetComponent<Character_Ctrl>().Get_CharEle],
                Skill_ON_Frames[(int)spawnChar.GetComponent<Character_Ctrl>().Get_character.Get_CharElement], spawnChar.GetComponent<Character_Ctrl>());

            skill.SetActive(false);

            // �ش� �ε����� �߰� �� �и� ĭ ����
            SkillIcon_List.Insert(UserInfo.Pos_Index[i], skill.GetComponent<Skill_Icon>());
            SkillIcon_List.RemoveAt(insertIndex + 1);


            // ĳ���� ���� UI
            GameObject CharStat = Instantiate(CharStat_Prefab);
            CharStat.transform.SetParent(Char_UI_Tr, false);
            CharStat.GetComponent<PrefabStat_UI>().Set_UI(spawnChar.GetComponent<Character_Ctrl>().Get_character.Get_Icon_Img,
                spawnChar.GetComponent<Character_Ctrl>().Get_character.Get_CharName, UserInfo.Pos_Index[i]);

            // �ش� �ε����� �߰� �� �и� ĭ ����
            CharStatUI_List.Insert(UserInfo.Pos_Index[i], CharStat.GetComponent<PrefabStat_UI>());
            CharStatUI_List.RemoveAt(insertIndex + 1);
        }

        // �����ؼ� ����Ʈ ����
        CharStatUI_List = CharStatUI_List.OrderBy(ui => ui.PosIndex).ToList();

        for (int i = 0; i < CharStatUI_List.Count; i++)
        {
            CharStatUI_List[i].transform.SetSiblingIndex(i);
        }
        #endregion 

        #region SpawnMonster
        // ���õ� �������� ������ ���͵��� �����Ѵ�.
        for (int i = 0; i < Stage_List.StageList[GameManager.Inst.StageIndex].Get_SpawnIndex.Length; i++)
        {
            GameObject monster =
                Resources.Load<GameObject>(Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].
                Get_SpawnIndex[i]].Get_MonPrefab_Path);

            GameObject spawnMonster = Instantiate(monster);
            spawnMonster.transform.position = EnemySpawnPos[i].position;
            spawnMonster.transform.SetParent(Enemy_Tr);

            // ���������� ���� ���� �ʱ�ȭ
            spawnMonster.GetComponent<Enemy_Ctrl>().Set_Stat(
                 Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].Get_SpawnIndex[i]].Get_Monster_Name,
                Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].Get_SpawnIndex[i]].Get_MaxHP,
                Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].Get_SpawnIndex[i]].Get_Mon_ATK,
                Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].Get_SpawnIndex[i]].Get_Mon_DEF,
                Stage_List.StageList[GameManager.Inst.StageIndex].Get_Mon_Increase_Value,
                Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].Get_SpawnIndex[i]].Get_TargetCount,
                Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].Get_SpawnIndex[i]].Get_Skill_Ratio,
                Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].Get_SpawnIndex[i]].Get_MonsterEle,
                Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].Get_SpawnIndex[i]].Get_MonSkill_Path,
                EnemySkill_Tr, Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].Get_SpawnIndex[i]]);

            InGame_Mgr.Inst.CurMonsters.Add(spawnMonster.GetComponent<Enemy_Ctrl>());

            // UI ------------
            GameObject monStat = Instantiate(MonStat_Prefab);
            monStat.transform.SetParent(Mon_UI_Tr, false);
            monStat.GetComponent<PrefabStat_UI>().Set_UI(Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].
                Get_SpawnIndex[i]].Get_MonIcon_Sprite, Monster_List.MonsterList[Stage_List.StageList[GameManager.Inst.StageIndex].
                Get_SpawnIndex[i]].Get_Monster_Name);

            MonStatUI_List.Add(monStat.GetComponent<PrefabStat_UI>());

        }
        #endregion

        #region UI
        // ���� ������ ���� ����
        for(int i = 0; i < 10; i++)
        {
            GameObject buffIcon = Instantiate(Buff_Prefab);
            buffIcon.gameObject.SetActive(false);
            buffIcon.transform.SetParent(BuffIcon_Tr);
            BuffIcon_List.Add(buffIcon.GetComponent<BuffIcon_UI>());
        }

        // ȯ�� ����
        GameObject config = Instantiate(Config_Prefab);
        config.SetActive(false);
        config.transform.SetParent(Config_Tr, false);
        config.GetComponent<Config_Ctrl>().Set_Btn_UI(true, true, true);
        config.GetComponent<Config_Ctrl>().Get_Lobby_Btn.onClick.AddListener(On_Click_GoLobby);
        config.GetComponent<Config_Ctrl>().Get_Retry_Btn.onClick.AddListener(On_Click_Retry);
       Config_Panel = config;
        #endregion

        #region NormalAtk
        for (int i = 0; i < NormalAtk_Prefab.Length; i++)
        {
            NormalAtk_List.Add(new List<NormalAtk_Ctrl>());

            GameObject atk = Instantiate(NormalAtk_Prefab[i]);
            atk.transform.SetParent(NormalAtk_Tr);
            atk.gameObject.SetActive(false);
            NormalAtk_List[i].Add(atk.GetComponent<NormalAtk_Ctrl>());
        }
        #endregion
    }

    #region BuffIcon_Return;
    public GameObject Get_BuffIcon(Character_Ctrl _charCtrl ,Transform _parent, Sprite _sprite, int _turn, float _buffValue, BUFF_TYPE _buffType)
    {
        for(int i = 0; i < BuffIcon_List.Count; i++)
        {
            if(BuffIcon_List[i].gameObject.activeSelf)
            {
                continue;
            }
            else
            {
                // �Ű������� ������ �ٽ� �־� ������ġ ����ֱ�
                BuffIcon_List[i].gameObject.SetActive(true);
                BuffIcon_List[i].Set_Skill_UI(_charCtrl, _parent, _sprite, _turn, _buffValue, _buffType);
                return BuffIcon_List[i].gameObject;
            }
        }

        // ����Ʈ�� ������Ʈ�� ���� Ȱ��ȭ ���̸� �����ؼ� return
        GameObject buffIcon = Instantiate(Buff_Prefab);
        buffIcon.GetComponent<BuffIcon_UI>().Set_Skill_UI(_charCtrl, _parent, _sprite, _turn, _buffValue, _buffType);
        BuffIcon_List.Add(buffIcon.GetComponent<BuffIcon_UI>());
        return buffIcon;
    }
    #endregion

    #region NormalAtk_Obj_Return
    // �⺻ ���� ������Ʈ ��ȯ
    public GameObject Get_NormalAtk(CHAR_ELE _ele, Transform _targetTr)
    {
        // �Ӽ��� �ִ� �⺻������ ������ŭ �ݺ�
        for(int i = 0; i < NormalAtk_List[(int)_ele].Count; i++)
        {
            // �Ӽ��� �⺻������ ���������� �ǳʶٱ�
            if(NormalAtk_List[(int)_ele][i].gameObject.activeSelf)
            {
                continue;
            }
            else // �Ӽ� ������ ���� �ִٸ�
            {
                // �����ִ� ���� Ÿ�� ���� �� ������Ʈ Ȱ��ȭ ��Ų �� ��ȯ
                NormalAtk_List[(int)_ele][i].Set_Target(_targetTr);
                NormalAtk_List[(int)_ele][i].gameObject.SetActive(true);
                return NormalAtk_List[(int)_ele][i].gameObject;
            }
        }

        // ���� for������ ����ٸ� Ȱ�� ������ ������Ʈ�� ���ٴ� ���� �ǹ�
        // ���� ���� ���� �� ��ȯ
        GameObject atk = Instantiate(NormalAtk_Prefab[(int)_ele]);
        atk.transform.SetParent(NormalAtk_Tr);
        atk.GetComponent<NormalAtk_Ctrl>().Set_Target(_targetTr);
        NormalAtk_List[(int)_ele].Add(atk.GetComponent<NormalAtk_Ctrl>());
        return atk;
    }
    #endregion

    #region Config_Btn
    // ȯ�漳�� �����ֱ�
    public void On_Click_Config()
    {
        SoundManager.Inst.PlayUISound();

        // ȯ�� ������ null�� �ƴϸ�
        if (Config_Panel != null)
        {
            // Ȱ��ȭ�� �����ֱ�
            Config_Panel.SetActive(true);
        }
        else
        {
            // null�̸� �����ؼ� �����ֱ�
            GameObject config = Instantiate(Config_Prefab);
            config.transform.SetParent(Config_Tr, false);
            Config_Panel = config;
        }
    }

    public void On_Click_GoLobby()
    {
        SoundManager.Inst.PlayUISound();
        SceneManager.LoadScene("LobbyScene");
    }

    public void On_Click_Retry()
    {
        SoundManager.Inst.PlayUISound();
        SceneManager.LoadScene("InGameScene");
    }
#endregion
}
