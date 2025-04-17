using RenderHeads.Media.AVProVideo;
using System.IO;
using UnityEngine;

public class Game_MediaPlayer : MonoBehaviour
{

    public MediaPlayer _mediaPlayer;
    int curentIndex;
    string currentPath;
    float nextDcTime;

    readonly string[] tab_Language = { "CN", "EN" };
    readonly int[] tab_MaxDefaultVideo = { 19, 20 };
    static int videoId = 0;
    static int defVideoId = 0;


    void Start()
    {
        _mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
    }

    void OnEnable()
    {
        Application.targetFrameRate = 18;
        Update_Volume();

        defVideoId = 0;
        _mediaPlayer.Stop();
    }

    // Callback function to handle events
    public void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.ReadyToPlay:
                break;
            case MediaPlayerEvent.EventType.Started:
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                break;
            case MediaPlayerEvent.EventType.MetaDataReady:
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                if (_mediaPlayer.Control != null)
                {
                    _mediaPlayer.Control.Stop();
                }
                break;
        }
    }

    void Update()
    {
        if (volumeInt != VolumeControl.sysVolume)
        {
            volumeInt = VolumeControl.sysVolume;
            volume = (float)volumeInt / VolumeControl.SET_c_MaxSysVolume;
        }
        if (_mediaPlayer.Control != null)
        {
            if (_mediaPlayer.Control.GetVolume() != volume)
            {
                Update_Volume();
            }
        }

        if (_mediaPlayer.Control != null)
        {
            if (_mediaPlayer.Control.IsPlaying())
            {
                if (_mediaPlayer.Control.GetCurrentTimeMs() + 0.2f >= _mediaPlayer.Info.GetDurationMs())
                {
                    _mediaPlayer.Control.Stop();
                }
            }
            else if (nextDcTime > 0)
            {
                nextDcTime -= Time.deltaTime;
            }
            else
            {
                PlayNextOne();
                nextDcTime = 1.5f;
            }
        }
    }

    public void PlayNextOne()
    {
        _mediaPlayer.Stop();

        // 自定义视频
        for (int i = 0; i < Main.MAX_USER_VIDEO; i++)
        {
            videoId = (videoId + 1) % Main.MAX_USER_VIDEO;
            string targetpath = Application.persistentDataPath + "/" + tab_Language[Set.setVal.Language] + (videoId + 1).ToString("D4") + ".mp4";
            if (File.Exists(targetpath) == false)
                continue;
            _mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToPeristentDataFolder, targetpath, true);
            Update_Volume();
            return;
        }

        // 默认视频
        int maxVideo = tab_MaxDefaultVideo[Set.setVal.Language];
        defVideoId = (defVideoId + 1) % maxVideo;
        if (defVideoId == 0)
        {
            defVideoId = 1;
        }
        if (Main.isFirstPlayVideo)
        {
            defVideoId = Random.Range(1, maxVideo + 1);
            Main.isFirstPlayVideo = false;
        }
        _mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, "Game_" + tab_Language[Set.setVal.Language] + "/" + defVideoId.ToString("D4") + ".mp4", true);
        Update_Volume();
    }
    // 音量
    float volume = -1;
    int volumeInt = -1;
    void Update_Volume()
    {
        volumeInt = VolumeControl.sysVolume;
        if (_mediaPlayer.Control != null)
        {
            volume = (float)volumeInt / VolumeControl.SET_c_MaxSysVolume;
            _mediaPlayer.Control.SetVolume(Mathf.Clamp(volume, 0, 1));
        }
    }
}
