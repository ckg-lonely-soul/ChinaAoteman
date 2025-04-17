using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AnimEvent : MonoBehaviour {

	public int shakePower;
	public int shakeCnt;
    public int strongShakePower;
    public int strongShakeCnt;
	public Game00_Monster monster;

	public ParticleSystem[] particleSystems;

    public AudioClip[] audioClips;
    public AudioSource audioSource;

	public void ShakeCamera()
	{
		monster.gameMain.ShakeStart(shakePower, shakeCnt);
        //脚踏声
        PlayAudio(1);
	}

    public void StrongShakeCamera()
    {
        monster.gameMain.ShakeStart(strongShakePower, strongShakeCnt);
    }

	public void PlayParticle(int index)
	{
        if (particleSystems != null)
        {
            particleSystems[index].gameObject.SetActive(true);
            particleSystems[index].Play();
        }
    }

    public void PlayAudio(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    public void EndAudio()
    {
        audioSource.Stop();
    }

	public void EndParticle()
	{
        if (particleSystems != null)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Stop();
                particleSystems[i].gameObject.SetActive(false);
            }
        }

        EndAudio();

    }
}
