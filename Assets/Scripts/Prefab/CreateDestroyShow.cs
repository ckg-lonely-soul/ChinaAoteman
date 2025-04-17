using UnityEngine;

public class CreateDestroyShow : MonoBehaviour
{
    public float showTime = 0.5f;

    Animator animator;

    float runTime;
    // Use this for initialization
    void Start()
    {
        runTime = 0;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (runTime < showTime)
        {
            runTime += Time.deltaTime;
            if (runTime >= showTime)
            {
                if (animator != null)
                {
                    animator.Play("Destroy");
                }
            }
        }
        else
        {
            if (animator == null)
            {
                Destroy(gameObject);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
