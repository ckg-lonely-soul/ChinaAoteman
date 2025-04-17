using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Zhcr_AnimEvent : MonoBehaviour {
    public CameraChange cameraChange;
    public AttackTarget[] attackTargets;
    public ParticleSystem[] particleSystems;
    public Zhcr zhcr;
    public Game00_FreshPos bossFresh;
    public Transform spawnBossPos;
    public int gainBloodValueAmount = 10;
    public float gainBloodTime = 3f;
    public Game00_Player[] players;
    public AudioSource source;
    public AudioClip[] clips;

    public void ChangeFirstPersonEvent()
    {
        cameraChange.ChangeCameraPos(CameraPos.FirstPerson);
        for (int i = 0; i < attackTargets.Length; i++)
        {
            attackTargets[i].canFire = true;
        }
    }

    public void ChangeThridPersonEvent()
    {
        cameraChange.ChangeCameraPos(CameraPos.ThridPerson);
        for (int i = 0; i < attackTargets.Length; i++)
        {
            attackTargets[i].canFire = false;
        }
    }

    public void ChangeSpawnEvent()
    {
        cameraChange.ChangeCameraPos(CameraPos.ZhcrSpawn);
        if (bossFresh.currMonster != null && bossFresh.currMonster.isBOSS)
        {
            bossFresh.currMonster.transform.position = spawnBossPos.position;
        }
    }

    public void ChangeBossSpawnEvent()
    {
        cameraChange.ChangeCameraPos(CameraPos.BossSpawn);
    }

    public void FarAttack(int index)
    {
        if (particleSystems != null)
        {
            particleSystems[index].gameObject.SetActive(true);
            particleSystems[index].Play();
        }
    }

    public void FarAttackEnd()
    {
        for (int i = 0;i < particleSystems.Length;i++)
        {
            particleSystems[i].Stop();
            particleSystems[i].gameObject.SetActive(false);
        }

        EndAudio();
    }

    public void PlayAudio(int index)
    {
        source.clip = clips[index];
        source.Play();
    }

    public void EndAudio()
    {
        source.Stop();
    }

    public void SpawnEnd()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
            particleSystems[i].gameObject.SetActive(false);
        }

        cameraChange.ChangeCameraPos(CameraPos.BossSpawn);

        zhcr.ChangeStatue(Zhcr_Sta.Attack);

        if (bossFresh.currMonster != null && bossFresh.currMonster.isBOSS)
        {
            
            bossFresh.currMonster.transform.position = spawnBossPos.position;
            bossFresh.currMonster.GetComponent<Game00_Boss_JiQiRen>()?
                .ChangeStatueToRoar();
        }
    }

    public void DoGainBlood()
    {
        if (players == null)
        {
            Debug.LogError("没有挂载相应player脚本！");
            return;
        }

        StartCoroutine(GainBlood());
    }

    public IEnumerator GainBlood()
    {
        float timer = 0f;
        int hasHeal = 0;
        while (hasHeal < gainBloodValueAmount)
        {
            timer += Time.deltaTime;
            if (gainBloodValueAmount * (timer / gainBloodTime) - hasHeal >= 1)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].GainBlood(1);
                }

                hasHeal++;
            }
            yield return null;
        }
    }
}

