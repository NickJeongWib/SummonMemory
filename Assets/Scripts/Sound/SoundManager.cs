using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [HideInInspector] public AudioSource AudioSrc = null;
    Dictionary<string, AudioClip> AudioClip_Dict = new Dictionary<string, AudioClip>();

    float m_bgmVolume = 0.2f;
    public bool isSFX_On = true;
    public bool isBGM_On = true;
    [HideInInspector] public float SoundVol = 1.0f;

    // 효과음 최적화를 위한 버퍼 변수
    // 5개의 레이어로 플레이
    int EffSoundCount = 5;
    // 5개까지 재생되게 제어
    int SoundCount = 0;
    
    GameObject[] SoundObj_List = new GameObject[10];
    AudioSource[] AudioSource_List = new AudioSource[10];
    float[] EffVolume = new float[10];

    public AudioSource VoiceAudioSrc;
    public AudioSource SelectUI_Audio;
    public AudioSource BGM_AudioSrc;
    public AudioSource Gacha_AudioSrc;

    #region Singleton
    // 싱글톤 
    public static SoundManager Inst = null;

    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
    }
    #endregion

    #region Init
    // Start is called before the first frame update
    void Start()
    {
        LoadInit_Sound();
        LoadChildGameObj();
    }

    void LoadInit_Sound()
    {
        // 게임 시작되면 효과음 OnOff, 사운드 볼륨 로컬 로딩 후 적용
        int SFX_OnOff = PlayerPrefs.GetInt("SFX_OnOff", 0);
        // 게임 시작되면 배경음 OnOff, 사운드 볼륨 로컬 로딩 후 적용
        int BGM_OnOff = PlayerPrefs.GetInt("BGM_OnOff", 0);

        if (SFX_OnOff == 1)
        {
            VoiceAudioSrc.mute = true;
            SelectUI_Audio.mute = true;

        }
        else
        {
            VoiceAudioSrc.mute = false;
            SelectUI_Audio.mute = false;
        }

        if (BGM_OnOff == 1)
        {
            BGM_AudioSrc.mute = true;
        }
        else
        {
            BGM_AudioSrc.mute = false;
        }

        #region Volume
        // 저장된 배경음 볼륨값이 없다면
        if (PlayerPrefs.HasKey("BGM_Vol") == false)
        {
            // 볼륨 50% 기본값으로 저장
            PlayerPrefs.SetFloat("BGM_Vol", 0.5f);
        }
        else
        {
            // 이미 저장된게 있으면 그 값으로
            BGM_AudioSrc.volume = PlayerPrefs.GetFloat("BGM_Vol");
        }

        // 저장된 효과음 볼륨값이 없다면
        if (PlayerPrefs.HasKey("SFX_Vol") == false)
        {
            // 볼륨 50% 기본값으로 저장
            PlayerPrefs.SetFloat("SFX_Vol", 0.5f);
        }
        else
        {
            // 이미 저장된게 있으면 그 값으로
            VoiceAudioSrc.volume = PlayerPrefs.GetFloat("SFX_Vol");
            SelectUI_Audio.volume = PlayerPrefs.GetFloat("SFX_Vol");
        }
        #endregion

        // apk용으로 확실히 저장
        PlayerPrefs.Save();
    }
    #endregion

    #region PlayBGM
    // BGM 재생
    public void PlayBGM(AudioClip _audioClip)
    {
        // 찾지못했으면 return
        if (BGM_AudioSrc == null)
            return;

        //  AudioSrc가 null이 아니고 AudioSrc의 클립 이름 파일이름과 같다면 이미 있는거니까 return
        if (BGM_AudioSrc.clip != null && BGM_AudioSrc.clip.name == _audioClip.name)
            return;

        float vol = PlayerPrefs.GetFloat("BGM_Vol");

        // 여기까지 왔다면 AudioSource 설정 후 플레이
        BGM_AudioSrc.clip = _audioClip;
        BGM_AudioSrc.volume = vol;
        m_bgmVolume = vol;
        BGM_AudioSrc.Play();
    }
    #endregion

    #region PlayUISound
    // UI 효과음 재생 함수
    public void PlayUISound()
    {
        // 사운드 재생 꺼져있으면 return
        if (isSFX_On == false)
            return;

        // AudioSource가 null이면 return;
        if (SelectUI_Audio == null)
            return;

        // 중복 재생 방지
        if (SelectUI_Audio.isPlaying)
            return;

        // AudioSource 한번 플레이
        SelectUI_Audio.PlayOneShot(SelectUI_Audio.clip, PlayerPrefs.GetFloat("SFX_Vol") * SoundVol);
    }

    public void PlayGachaSound(AudioClip _audioClip)
    {
        // 사운드 재생 꺼져있으면 return
        if (isSFX_On == false)
            return;

        // AudioSource가 null이면 return;
        if (Gacha_AudioSrc == null)
            return;

        // 중복 재생 방지
        if (Gacha_AudioSrc.isPlaying)
        {
            // 멈추고 새로운 사운드 재생
            Gacha_AudioSrc.Stop();
        }

        // AudioSource 한번 플레이
        Gacha_AudioSrc.PlayOneShot(_audioClip, PlayerPrefs.GetFloat("SFX_Vol") * SoundVol);
    }
    #endregion

    #region Skill_SFX
    // 이펙트 사운드 재생 함수
    public void PlayEffSound(string _filePath, float _vol = 1.0f)
    {
        // 사운드 재생 꺼져있으면 return
        if (isSFX_On == false)
            return;

        AudioClip audioClip = null;

        // Dictionary에 파일 이름이 포함 되어 있다면
        if (AudioClip_Dict.ContainsKey(_filePath) == true)
        {
            // 오디오 클립은 Dictionary에서 찾은 클립으로 설정
            audioClip = AudioClip_Dict[_filePath];
        }
        else
        {
            // Dictionary에 없으면 폴더에서 찾은 후 Dictionary에 저장
            audioClip = Resources.Load(_filePath) as AudioClip;
            AudioClip_Dict.Add(_filePath, audioClip);
        }

        // audioClip이 null이면 찾지 못했으면 return
        if (audioClip == null)
            return;

        // AudioSource의 SoundCount인덱스가 null이 아니면
        if (AudioSource_List[SoundCount] != null)
        {
            // 볼륨을 1로해주고 한번 재생
            AudioSource_List[SoundCount].volume = 1.0f;
            AudioSource_List[SoundCount].PlayOneShot(audioClip, _vol * SoundVol);
            // EffVolume[SoundCount]의 볼륨은 매개변수로 받은 _vol로 설정
            EffVolume[SoundCount] = _vol;
            // SoundCount증가
            SoundCount++;

            // EffSoundCount가 SoundCount보다 작으면 SoundCount다시 0으로
            if (EffSoundCount <= SoundCount)
                SoundCount = 0;
        }
    }
    #endregion

    #region Char_Voice

    public void PlaySelectVoice(string _filePath, float _vol = 0.5f)
    {
        // 사운드 재생 꺼져있으면 return
        if (isSFX_On == false)
            return;

        AudioClip audioClip = null;

        // Dictionary에 파일 이름이 포함 되어 있다면
        if (AudioClip_Dict.ContainsKey(_filePath) == true)
        {
            // 오디오 클립은 Dictionary에서 찾은 클립으로 설정
            audioClip = AudioClip_Dict[_filePath];
        }
        else
        {
            // Dictionary에 없으면 폴더에서 찾은 후 Dictionary에 저장
            audioClip = Resources.Load(_filePath) as AudioClip;
            AudioClip_Dict.Add(_filePath, audioClip);
        }

        // audioClip이 null이면 찾지 못했으면 return
        if (audioClip == null)
            return;

        // 이미 재생 중인 사운드면 return
        if (VoiceAudioSrc.clip != null && audioClip.name == VoiceAudioSrc.clip.name && VoiceAudioSrc.isPlaying)
            return;

        if (VoiceAudioSrc.isPlaying)
        {
            // 현재 진행 중인 보이스 멈추고 새 보이스로 출력
            VoiceAudioSrc.Stop();
            VoiceAudioSrc.clip = audioClip;
            VoiceAudioSrc.Play();
        }
        else
        {
            VoiceAudioSrc.clip = audioClip;
            VoiceAudioSrc.Play();
        }
    }
    #endregion

    #region LoadChild_AudioSrc
    void LoadChildGameObj()
    {
        AudioSrc = gameObject.AddComponent<AudioSource>();

        // 게임 효과음 플레이를 위한 5개의 레이어 생성 코드
        for (int i = 0; i < EffSoundCount; i++)
        {
            GameObject newSndObj = new GameObject();
            newSndObj.transform.SetParent(transform);
            newSndObj.transform.localPosition = Vector3.zero;
            AudioSource a_AudioSrc = newSndObj.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            a_AudioSrc.loop = false;
            newSndObj.name = "SoundEffObj";

            AudioSource_List[i] = a_AudioSrc;
            SoundObj_List[i] = newSndObj;
        }
    }
    #endregion
}
