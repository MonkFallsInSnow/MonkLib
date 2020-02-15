using System.Collections.Generic;

namespace MonkLib.Neat
{
    public class Genome
    {
        #region Properties

        public IDictionary<uint, NodeGene> Nodes { get; set; }
        public IDictionary<uint, ConnectionGene> Connections { get; set; }

        #endregion


        #region Constructors

        public Genome()
        {
            this.Nodes = new Dictionary<uint, NodeGene>();
            this.Connections = new SortedDictionary<uint, ConnectionGene>();
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

        public override string ToString()
        {
            uint count = 0;
            string output = "";

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
