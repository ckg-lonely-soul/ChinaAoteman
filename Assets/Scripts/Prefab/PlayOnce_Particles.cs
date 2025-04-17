using UnityEngine;

public class PlayOnce_Particles : MonoBehaviour
{
    public bool destroy = true;
    ParticleSystem[] particleSystems;

    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particleSystems != null)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                if (particleSystems[i].isPlaying)
                {
                    return;
                }
            }
        }
        if (destroy)
        {
            Destroy(gameObject);
        }

    }
}
