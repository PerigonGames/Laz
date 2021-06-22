using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Laz;
using UnityEngine;


namespace LazEditor
{
    [CustomEditor(typeof(PatrolBehaviour))]
    public class PatrolLocationHandler : Editor
    {
        private PatrolBehaviour _behaviour = null;
        private Vector3 _tempPosition = Vector3.zero;
        private List<Vector3> _patrolTrail;

        private void OnEnable()
        {
            _behaviour = (PatrolBehaviour) target;
        }

        private void OnSceneGUI()
        {
            if (_behaviour == null)
            {
                return;
            }
            
            Undo.RecordObject(_behaviour, "Patrol Positioning");
            for (int i = 0, count = _behaviour.PatrolPositions.Count; i < count; i++)
            {
                _tempPosition = Handles.PositionHandle(_behaviour.PatrolPositions[i], Quaternion.identity);
                _tempPosition.y = 0.0f; // Ensure the position is grounded onto the plane
                _behaviour.PatrolPositions[i] = _tempPosition;
            }
            
            _patrolTrail = new List<Vector3>();
            _patrolTrail.Add(_behaviour.gameObject.transform.position);
            _patrolTrail.AddRange(_behaviour.PatrolPositions);
            
            Handles.color = Color.yellow;
            Handles.DrawPolyLine(_patrolTrail.ToArray());
        }
    }
}
