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

            return child;
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

        private void PerturbWeights(Genome genome)
        {
            foreach (ConnectionGene connection in genome.Connections.Values)
            {

            }

        }
    }
}
