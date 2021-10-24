using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ductulator.Views_Cs
{
    public static class CheckValues
    { 

    public static bool CheckRatioRelation(string size01, string size02, string typeDuct)
    {
        bool result = false;
        double ductSize01 = Convert.ToDouble(size01);
        double ductSize02 = Convert.ToDouble(size02);
        double biggerSize = 0;
        double smallerSize = 0;

        if (ductSize01 > ductSize02)
        {
            biggerSize = ductSize01;
            smallerSize = ductSize02;
        }
        else if (ductSize01 < ductSize02)
        {
            biggerSize = ductSize02;
            smallerSize = ductSize01;
        }

        double ratio = biggerSize / smallerSize;

        if (typeDuct != "Round")
        {
            if(ratio > 4)
            {
                result = true;
            }
            else
            {
                result = false;
            }

        }

        return result;
    }
  }
}
