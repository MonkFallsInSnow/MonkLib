using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MonkLib.Markov
{
    public class MarkovChain : IMarkov
    {
        private Random rand;
        private List<object> stateTransitionSequence;
        private Dictionary<object, Dictionary<object, double>> transitionMatrix;
        private Dictionary<object, StateData> stateDataMap;
        private IEqualityComparer<object> stateComparer;

        private class StateData
        {
            public int FromStateFrequency { get; set; }
            public Dictionary<object, double> Transitions { get; set; }

            public StateData()
            {
                this.FromStateFrequency = 1;
                this.Transitions = new Dictionary<object, double>();
            }
        }

        public MarkovChain(List<object> stateTransitionSequence, IEqualityComparer<object> comparer = null)
        {
            this.rand = new Random();
            this.stateTransitionSequence = stateTransitionSequence;
            this.stateComparer = comparer;
            this.stateDataMap = new Dictionary<object, StateData>();
            this.transitionMatrix = new Dictionary<object, Dictionary<object, double>>();

            this.ParseData();
            this.BuildTransitionMatrix();
        }

        public object GetRandomState()
        {
            return this.transitionMatrix.Keys.ToList()[rand.Next(0, this.transitionMatrix.Count)]; ;
        }

        public object GetNextState(object fromState)
        {
            object nextState = fromState;
            double num = rand.NextDouble();
            double universalProbability = this.transitionMatrix[fromState].Sum(t => t.Value);
            double sum = 0;

            try
            {
                foreach (KeyValuePair<object, double> transitionProbability in this.transitionMatrix[fromState])
                {
                    sum += transitionProbability.Value;

                    if (num <= sum)
                    {
                        nextState = transitionProbability.Key;
                        break;
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                //do nothing
            }

            return nextState;
        }

        private void ParseData()
        {
            //aabaa
            for (int i = 0; i < this.stateTransitionSequence.Count - 1; i++)
            {
                object fromState = this.stateTransitionSequence[i];
                object toState = this.stateTransitionSequence[i + 1];

                if(!this.stateDataMap.Keys.Contains(fromState, this.stateComparer))
                {
                    this.stateDataMap.Add(fromState, new StateData());
                    this.stateDataMap[fromState].Transitions.Add(toState, 1);
                }
                else
                {
                    stateDataMap[fromState].FromStateFrequency++;

                    if(!stateDataMap[fromState].Transitions.Keys.Contains(toState, this.stateComparer))
                    {
                        stateDataMap[fromState].Transitions.Add(toState, 1);
                    }
                    else
                    {
                        stateDataMap[fromState].Transitions[toState]++;
                    }
                }
            }
        }

        private void BuildTransitionMatrix()
        {
            foreach(KeyValuePair<object, StateData> state in this.stateDataMap)
            {
                this.transitionMatrix.Add(state.Key, new Dictionary<object, double>());

                foreach(KeyValuePair<object, double> transition in state.Value.Transitions)
                {
                    this.transitionMatrix[state.Key].Add(transition.Key, transition.Value / state.Value.FromStateFrequency);
                }

            }
        }
    }
}

