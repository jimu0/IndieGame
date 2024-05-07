using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class 过场动画_视频 : MonoBehaviour
{
    public VideoPlayer 过场动画视频播放器;
    public GameObject 过场动画显示物体;

    public void 暂停或播放()
    {
        if (过场动画视频播放器.isPaused)
        {
            过场动画视频播放器.Play();
        } else
        {
            过场动画视频播放器.Pause();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        过场动画视频播放器.loopPointReached += 播放结束;
    }

    public void 播放结束(VideoPlayer 播放器)
    {
        过场动画显示物体.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
