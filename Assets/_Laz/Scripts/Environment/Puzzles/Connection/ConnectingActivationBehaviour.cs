using System.Collections.Generic;
using System.Linq;
using Shapes;
using Sirenix.Utilities;
using UnityEngine;

namespace Laz
{
    public class ConnectingActivationBehaviour : BasePuzzleBehaviour
    {
        [SerializeField] private NodeConnectionBehaviour[] _nodeBehaviours = null;
        [SerializeField] private Polyline _line = null;
        
        private Queue<Edge> _queueOfEdges = new Queue<Edge>();
        private Queue<Vector3> _queueOfNodePositions = new Queue<Vector3>();
        private Node[] _nodes = { };

        private int NumberOfNodes => _nodes.Length;
        private bool IsQueueOfEdgesEmpty => _queueOfEdges.Count == 0;

        public override void Initialize()
        {
            var numberOfNodes = _nodeBehaviours.Length;
            _nodes = CreateArrayOfNodes(numberOfNodes);
            EnqueueNodeBehaviourPositions();
            Reset();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _line.points = new List<PolylinePoint>();
            _queueOfEdges = new Queue<Edge>();
            _queueOfNodePositions = new Queue<Vector3>();
            _line.points = new List<PolylinePoint>();
            _nodeBehaviours.ForEach(node => node.CleanUp());
            _queueOfEdges.ForEach(edge => edge.CleanUp());
            _queueOfNodePositions.Clear();
        }

        public override void Reset()
        {
            base.Reset();
            _nodeBehaviours.ForEach(behaviour => behaviour.Reset());
            BindNodeBehaviour();
            SetupNodes();
            CreateAndEnqueueEdges();
            SetupFirstNode();
        }

        private void BindNodeBehaviour()
        {
            for (int i = 0; i < _nodes.Length; i++)
            {
                _nodeBehaviours[i].Initialize(_nodes[i]);
            }
        }

        private void SetupNodes()
        {
            foreach (var node in _nodes)
            {
                node.CanActivate = false;
            }
        }
        
        private void CreateAndEnqueueEdges()
        {
            for (int frontNodeIndex = 0; frontNodeIndex < NumberOfNodes; frontNodeIndex++)
            {
                var backNodeIndex = frontNodeIndex + 1;
                var isBackNodeWithinBounds = backNodeIndex < NumberOfNodes;
                if (isBackNodeWithinBounds)
                {
                    var frontNode = _nodes[frontNodeIndex];
                    var backNode = _nodes[backNodeIndex];
                    var edge = new Edge(frontNode, backNode, HandleOnEdgeCompleted);
                    _queueOfEdges.Enqueue(edge);
                }
            }
        }

        private void SetupFirstNode()
        {
            _nodes.First().CanActivate = true;
            var position = _queueOfNodePositions.Dequeue();
            AddPositionToLineRenderer(position);
        }

        private Node[] CreateArrayOfNodes(int numberOfNodes)
        {
            var arrayOfNodes = new Node[numberOfNodes];
            for (int i = 0; i < numberOfNodes; i++)
            {
                arrayOfNodes[i] = new Node();
            }

            return arrayOfNodes;
        }

        private void EnqueueNodeBehaviourPositions()
        {
            foreach (var behaviours in _nodeBehaviours)
            {
                _queueOfNodePositions.Enqueue(behaviours.transform.position);
            }
        }

        private void HandleOnEdgeCompleted()
        {
            var completedEdge = _queueOfEdges.Dequeue();
            var nextNodePosition = _queueOfNodePositions.Dequeue();
            AddPositionToLineRenderer(nextNodePosition);
            if (IsQueueOfEdgesEmpty)
            {
                completedEdge.CompleteBackNodeConnection();
                Activate();
            }
        }
        
        private void AddPositionToLineRenderer(Vector3 position)
        {
            _line.AddPoint(position);
        }

        
        #region Gizmo
        void OnDrawGizmosSelected()
        {
            if (!_nodeBehaviours.IsNullOrEmpty())
            {
                // Draws a blue line from this transform to the target
                for(int i = 0;i < _nodeBehaviours.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawSphere(_nodeBehaviours[i].transform.position, 1);
                    }
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(_nodeBehaviours[i].transform.position, _nodeBehaviours[i + 1].transform.position);
                }
            }
        }
        #endregion
    }
}