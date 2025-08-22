using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_List : MonoBehaviour
{
    [SerializeField] GoogleSheetSO GoogleSheetSORef;

    public static List<Stage_DB> StageList = new List<Stage_DB>();

    // Start is called before the first frame update
    void Start()
    {
        // 스테이지 데이터 정보 저장
        for (int i = 0; i < GoogleSheetSORef.STAGE_DBList.Count; i++)
        {
            Stage_DB node = new Stage_DB(GoogleSheetSORef.STAGE_DBList[i].STAGE_INDEX,
                GoogleSheetSORef.STAGE_DBList[i].STAGE_NUM, GoogleSheetSORef.STAGE_DBList[i].SPAWN_MON,
                GoogleSheetSORef.STAGE_DBList[i].MON_STAT_INCREASE_VALUE);


            StageList.Add(node);
        }
    }
}
