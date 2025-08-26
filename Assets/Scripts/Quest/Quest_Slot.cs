using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

[System.Serializable]
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
    [SerializeField] int QuestIndex;
    [SerializeField] Button Reward_Btn;

    public bool Get_isClear { get => isClear; }

    [SerializeField] Quest_UI QuestUI_Ref;
    public Quest_UI Set_QuestUI_Ref { set => QuestUI_Ref = value; }

    public void Set_UI(QUEST_REWARD_TYPE _type, Sprite _sprite, string _questTitle, string _questDesc, int _amount, int _index, bool _isClear)
    {
        RewardType = _type;
        Item_Img.sprite = _sprite;
        Quest_Text.text = _questTitle;
        Quest_Desc.text = _questDesc;
        Reward_Amount = _amount;
        Amount_Text.text = Reward_Amount.ToString();
        QuestIndex = _index;
        isClear = _isClear;

        ClearImage.SetActive(isClear);
        // Ŭ����� ������ ���� ������ ���� ���ߴٸ� ��ư ���ֱ�
        if (_index < UserInfo.StageClear.Count)
        {
            if (UserInfo.StageClear[QuestIndex] == true && isClear == false)
            {
                ClearImage.SetActive(false);
                Reward_Btn.interactable = true;
            }
        }

        UserInfo.QuestSlot_List.Add(this);
    }

    // ���� ȹ�� ��ư Ŭ�� �� 
    public void On_Click_Get_Reward()
    {
        SoundManager.Inst.PlayUISound();

        ClearImage.SetActive(true);
        isClear = true;
        // ��ų ������ �̼ǿϷ�
        UserInfo.QuestData_List[QuestIndex].Set_isClear = true;

        // ����ȹ��
        UserInfo.Dia += Reward_Amount;

        // �Ϸ��� ������ ���� ������ �������ش�.
        UserInfo.QuestSlot_List.Sort((a, b) => a.Get_isClear.CompareTo(b.Get_isClear));
        // UI�� Refresh
        QuestUI_Ref.Sort_Quest();

        //������ ����
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.DIA);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.QUEST);
    }
}
