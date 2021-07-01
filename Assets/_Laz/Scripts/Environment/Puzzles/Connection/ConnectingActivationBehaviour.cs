using System.Collections.Generic;
using PerigonGames;
using Shapes;
using UnityEngine;

namespace Laz
{
    public class ConnectingActivationBehaviour : BasePuzzleBehaviour
    {
        [SerializeField] private NodeConnectionBehaviour[] _nodeBehaviours = null;
        [SerializeField] private Polyline _line = null;
        
        private Queue<Edge> _listOfEdges = new Queue<Edge>();
        private Queue<Vector3> _listOfNodePositions = new Queue<Vector3>();

        public override void Initialize()
        {
            SetupQueueOfEdges();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            foreach (var nodeBehaviour in _nodeBehaviours)
            {
                nodeBehaviour.CleanUp();
            }
            _listOfEdges.Clear();
            _listOfNodePositions.Clear();
            _line.points = new List<PolylinePoint>();
        }

        public override void Reset()
        {
            base.Reset();
            foreach (var nodeBehaviour in _nodeBehaviours)
            {
                nodeBehaviour.Reset();
            }
            _line.points = new List<PolylinePoint>();
            SetupQueueOfEdges();
        }

        private void SetupQueueOfEdges()
        {
            var numberOfNodes = _nodeBehaviours.Length;
            QueueUpEdges(CreateArrayOfNodes(numberOfNodes));
        }

        private void AddPositionToLineRenderer(Vector3 position)
        {
            _line.AddPoint(position);
        }
        
        private void QueueUpEdges(Node[] nodes)
        {
            var numberOfNodes = nodes.Length;
            for (int i = 0; i < numberOfNodes; i++)
            {
                //Initializes Node before placing into the Edge
                var node = nodes[i];
                _nodeBehaviours[i].Initialize(node);
                _listOfNodePositions.Enqueue(_nodeBehaviours[i].gameObject.transform.localPosition);
                node.CanActivate = i == 0;
         
                if (i + 1 < numberOfNodes)
                {
                    var v = new Edge(node, nodes[i + 1]);
                    v.OnEdgeCompleted += HandleOnEdgeCompleted;
                    _listOfEdges.Enqueue(v);
                }
            }
            var position = _listOfNodePositions.Dequeue();
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

        private void HandleOnEdgeCompleted()
        {
            var edge = _listOfEdges.Dequeue();
            var nodePosition = _listOfNodePositions.Dequeue();
            AddPositionToLineRenderer(nodePosition);
            if (_listOfEdges.Count == 0)
            {
                edge.CompleteBackNodeConnection();
                Activate();
            }
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