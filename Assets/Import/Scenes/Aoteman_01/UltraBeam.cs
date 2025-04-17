using System.Collections;
using UnityEngine;
[ExecuteInEditMode]
public class UltraBeam : MonoBehaviour
{
    [Header("核心组件")]
    public LineRenderer beamLine;          // 光线主体
    public Light beamLight;                // 动态光源
    public ParticleSystem chargeParticles; // 蓄能粒子
    public ParticleSystem hitExplosion;    // 命中特效

    [Header("参数配置")]
    public float beamMaxLength = 50f;      // 光线射程
    public float beamDuration = 1.5f;      // 持续时间
    public float lightIntensity = 8f;      // 光源强度

    private bool isBeaming = false;

    void Start()
    {
        beamLine.enabled = true;
        beamLight.intensity = lightIntensity;
    }

    //=== 外部调用接口 ===
    public void StartBeam()
    {
        if (!isBeaming)
        {
            StartCoroutine(BeamProcess());
        }
    }

    IEnumerator BeamProcess()
    {
        isBeaming = true;

        // 蓄能阶段
        chargeParticles.Play();
        float chargeTime = 0.5f;
        while (chargeTime > 0)
        {
            beamLight.intensity = Mathf.Lerp(0, lightIntensity, 1 - chargeTime / 0.5f);
            chargeTime -= Time.deltaTime;
            yield return null;
        }

        // 发射阶段
        beamLine.enabled = true;
        beamLine.SetPosition(0, transform.position);

        float beamTimer = beamDuration;
        while (beamTimer > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, beamMaxLength))
            {
                beamLine.SetPosition(1, hit.point);
                if (!hitExplosion.isPlaying)
                {
                    hitExplosion.transform.position = hit.point;
                    hitExplosion.Play();
                }
            }
            else
            {
                beamLine.SetPosition(1, transform.position + transform.forward * beamMaxLength);
            }

            beamTimer -= Time.deltaTime;
            yield return null;
        }

        // 收束阶段
        beamLine.enabled = false;
        chargeParticles.Stop();
        while (beamLight.intensity > 0)
        {
            beamLight.intensity = Mathf.Max(0, beamLight.intensity - Time.deltaTime * 15);
            yield return null;
        }

        isBeaming = false;
    }
}


