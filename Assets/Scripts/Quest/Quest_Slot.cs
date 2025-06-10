using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class Quest_Slot : MonoBehaviour
{

    [SerializeField] QUEST_REWARD_TYPE RewardType;
    [SerializeField] GameObject ClearImage;
    [SerializeField] Image Item_Img;
    [SerializeField] Text Quest_Text;
    [SerializeField] Text Quest_Desc;
    [SerializeField] Text Amount_Text;
    [SerializeField] int Reward_Amount;
    [SerializeField] bool isClear;
    public bool Get_isClear { get => isClear; }

    [SerializeField] Quest_UI QuestUI_Ref;
    public Quest_UI Set_QuestUI_Ref { set => QuestUI_Ref = value; }

    public void Set_UI(QUEST_REWARD_TYPE _type, Sprite _sprite, string _questTitle, string _questDesc, int _amount)
    {
        RewardType = _type;
        Item_Img.sprite = _sprite;
        Quest_Text.text = _questTitle;
        Quest_Desc.text = _questDesc;
        Reward_Amount = _amount;
        Amount_Text.text = Reward_Amount.ToString();

        UserInfo.QuestSlot_List.Add(this);
    }

    // ���� ȹ�� ��ư Ŭ�� �� 
    public void On_Click_Get_Reward()
    {
        isClear = true;
        ClearImage.SetActive(true);

        // ����ȹ��
        UserInfo.Dia += Reward_Amount;

        // �Ϸ��� ������ ���� ������ �������ش�.
        UserInfo.QuestSlot_List.Sort((a, b) => a.Get_isClear.CompareTo(b.Get_isClear));
        // UI�� Refresh
        QuestUI_Ref.Sort_Quest();
    }
}
