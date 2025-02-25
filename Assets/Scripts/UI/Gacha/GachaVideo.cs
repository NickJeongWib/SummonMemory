using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GachaVideo : MonoBehaviour
{
    [SerializeField] GameObject GachaVideo_Renderer;

    VideoPlayer videoPlayer;


    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        GachaVideo_Renderer.SetActive(false);
    }
}
