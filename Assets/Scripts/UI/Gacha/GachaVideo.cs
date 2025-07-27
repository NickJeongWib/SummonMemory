using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GachaVideo : MonoBehaviour
{
    // ĳ���� �̱� ���� UI ������Ʈ
    [SerializeField] GameObject GachaVideo_Renderer;
    // ��� �̱� ���� UI ������Ʈ
    [SerializeField] GameObject EquipGachaVideo_Renderer;
    [SerializeField] GameObject Skip_Info;

    VideoPlayer videoPlayer;
    public VideoPlayer Get_VideoPlayer { get => videoPlayer;}

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // ���� ������ �� ȣ��Ǵ� �Լ�
    void OnVideoEnd(VideoPlayer vp)
    {
        // ĳ���ͻ̱� ui������ �����ִٸ�
        if(GachaVideo_Renderer.activeSelf)
        {
           GachaVideo_Renderer.SetActive(false);
        }
        // ���̱� ui������ �����ִٸ�
        if (EquipGachaVideo_Renderer.activeSelf)
        {
            EquipGachaVideo_Renderer.SetActive(false);
        }

        Skip_Info.SetActive(false);
    }
}
