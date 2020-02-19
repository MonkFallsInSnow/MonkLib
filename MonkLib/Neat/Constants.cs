namespace MonkLib.Neat
{
    public static class Constants
    {
        public const double PCHANGE_WEIGHT = 0.35;
        public const double COMPATIBILITY_THRESHOLD = 1;
        public const double EXCESS_SCALAR = 1;
        public const double DISJOINT_SCALAR = 1;
        public const double WEIGHT_SCALAR = 1;

        public enum NodeType
        {
            INPUT,
            HIDDEN,
            OUTPUT,
            BIAS
        }
    }
}
