using System.Collections.Generic;

namespace MonkLib.Fuzz
{
    public class LinguisticVariable
    {
        private readonly string name;

        public Dictionary<string, FuzzySet> Sets { get; set; }

        public LinguisticVariable(string name)
        {
            this.name = name;
            this.Sets = new Dictionary<string, FuzzySet>();
        }

        public FuzzySet AddTriangularSet(string setName, double start, double mid, double end)
        {
            FuzzySet set = new FuzzySet(setName, new TriangularSet(start, mid, end));
            this.Sets.Add(setName, set);

            return set;
        }

        public FuzzySet AddRightShoulderSet(string setName, double start, double mid, double end)
        {
            FuzzySet set = new FuzzySet(setName, new RightShoulderSet(start, mid, end));
            this.Sets.Add(setName, set);

            return set;
        }

        public FuzzySet AddLeftShoulderSet(string setName, double mid, double end)
        {
            FuzzySet set = new FuzzySet(setName, new LeftShoulderSet(mid, end));
            this.Sets.Add(setName, set);

            return set;
        }

        public FuzzySet AddTrapazoidalSet(string setName, double x1, double x2, double x3, double x4)
        {
            FuzzySet set = new FuzzySet(setName, new TrapazoidalSet(x1, x2, x3, x4));
            this.Sets.Add(setName, set);

            return set;
        }

        public FuzzySet AddGuassianSet(string setName, double center, double width)
        {
            FuzzySet set = new FuzzySet(setName, new GuassianSet(center, width));
            this.Sets.Add(setName, set);

            return set;
        }

        public FuzzySet AddSingletonSet(string setName, double representativeValue)
        {
            FuzzySet set = new FuzzySet(setName, new SingletonSet(representativeValue));
            this.Sets.Add(setName, set);

            return set;
        }

    }
}
