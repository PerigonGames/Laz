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
        private bool IsQueueOfEdgesEmpty => _queueOfEdges.Count == 0;

        public override void Initialize()
        {
            Reset();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _line.points.Clear();
            _queueOfEdges.Clear();
            _queueOfNodePositions.Clear();
            _nodeBehaviours.ForEach(node => node.CleanUp());
            _queueOfEdges.ForEach(edge => edge.CleanUp());
        }

        public override void Reset()
        {
            base.Reset();
            _nodeBehaviours.ForEach(behaviour => behaviour.Reset());
            EnqueueNodeBehaviourPositions();
            var nodes = CreateArrayOfNodes(_nodeBehaviours.Length);
            BindNodeBehaviour(nodes);
            SetupNodes(nodes);
            CreateAndEnqueueEdges(nodes);
            SetupFirstNode(nodes);
        }

        private void BindNodeBehaviour(Node[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                _nodeBehaviours[i].Initialize(nodes[i]);
            }
        }

        private void SetupNodes(Node[] nodes)
        {
            nodes.ForEach(node => node.IsActive = false);
        }
        
        private void CreateAndEnqueueEdges(Node[] nodes)
        {
            for (int frontNodeIndex = 0; frontNodeIndex < nodes.Length; frontNodeIndex++)
            {
                var backNodeIndex = frontNodeIndex + 1;
                var isBackNodeWithinBounds = backNodeIndex < nodes.Length;
                if (isBackNodeWithinBounds)
                {
                    var frontNode = nodes[frontNodeIndex];
                    var backNode = nodes[backNodeIndex];
                    var edge = new Edge(frontNode, backNode, HandleOnEdgeCompleted);
                    _queueOfEdges.Enqueue(edge);
                }
            }
        }

        private void SetupFirstNode(Node[] nodes)
        {
            nodes.First().CanActivate = true;
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