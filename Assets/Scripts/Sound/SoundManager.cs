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

    // ȿ���� ����ȭ�� ���� ���� ����
    // 5���� ���̾�� �÷���
    int EffSoundCount = 5;
    // 5������ ����ǰ� ����
    int SoundCount = 0;
    
    GameObject[] SoundObj_List = new GameObject[10];
    AudioSource[] AudioSource_List = new AudioSource[10];
    float[] EffVolume = new float[10];

    [SerializeField] AudioSource VoiceAudioSrc;

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

    // Start is called before the first frame update
    void Start()
    {
        LoadChildGameObj();

        // ���� ���ҽ� �̸� �ε�
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

        // ���� ȿ���� �÷��̸� ���� 5���� ���̾� ���� �ڵ�
        for (int i = 0; i < EffSoundCount; i++)
        {
            // ���� ������Ʈ ����
            GameObject newSndObj = new GameObject();
            // ���� ������Ʈ�� SoundManager�� �ڽ� ������Ʈ�� ����
            newSndObj.transform.SetParent(transform);
            // ��ġ�� Vector3.zero;
            newSndObj.transform.localPosition = Vector3.zero;
            // newSndObj�� AudioSource �ڵ�� ���� �� �ʱ� �� �ʱ�ȭ
            AudioSource a_AudioSrc = newSndObj.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            a_AudioSrc.loop = false;
            newSndObj.name = "SoundEffObj";

            AudioSource_List[i] = a_AudioSrc;
            SoundObj_List[i] = newSndObj;
        }

        // ���� ���۵Ǹ� ���� OnOff, ���� ���� ���� �ε� �� ����
        int a_SoundOnOff = PlayerPrefs.GetInt("SoundOnOff", 1);
        if (a_SoundOnOff == 1)
            SoundOnOff(true);
        else
            SoundOnOff(false);

        float a_Value = PlayerPrefs.GetFloat("SoundVolume", 1.0f);
        // apk������ Save
        PlayerPrefs.Save();
        SoundVolume(a_Value);
    }
    
    // BGM ���
    public void PlayBGM(string _filePath, float _vol = 0.2f)
    {
        // Ŭ���� ���� �������� ����
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

        // ã���������� return
        if (AudioSrc == null)
            return;

        //  AudioSrc�� null�� �ƴϰ� AudioSrc�� Ŭ�� �̸� �����̸��� ���ٸ� �̹� �ִ°Ŵϱ� return
        if (AudioSrc.clip != null && AudioSrc.clip.name == _filePath)
            return;

        // ������� �Դٸ� AudioSource ���� �� �÷���
        AudioSrc.clip = audioClip;
        AudioSrc.volume = _vol * SoundVol;
        m_bgmVolume = _vol;
        AudioSrc.loop = true;
        AudioSrc.Play();
    }
    
    // UI ȿ���� ��� �Լ�
    public void PlayUISound(string _filePath, float _vol = 0.2f)
    {
        // ���� ��� ���������� return
        if (isSoundOn == false)
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

        // AudioSource�� null�̸� return;
        if (AudioSrc == null)
            return;

        // AudioSource �ѹ� �÷���
        AudioSrc.PlayOneShot(audioClip, _vol * SoundVol);
    }

    // ����Ʈ ���� ��� �Լ�
    public void PlayEffSound(string _filePath, float _vol = 0.2f)
    {
        // ���� ��� ���������� return
        if (isSoundOn == false)
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

    public void PlaySelectVoice(string _filePath, float _vol = 0.2f)
    {
        // ���� ��� ���������� return
        if (isSoundOn == false)
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

        if(VoiceAudioSrc.isPlaying)
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

    // ���� On Off üũ
    public void SoundOnOff(bool _isOn = true)
    {
        // ���� ȿ�� On/Off
        bool isMute = !_isOn;

        // AudioSrc�� null�� �ƴϸ�
        if (AudioSrc != null)
        {
            //mute == true ���� mute == false �ѱ�
            AudioSrc.mute = isMute;
        }

        // EffSoundCount��ŭ �ݺ�
        for (int i = 0; i < EffSoundCount; i++)
        {
            // i��° AudioSource�� null�� �ƴϸ�
            if (AudioSource_List[i] != null)
            {
                // ������ isMute�� ����
                AudioSource_List[i].mute = isMute;

                // ���� isMute�� false��
                if (isMute == false)
                {
                    //ó������ �ٽ� �÷���
                    AudioSource_List[i].time = 0;
                }

            }
        }

        // �ʱ�ȭ
        isSoundOn = _isOn;
    }

    // ���� ���� ����
    public void SoundVolume(float _vol)
    {
        // AudioSource�� ���� ����
        if (AudioSrc != null)
            AudioSrc.volume = m_bgmVolume * _vol;

        SoundVol = _vol;
    }
}
