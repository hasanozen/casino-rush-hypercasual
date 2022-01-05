using UnityEngine;

namespace Game.CharacterSystem.Controllers
{
    public class CharacterAnimatorController
    {
        private Animator _animator;
        private readonly int _isRunning = Animator.StringToHash("IsRunning");

        public CharacterAnimatorController(Animator animator)
        {
            _animator = animator;
        }

        public void PerformRunAnimation()
        {
            _animator.SetBool(_isRunning, true);
        }
        
        public void PerformIdleAnimation()
        {
            _animator.SetBool(_isRunning, false);
        }

        public void Activate()
        {
            _animator.enabled = true;
        }
        
        public void Deactivate()
        {
            _animator.enabled = false;
        }
    }
}
