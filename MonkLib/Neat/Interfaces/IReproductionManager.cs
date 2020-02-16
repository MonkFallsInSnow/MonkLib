using System;
using System.Collections.Generic;
using System.Text;

namespace MonkLib.Neat
{
    public interface IReproductionManager
    {
        Genome PerformCrossover(Genome parent1, Genome parent2);
        void MutateNode(Genome genome);
        void MutateConnection(Genome genome);
        void PerturbWeights(Genome genome);
    }
}
