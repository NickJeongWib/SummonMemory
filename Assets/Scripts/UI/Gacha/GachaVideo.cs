using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GachaVideo : MonoBehaviour
{
    // 캐릭터 뽑기 연출 UI 오브젝트
    [SerializeField] GameObject GachaVideo_Renderer;
    // 장비 뽑기 연출 UI 오브젝트
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

    // 비디오 끝났을 떄 호출되는 함수
    void OnVideoEnd(VideoPlayer vp)
    {
        // 캐릭터뽑기 ui영상이 켜져있다면
        if(GachaVideo_Renderer.activeSelf)
        {
           GachaVideo_Renderer.SetActive(false);
        }
        // 장비뽑기 ui영상이 켜져있다면
        if (EquipGachaVideo_Renderer.activeSelf)
        {
            EquipGachaVideo_Renderer.SetActive(false);
        }

        Skip_Info.SetActive(false);
    }
}
