using UnityEngine;

namespace Game.CharacterSystem.Controllers
{
    public class CharacterMovementController : MonoBehaviour
    {
        private Transform _characterTransform;
        private float _forwardSpeed;
        private float _swipeSpeed;

        public void Init(Transform characterTransform, float forwardSpeed, float swipeSpeed)
        {
            _characterTransform = characterTransform;
            _forwardSpeed = forwardSpeed;
            _swipeSpeed = swipeSpeed;
        }
        
        public void MoveForward()
        {
            _characterTransform.Translate(_characterTransform.forward * (Time.deltaTime * _forwardSpeed));
        }

        public void MoveSide()
        {
            float input = Input.GetAxis("Mouse X") * _swipeSpeed * Time.fixedDeltaTime;
            
            _characterTransform.position = new Vector3(
                Mathf.Clamp(
                    _characterTransform.position.x + input, 
                    -2.5f, 
                    2.5f),
                _characterTransform.position.y,
                _characterTransform.position.z);
        }
    }
}
