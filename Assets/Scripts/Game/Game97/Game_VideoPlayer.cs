using UnityEngine;
using UnityEngine.Video;

public class Game_VideoPlayer : MonoBehaviour
{
    public const int MAX_DEFAULT_VIDEO = 1;        // 默认视频个数
    public const int MAX_VIDEO = 10;
    public const string videoName = "video_";
    public const string videoExName = ".mp4";

    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    int playId;
    int nullVideoNum;
    float runTime;

    //    void Start()
    //    {
    //        Init();
    //    }
    //    // Use this for initialization
    //    public void Init()
    //    {
    //        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
    //        videoPlayer.SetTargetAudioSource(0, audioSource);
    //        videoPlayer.Stop();
    //        audioSource.Stop();

    //        playId = 0;// UnityEngine.Random.Range(0, MAX_VIDEO);
    //        nullVideoNum = 0;
    //    }
    //    void OnEnable()
    //    {
    //        runTime = 0.2f;
    //        nullVideoNum = 0;
    //    }

    //    // Update is called once per frame
    //    void Update()
    //    {
    //        if (runTime > 0)
    //        {
    //            runTime -= Time.deltaTime;
    //            return;
    //        }

    //        runTime = 0.1f;
    //        //Main.Log("isPlaying: " + videoPlayer.isPlaying);
    //        if (videoPlayer.isPlaying)
    //        {
    //            runTime = 0.4f;
    //            return;
    //        }
    //        //Main.Log("A1");
    //        //if (videoPlayer.waitForFirstFrame == false) {
    //        //    return;
    //        //}
    //        //Main.Log("A2");

    //        // 播放外部视频
    //        if (nullVideoNum < MAX_VIDEO)
    //        {
    //            string videoPath = Application.persistentDataPath + "/" + videoName + (playId + 1).ToString() + videoExName;

    //            if (File.Exists(videoPath))
    //            {

    //#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    //                videoPlayer.url = "file://" + videoPath;
    //#elif UNITY_ANDROID
    //                        videoPlayer.url = videoPath;
    //#endif
    //                videoPlayer.Play();
    //                audioSource.Play();

    //                nullVideoNum = 0;
    //                runTime = 10f;
    //                //Main.Log("A3: " + playId);
    //            }
    //            else
    //            {
    //                if (nullVideoNum < MAX_VIDEO)
    //                {
    //                    nullVideoNum++;
    //                }
    //                else
    //                {
    //                    // 报错
    //                }
    //            }
    //            //Main.Log("playId: " + playId);
    //            playId++;
    //            if (playId >= MAX_VIDEO)
    //            {
    //                playId = 0;
    //            }
    //        }
    //        else
    //        {
    //            // 播放默认视频
    //            if (playId >= MAX_DEFAULT_VIDEO)
    //            {
    //                playId = 0;
    //            }
    //            string videoPath = Application.streamingAssetsPath + "/" + videoName + (playId + 1).ToString() + videoExName;

    //            //if (File.Exists(videoPath)) {   // 安卓读 Application.streamingAssetsPath 路径下的文件，永远返回 flase，原因不明
    //            //Main.Log("Def_PlayId: " + playId);
    //#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    //            videoPlayer.url = "file://" + videoPath;
    //#elif UNITY_ANDROID
    //                    videoPlayer.url = videoPath;
    //#endif
    //            try
    //            {
    //                videoPlayer.Play();
    //                audioSource.Play();

    //                nullVideoNum = 0;
    //                runTime = 10f;
    //                //Main.Log("A3: " + playId);
    //            }
    //            catch (Exception e)
    //            {
    //                Main.Log("Play Error: " + playId + e.Message);
    //            }
    //            finally
    //            {
    //                playId++;
    //                if (playId >= MAX_DEFAULT_VIDEO)
    //                {
    //                    playId = 0;
    //                }
    //                //Main.Log("Add: " + playId);
    //            }
    //        }
    //    }
}
