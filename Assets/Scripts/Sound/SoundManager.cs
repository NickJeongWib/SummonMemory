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

    // ȿ���� ����ȭ�� ���� ���� ����
    // 5���� ���̾�� �÷���
    int EffSoundCount = 5;
    // 5������ ����ǰ� ����
    int SoundCount = 0;
    
    GameObject[] SoundObj_List = new GameObject[10];
    AudioSource[] AudioSource_List = new AudioSource[10];
    float[] EffVolume = new float[10];

    public AudioSource VoiceAudioSrc;
    public AudioSource SelectUI_Audio;
    public AudioSource BGM_AudioSrc;
    public AudioSource Gacha_AudioSrc;

    #region Singleton
    // �̱��� 
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
        // ���� ���۵Ǹ� ȿ���� OnOff, ���� ���� ���� �ε� �� ����
        int SFX_OnOff = PlayerPrefs.GetInt("SFX_OnOff", 0);
        // ���� ���۵Ǹ� ����� OnOff, ���� ���� ���� �ε� �� ����
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
        // ����� ����� �������� ���ٸ�
        if (PlayerPrefs.HasKey("BGM_Vol") == false)
        {
            // ���� 50% �⺻������ ����
            PlayerPrefs.SetFloat("BGM_Vol", 0.5f);
        }
        else
        {
            // �̹� ����Ȱ� ������ �� ������
            BGM_AudioSrc.volume = PlayerPrefs.GetFloat("BGM_Vol");
        }

        // ����� ȿ���� �������� ���ٸ�
        if (PlayerPrefs.HasKey("SFX_Vol") == false)
        {
            // ���� 50% �⺻������ ����
            PlayerPrefs.SetFloat("SFX_Vol", 0.5f);
        }
        else
        {
            // �̹� ����Ȱ� ������ �� ������
            VoiceAudioSrc.volume = PlayerPrefs.GetFloat("SFX_Vol");
            SelectUI_Audio.volume = PlayerPrefs.GetFloat("SFX_Vol");
        }
        #endregion

        // apk������ Ȯ���� ����
        PlayerPrefs.Save();
    }
    #endregion

    #region PlayBGM
    // BGM ���
    public void PlayBGM(AudioClip _audioClip)
    {
        // ã���������� return
        if (BGM_AudioSrc == null)
            return;

        //  AudioSrc�� null�� �ƴϰ� AudioSrc�� Ŭ�� �̸� �����̸��� ���ٸ� �̹� �ִ°Ŵϱ� return
        if (BGM_AudioSrc.clip != null && BGM_AudioSrc.clip.name == _audioClip.name)
            return;

        float vol = PlayerPrefs.GetFloat("BGM_Vol");

        // ������� �Դٸ� AudioSource ���� �� �÷���
        BGM_AudioSrc.clip = _audioClip;
        BGM_AudioSrc.volume = vol;
        m_bgmVolume = vol;
        BGM_AudioSrc.Play();
    }
    #endregion

    #region PlayUISound
    // UI ȿ���� ��� �Լ�
    public void PlayUISound()
    {
        // ���� ��� ���������� return
        if (isSFX_On == false)
            return;

        // AudioSource�� null�̸� return;
        if (SelectUI_Audio == null)
            return;

        // �ߺ� ��� ����
        if (SelectUI_Audio.isPlaying)
            return;

        // AudioSource �ѹ� �÷���
        SelectUI_Audio.PlayOneShot(SelectUI_Audio.clip, PlayerPrefs.GetFloat("SFX_Vol") * SoundVol);
    }

    public void PlayGachaSound(AudioClip _audioClip)
    {
        // ���� ��� ���������� return
        if (isSFX_On == false)
            return;

        // AudioSource�� null�̸� return;
        if (Gacha_AudioSrc == null)
            return;

        // �ߺ� ��� ����
        if (Gacha_AudioSrc.isPlaying)
        {
            // ���߰� ���ο� ���� ���
            Gacha_AudioSrc.Stop();
        }

        // AudioSource �ѹ� �÷���
        Gacha_AudioSrc.PlayOneShot(_audioClip, PlayerPrefs.GetFloat("SFX_Vol") * SoundVol);
    }
    #endregion

    #region Skill_SFX
    // ����Ʈ ���� ��� �Լ�
    public void PlayEffSound(string _filePath, float _vol = 1.0f)
    {
        // ���� ��� ���������� return
        if (isSFX_On == false)
            return;

        AudioClip audioClip = null;

        // Dictionary�� ���� �̸��� ���� �Ǿ� �ִٸ�
        if (AudioClip_Dict.ContainsKey(_filePath) == true)
        {
            // ����� Ŭ���� Dictionary���� ã�� Ŭ������ ����
            audioClip = AudioClip_Dict[_filePath];
        }
        else
        {
            // Dictionary�� ������ �������� ã�� �� Dictionary�� ����
            audioClip = Resources.Load(_filePath) as AudioClip;
            AudioClip_Dict.Add(_filePath, audioClip);
        }

        // audioClip�� null�̸� ã�� �������� return
        if (audioClip == null)
            return;

        // AudioSource�� SoundCount�ε����� null�� �ƴϸ�
        if (AudioSource_List[SoundCount] != null)
        {
            // ������ 1�����ְ� �ѹ� ���
            AudioSource_List[SoundCount].volume = 1.0f;
            AudioSource_List[SoundCount].PlayOneShot(audioClip, _vol * SoundVol);
            // EffVolume[SoundCount]�� ������ �Ű������� ���� _vol�� ����
            EffVolume[SoundCount] = _vol;
            // SoundCount����
            SoundCount++;

            // EffSoundCount�� SoundCount���� ������ SoundCount�ٽ� 0����
            if (EffSoundCount <= SoundCount)
                SoundCount = 0;
        }
    }
    #endregion

    #region Char_Voice

    public void PlaySelectVoice(string _filePath, float _vol = 0.5f)
    {
        // ���� ��� ���������� return
        if (isSFX_On == false)
            return;

        AudioClip audioClip = null;

        // Dictionary�� ���� �̸��� ���� �Ǿ� �ִٸ�
        if (AudioClip_Dict.ContainsKey(_filePath) == true)
        {
            // ����� Ŭ���� Dictionary���� ã�� Ŭ������ ����
            audioClip = AudioClip_Dict[_filePath];
        }
        else
        {
            // Dictionary�� ������ �������� ã�� �� Dictionary�� ����
            audioClip = Resources.Load(_filePath) as AudioClip;
            AudioClip_Dict.Add(_filePath, audioClip);
        }

        // audioClip�� null�̸� ã�� �������� return
        if (audioClip == null)
            return;

        // �̹� ��� ���� ����� return
        if (VoiceAudioSrc.clip != null && audioClip.name == VoiceAudioSrc.clip.name && VoiceAudioSrc.isPlaying)
            return;

        if (VoiceAudioSrc.isPlaying)
        {
            // ���� ���� ���� ���̽� ���߰� �� ���̽��� ���
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

        // ���� ȿ���� �÷��̸� ���� 5���� ���̾� ���� �ڵ�
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
