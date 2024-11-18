using System;
using UnityEngine;

public class ClimbToCrawlAnimationBehaviour : StateMachineBehaviour
{
    public event Action endClimbing;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Climb", false);
        endClimbing?.Invoke();
    }
}
