using System;


namespace Ductulator.Model
{
    public static class DuctDiamEquiv
    {
        public static double Diam_equiv(double aSide, double bSide)
        {
            double result = (1.3 * Math.Pow(aSide * bSide, 0.625)) / Math.Pow((aSide + bSide), 0.25);
            return result;
        }
    }
}
