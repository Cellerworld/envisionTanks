using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.Tanks.Math
{
    public class Util
    {
        public static float DegreesToRadians(float degrees)
        {
            return (float)(degrees * (System.Math.PI / 180));
        }

        public static float RadiansToDegrees(float radians)
        {
            return (float)(radians * (180 / System.Math.PI));
        }
    }
}
