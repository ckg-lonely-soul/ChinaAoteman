using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnce : MonoBehaviour
{
    //
    public bool destroy = true;

    public void PlayEnd()
    {
        if (destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
