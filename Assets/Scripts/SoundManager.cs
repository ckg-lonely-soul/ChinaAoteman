using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip[] levelBgm;//普通背景音乐
    public AudioClip[] gunGodBgmCN;//枪神专版背景音乐，加儿歌
    public AudioClip[] gunGodBgmEN;
    void Awake()
    {
        Instance = this;
    }
}
