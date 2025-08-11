using System;


namespace Ductulator.Core
{
    public static class ResizeRectangular
    {
        public static double Ductulate(double vFactor, double rnDctSize, double a_proposed)
        {
            double i = 1;
            double temp_diam_equiv = GetEquivalentDiameter(a_proposed, i);

            while (temp_diam_equiv <= rnDctSize)
            {
                i += vFactor;
                temp_diam_equiv = GetEquivalentDiameter(a_proposed, i);
            }

            return Math.Round(i, 2);
        }

        private static double GetEquivalentDiameter(double a, double b)
        {
            return (1.3 * Math.Pow(a * b, 0.625)) / Math.Pow(a + b, 0.25);
        }
    }
}
