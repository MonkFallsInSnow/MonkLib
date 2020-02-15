using System;

namespace MonkLib.Markov
{
    public interface IMarkov
    {
        object GetNextState(object fromState);
        object GetRandomState();
    }
}
