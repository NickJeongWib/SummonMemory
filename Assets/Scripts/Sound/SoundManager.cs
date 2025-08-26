using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [HideInInspector] public AudioSource AudioSrc = null;
    Dictionary<string, AudioClip> AudioClip_Dict = new Dictionary<string, AudioClip>();

    float m_bgmVolume = 0.2f;
    [HideInInspector] public bool isSoundOn = true;
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
        LoadChildGameObj();

        // 사운드 리소스 미리 로딩
        AudioClip audioClip = null;
        object[] temp = Resources.LoadAll("Sounds");
        for (int i = 0; i < temp.Length; i++)
        {
            audioClip = temp[i] as AudioClip;

            if (AudioClip_Dict.ContainsKey(audioClip.name) == true)
                continue;

            AudioClip_Dict.Add(audioClip.name, audioClip);
        }
    }

    void LoadChildGameObj()
    {
        AudioSrc = gameObject.AddComponent<AudioSource>();

        // 게임 효과음 플레이를 위한 5개의 레이어 생성 코드
        for (int i = 0; i < EffSoundCount; i++)
        {
            // 게임 오브젝트 생성
            GameObject newSndObj = new GameObject();
            // 만든 오브젝트를 SoundManager의 자식 오브젝트로 생성
            newSndObj.transform.SetParent(transform);
            // 위치는 Vector3.zero;
            newSndObj.transform.localPosition = Vector3.zero;
            // newSndObj에 AudioSource 코드로 생성 후 초기 값 초기화
            AudioSource a_AudioSrc = newSndObj.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            a_AudioSrc.loop = false;
            newSndObj.name = "SoundEffObj";

            AudioSource_List[i] = a_AudioSrc;
            SoundObj_List[i] = newSndObj;
        }

        // 게임 시작되면 사운드 OnOff, 사운드 볼륨 로컬 로딩 후 적용
        int a_SoundOnOff = PlayerPrefs.GetInt("SoundOnOff", 1);
        if (a_SoundOnOff == 1)
            SoundOnOff(true);
        else
            SoundOnOff(false);

        float a_Value = PlayerPrefs.GetFloat("SoundVolume", 1.0f);
        // apk용으로 Save
        PlayerPrefs.Save();
        SoundVolume(a_Value);
    }
    #endregion

    #region PlayBGN
    // BGM 재생
    public void PlayBGM(string _filePath, float _vol = 0.2f)
    {
        // 클립을 받을 지역변수 선언
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

        // 찾지못했으면 return
        if (AudioSrc == null)
            return;

        //  AudioSrc가 null이 아니고 AudioSrc의 클립 이름 파일이름과 같다면 이미 있는거니까 return
        if (AudioSrc.clip != null && AudioSrc.clip.name == _filePath)
            return;

        // 여기까지 왔다면 AudioSource 설정 후 플레이
        AudioSrc.clip = audioClip;
        AudioSrc.volume = _vol * SoundVol;
        m_bgmVolume = _vol;
        AudioSrc.loop = true;
        AudioSrc.Play();
    }
    #endregion

    #region PlayUISound
    // UI 효과음 재생 함수
    public void PlayUISound(float _vol = 0.5f)
    {
        // 사운드 재생 꺼져있으면 return
        if (isSoundOn == false)
            return;

        // AudioSource가 null이면 return;
        if (SelectUI_Audio == null)
            return;

        // 중복 재생 방지
        if (SelectUI_Audio.isPlaying)
            return;

        // AudioSource 한번 플레이
        SelectUI_Audio.PlayOneShot(SelectUI_Audio.clip, _vol * SoundVol);
    }
    #endregion

    #region Skill_SFX
    // 이펙트 사운드 재생 함수
    public void PlayEffSound(string _filePath, float _vol = 1.0f)
    {
        // 사운드 재생 꺼져있으면 return
        if (isSoundOn == false)
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
        if (isSoundOn == false)
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
        if (VoiceAudioSrc.clip != null && audioClip.name == VoiceAudioSrc.clip.name)
            return;

        if(VoiceAudioSrc.isPlaying)
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

    // 사운드 On Off 체크
    public void SoundOnOff(bool _isOn = true)
    {
        // 사운드 효과 On/Off
        bool isMute = !_isOn;

        // AudioSrc가 null이 아니면
        if (AudioSrc != null)
        {
            //mute == true 끄기 mute == false 켜기
            AudioSrc.mute = isMute;
        }

        // EffSoundCount만큼 반복
        for (int i = 0; i < EffSoundCount; i++)
        {
            // i번째 AudioSource가 null이 아니면
            if (AudioSource_List[i] != null)
            {
                // 설정된 isMute로 변경
                AudioSource_List[i].mute = isMute;

                // 만약 isMute가 false면
                if (isMute == false)
                {
                    //처음부터 다시 플레이
                    AudioSource_List[i].time = 0;
                }

            }
        }

        // 초기화
        isSoundOn = _isOn;
    }

    // 사운드 볼륨 조절
    public void SoundVolume(float _vol)
    {
        // AudioSource의 볼륨 조절
        if (AudioSrc != null)
            AudioSrc.volume = m_bgmVolume * _vol;

        SoundVol = _vol;
    }
}
