using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_DB
{
    int StageIndex;
    string Stage_Num;

    float Mon_Increase_Value;
    public float Get_Mon_Increase_Value { get => Mon_Increase_Value; }

    string[] SpawnMon_Index;
    int[] SpawnIndex;
    public int[] Get_SpawnIndex { get => SpawnIndex; }

    #region Constructor
    // 생성자
    public Stage_DB(int _index, string _stageNum, string _spawnMon, float _upValue)
    {
        StageIndex = _index;
        Stage_Num = _stageNum;
        SpawnMon_Index = _spawnMon.Split(",");
        Mon_Increase_Value = _upValue;

        Set_SpawnMon_Index();
    }
    #endregion

    public void Set_SpawnMon_Index()
    {
        // 스테이지에 소환될 몬스터 프리펩을 가져오기 위한 밑작업
        SpawnIndex = new int[SpawnMon_Index.Length];

        for (int i = 0; i < SpawnMon_Index.Length; i++)
        {
            SpawnIndex[i] = int.Parse(SpawnMon_Index[i].Trim());
        }
    }
}
