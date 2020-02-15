using System;

namespace MonkLib.Fuzz
{
    public class Very : IFuzzyTerm
    {
        private readonly IFuzzyTerm term;

        public double DOM
        {
            get
            {
                return Math.Pow(this.term.DOM, 2);
            }
        }

        public Very(IFuzzyTerm term)
        {
            this.term = term;
        }

        public void OrWithDom(double val)
        {
            term.OrWithDom(this.DOM);
        }
    }

    public class Somewhat : IFuzzyTerm
    {
        private readonly IFuzzyTerm term;

        public double DOM
        {
            get
            {
                return Math.Sqrt(this.term.DOM);
            }
        }

        public Somewhat(IFuzzyTerm term)
        {
            this.term = term;
        }

        public void OrWithDom(double val)
        {
            term.OrWithDom(this.DOM);
        }
    }

    public class Not : IFuzzyTerm
    {
        private readonly IFuzzyTerm term;

        public double DOM
        {
            get
            {
                return 1 - this.term.DOM;
            }
        }

        public Not(IFuzzyTerm term)
        {
            this.term = term;
        }

        public void OrWithDom(double val)
        {
            term.OrWithDom(this.DOM);
        }
    }
}
