using System.Collections.Generic;
using UnityEditor;
using Laz;
using UnityEngine;


namespace LazEditor
{
    [CustomEditor(typeof(PatrolBehaviour))]
    public class PatrolLocationHandler : Editor
    {
        private const float DRAW_PLANE_HEIGHT = 0f;
        private const float CLOSEST_DISTANCE_FOR_DELETION = 0.6f;
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
            
            Event guiEvent = Event.current;
            
            HandleInput(guiEvent);
            HandlePatrolPositioning();
        }

        private void HandlePatrolPositioning()
        {
            for (int i = 0, count = _behaviour.PatrolPositions.Count; i < count; i++)
            {
                _tempPosition = Handles.PositionHandle(_behaviour.PatrolPositions[i], Quaternion.identity);
                _tempPosition.y = 0.0f; // Ensure the position is grounded onto the plane

                if (!_behaviour.PatrolPositions[i].Equals(_tempPosition))
                {
                    Undo.RecordObject(_behaviour, "Patrol Positioning");
                    _behaviour.PatrolPositions[i] = _tempPosition;   
                }
            }
            
            _patrolTrail = new List<Vector3>();
            _patrolTrail.Add(_behaviour.transform.position);
            _patrolTrail.AddRange(_behaviour.PatrolPositions);
            
            Handles.color = Color.yellow;
            Handles.DrawPolyLine(_patrolTrail.ToArray());
        }

        private void HandleInput(Event guiEvent)
        {
            if (guiEvent.type != EventType.MouseDown)
            {
                return;
            }

            //Get Mouse coordinates in Correct Coordinates (at y = 0)
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);

            float rayYOrigin = mouseRay.origin.y;
            float rayYDirection = mouseRay.direction.y;
            float distanceToPlane = rayYDirection.Equals(0.0f) ? DRAW_PLANE_HEIGHT - rayYOrigin : (DRAW_PLANE_HEIGHT - rayYOrigin) / rayYDirection;
            Vector3 mousePosition = mouseRay.GetPoint(distanceToPlane);
            
            // Add New patrol point if User Presses Left Click
            if (guiEvent.button == 0)
            {
                // If User uses Ctrl/Cmd then add patrol point at end of list
                if (guiEvent.modifiers == EventModifiers.Command || guiEvent.modifiers == EventModifiers.Control)
                {
                    Undo.RecordObject(_behaviour, "New Patrol Point");
                    _behaviour.PatrolPositions.Add(mousePosition);
                }
                
                // If User uses Shift then insert patrol point between two closest patrol points
                // UNFORTUNATELY it is a bit finicky in get the closest line segment
                if (guiEvent.modifiers == EventModifiers.Shift)
                {
                    float closestDistance = HandleUtility.DistancePointLine(mousePosition,
                        _patrolTrail[0], _patrolTrail[1]);

                    int indexToInsert = 0;
                    
                    for (int i = 0, count = _patrolTrail.Count; i < count-1; i++)
                    {
                        if (HandleUtility.DistancePointLine(mousePosition, _patrolTrail[i],
                            _patrolTrail[i + 1]) < closestDistance)
                        {
                            indexToInsert = i;
                        }
                    }
                    
                    Undo.RecordObject(_behaviour, "New Patrol Point");
                    _behaviour.PatrolPositions.Insert(indexToInsert, mousePosition);
                }

            }
            
            // Removes Patrol Point if User Press Right Click really close to the position
            if (guiEvent.button == 1 && guiEvent.modifiers == EventModifiers.None)
            {
                for (int i = 0, count = _behaviour.PatrolPositions.Count; i < count; i++)
                {
                    if (Vector3.Distance(mousePosition, _behaviour.PatrolPositions[i]) <= CLOSEST_DISTANCE_FOR_DELETION)
                    {
                        _behaviour.PatrolPositions.RemoveAt(i);
                        break;
                    }
                }
            }
            
            HandleUtility.Repaint();
        }
    }
}
