using System;
using System.Collections.Generic;
using System.Linq;

namespace MonkLib.Neat
{
    public class ReproductionManager : IReproductionManager
    {
        private static System.Random rand = new Random();
        private GeneArchive archive;

        public ReproductionManager(GeneArchive archive)
        {
            this.archive = archive;
        }

        public Genome PerformCrossover(Genome parent1, Genome parent2)
        {

            Genome child = new Genome();

            child.Add(this.SelectConnections(parent1, parent2));
            child.Add(this.SelectConnections(parent2, parent1));

            return child;
        }

        private List<ConnectionGene> SelectConnections(Genome genome1, Genome genome2)
        {
            List<ConnectionGene> connections = new List<ConnectionGene>();

            foreach (KeyValuePair<uint, ConnectionGene> keyValuePair in genome1.Connections)
            {
                if (genome2.Connections.ContainsKey(keyValuePair.Key))
                {
                    ConnectionGene gene = rand.Next(0, 2) == 0 ?
                        genome1.Connections[keyValuePair.Key] :
                        genome2.Connections[keyValuePair.Key];

                    connections.Add(gene);
                }
                else
                {
                    if (genome1.Fitness >= genome2.Fitness)
                    {
                        connections.Add(genome1.Connections[keyValuePair.Key]);
                    }
                }
            }

            return connections;
        }

        public void MutateNode(Genome genome)
        {
            NodeGene newNode;

            List<ConnectionGene> candidateConnections = genome.Connections.Values.ToList();
            int randomIndex = rand.Next(0, candidateConnections.Count);
            ConnectionGene connectionToSplit = candidateConnections[randomIndex];

            RecordIndex index = new RecordIndex(connectionToSplit.Input.Innovation, connectionToSplit.Output.Innovation);
            NodeGene wasSplitBy = this.archive.GetRecord(index).wasSplitBy;

            if (wasSplitBy != null)
            {
                newNode = new NodeGene(wasSplitBy);
            }
            else
            {
                newNode = new NodeGene(InnovationGenerator.ID, Constants.NodeType.HIDDEN);
            }

            ConnectionGene connection1 = new ConnectionGene(InnovationGenerator.ID, connectionToSplit.Input, newNode, connectionToSplit.Weight);
            ConnectionGene connection2 = new ConnectionGene(InnovationGenerator.ID, newNode, connectionToSplit.Output);

            //genome.Connections.Remove(connectionToSplit.Innovation);
            genome.Connections[connectionToSplit.Innovation].Enabled = false;
            genome.Add(newNode);
            genome.Add(connection1, connection2);

            this.archive.CreateRecord(genome.Connections[connectionToSplit.Innovation], newNode);
            this.archive.CreateRecord(connection1);
            this.archive.CreateRecord(connection2);
        }

        public void MutateConnection(Genome genome)
        {
            List<NodeGene> candidateInputNodes = genome.Nodes.Values
                .Where(n => n.Type != Constants.NodeType.OUTPUT && n.Type != Constants.NodeType.BIAS)
                .ToList();

            List<NodeGene> candidateOutputNodes = genome.Nodes.Values
                .Where(n => n.Type != Constants.NodeType.INPUT && n.Type != Constants.NodeType.BIAS)
                .ToList();

            List<(NodeGene, NodeGene)> candidateConnections = new List<(NodeGene, NodeGene)>();

            foreach(NodeGene input in candidateInputNodes)
            {
                foreach (NodeGene output in candidateOutputNodes)
                {
                    if(!this.archive.HasActiveRecord(new RecordIndex(input.Innovation, output.Innovation)))
                    {
                        candidateConnections.Add((input, output));
                    }
                }
                
            }

            if (candidateConnections.Count > 0)
            {
                int index = rand.Next(0, candidateConnections.Count);

                ConnectionGene newConnection = new ConnectionGene(
                    InnovationGenerator.ID,
                    candidateConnections[index].Item1,
                    candidateConnections[index].Item2
                );

                genome.Add(newConnection);
                this.archive.CreateRecord(newConnection);
            }
        }

        public void PerturbWeights(Genome genome)
        {
            foreach (ConnectionGene connection in genome.Connections.Values)
            {
                if (rand.NextDouble() <= Constants.PCHANGE_WEIGHT)
                    connection.Weight += rand.Next(0, 2) == 0 ? rand.NextDouble() : -rand.NextDouble();
            }

        }

        public bool IsDifferentSpecies(Genome genome1, Genome genome2)
        {
            int geneCount = genome1.Connections.Count + genome1.Connections.Count;
            int excessCount = this.GetExcessGenes(genome1, genome2).Count;
            int disjointCount = this.GetDisjointGenes(genome1, genome2).Count;
            double averageWeightDifference = this.GetAverageWeightDifference(genome1, genome2);

            double delta = Constants.EXCESS_SCALAR * ((double)excessCount / geneCount) + 
                Constants.DISJOINT_SCALAR * ((double)disjointCount / geneCount) + 
                Constants.WEIGHT_SCALAR * averageWeightDifference;

            Console.WriteLine(delta + "\n");
            return delta < Constants.COMPATIBILITY_THRESHOLD;
        }

        private List<ConnectionGene> GetDisjointGenes(Genome genome1, Genome genome2)
        {
            List<ConnectionGene> disjointGenes = new List<ConnectionGene>();

            uint earliestInnovation = Math.Min(genome1.Connections.Keys.Max<uint>(), genome2.Connections.Keys.Max<uint>());
            
            genome1.Connections
                .Except(genome2.Connections)
                .Where(g => g.Key <= earliestInnovation)
                .ToList()
                .ForEach(c => disjointGenes.Add(c.Value));

            genome2.Connections
                .Except(genome1.Connections)
                .Where(g => g.Key <= earliestInnovation)
                .ToList()
                .ForEach(c => disjointGenes.Add(c.Value));

            return disjointGenes;
        }

        private List<ConnectionGene> GetExcessGenes(Genome genome1, Genome genome2)
        {
            List<ConnectionGene> excessGenes = new List<ConnectionGene>();

            uint earliestInnovation = Math.Min(genome1.Connections.Keys.Max<uint>(), genome2.Connections.Keys.Max<uint>());
            uint latestInnovation = Math.Max(genome1.Connections.Keys.Max<uint>(), genome2.Connections.Keys.Max<uint>());

            genome1.Connections
                .Except(genome2.Connections)
                .Where(g => g.Key > earliestInnovation && g.Key <= latestInnovation)
                .ToList()
                .ForEach(c => excessGenes.Add(c.Value));

            genome2.Connections
                .Except(genome1.Connections)
                .Where(g => g.Key > earliestInnovation && g.Key <= latestInnovation)
                .ToList()
                .ForEach(c => excessGenes.Add(c.Value));

            return excessGenes;
        }

        private double GetAverageWeightDifference(Genome genome1, Genome genome2)
        {
            double sum = genome1.Connections.Sum(c => c.Value.Weight) + genome2.Connections.Sum(c => c.Value.Weight);

            return sum / (genome1.Connections.Count + genome2.Connections.Count);
        }
    }
}
