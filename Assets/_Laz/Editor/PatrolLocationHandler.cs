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
        private const string NEW_PATROL_TITLE = "New Patrol Point";
        private PatrolBehaviour _patrolBehaviour = null;
        private Vector3 _tempPosition = Vector3.zero;
        private List<Vector3> _patrolTrail;

        private void OnEnable()
        {
            _patrolBehaviour = (PatrolBehaviour) target;
        }

        private void OnSceneGUI()
        {
            if (_patrolBehaviour == null)
            {
                return;
            }
            
            Event guiEvent = Event.current;
            
            HandleInput(guiEvent);
            HandlePatrolPositioning();
        }

        private void HandlePatrolPositioning()
        {
            for (int i = 0, count = _patrolBehaviour.PatrolPositions.Count; i < count; i++)
            {
                _tempPosition = Handles.PositionHandle(_patrolBehaviour.PatrolPositions[i], Quaternion.identity);
                _tempPosition.y = 0.0f; // Ensure the position is grounded onto the plane

                if (!_patrolBehaviour.PatrolPositions[i].Equals(_tempPosition))
                {
                    Undo.RecordObject(_patrolBehaviour, "Patrol Positioning");
                    _patrolBehaviour.PatrolPositions[i] = _tempPosition;   
                }
            }
            
            _patrolTrail = new List<Vector3>();
            _patrolTrail.Add(_patrolBehaviour.transform.position);
            _patrolTrail.AddRange(_patrolBehaviour.PatrolPositions);
            
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
            Vector3 mousePosition = GetAdjustedMousePosition(guiEvent);
            
            // Add New patrol point if User Presses Left Click
            if (guiEvent.button == 0)
            {
                HandleLeftClick(mousePosition, guiEvent.modifiers);
            }
            
            // Removes Patrol Point if User Press Right Click really close to the position
            if (guiEvent.button == 1 && guiEvent.modifiers == EventModifiers.None)
            {
                RemovePatrolPosition(mousePosition);
            }
            
            HandleUtility.Repaint();
        }

        private Vector3 GetAdjustedMousePosition(Event guiEvent)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);

            float rayYOrigin = mouseRay.origin.y;
            float rayYDirection = mouseRay.direction.y;
            float distanceToPlane = rayYDirection.Equals(0.0f) ? DRAW_PLANE_HEIGHT - rayYOrigin : (DRAW_PLANE_HEIGHT - rayYOrigin) / rayYDirection;
            return mouseRay.GetPoint(distanceToPlane);
        }

        private void HandleLeftClick(Vector3 mousePosition, EventModifiers modifier)
        {
            // If User uses Ctrl/Cmd then add patrol point at end of list
            if (modifier == EventModifiers.Command || modifier == EventModifiers.Control)
            {
                AppendPatrolPosition(mousePosition);
            }
                
            // If User uses Shift then insert patrol point between two closest patrol points
            // UNFORTUNATELY it is a bit finicky in get the closest line segment
            if (modifier == EventModifiers.Shift)
            {
                InsertPatrolPosition(mousePosition);
            }

        }

        private void AppendPatrolPosition(Vector3 mousePosition)
        {
            Undo.RecordObject(_patrolBehaviour, NEW_PATROL_TITLE);
            _patrolBehaviour.PatrolPositions.Add(mousePosition);
        }

        private void InsertPatrolPosition(Vector3 mousePosition)
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
                    
            Undo.RecordObject(_patrolBehaviour, NEW_PATROL_TITLE);
            _patrolBehaviour.PatrolPositions.Insert(indexToInsert, mousePosition);
        }

        private void RemovePatrolPosition(Vector3 mousePosition)
        {
            for (int i = 0, count = _patrolBehaviour.PatrolPositions.Count; i < count; i++)
            {
                if (Vector3.Distance(mousePosition, _patrolBehaviour.PatrolPositions[i]) <= CLOSEST_DISTANCE_FOR_DELETION)
                {
                    _patrolBehaviour.PatrolPositions.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
