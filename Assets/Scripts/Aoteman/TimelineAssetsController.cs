using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineAssetsController : MonoBehaviour {

    public PlayableDirector director;

    public TimelineAsset[] timelines;

    private void Update()
    {
        if (director.enabled)
        {
            //播完之后停止播放
            if (director.state == PlayState.Playing &&
                director.time >= director.duration)
            {
                director.Stop();
                director.enabled = false;
            }
        }
    }

    public void ChangeTimeline(int index)
    {
        director.playableAsset = timelines[index];
    }

    public void PlayTimeline(int index)
    {
        //没有激活该组件
        if (!director.enabled)
        {
            director.enabled = true;
        }

        ChangeTimeline(index);

        director.Play();
    }
}
