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

    public struct ConnectionDistributionWeights
    {
        public readonly double Average;
        public readonly double BelowAverage;
        public readonly double AboveAverage;

        public ConnectionDistributionWeights(double average, double belowAverage, double aboveAverage)
        {
            if(average + belowAverage + aboveAverage != 1)
            {
                throw new ArgumentException("Parameter values must sum to 1");
            }

            this.Average = average;
            this.BelowAverage = belowAverage;
            this.AboveAverage = aboveAverage;
        }
    }

    public class PopulationManager : IPopulationManager
    {
        private readonly PopulationMetrics metrics;
        private readonly ConnectionDistributionWeights connectionDistributionWeights;

        private static Random rand = new Random();
        private GeneArchive archive;

        public List<Genome> Population { get; private set; }
        public Dictionary<int, Genome> Species { get; private set; }

        public PopulationManager(PopulationMetrics metrics, ConnectionDistributionWeights connectionDistribution, GeneArchive archive)
        {
            this.metrics = metrics;
            this.archive = archive;
            this.Population = new List<Genome>();
            this.Species = new Dictionary<int, Genome>();
            this.connectionDistributionWeights = connectionDistribution;
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

                int numConnections = 0;
                double chance = rand.NextDouble();

                genome.Add(this.GenerateConnections(numConnections, inputs, outputs));
                this.Population.Add(genome);
            }
        }

        private List<NodeGene> GenerateNodes(int count, Constants.NodeType type)
        {
            List<NodeGene> nodes = new List<NodeGene>();

            for (int i = 0; i < this.metrics.OutputCount; i++)
            {
                NodeGene node = new NodeGene(InnovationGenerator.ID, type);
                nodes.Add(node);
            }

            return nodes;
        }

        private int GetConnectionDistribution()
        {

            return 0;
        }

        private List<ConnectionGene> GenerateConnections(int maxConnections, List<NodeGene> inputs, List<NodeGene> outputs)
        {
            List<ConnectionGene> connections = new List<ConnectionGene>();

            int maxConnectionsPerNode = outputs.Count;

            while(connections.Count < maxConnections)
            { }

            //remember to archive connections
            foreach (ConnectionGene connection in connections)
            {
                this.archive.CreateRecord(connection);
            }

            return connections;
        }
    }
}
