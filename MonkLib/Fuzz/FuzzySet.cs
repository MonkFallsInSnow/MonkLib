namespace MonkLib.Fuzz
{
    //proxy class
    public class FuzzySet : IFuzzyTerm
    {
        private Set set;

        public double DOM { get; set; }
        public double RepresentativeValue { get; private set; }
        public string Name { get; private set; }

        public FuzzySet(string name, Set set)
        {
            this.Name = name;
            this.set = set;
            this.RepresentativeValue = set.RepresentativeValue;
        }

        public double CalculateDOM(double crispValue)
        {
            this.DOM = set.CalculateDOM(crispValue);
            return this.DOM;
        }

        public void OrWithDom(double value)
        {
            if(value > this.DOM)
            {
                this.DOM = value;
            }
        }
    }
}
