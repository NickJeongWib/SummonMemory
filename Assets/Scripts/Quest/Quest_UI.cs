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
            panel.SetSiblingIndex(i); // Hierarchy������ ������ �ٲ�
        }

        LobbyManger_Ref.Refresh_UI_Dia();
    }

    public void Reset_Scroll_Value()
    {
        // ��ũ�� �ִ��
        ScrollValue.verticalNormalizedPosition = 1;
    }
}
