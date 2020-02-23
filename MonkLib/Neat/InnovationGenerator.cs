using System;
using System.Collections.Generic;
using System.Text;

namespace MonkLib.Neat
{
    public static class InnovationGenerator
    {
        private static int id = 0;

        public static int ID { get => ++id; }

        public static void Reset(int start = 0)
        {
            id = start;
        }
    }

}
