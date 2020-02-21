using System;
using System.Collections.Generic;
using System.Text;

namespace MonkLib.Neat
{
    interface IPopulationManager
    {
        List<Genome> Population { get; }
        Dictionary<int, Genome> Species { get; }
        void InitializePopulation();
    }
}
