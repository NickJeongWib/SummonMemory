using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Character SelectCharacter;
    public Character Get_SelectChar { get => SelectCharacter; set => SelectCharacter = value; }
    public static GameManager Inst;
    public bool TestMode = false;
    public bool isAutoBattle;

    int CharMaxCount;
    public int Get_CharMaxCount { get => CharMaxCount; set => CharMaxCount = value; }

    public int StageIndex;

    // Start is called before the first frame update
    void Awake()
    {
        if (Inst == null)
        {
            // Debug.Log(1);
            Inst = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            // Debug.Log(2);
            Destroy(this);
        }
    }
}
