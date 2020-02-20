using System;
using System.Collections.Generic;
using System.Text;

namespace MonkLib.Neat
{
    interface IPopulationManager
    {
        List<Genome> Population { get; }
        Dictionary<uint, Genome> Species { get; }
        void InitializePopulation();
    }
}
