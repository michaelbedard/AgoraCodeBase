using UnityEngine;

namespace _src.Code.Core.Actors
{
    public class AnimationEndBehaviour : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Call the method to do something when the animation finishes
            animator.GetComponent<AnimatorController>().OnAnimationFinish();
        }
    }

}