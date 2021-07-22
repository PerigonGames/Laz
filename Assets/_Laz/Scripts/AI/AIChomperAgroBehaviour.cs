using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

namespace Laz
{
    public class AIChomperAgroBehaviour : MonoBehaviour
    {
        // Dependencies
        private IAstarAI _ai;
        private int _positionIndex = 0;
        private Lazo _lazo = null;
        
        public void Initialize(IAstarAI ai, Lazo lazo)
        {
            _ai = ai;
            _lazo = lazo;
        }

        public void StartAgroAt(LazoPosition lazoPosition)
        {
            _lazo.IsTimeToLiveFrozen = true;
            _positionIndex = _lazo.GetListOfLazoPositions.IndexOf(lazoPosition);
            if (lazoPosition != null)
            {
                _ai.destination = lazoPosition.Position;
            }
        }

        public void Agro()
        {
            if (_ai.reachedEndOfPath && !_ai.pathPending)
            {
                _ai.canSearch = false;
                var listOfPositions = CreateListOfPositionsStartingFrom(_positionIndex);
                SetAIPathWith(listOfPositions);
            }
        }

        private List<Vector3> CreateListOfPositionsStartingFrom(int startingPosition)
        {
            var lazoPositions = _lazo.GetListOfLazoPositions;
            var rangeOfPositions = lazoPositions.Count - startingPosition;
            _positionIndex = lazoPositions.Count - 1;
            if (rangeOfPositions > 1)
            {
                var listOfLazoPositionsToFollow = _lazo.GetListOfLazoPositions.GetRange(startingPosition, rangeOfPositions);
                return listOfLazoPositionsToFollow.Select(lazoPosition => lazoPosition.Position).ToList();
            }

            Debug.Log("Returned Empty List");
            return new List<Vector3>();
        }

        private void SetAIPathWith(List<Vector3> listOfPositions)
        {
            if (listOfPositions.Count > 2)
            {
                var aiPath = ABPath.FakePath(listOfPositions);
                _ai.SetPath(aiPath);
            }
        }
    }
}