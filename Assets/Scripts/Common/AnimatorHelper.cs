using UnityEngine;
public delegate void AnimatorAction();
public static class AnimatorHelper
{
    private static event AnimatorAction Action;
    public static void AnimationIsEndPlaying(this Animator anima, string name, AnimatorAction action, float value = 0.99f)
    {
        Action = action;
        if (Action != null && AnimationIsEndPlaying(anima, name, value))
        {
            Action();
        }
    }
    public static bool AnimationIsEndPlaying(this Animator anima, string name, float value = 0.99f)
    {
        AnimatorStateInfo info = anima.GetCurrentAnimatorStateInfo(0);
        Debug.Log(info.normalizedTime);
        if (info.normalizedTime >= value && info.IsName(name))
            return true;
        return false;
    }
}
