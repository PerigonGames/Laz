using System;
using UnityEngine;

namespace Laz
{
    public class PlaceHolderMovementBehaviour : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        private LazoBehaviour _lazoBehaviour = null;

        private void Awake()
        {
            _lazoBehaviour = GetComponent<LazoBehaviour>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _lazoBehaviour.TurnLazoing(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _lazoBehaviour.TurnLazoing(false);
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * _speed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * _speed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * _speed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * _speed);
            }
        }
    }
}