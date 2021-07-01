using System.Collections.Generic;
using PerigonGames;
using Shapes;
using UnityEngine;

namespace Laz
{
    public class ConnectingActivation : BasePuzzleBehaviour
    {
        [SerializeField] private NodeConnectionBehaviour[] _nodeBehaviours = null;
        [SerializeField] private Polyline _line = null;
        
        private Queue<Vertex> _listOfVertex = new Queue<Vertex>();
        private Queue<Vector3> _listOfNodePositions = new Queue<Vector3>();

        private void Awake()
        {
            var numberOfNodes = _nodeBehaviours.Length;
            var arrayOfNodes = CreateArrayOfNodes(numberOfNodes);
            _line.points = new List<PolylinePoint>();
            QueueUpVertex(arrayOfNodes);
        }

        private void AddPositionToLineRenderer(Vector3 position)
        {
            _line.AddPoint(position);
        }


        private void QueueUpVertex(Node[] nodes)
        {
            var numberOfNodes = nodes.Length;
            for (int i = 0; i < numberOfNodes; i++)
            {
                //Initializes Node before placing into Vertex
                var node = nodes[i];
                _nodeBehaviours[i].Initialize(node);
                _listOfNodePositions.Enqueue(_nodeBehaviours[i].gameObject.transform.localPosition);
                node.CanActivate = i == 0;
         
                if (i + 1 < numberOfNodes)
                {
                    var v = new Vertex(node, nodes[i + 1]);
                    v.OnVertexCompleted += HandleOnVertexCompleted;
                    _listOfVertex.Enqueue(v);
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

        private void HandleOnVertexCompleted()
        {
            var vertex = _listOfVertex.Dequeue();
            var nodePosition = _listOfNodePositions.Dequeue();
            AddPositionToLineRenderer(nodePosition);
            if (_listOfVertex.Count == 0)
            {
                vertex.CompleteBackNodeConnection();
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