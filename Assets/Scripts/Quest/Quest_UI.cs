using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest_UI : MonoBehaviour
{
    [SerializeField] Transform Quest_Tr;
    [SerializeField] Lobby_Manager LobbyManger_Ref;

    [SerializeField] ScrollRect ScrollValue;

    public void Sort_Quest()
    {
        for (int i = 0; i < UserInfo.QuestSlot_List.Count; i++)
        {
            Transform panel = UserInfo.QuestSlot_List[i].gameObject.transform;
            panel.SetSiblingIndex(i); // Hierarchy에서의 순서를 바꿈
        }

        LobbyManger_Ref.Refresh_UI_Dia();
    }

    public void Reset_Scroll_Value()
    {
        // 스크롤 최대로
        ScrollValue.verticalNormalizedPosition = 1;
    }
}
