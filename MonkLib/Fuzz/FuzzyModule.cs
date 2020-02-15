using System.Collections.Generic;
using System.Linq;

namespace MonkLib.Fuzz
{
    public class FuzzyModule
    {
        private readonly List<Rule> ruleSet;
        private readonly Dictionary<string, LinguisticVariable> variables;

        public FuzzyModule()
        {
            this.variables = new Dictionary<string, LinguisticVariable>();
            this.ruleSet = new List<Rule>();
        }

        public LinguisticVariable CreateFLV(string name)
        {
            this.variables.Add(name, new LinguisticVariable(name));

            return this.variables[name];
        }

        public void AddRule(Rule rule)
        {
            this.ruleSet.Add(rule);
        }

        public void AddRule(params Rule[] rules)
        {
            this.ruleSet.AddRange(rules);
        }

        public void Fuzzify(string variableName, double crispValue)
        {
            LinguisticVariable variable = this.variables[variableName];

            foreach (FuzzySet set in variable.Sets.Values)
            {
                set.CalculateDOM(crispValue);
            }
        }

        public double DeFuzzify(string variableName)
        {
            foreach (Rule rule in this.ruleSet)
            {
                rule.ApplyRule();
            }

            return this.GetAverageOfMaxima(this.variables[variableName].Sets.Values.ToArray());
        }

        private double GetAverageOfMaxima(params FuzzySet[] sets)
        {
            double scaledRepresentativeValueSum = 0;
            double sumOfDOMs = 0;

            foreach (FuzzySet set in sets)
            {
                scaledRepresentativeValueSum += set.RepresentativeValue * set.DOM;
                sumOfDOMs += set.DOM;
            }

            return scaledRepresentativeValueSum / sumOfDOMs;
        }
    }
}