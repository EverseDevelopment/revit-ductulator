using System;


namespace Ductulator.Model
{
    public static class DuctSide_equiv
    {
        public static double Side_equiv(double diam)
        {
            double tempSide = 0.5 * diam * Math.Sqrt(Math.PI);
            return Math.Round(tempSide);
        }
    }
}
