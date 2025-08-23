using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage_Btn : MonoBehaviour
{
    [SerializeField] int StageIndex;
    [SerializeField] GameObject Lock_Image;

    private void Start()
    {
        if (StageIndex < UserInfo.StageClear.Count)
        {
            Lock_Image.SetActive(!UserInfo.StageClear[StageIndex - 1]);
        }
    }
}
