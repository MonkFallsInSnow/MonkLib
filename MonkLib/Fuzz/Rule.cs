namespace MonkLib.Fuzz
{
    public class Rule
    {
        public IFuzzyTerm Antecedent { get; private set; }
        public IFuzzyTerm Consequent { get; private set; }

        public Rule(IFuzzyTerm antecedent, IFuzzyTerm consequent)
        {
            this.Antecedent = antecedent;
            this.Consequent = consequent;
        }

        public void ApplyRule()
        {
            this.Consequent.OrWithDom(this.Antecedent.DOM);
        }
    }
}
