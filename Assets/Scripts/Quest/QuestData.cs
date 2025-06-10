using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class QuestData
{
    string QuestTitle;
    public string Get_QuestTitle { get => QuestTitle; }

    string QuestDesc;
    public string Get_QuestDesc { get => QuestDesc; }

    int Reward_Amount;
    public int Get_RewardAmount { get => Reward_Amount; }

    QUEST_REWARD_TYPE RewardType;
    public QUEST_REWARD_TYPE Get_RewardType { get => RewardType; }

    Sprite Reward_Img;
    public Sprite Get_Reward_Img { get => Reward_Img; }


    public QuestData(string _title, string _desc, int _amount, QUEST_REWARD_TYPE _reward, string _path)
    {
        QuestTitle = _title;
        QuestDesc = _desc;
        Reward_Amount = _amount;
        RewardType = _reward;
        Reward_Img = Resources.Load<Sprite>($"{_path}");
    }
}
