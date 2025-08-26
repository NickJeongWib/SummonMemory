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
        // 클리어는 했지만 아직 보상을 수령 안했다면 버튼 켜주기
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

    // 보상 획득 버튼 클릭 시 
    public void On_Click_Get_Reward()
    {
        SoundManager.Inst.PlayUISound();

        ClearImage.SetActive(true);
        isClear = true;
        // 스킬 데이터 미션완료
        UserInfo.QuestData_List[QuestIndex].Set_isClear = true;

        // 보상획득
        UserInfo.Dia += Reward_Amount;

        // 완료한 업적은 제일 밑으로 정렬해준다.
        UserInfo.QuestSlot_List.Sort((a, b) => a.Get_isClear.CompareTo(b.Get_isClear));
        // UI에 Refresh
        QuestUI_Ref.Sort_Quest();

        //데이터 저장
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.DIA);
        DataNetwork_Mgr.Inst.PushPacket(PACKETTYPE.QUEST);
    }
}
