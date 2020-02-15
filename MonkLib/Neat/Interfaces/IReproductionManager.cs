using System;
using System.Collections.Generic;
using System.Text;

namespace MonkLib.Neat
{
    public interface IReproductionManager
    {
        Genome PerformCrossover(Genome parent1, Genome parent2);
    }
}
