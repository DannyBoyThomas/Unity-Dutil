using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Dutil
{
    public class NodeNetwork
    {
        public List<NNode> nodes = new List<NNode>();
        Dictionary<NNode, List<NNode>> connections = new Dictionary<NNode, List<NNode>>();
        public NodeNetwork(List<Vector3> nodePositions)
        {
            foreach (Vector3 pos in nodePositions)
            {
                nodes.Add(new NNode(this, pos));
            }

        }
        /// <summary>
        /// All connections are omni directional
        /// </summary>
        /// <param name="nodePositions"></param>
        /// <param name="IsConnected"></param>
        /// <returns></returns>
        public static NodeNetwork OmniDirectional(List<Vector3> nodePositions, Func<NNode, NNode, bool> IsConnected)
        {
            NodeNetwork network = new NodeNetwork(nodePositions);
            for (int i = 0; i < network.nodes.Count; i++)
            {
                for (int j = i + 1; j < network.nodes.Count; j++)
                {
                    if (IsConnected(network.nodes[i], network.nodes[j]))
                    {
                        network.AddConnection(network.nodes[i], network.nodes[j]);
                    }
                }
            }
            return network;
        }
        /// <summary>
        /// Creates a Node Network capable of one-way or dual-way connections
        /// </summary>
        /// <param name="nodePositions"></param>
        /// <param name="IsConnected"> (bool,bool) =>First bool is whether nodeA is connected to NodeB. Second bool determines if the connection is omnidirectional</param>
        /// <returns></returns>
        public static NodeNetwork UniDirectional(List<Vector3> nodePositions, Func<NNode, NNode, (bool, bool)> IsConnected)
        {
            NodeNetwork network = new NodeNetwork(nodePositions);

            for (int i = 0; i < network.nodes.Count; i++)
            {
                NNode node = network.nodes[i];
                for (int j = 0; j < network.nodes.Count; j++)
                {
                    NNode other = network.nodes[j];
                    if (i == j)
                    {
                        continue;
                    }
                    (bool connected, bool bothWays) = IsConnected(node, other);
                    if (connected)
                    {
                        network.AddConnection(node, other, !bothWays);
                    }
                }
            }
            return network;
        }
        public void CreateConnections(Func<NNode, NNode, bool> IsConnected)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                NNode node = nodes[i];
                for (int j = 0; j < nodes.Count; j++)
                {
                    NNode other = nodes[j];
                    if (i == j)
                    {
                        continue;
                    }
                    if (IsConnected(node, other))
                    {
                        AddConnection(node, other);
                    }
                }
            }
        }

        public void AddConnection(NNode node1, NNode node2, bool oneWay = false)
        {
            if (!connections.ContainsKey(node1))
            {
                connections.Add(node1, new List<NNode>());
            }
            connections[node1].AddUnique(node2);
            if (oneWay) { return; }

            if (!connections.ContainsKey(node2))
            {
                connections.Add(node2, new List<NNode>());
            }
            connections[node2].AddUnique(node1);

        }
        public void RemoveConnection(NNode node1, NNode node2, bool oneWay = false)
        {
            if (connections.ContainsKey(node1))
            {
                connections[node1].Remove(node2);
            }
            if (oneWay) { return; }
            if (connections.ContainsKey(node2))
            {
                connections[node2].Remove(node1);
            }
        }
        //Add Node
        public void AddNode(NNode node)
        {
            nodes.Add(node);
        }


        public void Draw()
        {
            foreach (NNode node in nodes)
            {
                Gizmos.color = Colours.Orange;
                Gizmos.DrawSphere(node.position, 0.3f);
                Gizmos.color = Colours.Yellow;
                foreach (NNode connectedNode in connections[node])
                {
                    Gizmos.DrawLine(node.position, connectedNode.position);
                }
            }
        }
    }
}