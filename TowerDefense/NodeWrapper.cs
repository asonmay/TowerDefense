using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    internal class NodeWrapper
    {
        public PathTile WrappedNode;
        public float cumulativeDistance;
        public NodeWrapper Founder { get; set; }

        public NodeWrapper(PathTile wrappedNode, float cumulativeDistance, NodeWrapper founder)
        {
            WrappedNode = wrappedNode;
            this.cumulativeDistance = cumulativeDistance;
            Founder = founder;
        }
    }
}
