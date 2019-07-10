using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationSettings : MonoBehaviour
{
    [SerializeField]
    float speedMultiplier = 1f;
    [SerializeField]
    string speedMultiplierString = "Multiplier";
    [SerializeField]
    float frameOffset = 0f;
    [SerializeField]
    string frameOffsetString = "Offset";
    Animator anim;
    
    void OnEnable()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat(speedMultiplierString, speedMultiplier);
        anim.SetFloat(frameOffsetString, frameOffset);
    }
}
