using System;
using System.Collections.Generic;
using UnityEngine;

namespace Laz
{
    public class ConnectingActivation : BaseActivatingBehaviour
    {
        [SerializeField] private NodeConnectionBehaviour[] _nodeBehaviours = null;
        
        public override bool IsActivated { get; }
        private Queue<Vertex> _queueOfVertex = new Queue<Vertex>();

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
                    _queueOfVertex.Enqueue(v);
                }
            }
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
    }

    public class Node
    {
        private bool _canActivate = false;

        private bool _isActive = false;

        public event Action<bool> OnCanActivateChanged;
        
        public bool CanActivate
        {
            get => _canActivate;

            set
            {
                _canActivate = value;
                if (OnCanActivateChanged != null)
                {
                    OnCanActivateChanged(value);
                }
            }
        }
    }

    public class Vertex
    {
        private bool _isActivated = false;
        private Node _frontNode = null;
        private Node _backNode = null;

        public Node FrontNode => _frontNode;

        public Vertex(Node frontNode, Node backNode)
        {
            _frontNode = frontNode;
            _backNode = backNode;
        }
    }
}