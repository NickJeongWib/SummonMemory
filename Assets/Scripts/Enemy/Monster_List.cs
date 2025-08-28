using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster_List : MonoBehaviour
{
    GoogleSheetSO GoogleSheetSORef;

    public static List<Monster_DB> MonsterList = new List<Monster_DB>();

    // Start is called before the first frame update
    void Awake()
    {
        GoogleSheetSORef = GoogleSheetManager.SO<GoogleSheetSO>();

        // 몬스터 데이터 저장
        for (int i = 0; i < GoogleSheetSORef.ENEMY_DBList.Count; i++)
        {
            MONSTER_ELE.TryParse(GoogleSheetSORef.ENEMY_DBList[i].MON_ELEMENT, out MONSTER_ELE element);

            Monster_DB node = new Monster_DB(GoogleSheetSORef.ENEMY_DBList[i].MON_NAME, element, GoogleSheetSORef.ENEMY_DBList[i].MON_HP,
                GoogleSheetSORef.ENEMY_DBList[i].MON_ATK, GoogleSheetSORef.ENEMY_DBList[i].MON_DEF,
                GoogleSheetSORef.ENEMY_DBList[i].TARGET_NUM, GoogleSheetSORef.ENEMY_DBList[i].SKILL_RATIO, GoogleSheetSORef.ENEMY_DBList[i].MON_PREFAB_PATH,
                GoogleSheetSORef.ENEMY_DBList[i].MON_SKILL_PATH, GoogleSheetSORef.ENEMY_DBList[i].MON_ICON_PATH,
                GoogleSheetSORef.ENEMY_DBList[i].MON_ILLUST_PATH, GoogleSheetSORef.ENEMY_DBList[i].MON_SFX_PATH);

            MonsterList.Add(node);
        }    
    }
}
