using System;

namespace MonkLib.Fuzz
{
    public class And : IFuzzyTerm
    {
        private readonly IFuzzyTerm[] terms;

        public double DOM
        {
            get
            {
                double min = double.MaxValue;

                foreach (IFuzzyTerm term in terms)
                {
                    if (term.DOM < min)
                        min = term.DOM;
                }

                return min;
            }
        }

        public And(params IFuzzyTerm[] terms)
        {
            this.terms = terms;
        }

        public void OrWithDom(double value)
        {
            for(int i = 0; i < this.terms.Length; i++)
            {
                terms[i].OrWithDom(value);
            }
        }
    }

    public class Or : IFuzzyTerm
    {
        private readonly IFuzzyTerm[] terms;

        public double DOM
        {
            get
            {
                double max = double.MinValue;

                foreach (IFuzzyTerm term in terms)
                {
                    if (term.DOM > max)
                        max = term.DOM;
                }

                return max;
            }
        }

        public Or(params IFuzzyTerm[] terms)
        {
            this.terms = terms;
        }

        public void OrWithDom(double value)
        {
            for (int i = 0; i < this.terms.Length; i++)
            {
                terms[i].OrWithDom(value);
            }
        }
    }
}