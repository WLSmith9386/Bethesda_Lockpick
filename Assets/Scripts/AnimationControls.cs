using UnityEngine;
using System.Collections;

public class AnimationControls : MonoBehaviour {

    public Animator anim;

    void Start()
    {
        if (anim == null)
        {
            anim = gameObject.GetComponent<Animator>();
        }
    }

    public void StopAnimation()
    {
        anim.enabled = false;
    }

    public void PlayAnimation(string AnimationName)
    {
        anim.Play(AnimationName);
        anim.enabled = true;
    }
}
