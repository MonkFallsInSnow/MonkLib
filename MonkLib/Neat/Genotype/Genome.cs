using System.Collections.Generic;

namespace MonkLib.Neat
{
    public class Genome
    {
        #region Properties

        public SortedDictionary<int, NodeGene> Nodes { get; set; }
        public SortedDictionary<int, ConnectionGene> Connections { get; set; }
        public double Fitness { get; set; }
        #endregion


        #region Constructors

        public Genome()
        {
            this.Nodes = new SortedDictionary<int, NodeGene>();
            this.Connections = new SortedDictionary<int, ConnectionGene>();
        }

        #endregion

        #region Utils

        public void Add(params ConnectionGene[] connections)
        {
            foreach (ConnectionGene connection in connections)
            {
                if (!this.Connections.ContainsKey(connection.Innovation))
                {
                    this.Connections.Add(connection.Innovation, connection);
                }
            }
        }

        public void Add(List<ConnectionGene> connections)
        {
            foreach (ConnectionGene connection in connections)
                this.Add(connection);
        }

        public void Add(params NodeGene[] nodes)
        {
            foreach (NodeGene node in nodes)
            {
                if (!this.Nodes.ContainsKey(node.Innovation))
                {
                    this.Nodes.Add(node.Innovation, node);

                }
            }
        }

        public void Add(List<NodeGene> nodes)
        {
            foreach (NodeGene node in nodes)
                this.Add(node);
        }

        public override string ToString()
        {
            int count = 0;
            string output = string.Format("Fitness: {0}\n",this.Fitness);

            foreach (ConnectionGene connection in this.Connections.Values)
            {
                count++;
                output += string.Format("Connection {0}\n{1}\n\n", count, connection);
            }

            return output;
        }
        #endregion

    }
}
