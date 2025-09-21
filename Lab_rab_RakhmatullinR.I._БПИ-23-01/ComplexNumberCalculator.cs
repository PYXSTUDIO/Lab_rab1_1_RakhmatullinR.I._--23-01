using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_rab_RakhmatullinR.I._БПИ_23_01
{
    public class ComplexNumberCalculator
    {
        public double RealPart { get; private set; }      // a1 - действительная часть
        public double ImaginaryPart { get; private set; } // b1 - мнимая часть

        public ComplexNumberCalculator(double real, double imaginary)
        {
            RealPart = real;
            ImaginaryPart = imaginary;
        }


        public double CalculateModulus()
        {
            return Math.Sqrt(RealPart * RealPart + ImaginaryPart * ImaginaryPart);
        }


        public double CalculateArgument()
        {
            if (RealPart == 0 && ImaginaryPart == 0)
                return 0;

            double radians = Math.Atan2(ImaginaryPart, RealPart);
            double degrees = radians * (180 / Math.PI);


            if (degrees < 0)
                degrees += 360;

            return degrees;


        }

    }
}