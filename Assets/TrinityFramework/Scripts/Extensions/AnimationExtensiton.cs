using UnityEngine;
using System.Collections;

public static class AnimationExtensiton
{
    public static void PlayForward(this Animation animation)
    {
        if (animation == null)
        {
            Debug.LogError("animation が　nullです");
            return;
        }
        AnimationState state = animation[animation.clip.name];
        state.time = 0f;
        state.speed = 1f;
        animation.Play();
    }
    public static void PlayReverse(this Animation animation)
    {
        if (animation == null)
        {
            Debug.LogError("animation が　nullです");
            return;
        }
        AnimationState state = animation[animation.clip.name];
        state.time = animation.clip.length;
        state.speed = -1f;
        animation.Play();
    }
}
