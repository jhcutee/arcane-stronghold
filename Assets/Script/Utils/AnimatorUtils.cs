using UnityEngine;

public static class AnimatorUtils
{
    public static float GetCurrentAnimationLength(Animator animator)
    {
        if (animator == null || animator.GetCurrentAnimatorClipInfo(0).Length == 0)
            return 0f;

        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }
}