using System;
using DG.Tweening;
using Game.CharacterSystem.Base;
using UnityEngine;

namespace Game.CameraSystem
{
    public class CameraFollow : MonoBehaviour
    {
        private Transform _target;
        private Vector3 _offset;
        private float _sharpness;

        public float sharpness;

        private void Awake()
        {
            SetTargetAsPlayer();
            _offset = transform.position - _target.transform.position;
        }

        public void EaseSharpness()
        {
            DOTween.To(x => _sharpness = x, _sharpness, sharpness, 1.2f);
        }

        public void SetTargetAsPlayer()
        {
            _target = FindObjectOfType<PlayerCharacter>().transform;
        }
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void SetLevelGameBehavior(Sequence sequence)
        {
            _target = null;
            sequence.Play();
        }

        private void FixedUpdate()
        {
            if (_target == null)
                return;

            Vector3 desired = new Vector3(transform.position.x, transform.position.y,
                _target.transform.position.z + _offset.z);
            
            Vector3 smooth = Vector3.Lerp(transform.position, desired, _sharpness * Time.fixedDeltaTime);

            transform.position = smooth;
        }
    }
}
