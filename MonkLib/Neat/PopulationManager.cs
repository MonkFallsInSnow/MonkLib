using System;
using System.Collections.Generic;
using System.Text;

namespace MonkLib.Neat
{
    public struct PopulationMetrics
    {
        public readonly uint InitialSize;
        public readonly uint InputCount;
        public readonly uint OutputCount;

        public PopulationMetrics(uint initialSize, uint inputCount, uint outputCount)
        {
            this.InitialSize = initialSize;
            this.InputCount = inputCount;
            this.OutputCount = outputCount;
        }
    }

    public class PopulationManager : IPopulationManager
    {
        private readonly PopulationMetrics metrics;
        private GeneArchive archive;

        public List<Genome> Population { get; private set; }
        public Dictionary<uint, Genome> Species { get; private set; }

        public PopulationManager(PopulationMetrics metrics, GeneArchive archive)
        {
            this.metrics = metrics;
            this.archive = archive;
            this.Population = new List<Genome>();
            this.Species = new Dictionary<uint, Genome>();
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

                //generate connections
                genome.Add(this.GenerateConnections(inputs, outputs));

            }
        }

        private List<NodeGene> GenerateNodes(uint count, Constants.NodeType type)
        {
            List<NodeGene> nodes = new List<NodeGene>();

            for (int i = 0; i < this.metrics.OutputCount; i++)
            {
                NodeGene node = new NodeGene(InnovationGenerator.ID, type);
                nodes.Add(node);
            }

            return nodes;
        }

        private List<ConnectionGene> GenerateConnections(List<NodeGene> inputs, List<NodeGene> outputs)
        {
            return null;
        }
    }
}
