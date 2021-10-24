using System;


namespace Ductulator.Core
{
    public static class ResizeRectangular
    {
        public static double Ductulate(double vFactor,string rnDctSize, string sizeDuct)
        {
            double RoundDuctSize = Convert.ToDouble(rnDctSize);
            double b_new_side = 0;
            double i = 1;
            double a_proposed = Convert.ToDouble(sizeDuct);
            double temp_diam_equiv = (1.3 * Math.Pow(a_proposed * i, 0.625)) / Math.Pow(a_proposed + i, 0.25);

            while (temp_diam_equiv <= RoundDuctSize)
            {
                i += vFactor;
                temp_diam_equiv = (1.3 * Math.Pow(a_proposed * i, 0.625)) / Math.Pow(a_proposed + i, 0.25);
            }
            b_new_side = i;

            return Math.Round(b_new_side, 2);
        }
    }
}
