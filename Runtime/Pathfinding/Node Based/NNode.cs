using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{


    public class NNode
    {
        public NodeNetwork network;
        public Vector3 position;
        public NNode(NodeNetwork network, Vector3 position)
        {
            this.network = network;
            this.position = position;
        }
    }
}