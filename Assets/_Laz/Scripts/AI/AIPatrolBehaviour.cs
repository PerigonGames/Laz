using Pathfinding;
using UnityEngine;

namespace Laz
{
    public class AIPatrolBehaviour : MonoBehaviour
    {
        private IAstarAI _ai = null;
        private IRandomPosition _randomPosition = null;
        
        public void Initialize(IAstarAI ai, float idleRadius, IRandomPosition randomPosition = null)
        {
            _ai = ai;
            _randomPosition = randomPosition ?? new RandomPositionInsideCircle(transform.position, idleRadius);
            _ai.destination = _randomPosition.GetRandomPosition();
        }

        public void PatrolCircularArea()
        {
            if (_ai.reachedEndOfPath)
            {
                var destination = _randomPosition.GetRandomPosition();
                MoveTo(destination);
            }
        }
        
        public void CleanUp()
        {
            _ai.destination = Vector3.zero;
        }
        
        public void Reset()
        {
            _ai.destination = _randomPosition.GetRandomPosition();
        }
        
        private void MoveTo(Vector3 position)
        {
            _ai.destination = position;
        }
    }   
}
