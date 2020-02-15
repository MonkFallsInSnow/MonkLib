namespace MonkLib.Fuzz
{
    public interface IFuzzyTerm
    {
        double DOM { get; }
        void OrWithDom(double value);
    }
}
