using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Laz
{
    public class BidirectionalConnectingPuzzleBehaviour : BasePuzzleBehaviour
    {
        [SerializeField] private NodeConnectionBehaviour[] _nodeBehaviours = null;
        private BidirectionalConnectingPuzzle _connectingPuzzle = null;
        private Dictionary<Node, Vector3> _dictionaryOfNodePositions = new Dictionary<Node, Vector3>();
        
        public override void Initialize()
        {
            base.Initialize();
            Reset();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _connectingPuzzle.CleanUp();
            _connectingPuzzle = null;
            _dictionaryOfNodePositions.Clear();
            GetComponentsInChildren<LineRenderer>().ForEach(line => Destroy(line.gameObject));
        }

        public override void Reset()
        {
            base.Reset();
            _connectingPuzzle = new BidirectionalConnectingPuzzle(_nodeBehaviours.Length);
            _connectingPuzzle.OnBidirectionalPuzzleCompleted += ActivatePuzzle;
            _connectingPuzzle.OnEdgeCompleted += HandleOnEdgeCompleted;
            BindNodeToBehaviour();
            _connectingPuzzle.SetNodesCanActivate();
            MapPositionToNode();
        }

        private void HandleOnEdgeCompleted(Node source, Node destination)
        {
            var line = LineFactory.CreateLineBetween(_dictionaryOfNodePositions[source], _dictionaryOfNodePositions[destination]);
            line.transform.parent = transform;
        }

        private void BindNodeToBehaviour()
        {
            for(int i = 0 ; i < _nodeBehaviours.Length; i++)
            {
                var node = _connectingPuzzle.Nodes[i];
                _nodeBehaviours[i].Initialize(node);
            }
        }

        private void MapPositionToNode()
        {
            for (int i = 0; i < _nodeBehaviours.Length; i++)
            {
                _dictionaryOfNodePositions.Add(_connectingPuzzle.Nodes[i], _nodeBehaviours[i].transform.position);
            }
        }

        #region Mono

        private void Awake()
        {
            if (_nodeBehaviours.Length < 2)
            {
                PanicHelper.Panic(new Exception("You only have 1 or less Nodes in your Bidirectional Connecting puzzle"));
            }
        }

        #endregion
        
        #region Gizmo
        void OnDrawGizmosSelected()
        {
            if (!_nodeBehaviours.IsNullOrEmpty())
            {
                // Draws a blue line from this transform to the target
                for(int i = 0;i < _nodeBehaviours.Length - 1; i++)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(_nodeBehaviours[i].transform.position, _nodeBehaviours[i + 1].transform.position);
                }
            }
        }
        #endregion
    }
}