using System;

namespace MonkLib.Neat
{
    public class NodeGene
    {
        #region Properties
        public int Innovation         { get; private set; }
        public Constants.NodeType Type { get; private set; }
        public double Value { get; set; }
        #endregion


        #region Constructors

        public NodeGene(int innovation, Constants.NodeType type)
        {
            this.Innovation = innovation;
            this.Type = type;
            this.Value = 0;
        }

        public NodeGene(NodeGene node)
        {
            this.Innovation = node.Innovation;
            this.Type = node.Type;
            this.Value = node.Value;
        }

        #endregion


        #region Utils

        public NodeGene Copy()
        {
            return new NodeGene(this);
        }

        public override string ToString()
        {
            return string.Format("Innovation: {0} Type: {1} Value: {2}",
                this.Innovation, System.Enum.GetName(typeof(Constants.NodeType), this.Type), this.Value);
        }

        #endregion

    }
}
