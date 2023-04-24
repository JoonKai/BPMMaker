using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellsysLibrary.Helpers
{
    public class MathHelper
    {
        /// <summary>
        /// 두개의 좌표사이의 거리를 계산
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns></returns>
        public double Distance(double x0, double y0, double x1, double y1)
        {
            double num1 = x1- x0;
            double num2 = y1- y0;
            return Math.Sqrt(num1* num1 + num2 * num2);
        }
        /// <summary>
        /// 밑변길이와 높이를 통해 끼인각 계산 x:밑변길이 z: 높이
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double CalcAngle(double x, double z)
        {
            double num = Math.Atan2(x, z) * 180/Math.PI;
            if (num < 0)
            {
                num += 360.0;
            }
            return num;
        }
    }
}
