using System;
using System.Threading;

namespace BankingSite.FunctionalUITests.DemoHelperCode
{
    static class Utilities
    {
        // Slow down browser automation so can see it in recorded Pluralsight video 
        public static void Wait(int ms = 1000)
        {
            Thread.Sleep(ms);
        }

        public static int GenerateRandomNumber(int min = 0, int max = 9999999)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int rInt = r.Next(min, max); //for ints
            return rInt;
        }
    }
}
