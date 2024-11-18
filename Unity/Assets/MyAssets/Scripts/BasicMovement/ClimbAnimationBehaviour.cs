using System;
using UnityEngine;

public class ClimbAnimationBehaviour : StateMachineBehaviour
{
    public event Action endClimbing;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Climb", false);
        endClimbing?.Invoke();
    }
}
