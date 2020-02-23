using System;
using System.Collections.Generic;
using System.Text;

namespace MonkLib.Neat
{
    public struct PopulationMetrics
    {
        public readonly int InitialSize;
        public readonly int InputCount;
        public readonly int OutputCount;

        public PopulationMetrics(int initialSize, int inputCount, int outputCount)
        {
            this.InitialSize = initialSize;
            this.InputCount = inputCount;
            this.OutputCount = outputCount;
        }
    }


    public class PopulationManager : IPopulationManager
    {
        private readonly PopulationMetrics metrics;

        private static Random rand = new Random();
        private GeneArchive archive;

        public List<Genome> Population { get; private set; }
        public Dictionary<int, Genome> Species { get; private set; }

        public PopulationManager(PopulationMetrics metrics, GeneArchive archive)
        {
            this.metrics = metrics;
            this.archive = archive;
            this.Population = new List<Genome>();
            this.Species = new Dictionary<int, Genome>();
        }

        public void InitializePopulation()
        {
            for (int i = 0; i < this.metrics.InitialSize; i++)
            {
                Genome genome = new Genome();

                List<NodeGene> inputs = this.GenerateNodes(this.metrics.InputCount, Constants.NodeType.INPUT);
                List<NodeGene> outputs = this.GenerateNodes(this.metrics.OutputCount, Constants.NodeType.OUTPUT);

                genome.Add(inputs);
                genome.Add(outputs);

                genome.Add(this.GenerateConnections(inputs, outputs));
                this.Population.Add(genome);

                InnovationGenerator.Reset();
            }
        }
 

        private List<NodeGene> GenerateNodes(int count, Constants.NodeType type)
        {
            List<NodeGene> nodes = new List<NodeGene>();

            for (int i = 0; i < count; i++)
            {
                NodeGene node = new NodeGene(InnovationGenerator.ID, type);
                nodes.Add(node);
            }

            return nodes;
        }


        private List<ConnectionGene> GenerateConnections(List<NodeGene> inputs, List<NodeGene> outputs)
        {
            List<ConnectionGene> connections = new List<ConnectionGene>();
            
            int maxConnectionsPerNode = this.metrics.OutputCount;
            int maxNetworkConnections = this.metrics.InputCount * this.metrics.OutputCount;
            int currentConnectionCount = 0;

            while(currentConnectionCount != maxNetworkConnections && inputs.Count > 0)
            {
                int inputIndex = rand.Next(0, inputs.Count);
                int outputIndex = rand.Next(0, outputs.Count);
                int numConnections = rand.Next(1, Math.Min(maxConnectionsPerNode, maxNetworkConnections) + 1);

                for(int i = 0; i < numConnections; i++)
                {
                    if(outputIndex > outputs.Count - 1)
                    {
                        outputIndex = 0;
                    }

                    connections.Add(
                        new ConnectionGene(
                            InnovationGenerator.ID,
                            inputs[inputIndex],
                            outputs[outputIndex++]
                        )
                    );
                }

                inputs.RemoveAt(inputIndex);
                currentConnectionCount++;
            }

            //remember to archive connections
            foreach (ConnectionGene connection in connections)
            {
                this.archive.CreateRecord(connection);
            }

            return connections;
        }

        public override string ToString()
        {
            string output = "";

            for(int i = 0; i < this.Population.Count; i++)
            {
                output += string.Format("Genome: {0}\n{1}\n\n", i+1, this.Population[i].ToString());
            }

            return output;
        }
    }
}
