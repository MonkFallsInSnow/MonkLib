using System;

namespace MonkLib.Neat
{
    public class ConnectionGene
    {
        #region Properties

        public int Innovation { get; private set; }
        public NodeGene Input { get; private set; }
        public NodeGene Output { get; private set; }
        public double Weight { get; set; }
        public bool Enabled { get; set; } //consider removing the gene from the genome entirely when this is set to false

        #endregion


        #region Constructors

        public ConnectionGene(int innovation, NodeGene input, NodeGene output, double weight = 1, bool enabled = true)
        {
            this.Innovation = innovation;
            this.Input = input;
            this.Output = output;
            this.Weight = weight;
            this.Enabled = enabled;            
        }

        public ConnectionGene(ConnectionGene connection)
        {
            this.Innovation = connection.Innovation;
            this.Input = connection.Input.Copy();
            this.Output = connection.Output.Copy();
            this.Weight = connection.Weight;
            this.Enabled = connection.Enabled;
        }

        #endregion


        #region Utils

        public ConnectionGene Copy()
        {
            return new ConnectionGene(this);
        }

        public override string ToString()
        {
            return string.Format("Innovation: {0}\nInput: [ {1} ]\nOutput: [ {2} ]\nWeight: {3}\nEnabled: {4}",
                this.Innovation, this.Input.ToString(), this.Output.ToString(), this.Weight, this.Enabled
            );
        }


        #endregion

    }
}
