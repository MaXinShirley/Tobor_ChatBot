using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    public Action<string> AnimationFinished; 

    public void OnAnimationFinished(string AnimationName)
    {
        AnimationFinished?.Invoke(AnimationName);
    }
}
