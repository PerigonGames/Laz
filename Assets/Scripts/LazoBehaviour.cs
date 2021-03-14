using UnityEngine;

namespace Laz
{
    public class LazoBehaviour : MonoBehaviour
    {
        private Lazo _lazo;
        private TrailRenderer _trail = null;
        
        [SerializeField] private LazoPropertiesScriptableObject properties;


        /// <summary>
        /// Turns Lazoing on or off
        /// </summary>
        /// <param name="isOn">is On</param>
        public void TurnLazoing(bool isOn)
        {
            _lazo.IsLazoing = isOn;
        }

        private void Awake()
        {
            _trail = GetComponent<TrailRenderer>();
        }

        private void OnEnable()
        {
            _lazo = new Lazo(properties);
            _trail.time = properties.TimeToLivePerPoint;
        }

        private void OnDisable()
        {
            _lazo = null;
        }

        private void Update()
        {
            _trail.emitting = _lazo.IsLazoing;
            if (!_lazo.IsLazoing)
            {
                _trail.Clear();
                return;
            }

            _lazo.RunLazoIfAble(transform.position, Time.deltaTime);
        }


    }
}
