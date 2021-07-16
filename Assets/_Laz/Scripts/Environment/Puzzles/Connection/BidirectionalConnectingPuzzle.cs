using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;

namespace Laz
{
    public class BidirectionalConnectingPuzzle
    {
        private readonly Dictionary<Node, List<Node>> _dictionaryOfNeighbouringNodes = new Dictionary<Node, List<Node>>();
        private readonly Node[] _nodes;

        public event Action OnBidirectionalPuzzleCompleted;
        public event Action<Node, Node> OnEdgeCompleted;

        public Node[] Nodes => _nodes;

        public BidirectionalConnectingPuzzle(int numberOfNodes)
        {
            _nodes = CreateArrayOfNodes(numberOfNodes);
            SetupNeighbourOfNodes(_nodes);
        }

        public void CleanUp()
        {
            _nodes.ForEach(node => node.OnIsActiveChanged -= HandleNodeIsActiveChanged);
            OnBidirectionalPuzzleCompleted = null;
        }

        public void SetNodesCanActivate()
        {
            _nodes.ForEach(node => node.CanActivate = true);
        }

        private Node[] CreateArrayOfNodes(int numberOfNodes)
        {
            var arrayOfNodes = new Node[numberOfNodes];
            for (int i = 0; i < numberOfNodes; i++)
            {
                var node = new Node();
                arrayOfNodes[i] = node;
                node.OnIsActiveChanged += HandleNodeIsActiveChanged;
            }

            return arrayOfNodes;
        }

        private void SetupNeighbourOfNodes(Node[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                _dictionaryOfNeighbouringNodes.Add(nodes[i], new List<Node>());

                var isFirstNode = i == 0;
                var isLastNode = i == nodes.Length - 1;
                var isMiddleNodes = !isFirstNode && !isLastNode;
                if (isFirstNode)
                {
                    var firstNode = nodes[i];
                    var secondNode = nodes[i + 1];
                    _dictionaryOfNeighbouringNodes[firstNode].Add(secondNode);
                }

                if (isMiddleNodes)
                {
                    var previousNode = nodes[i - 1];
                    var middleNode = nodes[i];
                    var nextNode = nodes[i + 1];
                    _dictionaryOfNeighbouringNodes[middleNode].Add(previousNode);
                    _dictionaryOfNeighbouringNodes[middleNode].Add(nextNode);
                }

                if (isLastNode)
                {
                    var lastNode = nodes[i];
                    var secondLastNode = nodes[i - 1];
                    _dictionaryOfNeighbouringNodes[lastNode].Add(secondLastNode);
                }
            }
        }

        private void HandleNodeIsActiveChanged(Node node)
        {
            var areOnlyTwoNodesActive = Nodes.Count(n => n.IsActive) == 2;
            if (node.IsActive && areOnlyTwoNodesActive)
            {
                var listOfLazActivatedNodeNeighbors =
                    GetNeighborsFrom(node).Where(neighbor => neighbor.IsActive).ToList();
                foreach (var connectingNode in listOfLazActivatedNodeNeighbors)
                {
                    RemoveConnectingNeighbors(node, connectingNode);
                    RemoveConnectingNeighbors(connectingNode, node);
                    RenderEdgeBetween(node, connectingNode);
                    Deactivate(new[] {node, connectingNode});
                }

                CallBackOnPuzzleCompletedIfNeeded();
            }
        }

        private List<Node> GetNeighborsFrom(Node node)
        {
            if (_dictionaryOfNeighbouringNodes.ContainsKey(node))
            {
                return _dictionaryOfNeighbouringNodes[node];
            }

            return new List<Node>();
        }

        private void RemoveConnectingNeighbors(Node source, Node destination)
        {
            if (_dictionaryOfNeighbouringNodes.ContainsKey(source))
            {
                _dictionaryOfNeighbouringNodes[source].Remove(destination);
                if (_dictionaryOfNeighbouringNodes[source].Count == 0)
                {
                    source.CompletedConnection();
                }
            }
        }

        private bool AreAllNeighbouringConnectionsCompleted()
        {
            foreach (var node in _dictionaryOfNeighbouringNodes)
            {
                if (node.Value.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }
        
        private void RenderEdgeBetween(Node source, Node destination)
        {
            OnEdgeCompleted?.Invoke(source, destination);
        }

        private void Deactivate(Node[] nodes)
        {
            nodes.ForEach(node => node.IsActive = false);
        }

        private void CallBackOnPuzzleCompletedIfNeeded()
        {
            if (AreAllNeighbouringConnectionsCompleted())
            {
                OnBidirectionalPuzzleCompleted?.Invoke();
            }
        }

    }
}