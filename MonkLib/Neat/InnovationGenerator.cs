using System;
using System.Collections.Generic;
using System.Text;

namespace MonkLib.Neat
{
    public static class InnovationGenerator
    {
        private static uint id = 0;

        public static uint ID { get => ++id; }

        public static void Reset()
        {
            id = 0;
        }

        public static void Reset(uint start)
        {
            id = start;
        }
    }

}
