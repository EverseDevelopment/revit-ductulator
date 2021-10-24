using System;


namespace Ductulator.Model
{
    public static class DuctSide_equiv
    {
        public static double Side_equiv(double diam)
        {
            /* local variable declaration */
            double result;

            double tempSide;

            //Obtain equivalent diameter
            tempSide = Math.Sqrt((diam * diam)* Math.PI);


            //Obtain equivalent side
            result = Convert.ToInt32(Math.Round(tempSide));

            return result;
        }
    }
}
