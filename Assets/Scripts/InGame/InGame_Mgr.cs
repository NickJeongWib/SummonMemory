using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Define;

public class InGame_Mgr : MonoBehaviour
{
    // 현재 게임진행 상태
    INGAME_STATE InGameState = INGAME_STATE.READY;

    // 싱글톤
    public static InGame_Mgr Inst = null;

    private void Awake()
    {
        #region Singleton
        if (Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
    }

    private void Start()
    {

    }

    public void Test_Back()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
