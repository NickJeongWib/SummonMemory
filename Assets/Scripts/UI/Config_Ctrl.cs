using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Config_Ctrl : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Test_Root")] 
    [SerializeField] GameObject TestToggle_BG;
    [SerializeField] Toggle TestMode_Toggle;

    [Header("BGM_Root")]
    [SerializeField] GameObject BGMToggle_BG;
    [SerializeField] Slider BGM_Slider;
    [SerializeField] Toggle BGM_Toggle;

    [Header("SFX_Root")]
    [SerializeField] GameObject SFXToggle_BG;
    [SerializeField] Slider SFX_Slider;
    [SerializeField] Toggle SFX_Toggle;

    [Header("Btn_Root")]
    [SerializeField] Button Save_Btn;
    [SerializeField] Button Lobby_Btn;
    public Button Get_Lobby_Btn { get => Lobby_Btn; }
    [SerializeField] Button Retry_Btn;
    public Button Get_Retry_Btn { get => Retry_Btn; }



    void OnEnable()
    {
        // �����̴� ����� ����� ������ ȣ�����ش�.
        BGM_Slider.value = PlayerPrefs.GetFloat("BGM_Vol");
        SFX_Slider.value = PlayerPrefs.GetFloat("SFX_Vol");
    }

    private void Start()
    {
        // ���� ��� �ʱ�ȭ
        if (PlayerPrefs.GetInt("BGM_OnOff") == 1)
        {
            BGM_Toggle.isOn = true;
        }
        else
        {
            BGM_Toggle.isOn = false;
        }

        if (PlayerPrefs.GetInt("SFX_OnOff") == 1)
        {
            SFX_Toggle.isOn = true;
        }
        else
        {
            SFX_Toggle.isOn = false;
        }
    }

    // ������ �κ����� �ΰ��������� ���� ��ư Ȱ��ȭ
    public void Set_Btn_UI(bool _saveBtnActive, bool _lobbyBtnActive = false, bool _rePlayBtnActive = false)
    {
        Save_Btn.gameObject.SetActive(_saveBtnActive);
        Lobby_Btn.gameObject.SetActive(_lobbyBtnActive);
        Retry_Btn.gameObject.SetActive(_rePlayBtnActive);
    }

    #region Toggle
    public void On_Click_Toggle(int _index)
    {
        SoundManager.Inst.PlayUISound();

        // �ε��� ���� �ٸ� ��� �۵�
        // �׽�Ʈ ���
        if (_index == 0)
        {
            // �׽�Ʈ��� ���������� �� ���������� ����
            GameManager.Inst.TestMode = !TestMode_Toggle.isOn;
            TestToggle_BG.SetActive(!TestMode_Toggle.isOn);

        }
        // ȿ���� On/Off
        else if(_index == 1)
        {
            SoundManager.Inst.SelectUI_Audio.mute = SFX_Toggle.isOn;
            SoundManager.Inst.VoiceAudioSrc.mute = SFX_Toggle.isOn;

            SFXToggle_BG.SetActive(!SFX_Toggle.isOn);
        }
        // ����� On/Off
        else if(_index == 2)
        {
            SoundManager.Inst.BGM_AudioSrc.mute = BGM_Toggle.isOn;
            BGMToggle_BG.SetActive(!BGM_Toggle.isOn);
        }
    }
    #endregion

    #region Save
    public void On_Click_Save()
    {
        SoundManager.Inst.PlayUISound();

        // ���� �� ����
        PlayerPrefs.SetFloat("BGM_Vol", BGM_Slider.value);
        PlayerPrefs.SetFloat("SFX_Vol", SFX_Slider.value);

        // ����� �������� ���������� ����
        if(BGM_Toggle.isOn)
        {
            PlayerPrefs.SetInt("BGM_OnOff", 1);
        }
        else
        {
            PlayerPrefs.SetInt("BGM_OnOff", 0);
        }

        if(SFX_Toggle.isOn)
        {
            PlayerPrefs.SetInt("SFX_OnOff", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SFX_OnOff", 0);
        }

        // apk�� ����
        PlayerPrefs.Save();
        animator.Play("Pop_Down");
    }
    #endregion

    #region UI_Close
    // ȯ�漳�� �ݱ�
    public void On_Click_Close()
    {
        SoundManager.Inst.PlayUISound();

        // ��� �� ȿ���� ���� ���� ������ ����
        BGM_Slider.value = PlayerPrefs.GetFloat("BGM_Vol");
        SFX_Slider.value = PlayerPrefs.GetFloat("SFX_Vol");

        // ���� ��� �ʱ�ȭ
        if (PlayerPrefs.GetInt("BGM_OnOff") == 1)
        {
            BGM_Toggle.isOn = true;
        }
        else
        {
            BGM_Toggle.isOn = false;
        }

        if (PlayerPrefs.GetInt("SFX_OnOff") == 1)
        {
            SFX_Toggle.isOn = true;
        }
        else
        {
            SFX_Toggle.isOn = false;
        }

        animator.Play("Pop_Down");
    }
    #endregion

    #region SoundVolume
    // ����� �����̴� value ���� �� ȣ��Ǵ� �Լ�
    public void Set_BGM_Vol()
    {
        Set_Vol(SoundManager.Inst.BGM_AudioSrc, BGM_Slider.value);
    }

    // ȿ���� �����̴� value ���� �� ȣ��Ǵ� �Լ�
    public void Set_SFX_Vol()
    {
        Set_Vol(SoundManager.Inst.SelectUI_Audio, SFX_Slider.value);
        Set_Vol(SoundManager.Inst.VoiceAudioSrc, SFX_Slider.value);
    }

    void Set_Vol(AudioSource _audioSrc, float _value)
    {
        _audioSrc.volume = _value;
    }

    #endregion
}
