using System;

namespace MonkLib.Fuzz
{
    public abstract class Set
    {
        public double RepresentativeValue { get; private set; }

        internal Set(double representativeValue)
        {
            this.RepresentativeValue = representativeValue;
        }

        protected double GetSlope(double x1, double x2, double y1, double y2)
        {
            return (y1 - y2) / (x1 - x2);
        }

        protected double FindY(double slope, double x1, double x2, double y2)
        {
            return slope * (x1 - x2) + y2;
        }

        protected void TryValidate(double val, out double result)
        {
            if(val < 0)
            {
                result = 0;
            }
            else
            {
                result = val;
            }
        }

        public abstract double CalculateDOM(double crispValue);
    }

    public class TriangularSet : Set
    {
        private readonly double start;
        private readonly double end;

        public TriangularSet(double start, double mid, double end) : base(mid)
        {
            this.TryValidate(start, out this.start);
            this.TryValidate(end, out this.end);

        }

        public override double CalculateDOM(double crispValue)
        {
            double dom = 0f;
            this.TryValidate(crispValue, out crispValue);

            if (crispValue == this.RepresentativeValue)
            {
                dom = 1f;
            }
            else if (crispValue > this.start && crispValue < this.RepresentativeValue)
            {
                double slope = this.GetSlope(this.start, this.RepresentativeValue, 0f, 1f);
                dom = this.FindY(slope, crispValue, this.RepresentativeValue, 1f);
            }
            else if (crispValue > this.RepresentativeValue && crispValue < this.end)
            {
                double slope = this.GetSlope(this.RepresentativeValue, this.end, 1f, 0f);
                dom = this.FindY(slope, crispValue, this.end, 0f);
            }

            return dom;
        }
    }

    public class LeftShoulderSet : Set
    {
        private readonly double mid;
        private readonly double end;

        public LeftShoulderSet(double mid, double end) : base(mid / 2f)
        {
            this.TryValidate(mid, out this.mid);
            this.TryValidate(end, out this.end);
        }

        public override double CalculateDOM(double crispValue)
        {
            double dom = 0f;
            this.TryValidate(crispValue, out crispValue);

            if (crispValue >= 0 && crispValue <= this.mid)
            {
                dom = 1f;
            }
            else if (crispValue > this.mid && crispValue <= this.end)
            {
                double slope = this.GetSlope(this.mid, this.end, 1f, 0f);
                dom = this.FindY(slope, crispValue, this.end, 0f);
            }

            return dom;
        }
    }

    public class RightShoulderSet : Set
    {
        private readonly double start;
        private readonly double mid;

        public RightShoulderSet(double start, double mid, double end) : base((mid + end) / 2f)
        {
            this.TryValidate(start, out this.start);
            this.TryValidate(mid, out this.mid);
        }

        public override double CalculateDOM(double crispValue)
        {
            double dom = 0f;
            this.TryValidate(crispValue, out crispValue);

            if (crispValue >= this.mid)
            {
                dom = 1f;
            }
            else if (crispValue > this.start && crispValue < this.mid)
            {
                double slope = this.GetSlope(this.start, this.mid, 0f, 1f);
                dom = this.FindY(slope, crispValue, this.mid, 1f);
            }

            return dom;
        }
    }

    public class TrapazoidalSet : Set
    {
        //points from left to right
        private readonly double x1;
        private readonly double x2;
        private readonly double x3;
        private readonly double x4;

        public TrapazoidalSet(double x1, double x2, double x3, double x4) : base((x2 + x3) / 2)
        {
            this.TryValidate(x1, out this.x1);
            this.TryValidate(x2, out this.x2);
            this.TryValidate(x3, out this.x3);
            this.TryValidate(x4, out this.x4);
        }

        public override double CalculateDOM(double crispValue)
        {
            double dom = 0f;
            this.TryValidate(crispValue, out crispValue);

            if (crispValue >= this.x1 && crispValue < this.x2)
            {
                double slope = this.GetSlope(this.x1, this.x2, 0f, 1f);
                dom = this.FindY(slope, crispValue, this.x2, 1f);
            }
            else if (crispValue > this.x3 && crispValue <= this.x4)
            {
                double slope = this.GetSlope(this.x3, this.x4, 1f, 0f);
                dom = this.FindY(slope, crispValue, this.x4, 0f);
            }

            return dom;
        }
    }

    public class GuassianSet : Set
    {
        private readonly double center;
        private readonly double width;

        public GuassianSet(double center, double width) : base(center)
        {
            this.TryValidate(center, out this.center);
            this.TryValidate(width, out this.width);
        }

        public override double CalculateDOM(double crispValue)
        {
            double exponent = 0;
            this.TryValidate(crispValue, out crispValue);

            try
            {
                exponent = -0.5f * (crispValue - this.center) * (1 / this.width);
            }
            catch(DivideByZeroException)
            {
                exponent = -0.5f * (crispValue - this.center) * (1 / this.width + 1);
                Console.WriteLine("Division by zero occured because width was equal to zero. Value of width increased by 1.");
            }

            return (double)Math.Pow(Math.E, exponent);
        }
    }

    public class SingletonSet : Set
    {
        public SingletonSet(double representativeValue) : base(representativeValue) { }

        public override double CalculateDOM(double crispValue)
        {
            return crispValue == this.RepresentativeValue ? 1 : 0;
        }
    }
}
