using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Quest_List : MonoBehaviour
{
    GoogleSheetSO GoogleSheetSORef;

    public static List<QuestData> QuestList = new List<QuestData>();

    private void Awake()
    {
        GoogleSheetSORef = GoogleSheetManager.SO<GoogleSheetSO>();

        for (int i = 0; i < GoogleSheetSORef.Quest_DBList.Count; i++)
        {
            QUEST_REWARD_TYPE.TryParse(GoogleSheetSORef.Quest_DBList[i].REWARD_TYPE, out QUEST_REWARD_TYPE rewardType);

            QuestData node = new QuestData(GoogleSheetSORef.Quest_DBList[i].QUEST_NAME, GoogleSheetSORef.Quest_DBList[i].QUEST_DESC,
                GoogleSheetSORef.Quest_DBList[i].REWARD_AMOUNT, rewardType, GoogleSheetSORef.Quest_DBList[i].REWARD_IMAGE);

            QuestList.Add(node);
            UserInfo.QuestData_List.Add(node);
        }

        // Debug.Log(QuestList.Count);
    }
}
