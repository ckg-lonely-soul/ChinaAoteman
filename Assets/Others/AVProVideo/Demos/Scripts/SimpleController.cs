﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//-----------------------------------------------------------------------------
// Copyright 2015-2017 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------


namespace RenderHeads.Media.AVProVideo.Demos
{
    /// <summary>
    /// Simple GUI built using IMGUI to show scripting examples
    /// </summary>
    public class SimpleController : MonoBehaviour
    {
        public string _folder = "AVProVideoSamples/";
        public string[] _filenames;
        public string[] _streams;
        public MediaPlayer _mediaPlayer;
        public DisplayIMGUI _display;
        public GUISkin _guiSkin;

        private int _width;
        private int _height;
        private float _durationSeconds;
        public bool _useFading = true;
        private Queue<string> _eventLog = new Queue<string>(8);
        private float _eventTimer = 1f;
        private MediaPlayer.FileLocation _nextVideoLocation;
        private string _nextVideoPath;
        //private bool _seekDragStarted;
        //private bool _seekDragWasPlaying;
        int i = 0;
        float nextDcTime;

        void Start()
        {
            _mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
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
                    GatherProperties();
                    break;
                case MediaPlayerEvent.EventType.FinishedPlaying:
                    //PlayNextOne();
                    _mediaPlayer.Control.Stop();
                    //if (playerEvent.isSet)
                    //{
                    //    playerEvent.PlayerVideo();
                    //}
                    //else
                    //{

                    //    if(_filenames[i] == "0000.mp4")
                    //    {
                    //        i = 1;
                    //        LoadVideo(this._filenames[i]);
                    //    }
                    //   else
                    //    {
                    //        LoadVideo(this._filenames[0]);
                    //    }
                    //}
                    break;
            }

            AddEvent(et);
        }

        private void AddEvent(MediaPlayerEvent.EventType et)
        {
            //Debug.Log("[SimpleController] Event: " + et.ToString());
            _eventLog.Enqueue(et.ToString());
            if (_eventLog.Count > 5)
            {
                _eventLog.Dequeue();
                _eventTimer = 1f;
            }
        }

        private void GatherProperties()
        {
            if (_mediaPlayer != null && _mediaPlayer.Info != null)
            {
                _width = _mediaPlayer.Info.GetVideoWidth();
                _height = _mediaPlayer.Info.GetVideoHeight();
                _durationSeconds = _mediaPlayer.Info.GetDurationMs() / 1000f;
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

            if (!_useFading)
            {
                if (_display != null && _display._mediaPlayer != null && _display._mediaPlayer.Control != null)
                {
                    _display._color = Color.white;
                    //_display._mediaPlayer.Control.SetVolume(1f);
                    //if (volume != VolumeCotroy.sysVolume) {
                    //    volume = VolumeCotroy.sysVolume;
                    //    _display._mediaPlayer.Control.SetVolume(Mathf.Clamp((float)volume / VolumeCotroy.SET_c_MaxSysVolume, 0, 1));
                    //}
                }
            }

            if (_eventLog != null && _eventLog.Count > 0)
            {
                _eventTimer -= Time.deltaTime;
                if (_eventTimer < 0f)
                {
                    _eventLog.Dequeue();
                    _eventTimer = 1f;
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
                    nextDcTime = 0.5f;
                }
            }
        }

        static int videoId = 0;
        public void PlayNextOne()
        {
            if (_mediaPlayer.Control != null)
            {
                _mediaPlayer.Control.Stop();
            }

            // 自定义视频
            for (int i = 0; i < Main.MAX_USER_VIDEO; i++)
            {
                videoId = (videoId + 1) % Main.MAX_USER_VIDEO;
                string targetpath = Application.persistentDataPath + "/" + (videoId + 1).ToString("D4") + ".mp4";
                if (File.Exists(targetpath) == false)
                    continue;
                _mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToPeristentDataFolder, targetpath, true);
                return;
            }

            // 默认视频
            _mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, "0000.mp4", true);
            volume = -1;
            //Update_Volume();
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

        //public void LoadVideo(string filePath, bool url = false) {
        //    // Set the video file name and to load. 
        //    if (!url) {
        //        if (playerEvent.isSet) {
        //            _nextVideoLocation = MediaPlayer.FileLocation.RelativeToPeristentDataFolder;
        //        } else {
        //            _nextVideoLocation = MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder;
        //        }
        //    } else
        //        _nextVideoLocation = MediaPlayer.FileLocation.AbsolutePathOrURL;
        //    _nextVideoPath = filePath;

        //    // IF we're not using fading then load the video immediately
        //    if (!_useFading) {
        //        // Load the video
        //        if (!_mediaPlayer.OpenVideoFromFile(_nextVideoLocation, _nextVideoPath, _mediaPlayer.m_AutoStart)) {
        //            Debug.LogError("Failed to open video!");
        //        }
        //    } else {
        //        StartCoroutine("LoadVideoWithFading");
        //    }
        //}

        private static bool VideoIsReady(MediaPlayer mp)
        {
            return (mp != null && mp.TextureProducer != null && mp.TextureProducer.GetTextureFrameCount() <= 0);

        }
        private static bool AudioIsReady(MediaPlayer mp)
        {
            return (mp != null && mp.Control != null && mp.Control.CanPlay() && mp.Info.HasAudio() && !mp.Info.HasVideo());
        }

        private IEnumerator LoadVideoWithFading()
        {
            const float FadeDuration = 0.25f;
            float fade = FadeDuration;

            // Fade down
            while (fade > 0f && Application.isPlaying)
            {
                fade -= Time.deltaTime;
                fade = Mathf.Clamp(fade, 0f, FadeDuration);

                _display._color = new Color(1f, 1f, 1f, fade / FadeDuration);
                _display._mediaPlayer.Control.SetVolume(fade / FadeDuration);

                yield return null;
            }

            // Wait 3 frames for display object to update
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            // Load the video
            if (Application.isPlaying)
            {
                if (!_mediaPlayer.OpenVideoFromFile(_nextVideoLocation, _nextVideoPath, _mediaPlayer.m_AutoStart))
                {
                    Debug.LogError("Failed to open video!");
                }
                else
                {
                    // Wait for the first frame to come through (could also use events for this)
                    while (Application.isPlaying && (VideoIsReady(_mediaPlayer) || AudioIsReady(_mediaPlayer)))
                    {
                        yield return null;
                    }

                    // Wait 3 frames for display object to update
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                }
            }

            // Fade up
            while (fade < FadeDuration && Application.isPlaying)
            {
                fade += Time.deltaTime;
                fade = Mathf.Clamp(fade, 0f, FadeDuration);

                _display._color = new Color(1f, 1f, 1f, fade / FadeDuration);
                _display._mediaPlayer.Control.SetVolume(fade / FadeDuration);

                yield return null;
            }
        }

        void OnGUI()
        {
            if (_mediaPlayer == null)
            {
                return;
            }

            // Make sure we're set to render after the background IMGUI
            GUI.depth = -10;

            // Apply skin
            if (_guiSkin != null)
            {
                GUI.skin = _guiSkin;
            }

            // Make sure the UI scales with screen resolution
            const float UIWidth = 1920f / 2.0f;
            const float UIHeight = 1080f / 2.0f;
            //GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / UIWidth, Screen.height / UIHeight, 1f));
            GUI.matrix = Matrix4x4.TRS(new Vector3(-1000, -1000, 100), Quaternion.identity, new Vector3(Screen.width / UIWidth, Screen.height / UIHeight, 1f));

            GUILayout.BeginVertical("box");

            if (_mediaPlayer.Control != null)
            {
                // Display properties
                //GUILayout.Label("Loaded: " + _mediaPlayer.m_VideoPath);
                GUILayout.Label(string.Format("Size: {0}x{1} FPS: {3} Duration: {2}ms", _width, _height, _mediaPlayer.Info.GetDurationMs(), _mediaPlayer.Info.GetVideoFrameRate().ToString("F2")));
                GUILayout.Label("Updates: " + _mediaPlayer.TextureProducer.GetTextureFrameCount() + "    Rate: " + _mediaPlayer.Info.GetVideoDisplayRate().ToString("F1"));

                GUILayout.BeginHorizontal();

                // Fade option
                _useFading = GUILayout.Toggle(_useFading, "Fade to Black During Loading");

                // Auto play
                _mediaPlayer.m_AutoStart = GUILayout.Toggle(_mediaPlayer.m_AutoStart, "Auto Play After Load");

                // Looping
                bool loopStatus = _mediaPlayer.m_Loop;
                bool newLoopStatus = GUILayout.Toggle(loopStatus, "Loop");
                if (newLoopStatus != loopStatus)
                {
                    _mediaPlayer.m_Loop = newLoopStatus;
                    _mediaPlayer.Control.SetLooping(newLoopStatus);
                }

                GUILayout.EndHorizontal();

                // Timeline scrubbing (note as use int as WebGL has float == precision issues)
                int currentTime = (int)_mediaPlayer.Control.GetCurrentTimeMs();
                int newTime = (int)GUILayout.HorizontalSlider(currentTime, 0f, _durationSeconds * 1000f);
                Rect timeSliderRect = GUILayoutUtility.GetLastRect();

                float thumbWidth = GUI.skin.horizontalSliderThumb.CalcSize(GUIContent.none).x;


                // Draw buffering indication
                Rect bufferingRect = timeSliderRect;
                GUI.color = Color.green;
                bufferingRect.xMin += thumbWidth;
                bufferingRect.y = bufferingRect.yMax - 4;
                bufferingRect.width -= thumbWidth * 1f;
                bufferingRect.width *= _mediaPlayer.Control.GetBufferingProgress();
                bufferingRect.height = 4;
                GUI.DrawTexture(bufferingRect, Texture2D.whiteTexture, ScaleMode.StretchToFill);


                GUI.color = Color.green;
                int timeRangeCount = _mediaPlayer.Control.GetBufferedTimeRangeCount();
                for (int i = 0; i < timeRangeCount; i++)
                {
                    float startTimeMs = 0f;
                    float endTimeMs = 0f;
                    if (_mediaPlayer.Control.GetBufferedTimeRange(i, ref startTimeMs, ref endTimeMs))
                    {
                        bufferingRect.xMin = thumbWidth + timeSliderRect.x + (timeSliderRect.width - thumbWidth * 1f) * (startTimeMs / (_durationSeconds * 1000f));
                        bufferingRect.xMax = thumbWidth + timeSliderRect.x + (timeSliderRect.width - thumbWidth * 1f) * (endTimeMs / (_durationSeconds * 1000f));
                        GUI.DrawTexture(bufferingRect, Texture2D.whiteTexture, ScaleMode.StretchToFill);
                    }
                }
                GUI.color = Color.white;


                // Handle possible slider move
                if (newTime != currentTime)
                {
                    _mediaPlayer.Control.Seek((float)newTime);
                }

                if (!_mediaPlayer.Control.IsPlaying())
                {
                    if (GUILayout.Button("Play"))
                    {
                        _mediaPlayer.Control.Play();
                    }
                }
                else
                {
                    if (GUILayout.Button("Pause"))
                    {
                        _mediaPlayer.Control.Pause();
                    }
                }

                // Audio tracks
                GUILayout.BeginHorizontal();
                int numAudioTracks = _mediaPlayer.Info.GetAudioTrackCount();
                int selectedTrackIndex = _mediaPlayer.Control.GetCurrentAudioTrack();
                for (int i = 0; i < numAudioTracks; i++)
                {
                    if (i == selectedTrackIndex)
                    {
                        GUI.color = Color.green;
                    }
                    if (GUILayout.Button("Audio Track #" + (i + 1)))
                    {
                        _mediaPlayer.Control.SetAudioTrack(i);
                    }
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Label("Select a new file to play:");

            // Display a grid of buttons containing the file names of videos to play
            int newSelection = GUILayout.SelectionGrid(-1, _filenames, 3);
            //if (newSelection >= 0) {
            //    LoadVideo(System.IO.Path.Combine(_folder, _filenames[newSelection]));
            //}

            GUILayout.Space(8f);

            GUILayout.Label("Select a new stream to play:");

            // Display a grid of buttons containing the file names of videos to play
            int newSteam = GUILayout.SelectionGrid(-1, _streams, 1);
            //if (newSteam >= 0) {
            //    LoadVideo(_streams[newSteam], true);
            //}

            GUILayout.Space(8f);

            GUILayout.Label("Recent Events: ");
            GUILayout.BeginVertical("box");
            int eventIndex = 0;
            foreach (string eventString in _eventLog)
            {
                GUI.color = Color.white;
                if (eventIndex == 0)
                {
                    GUI.color = new Color(1f, 1f, 1f, _eventTimer);
                }
                GUILayout.Label(eventString);
                eventIndex++;
            }
            GUILayout.EndVertical();
            GUI.color = Color.white;


            GUILayout.EndVertical();
        }

    }
}
