using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class QuestData
{
    [SerializeField] string QuestTitle;
    public string Get_QuestTitle { get => QuestTitle; }

    [SerializeField] string QuestDesc;
    public string Get_QuestDesc { get => QuestDesc; }

    [SerializeField] int Reward_Amount;
    public int Get_RewardAmount { get => Reward_Amount; }

    [SerializeField] QUEST_REWARD_TYPE RewardType;
    public QUEST_REWARD_TYPE Get_RewardType { get => RewardType; }

    [SerializeField] string RewardIcon_Path;
    public string Get_RewardIcon_Path { get => RewardIcon_Path; }
    Sprite Reward_Img;
    public Sprite Get_Reward_Img { get => Reward_Img; }
    [SerializeField] bool isClear;
    public bool Set_isClear { get => isClear; set => isClear = value; }

    public QuestData(string _title, string _desc, int _amount, QUEST_REWARD_TYPE _reward, string _path)
    {
        QuestTitle = _title;
        QuestDesc = _desc;
        Reward_Amount = _amount;
        RewardType = _reward;
        RewardIcon_Path = _path;
        Reward_Img = Resources.Load<Sprite>($"{_path}");
        isClear = false;
    }
}
