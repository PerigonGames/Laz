using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    public class ElectricGate
    {
        public event Action<GateState> OnGateFlickerChange;

        private bool _flickering;
        private bool _flickeringState = true;

        private float _elapsedTime = 0.0f, _onTime = 0.5f, _offTime = 0.5f;

        public ElectricGate(float onTime, float offTime, bool flickering)
        {
            _onTime = onTime;
            _offTime = offTime;
            _flickering = flickering;
            _elapsedTime = 0f;
        }

        public void Update()
        {
            if (_flickering)
            {
                _elapsedTime += Time.deltaTime;
                if (_flickeringState && _elapsedTime > _onTime)
                {
                    _flickeringState = false;
                    _elapsedTime -= _onTime;
                    OnGateFlickerChange?.Invoke(GateState.flicker_off);
                }
                else if (!_flickeringState && _elapsedTime > _offTime)
                {
                    _flickeringState = true;
                    _elapsedTime -= _offTime;
                    OnGateFlickerChange?.Invoke(GateState.on);
                }
            }
        }

        public void CleanUp()
        {

        }

        public void Reset()
        {
            _elapsedTime = 0f;
        }
    }

    public enum GateState
    {
        on, flicker_off, off
    }
}
