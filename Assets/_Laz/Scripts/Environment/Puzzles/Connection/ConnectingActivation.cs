using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laz
{
    public class ConnectingActivation : BaseActivatingBehaviour
    {
        [SerializeField] private NodeConnectionBehaviour[] _nodeBehaviours = null;
        
        public override bool IsActivated { get; }
        private List<Vertex> _listOfVertex = new List<Vertex>();

        private void Awake()
        {
            var numberOfNodes = _nodeBehaviours.Length;
            var arrayOfNodes = CreateArrayOfNodes(numberOfNodes);
            QueueUpVertex(arrayOfNodes);
        }


        private void QueueUpVertex(Node[] nodes)
        {
            var numberOfNodes = nodes.Length;
            for (int i = 0; i < numberOfNodes; i++)
            {
                //Initializes Node before placing into Vertex
                var node = nodes[i];
                _nodeBehaviours[i].Initialize(node);
                node.CanActivate = i == 0;
         
                if (i + 1 < numberOfNodes)
                {
                    var v = new Vertex(node, nodes[i + 1]);
                    v.OnVertexCompleted += HandleOnVertexCompleted;
                    _listOfVertex.Add(v);
                }
            }
        }

        private Node[] CreateArrayOfNodes(int numberOfNodes)
        {
            var arrayOfNodes = new Node[numberOfNodes];
            for (int i = 0; i < numberOfNodes; i++)
            {
                arrayOfNodes[i] = new Node(i);
            }

            return arrayOfNodes;
        }

        private void HandleOnVertexCompleted()
        {
            if (_listOfVertex.All(v => v.IsActivated))
            {
                // TODO puzzle completed
            }
        }
    }
}