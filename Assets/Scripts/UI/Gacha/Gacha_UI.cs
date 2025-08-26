using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha_UI : MonoBehaviour
{
    [Header("---Summon_Character---")]
    [SerializeField] GameObject Summon_Btn_Pop;
    [SerializeField] GameObject Summon_Panel;

    [Header("---Summon_Equipment---")]
    [SerializeField] GameObject Equipment_Btn_Pop;
    [SerializeField] GameObject Equipment_Panel;

    public void On_Click_Summon_Btn()
    {
        SoundManager.Inst.PlayUISound();

        if (Summon_Btn_Pop.activeSelf == false)
        {
            Summon_Btn_Pop.SetActive(true);
            Equipment_Btn_Pop.SetActive(false);
        }

        Summon_Panel.SetActive(true);
        Equipment_Panel.SetActive(false);
    }

    public void On_Click_Equipment_Btn()
    {
        SoundManager.Inst.PlayUISound();

        if (Equipment_Btn_Pop.activeSelf == false)
        {
            Summon_Btn_Pop.SetActive(false);
            Equipment_Btn_Pop.SetActive(true);
        }

        Summon_Panel.SetActive(false);
        Equipment_Panel.SetActive(true);
    }
 }
