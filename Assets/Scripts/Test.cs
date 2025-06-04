using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Test : MonoBehaviour
{
    public void On_Click_Test()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void On_Click_TestSce()
    {
        SceneManager.LoadScene("TestScne");
    }

}
