using System;


namespace Ductulator.Model
{
    public static class DuctDiamEquiv
    {
        public static double Diam_equiv(double aSide, double bSide)
        {
            /* local variable declaration */
            int result;

            double tempdiam;

            //Obtain equivalent diameter
            tempdiam = (1.3 * Math.Pow(aSide * bSide, 0.625)) / Math.Pow((aSide + bSide), 0.25);

            //Obtain equivalent diameter
            result = Convert.ToInt32(Math.Round(tempdiam));
            return result;
        }
    }
}
