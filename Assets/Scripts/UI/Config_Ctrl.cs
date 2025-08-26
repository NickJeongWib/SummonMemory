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
        // 슬라이더 밸류는 저장된 값으로 호출해준다.
        BGM_Slider.value = PlayerPrefs.GetFloat("BGM_Vol");
        SFX_Slider.value = PlayerPrefs.GetFloat("SFX_Vol");
    }

    private void Start()
    {
        // 사운드 토글 초기화
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

    // 생성시 로비인지 인게임인지에 따라 버튼 활성화
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

        // 인덱스 별로 다른 토글 작동
        // 테스트 모드
        if (_index == 0)
        {
            // 테스트모드 켜져있으면 온 꺼져있으면 오프
            GameManager.Inst.TestMode = !TestMode_Toggle.isOn;
            TestToggle_BG.SetActive(!TestMode_Toggle.isOn);

        }
        // 효과음 On/Off
        else if(_index == 1)
        {
            SoundManager.Inst.SelectUI_Audio.mute = SFX_Toggle.isOn;
            SoundManager.Inst.VoiceAudioSrc.mute = SFX_Toggle.isOn;

            SFXToggle_BG.SetActive(!SFX_Toggle.isOn);
        }
        // 배경음 On/Off
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

        // 볼륨 값 저장
        PlayerPrefs.SetFloat("BGM_Vol", BGM_Slider.value);
        PlayerPrefs.SetFloat("SFX_Vol", SFX_Slider.value);

        // 토글이 켜졌는지 안켜졌는지 저장
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

        // apk용 저장
        PlayerPrefs.Save();
        animator.Play("Pop_Down");
    }
    #endregion

    #region UI_Close
    // 환경설정 닫기
    public void On_Click_Close()
    {
        SoundManager.Inst.PlayUISound();

        // 취소 시 효과음 설정 기존 값으로 변경
        BGM_Slider.value = PlayerPrefs.GetFloat("BGM_Vol");
        SFX_Slider.value = PlayerPrefs.GetFloat("SFX_Vol");

        // 사운드 토글 초기화
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
    // 배경음 슬라이더 value 변경 시 호출되는 함수
    public void Set_BGM_Vol()
    {
        Set_Vol(SoundManager.Inst.BGM_AudioSrc, BGM_Slider.value);
    }

    // 효과음 슬라이더 value 변경 시 호출되는 함수
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
