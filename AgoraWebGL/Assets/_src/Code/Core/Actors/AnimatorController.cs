using _src.Code.Core.Signals.Inputs;
using UnityEngine;
using Zenject;

namespace _src.Code.Core.Actors
{
    public class AnimatorController : InputObject
    {
        public BoxCollider boxCollider;
        public Animator animator;

        private bool _isDoingAnimation;
        
        public void EndCurrentAnimation()
        {
            if (_isDoingAnimation)
            {
                animator.SetTrigger("CancelKey");
                _isDoingAnimation = false;
            }
        }
        
        public void DoWalkAnimation()
        {
            if (_isDoingAnimation)
                return;

            _isDoingAnimation = true;
            animator.SetTrigger("WalkKey");
        }
        
        public void DoSitAnimation()
        {
            if (_isDoingAnimation)
                return;

            _isDoingAnimation = true;
            animator.SetTrigger("SitKey");
        }
        
        public void DoJumpAnimation()
        {
            if (_isDoingAnimation)
                return;

            _isDoingAnimation = true;
            animator.SetTrigger("JumpKey");
        }
        
        public void DoWinningAnimation()
        {
            if (_isDoingAnimation)
                return;

            _isDoingAnimation = true;
            animator.SetTrigger("winKey");
        }
        
        // This method will be called by the StateMachineBehaviour when the animation finishes
        public void OnAnimationFinish()
        {
            Debug.Log("Animation finished");
            _isDoingAnimation = false;
        }
    }
}